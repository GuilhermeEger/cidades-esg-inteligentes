namespace ResiduosApi.ViewModels;

/// <summary>Total coletado (kg) de um tipo de resíduo.</summary>
public class TipoTotalViewModel
{
    public int TipoResiduoId { get; set; }
    public string TipoResiduoNome { get; set; } = string.Empty;
    public decimal TotalKg { get; set; }
}

/// <summary>Indicadores agregados de coleta: total geral e quebra por tipo.</summary>
public class IndicadorViewModel
{
    public decimal TotalGeralKg { get; set; }
    public IEnumerable<TipoTotalViewModel> PorTipo { get; set; } = new List<TipoTotalViewModel>();
}
