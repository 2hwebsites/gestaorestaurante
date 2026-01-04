namespace EstoqueLocal.Domain.Entities;

public class SaidaConsumo
{
    public int Id { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public decimal Quantidade { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; }
}
