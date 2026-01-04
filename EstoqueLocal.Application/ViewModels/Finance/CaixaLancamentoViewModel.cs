using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities.Finance;

namespace EstoqueLocal.Application.ViewModels.Finance;

public class CaixaLancamentoViewModel
{
    public int Id { get; set; }

    [Required]
    public TipoLancamento Tipo { get; set; }

    public OrigemVenda? OrigemVenda { get; set; }

    [Required]
    public int CategoriaFinanceiraId { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [Required]
    public FormaPagamento FormaPagamento { get; set; }

    public string? Observacao { get; set; }
}
