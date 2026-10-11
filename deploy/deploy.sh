#!/usr/bin/env bash
# =====================================================================
# Deploy de um ambiente com Docker Compose.
#   Uso: deploy/deploy.sh <staging|production> <imagem>
#   Ex.: deploy/deploy.sh staging ghcr.io/usuario/cidades-esg-inteligentes:sha-abc1234
# Secrets esperados no ambiente: DB_PASSWORD, JWT_SECRET, API_PASSWORD
# =====================================================================
set -euo pipefail

AMBIENTE="${1:?informe o ambiente: staging ou production}"
export API_IMAGE="${2:?informe a imagem a publicar}"
ENV_FILE="deploy/${AMBIENTE}.env"
PROJETO="residuos-${AMBIENTE}"

[ -f "$ENV_FILE" ] || { echo "Arquivo $ENV_FILE não encontrado"; exit 1; }
for var in DB_PASSWORD JWT_SECRET API_PASSWORD; do
  [ -n "${!var:-}" ] || { echo "Secret $var não definido para $AMBIENTE"; exit 1; }
done

compose() { docker compose -p "$PROJETO" --env-file "$ENV_FILE" "$@"; }

echo "==> [$AMBIENTE] Baixando a imagem $API_IMAGE"
compose pull --quiet

echo "==> [$AMBIENTE] Subindo os serviços (aguardando health checks)"
compose up -d --no-build --remove-orphans --wait --wait-timeout 300

echo "==> [$AMBIENTE] Containers em execução"
compose ps

echo "==> [$AMBIENTE] Limpando imagens antigas sem uso"
docker image prune -f >/dev/null
