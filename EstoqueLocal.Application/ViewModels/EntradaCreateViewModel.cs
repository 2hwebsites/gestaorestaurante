using System.ComponentModel.DataAnnotations;

namespace EstoqueLocal.Application.ViewModels;

public class EntradaCreateViewModel
{
    public int Id { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Range(0.0001, double.MaxValue)]
    public decimal Quantidade { get; set; }

    [Range(0, double.MaxValue)]
    public decimal ValorTotal { get; set; }

    public string Fornecedor { get; set; } = string.Empty;
}
