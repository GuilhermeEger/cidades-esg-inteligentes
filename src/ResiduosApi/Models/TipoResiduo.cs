namespace ResiduosApi.Models;

/// <summary>
/// Categoria de resíduo reciclável (PAPEL, PLASTICO, VIDRO, METAL, ORGANICO, ELETRONICO).
/// Tabela de domínio populada por seed na migração.
/// </summary>
public class TipoResiduo
{
    public int Id { get; set; }

    /// <summary>Nome único do tipo (ex.: PAPEL).</summary>
    public string Nome { get; set; } = string.Empty;

    /// <summary>Cor padrão do contêiner (ex.: AZUL para papel).</summary>
    public string Cor { get; set; } = string.Empty;

    /// <summary>Orientações de descarte exibidas ao usuário.</summary>
    public string InstrucoesDescarte { get; set; } = string.Empty;

    /// <summary>Coletas associadas a este tipo (lado N do relacionamento).</summary>
    public ICollection<Coleta> Coletas { get; set; } = new List<Coleta>();
}
