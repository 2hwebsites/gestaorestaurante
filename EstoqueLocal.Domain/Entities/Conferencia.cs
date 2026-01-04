namespace EstoqueLocal.Domain.Entities;

public class Conferencia
{
    public int Id { get; set; }
    public DateTime CriadoEm { get; set; }
    public ICollection<ConferenciaLinha> Linhas { get; set; } = new List<ConferenciaLinha>();
}
