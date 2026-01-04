namespace EstoqueLocal.Application.ViewModels;

public class ConferenciaIndexViewModel
{
    public int Id { get; set; }
    public DateTime CriadoEm { get; set; }
    public string Titulo => $"Conferência - {CriadoEm:dd/MM/yyyy} - {CriadoEm:HH:mm}";
}
