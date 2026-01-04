namespace EstoqueLocal.Domain.Entities.Finance;

public class CompraItem
{
    public int Id { get; set; }
    public int CompraId { get; set; }
    public Compra Compra { get; set; } = null!;
    public string DescricaoItem { get; set; } = string.Empty;
    public string CategoriaItem { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public string Unidade { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
