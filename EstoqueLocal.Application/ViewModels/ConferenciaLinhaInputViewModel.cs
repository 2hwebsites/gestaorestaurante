namespace EstoqueLocal.Application.ViewModels;

public class ConferenciaLinhaInputViewModel
{
    public int ItemId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal QuantidadeAtual { get; set; }
    public decimal QuantidadeContada { get; set; }
}
