#!/usr/bin/env bash
# =====================================================================
# Smoke test: confere se um ambiente recém-publicado está respondendo.
#   Uso: deploy/smoke-test.sh <url-base> [arquivo-de-resumo]
#   Ex.: deploy/smoke-test.sh http://localhost:8081 "$GITHUB_STEP_SUMMARY"
# Secret esperado: API_PASSWORD (API_USER é opcional, padrão admin)
# =====================================================================
set -uo pipefail

URL="${1:?informe a URL base, ex.: http://localhost:8081}"
RESUMO="${2:-/dev/null}"
FALHAS=0

ok()    { echo "OK    $1 -> $2";  echo "| ✅ $1 | $2 |" >> "$RESUMO"; }
falha() { echo "FALHA $1 -> $2";  echo "| ❌ $1 | $2 |" >> "$RESUMO"; FALHAS=$((FALHAS + 1)); }
# Lê um JSON da entrada e imprime a expressão Python pedida (d = documento).
campo() { python3 -c "import json,sys; d=json.load(sys.stdin); print($1)" 2>/dev/null; }

echo "==> Aguardando $URL/health"
for _ in $(seq 1 30); do
  curl -fsS "$URL/health" >/dev/null 2>&1 && break
  sleep 5
done

echo "| Verificação | Resultado |" >> "$RESUMO"
echo "|---|---|" >> "$RESUMO"

# 1. Saúde da API e do banco
r=$(curl -fsS "$URL/health" | campo "d['status'] + ' (banco: ' + d['checks']['banco-de-dados'] + ')'")
if [[ "$r" == Healthy* ]]; then ok "GET /health" "$r"; else falha "GET /health" "${r:-sem resposta}"; fi

# 2. Ambiente e versão no ar
r=$(curl -fsS "$URL/" | campo "d['ambiente'] + ' | versão ' + d['versao']")
if [ -n "$r" ]; then ok "GET / (ambiente e versão)" "$r"; else falha "GET /" "sem resposta"; fi

# 3. Endpoints públicos lendo do banco
r=$(curl -fsS "$URL/api/v1/tipos-residuo" | campo "str(len(d)) + ' tipos: ' + ', '.join(t['nome'] for t in d)")
if [ -n "$r" ]; then ok "GET /api/v1/tipos-residuo" "$r"; else falha "GET /api/v1/tipos-residuo" "sem resposta"; fi

r=$(curl -fsS "$URL/api/v1/indicadores" | campo "'total coletado: ' + str(d['totalGeralKg']) + ' kg'")
if [ -n "$r" ]; then ok "GET /api/v1/indicadores" "$r"; else falha "GET /api/v1/indicadores" "sem resposta"; fi

# 4. Autenticação JWT
TOKEN=$(curl -fsS -X POST "$URL/api/v1/auth/login" -H "Content-Type: application/json" \
  -d "{\"usuario\":\"${API_USER:-admin}\",\"senha\":\"${API_PASSWORD:?defina API_PASSWORD}\"}" | campo "d['token']")
if [ -n "$TOKEN" ]; then ok "POST /api/v1/auth/login" "token JWT emitido"; else falha "POST /api/v1/auth/login" "login recusado"; fi

# 5. Rota protegida: com token = 200, sem token = 401
code=$(curl -s -o /dev/null -w '%{http_code}' -H "Authorization: Bearer $TOKEN" "$URL/api/v1/pontos-coleta")
if [ "$code" = 200 ]; then ok "GET /api/v1/pontos-coleta com token" "HTTP $code"; else falha "GET /api/v1/pontos-coleta com token" "HTTP $code"; fi

code=$(curl -s -o /dev/null -w '%{http_code}' "$URL/api/v1/pontos-coleta")
if [ "$code" = 401 ]; then ok "GET /api/v1/pontos-coleta sem token" "HTTP $code (bloqueado)"; else falha "GET /api/v1/pontos-coleta sem token" "HTTP $code"; fi

if [ "$FALHAS" -gt 0 ]; then
  echo "Smoke test falhou em $FALHAS verificação(ões) em $URL"
  exit 1
fi
echo "Smoke test OK em $URL"
