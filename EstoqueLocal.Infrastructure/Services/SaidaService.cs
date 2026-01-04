using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure.Services;

public class SaidaService : ISaidaService
{
    private readonly AppDbContext _ctx;
    public SaidaService(AppDbContext ctx) => _ctx = ctx;

    public async Task CriarAsync(SaidaCreateViewModel vm)
    {
        var saida = new SaidaConsumo
        {
            ItemId = vm.ItemId,
            Quantidade = vm.Quantidade,
            Motivo = vm.Motivo,
            CriadoEm = DateTime.Now
        };
        _ctx.Saidas.Add(saida);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<SaidaConsumo>> ListarAsync() =>
        await _ctx.Saidas.AsNoTracking()
            .Include(s => s.Item)
            .OrderByDescending(s => s.CriadoEm)
            .ToListAsync();

    public async Task<SaidaConsumo?> ObterAsync(int id) =>
        await _ctx.Saidas.Include(s => s.Item).FirstOrDefaultAsync(s => s.Id == id);

    public async Task AtualizarAsync(SaidaCreateViewModel vm)
    {
        var saida = await _ctx.Saidas.FindAsync(vm.Id);
        if (saida == null) return;
        saida.ItemId = vm.ItemId;
        saida.Quantidade = vm.Quantidade;
        saida.Motivo = vm.Motivo;
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var saida = await _ctx.Saidas.FindAsync(id);
        if (saida == null) return;
        _ctx.Saidas.Remove(saida);
        await _ctx.SaveChangesAsync();
    }
}
