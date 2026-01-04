using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities.Finance;

namespace EstoqueLocal.Application.ViewModels.Finance;

public class CompraCreateViewModel
{
    public int Id { get; set; }

    [Required]
    public string Fornecedor { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal ValorTotal { get; set; }

    [Required]
    public FormaPagamento FormaPagamento { get; set; }

    [Required]
    public AlocacaoCusto AlocacaoCusto { get; set; }

    [Required]
    public int CategoriaFinanceiraId { get; set; }

    public string? Observacao { get; set; }
}
