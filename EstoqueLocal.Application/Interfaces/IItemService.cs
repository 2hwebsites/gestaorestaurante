using EstoqueLocal.Application.ViewModels;

namespace EstoqueLocal.Application.Interfaces;

public interface IItemService
{
    Task<List<ItemViewModel>> ListarAsync();
    Task<ItemViewModel?> ObterAsync(int id);
    Task CriarAsync(ItemViewModel vm);
    Task AtualizarAsync(ItemViewModel vm);
    Task RemoverAsync(int id);
}
