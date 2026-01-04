using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure.Services;

public class EstoqueService : IEstoqueService
{
    private readonly AppDbContext _ctx;
    public EstoqueService(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<EstoqueAtualItemViewModel>> ObterEstoqueAtualAsync()
    {
        var entradas = await _ctx.Entradas
            .GroupBy(e => e.ItemId)
            .Select(g => new { ItemId = g.Key, Quantidade = g.Sum(x => x.Quantidade) })
            .ToListAsync();

        var saidas = await _ctx.Saidas
            .GroupBy(s => s.ItemId)
            .Select(g => new { ItemId = g.Key, Quantidade = g.Sum(x => x.Quantidade) })
            .ToListAsync();

        var ultimaContagem = await _ctx.ConferenciaLinhas
            .Include(l => l.Conferencia)
            .GroupBy(l => l.ItemId)
            .Select(g => g.OrderByDescending(x => x.Conferencia.CriadoEm).FirstOrDefault()!)
            .ToListAsync();

        var itens = await _ctx.Itens.AsNoTracking()
            .Where(i => i.Ativo)
            .OrderBy(i => i.Nome)
            .ToListAsync();

        var result = new List<EstoqueAtualItemViewModel>();
        foreach (var item in itens)
        {
            var ent = entradas.FirstOrDefault(x => x.ItemId == item.Id)?.Quantidade ?? 0m;
            var sai = saidas.FirstOrDefault(x => x.ItemId == item.Id)?.Quantidade ?? 0m;
            var atual = item.QuantidadeInicial + ent - sai;
            var ultima = ultimaContagem.FirstOrDefault(x => x.ItemId == item.Id);
            var contagem = ultima?.QuantidadeContada ?? 0m;

            result.Add(new EstoqueAtualItemViewModel
            {
                ItemId = item.Id,
                Nome = item.Nome,
                Categoria = item.Categoria,
                Unidade = item.Unidade,
                QuantidadeInicial = item.QuantidadeInicial,
                QuantidadeAtual = atual,
                ConsumoMedio = item.ConsumoMedio,
                QuantidadeContagem = contagem,
                DataUltimaContagem = ultima?.Conferencia.CriadoEm
            });
        }

        return result;
    }

    public async Task AtualizarQuantidadeInicialAsync(int itemId, decimal quantidade)
    {
        var item = await _ctx.Itens.FindAsync(itemId);
        if (item == null) return;
        item.QuantidadeInicial = quantidade;
        await _ctx.SaveChangesAsync();
    }

    public async Task AtualizarConsumoMedioAsync(int itemId, ConsumoMedio consumo)
    {
        var item = await _ctx.Itens.FindAsync(itemId);
        if (item == null) return;
        item.ConsumoMedio = consumo;
        await _ctx.SaveChangesAsync();
    }
}
