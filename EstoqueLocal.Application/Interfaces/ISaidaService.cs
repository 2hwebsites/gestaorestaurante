using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;

namespace EstoqueLocal.Application.Interfaces;

public interface ISaidaService
{
    Task CriarAsync(SaidaCreateViewModel vm);
    Task<List<SaidaConsumo>> ListarAsync();
    Task<SaidaConsumo?> ObterAsync(int id);
    Task AtualizarAsync(SaidaCreateViewModel vm);
    Task RemoverAsync(int id);
}
