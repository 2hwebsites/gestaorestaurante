using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _ctx;
    public ItemService(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<ItemViewModel>> ListarAsync() =>
        await _ctx.Itens.AsNoTracking()
            .OrderBy(i => i.Nome)
            .Select(i => new ItemViewModel
            {
                Id = i.Id,
                Nome = i.Nome,
                Categoria = i.Categoria,
                Unidade = i.Unidade,
                QuantidadeInicial = i.QuantidadeInicial,
                ConsumoMedio = i.ConsumoMedio,
                Ativo = i.Ativo
            }).ToListAsync();

    public async Task<ItemViewModel?> ObterAsync(int id) =>
        await _ctx.Itens.AsNoTracking().Where(i => i.Id == id)
            .Select(i => new ItemViewModel
            {
                Id = i.Id,
                Nome = i.Nome,
                Categoria = i.Categoria,
                Unidade = i.Unidade,
                QuantidadeInicial = i.QuantidadeInicial,
                ConsumoMedio = i.ConsumoMedio,
                Ativo = i.Ativo
            }).FirstOrDefaultAsync();

    public async Task CriarAsync(ItemViewModel vm)
    {
        var entity = new Item
        {
            Nome = vm.Nome,
            Categoria = vm.Categoria,
            Unidade = vm.Unidade,
            QuantidadeInicial = vm.QuantidadeInicial,
            ConsumoMedio = vm.ConsumoMedio,
            Ativo = vm.Ativo
        };
        _ctx.Itens.Add(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task AtualizarAsync(ItemViewModel vm)
    {
        var entity = await _ctx.Itens.FindAsync(vm.Id);
        if (entity is null) return;

        entity.Nome = vm.Nome;
        entity.Categoria = vm.Categoria;
        entity.Unidade = vm.Unidade;
        entity.QuantidadeInicial = vm.QuantidadeInicial;
        entity.ConsumoMedio = vm.ConsumoMedio;
        entity.Ativo = vm.Ativo;
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var entity = await _ctx.Itens.FindAsync(id);
        if (entity is null) return;
        _ctx.Itens.Remove(entity);
        await _ctx.SaveChangesAsync();
    }
}
