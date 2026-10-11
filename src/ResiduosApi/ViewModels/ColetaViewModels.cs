using System.ComponentModel.DataAnnotations;

namespace ResiduosApi.ViewModels;

/// <summary>Dados para registrar uma coleta.</summary>
public class ColetaCreateViewModel
{
    [Range(1, int.MaxValue, ErrorMessage = "Informe um ponto de coleta válido.")]
    public int PontoColetaId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe um tipo de resíduo válido.")]
    public int TipoResiduoId { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "O peso (kg) deve ser maior que zero.")]
    public decimal PesoKg { get; set; }

    [Required(ErrorMessage = "A data da coleta é obrigatória.")]
    public DateTime DataColeta { get; set; }

    [StringLength(100, ErrorMessage = "O responsável deve ter no máximo 100 caracteres.")]
    public string? Responsavel { get; set; }
}

/// <summary>Saída de uma coleta (com nomes resolvidos do ponto e do tipo).</summary>
public class ColetaViewModel
{
    public int Id { get; set; }
    public int PontoColetaId { get; set; }
    public string? PontoColetaNome { get; set; }
    public int TipoResiduoId { get; set; }
    public string? TipoResiduoNome { get; set; }
    public decimal PesoKg { get; set; }
    public DateTime DataColeta { get; set; }
    public string? Responsavel { get; set; }
}
