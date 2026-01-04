namespace EstoqueLocal.Domain.Entities;

public enum ConsumoMedio
{
    Baixo = 0,
    Medio = 1,
    Alto = 2
}

public class Item
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal QuantidadeInicial { get; set; }
    public ConsumoMedio ConsumoMedio { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<EntradaEstoque> Entradas { get; set; } = new List<EntradaEstoque>();
    public ICollection<SaidaConsumo> Saidas { get; set; } = new List<SaidaConsumo>();
    public ICollection<ConferenciaLinha> ConferenciaLinhas { get; set; } = new List<ConferenciaLinha>();
}
