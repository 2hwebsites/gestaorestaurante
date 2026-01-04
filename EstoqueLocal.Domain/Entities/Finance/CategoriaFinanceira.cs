namespace EstoqueLocal.Domain.Entities.Finance;

public class CategoriaFinanceira
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public TipoLancamento? TipoPadrao { get; set; }
    public bool Ativo { get; set; } = true;

    public ICollection<LancamentoCaixa> LancamentosCaixa { get; set; } = new List<LancamentoCaixa>();
    public ICollection<ContaAPagar> ContasAPagar { get; set; } = new List<ContaAPagar>();
    public ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
