using System.ComponentModel.DataAnnotations.Schema;

namespace EstoqueLocal.Domain.Entities.Finance;

public class LancamentoCaixa
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public TipoLancamento Tipo { get; set; }
    public OrigemVenda? OrigemVenda { get; set; }
    public int CategoriaFinanceiraId { get; set; }
    public CategoriaFinanceira CategoriaFinanceira { get; set; } = null!;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public string? Observacao { get; set; }
    public int Ano { get; set; }
    public int Mes { get; set; }
}
