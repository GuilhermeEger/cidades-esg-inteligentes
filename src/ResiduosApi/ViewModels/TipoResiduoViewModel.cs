namespace ResiduosApi.ViewModels;

/// <summary>Saída pública de um tipo de resíduo.</summary>
public class TipoResiduoViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;
    public string InstrucoesDescarte { get; set; } = string.Empty;
}
