using System.ComponentModel.DataAnnotations;
using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Application.ViewModels;

public class ItemViewModel
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Unidade { get; set; } = string.Empty;
    public decimal QuantidadeInicial { get; set; }
    public ConsumoMedio ConsumoMedio { get; set; }
    public bool Ativo { get; set; } = true;
}
