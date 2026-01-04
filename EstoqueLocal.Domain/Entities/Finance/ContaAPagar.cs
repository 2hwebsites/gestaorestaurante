namespace EstoqueLocal.Domain.Entities.Finance;

public class ContaAPagar
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public int CategoriaFinanceiraId { get; set; }
    public CategoriaFinanceira CategoriaFinanceira { get; set; } = null!;
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public StatusContaPagar Status { get; set; }
    public DateTime? DataPagamento { get; set; }
    public FormaPagamento? FormaPagamento { get; set; }
    public AlocacaoCusto AlocacaoCusto { get; set; }
    public string? Observacao { get; set; }
    public int? LancamentoCaixaId { get; set; }
}
