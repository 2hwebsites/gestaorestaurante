namespace EstoqueLocal.Domain.Entities;

public class EntradaEstoque
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public decimal Quantidade { get; set; }
    public decimal ValorTotal { get; set; }
    public string Fornecedor { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
