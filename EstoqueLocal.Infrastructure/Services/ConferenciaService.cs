using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using EstoqueLocal.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Infrastructure.Services;

public class ConferenciaService : IConferenciaService
{
    private readonly AppDbContext _ctx;
    private readonly IEstoqueService _estoqueService;

    public ConferenciaService(AppDbContext ctx, IEstoqueService estoqueService)
    {
        _ctx = ctx;
        _estoqueService = estoqueService;
    }

    public async Task<List<ConferenciaIndexViewModel>> ListarAsync() =>
        await _ctx.Conferencias.AsNoTracking()
            .OrderByDescending(c => c.CriadoEm)
            .Select(c => new ConferenciaIndexViewModel
            {
                Id = c.Id,
                CriadoEm = c.CriadoEm
            }).ToListAsync();

    public async Task<ConferenciaNovaViewModel> PrepararNovaAsync()
    {
        var estoque = await _estoqueService.ObterEstoqueAtualAsync();
        return new ConferenciaNovaViewModel
        {
            Linhas = [.. estoque.Select(e => new ConferenciaLinhaInputViewModel
            {
                ItemId = e.ItemId,
                Nome = e.Nome,
                Categoria = e.Categoria,
                Unidade = e.Unidade,
                QuantidadeAtual = e.QuantidadeAtual,
                QuantidadeContada = 0
            })]
        };
    }

    public async Task CriarAsync(ConferenciaNovaViewModel vm)
    {
        var conf = new Conferencia
        {
            CriadoEm = DateTime.Now,
            Linhas = [.. vm.Linhas.Select(l => new ConferenciaLinha
            {
                ItemId = l.ItemId,
                QuantidadeContada = l.QuantidadeContada
            })]
        };

        _ctx.Conferencias.Add(conf);
        await _ctx.SaveChangesAsync();
    }

    public async Task<ConferenciaNovaViewModel?> ObterParaEdicaoAsync(int id)
    {
        var conf = await _ctx.Conferencias
            .Include(c => c.Linhas)
            .ThenInclude(l => l.Item)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (conf == null) return null;

        // Usa estoque atual para mostrar QuantidadeAtual atualizada na edição
        var estoque = await _estoqueService.ObterEstoqueAtualAsync();
        var lookup = estoque.ToDictionary(e => e.ItemId, e => e.QuantidadeAtual);

        return new ConferenciaNovaViewModel
        {
            ConferenciaId = conf.Id,
            Linhas = [.. conf.Linhas.Select(l => new ConferenciaLinhaInputViewModel
            {
                ItemId = l.ItemId,
                Nome = l.Item.Nome,
                Categoria = l.Item.Categoria,
                Unidade = l.Item.Unidade,
                QuantidadeAtual = lookup.TryGetValue(l.ItemId, out var qa) ? qa : 0,
                QuantidadeContada = l.QuantidadeContada
            }).OrderBy(x => x.Nome)]
        };
    }

    public async Task AtualizarAsync(ConferenciaNovaViewModel vm)
    {
        if (vm.ConferenciaId is null) return;
        var conf = await _ctx.Conferencias
            .Include(c => c.Linhas)
            .FirstOrDefaultAsync(c => c.Id == vm.ConferenciaId.Value);
        if (conf == null) return;

        foreach (var linhaVm in vm.Linhas)
        {
            var linha = conf.Linhas.FirstOrDefault(l => l.ItemId == linhaVm.ItemId);
            if (linha != null)
            {
                linha.QuantidadeContada = linhaVm.QuantidadeContada;
            }
        }
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoverAsync(int id)
    {
        var conf = await _ctx.Conferencias.FindAsync(id);
        if (conf == null) return;
        _ctx.Conferencias.Remove(conf);
        await _ctx.SaveChangesAsync();
    }
}
