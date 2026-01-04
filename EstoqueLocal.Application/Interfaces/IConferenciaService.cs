using EstoqueLocal.Application.ViewModels;

namespace EstoqueLocal.Application.Interfaces;

public interface IConferenciaService
{
    Task<List<ConferenciaIndexViewModel>> ListarAsync();
    Task<ConferenciaNovaViewModel> PrepararNovaAsync();
    Task CriarAsync(ConferenciaNovaViewModel vm);
    Task<ConferenciaNovaViewModel?> ObterParaEdicaoAsync(int id);
    Task AtualizarAsync(ConferenciaNovaViewModel vm);
    Task RemoverAsync(int id);
}
