namespace EstoqueLocal.Domain.Entities.Finance;

public class ContaAReceber
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
    public StatusContaReceber Status { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public FormaPagamento FormaRecebimento { get; set; }
    public OrigemVenda OrigemVenda { get; set; }
    public string? Observacao { get; set; }
    public int? LancamentoCaixaId { get; set; }
}
