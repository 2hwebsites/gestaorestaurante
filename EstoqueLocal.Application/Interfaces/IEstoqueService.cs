using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Application.Interfaces;

public interface IEstoqueService
{
    Task<List<EstoqueAtualItemViewModel>> ObterEstoqueAtualAsync();
    Task AtualizarQuantidadeInicialAsync(int itemId, decimal quantidade);
    Task AtualizarConsumoMedioAsync(int itemId, ConsumoMedio consumo);
}
