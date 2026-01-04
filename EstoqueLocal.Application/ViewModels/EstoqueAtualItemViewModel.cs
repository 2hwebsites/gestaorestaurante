using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Application.ViewModels;

public class EstoqueAtualItemViewModel
{
    public int ItemId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal QuantidadeInicial { get; set; }
    public decimal QuantidadeAtual { get; set; }
    public ConsumoMedio ConsumoMedio { get; set; }
    public decimal QuantidadeContagem { get; set; }
    public DateTime? DataUltimaContagem { get; set; }
    public decimal DiferencaContagem => QuantidadeAtual - QuantidadeContagem;
    public string DiferencaExibicao => QuantidadeContagem <= 0 ? "NA" : DiferencaContagem.ToString();
}
