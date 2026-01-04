using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities.Finance;

namespace EstoqueLocal.Application.ViewModels.Finance;

public class CategoriaFinanceiraViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;
    public TipoLancamento? TipoPadrao { get; set; }
    public bool Ativo { get; set; } = true;
}
