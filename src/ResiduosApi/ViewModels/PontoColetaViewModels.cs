using System.ComponentModel.DataAnnotations;

namespace ResiduosApi.ViewModels;

/// <summary>Dados de criação de um ponto de coleta (validados via Data Annotations).</summary>
public class PontoColetaCreateViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(200, ErrorMessage = "O endereço deve ter no máximo 200 caracteres.")]
    public string Endereco { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "Latitude inválida.")]
    public double? Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude inválida.")]
    public double? Longitude { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "A capacidade (kg) deve ser maior que zero.")]
    public decimal CapacidadeKg { get; set; }
}

/// <summary>Dados de atualização de um ponto de coleta.</summary>
public class PontoColetaUpdateViewModel : PontoColetaCreateViewModel
{
    public bool Ativo { get; set; } = true;
}

/// <summary>Saída de um ponto de coleta.</summary>
public class PontoColetaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public decimal CapacidadeKg { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
}
