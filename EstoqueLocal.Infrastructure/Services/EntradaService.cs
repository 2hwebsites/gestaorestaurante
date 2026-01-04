using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure.Services;

public class EntradaService : IEntradaService
{
    private readonly AppDbContext _ctx;
    public EntradaService(AppDbContext ctx) => _ctx = ctx;

    public async Task CriarAsync(EntradaCreateViewModel vm)
    {
        var entrada = new EntradaEstoque
        {
            ItemId = vm.ItemId,
            Quantidade = vm.Quantidade,
            ValorTotal = vm.ValorTotal,
            Fornecedor = vm.Fornecedor,
            CriadoEm = DateTime.Now
        };
        _ctx.Entradas.Add(entrada);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<EntradaEstoque>> ListarAsync() =>
        await _ctx.Entradas.AsNoTracking()
            .Include(e => e.Item)
            .OrderByDescending(e => e.CriadoEm)
            .ToListAsync();

    public async Task<EntradaEstoque?> ObterAsync(int id) =>
        await _ctx.Entradas.Include(e => e.Item).FirstOrDefaultAsync(e => e.Id == id);

    public async Task AtualizarAsync(EntradaCreateViewModel vm)
    {
        var entrada = await _ctx.Entradas.FindAsync(vm.Id);
        if (entrada == null) return;
        entrada.ItemId = vm.ItemId;
        entrada.Quantidade = vm.Quantidade;
        entrada.ValorTotal = vm.ValorTotal;
        entrada.Fornecedor = vm.Fornecedor;
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var entrada = await _ctx.Entradas.FindAsync(id);
        if (entrada == null) return;
        _ctx.Entradas.Remove(entrada);
        await _ctx.SaveChangesAsync();
    }
}
