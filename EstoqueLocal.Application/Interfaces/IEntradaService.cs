using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Application.Interfaces;

public interface IEntradaService
{
    Task CriarAsync(EntradaCreateViewModel vm);
    Task<List<EntradaEstoque>> ListarAsync();
    Task<EntradaEstoque?> ObterAsync(int id);
    Task AtualizarAsync(EntradaCreateViewModel vm);
    Task RemoverAsync(int id);
}
