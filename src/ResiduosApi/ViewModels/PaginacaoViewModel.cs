namespace ResiduosApi.ViewModels;

/// <summary>
/// Envelope de resposta paginada. Acompanha a coleção com os metadados de
/// paginação, permitindo ao cliente navegar entre páginas.
/// </summary>
public class PaginacaoViewModel<T>
{
    public int Pagina { get; set; }
    public int Tamanho { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas => Tamanho > 0 ? (int)Math.Ceiling((double)TotalItens / Tamanho) : 0;
    public IEnumerable<T> Itens { get; set; } = new List<T>();
}
