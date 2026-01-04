namespace EstoqueLocal.Domain.Entities;

public class ConferenciaLinha
{
    public int Id { get; set; }
    public int ConferenciaId { get; set; }
    public Conferencia Conferencia { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public decimal QuantidadeContada { get; set; }
}
