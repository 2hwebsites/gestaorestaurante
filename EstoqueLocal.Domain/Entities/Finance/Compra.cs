namespace EstoqueLocal.Domain.Entities.Finance;

public class Compra
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Fornecedor { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public string? Observacao { get; set; }
    public AlocacaoCusto AlocacaoCusto { get; set; }
    public int CategoriaFinanceiraId { get; set; }
    public CategoriaFinanceira CategoriaFinanceira { get; set; } = null!;
    public ICollection<CompraItem> Itens { get; set; } = new List<CompraItem>();
}
