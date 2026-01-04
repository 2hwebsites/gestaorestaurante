using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities.Finance;

namespace EstoqueLocal.Application.ViewModels.Finance;

public class ContaPagarCreateViewModel
{
    public int Id { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public int CategoriaFinanceiraId { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [DataType(DataType.Date)]
    public DateTime Vencimento { get; set; }

    public StatusContaPagar Status { get; set; } = StatusContaPagar.Aberta;
    public DateTime? DataPagamento { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }
    public AlocacaoCusto AlocacaoCusto { get; set; }
    public string? Observacao { get; set; }
}
