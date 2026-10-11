namespace ResiduosApi.Models;

/// <summary>
/// Registro de uma coleta: o peso (kg) de um tipo de resíduo retirado de um ponto,
/// numa data, por um responsável. É o fato central para os indicadores agregados.
/// </summary>
public class Coleta
{
    public int Id { get; set; }

    public int PontoColetaId { get; set; }
    public PontoColeta? PontoColeta { get; set; }

    public int TipoResiduoId { get; set; }
    public TipoResiduo? TipoResiduo { get; set; }

    /// <summary>Peso coletado em kg (deve ser &gt; 0).</summary>
    public decimal PesoKg { get; set; }

    public DateTime DataColeta { get; set; }

    public string? Responsavel { get; set; }
}
