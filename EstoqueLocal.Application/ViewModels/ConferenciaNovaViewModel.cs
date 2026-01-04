using System.ComponentModel.DataAnnotations;

namespace EstoqueLocal.Application.ViewModels;

public class ConferenciaNovaViewModel
{
    public int? ConferenciaId { get; set; }

    [Required]
    public List<ConferenciaLinhaInputViewModel> Linhas { get; set; } = new();
}
