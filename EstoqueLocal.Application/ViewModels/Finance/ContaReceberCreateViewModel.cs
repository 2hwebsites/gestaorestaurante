using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities.Finance;

namespace EstoqueLocal.Application.ViewModels.Finance;

public class ContaReceberCreateViewModel
{
    public int Id { get; set; }

    [Required]
    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }

    [DataType(DataType.Date)]
    public DateTime Vencimento { get; set; }

    public StatusContaReceber Status { get; set; } = StatusContaReceber.Aberta;
    public DateTime? DataRecebimento { get; set; }
    public FormaPagamento FormaRecebimento { get; set; }
    public OrigemVenda OrigemVenda { get; set; }
    public string? Observacao { get; set; }
}
