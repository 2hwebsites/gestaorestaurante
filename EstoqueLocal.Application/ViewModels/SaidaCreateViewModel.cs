using System.ComponentModel.DataAnnotations;

namespace EstoqueLocal.Application.ViewModels;

public class SaidaCreateViewModel
{
    public int Id { get; set; }

    [Required]
    public int ItemId { get; set; }

    [Range(0.0001, double.MaxValue)]
    public decimal Quantidade { get; set; }

    public string Motivo { get; set; } = string.Empty;
}
