namespace ResiduosApi.Models;

/// <summary>
/// Local físico onde resíduos são descartados/coletados.
/// Usa soft-delete (campo <see cref="Ativo"/>) para preservar histórico de coletas.
/// </summary>
public class PontoColeta
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    /// <summary>Capacidade total do ponto em kg (deve ser &gt; 0).</summary>
    public decimal CapacidadeKg { get; set; }

    /// <summary>Soft-delete: false = desativado (não some do banco).</summary>
    public bool Ativo { get; set; } = true;

    public DateTime CriadoEm { get; set; }

    /// <summary>Coletas registradas neste ponto (lado N do relacionamento).</summary>
    public ICollection<Coleta> Coletas { get; set; } = new List<Coleta>();
}
