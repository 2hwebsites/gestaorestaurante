using EstoqueLocal.Application.ViewModels.Finance;
using EstoqueLocal.Domain.Entities.Finance;
using EstoqueLocal.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EstoqueLocal.Web.Controllers;

public class FinanceiroController : Controller
{
    private readonly AppDbContext _ctx;
    public FinanceiroController(AppDbContext ctx) => _ctx = ctx;

    public async Task<IActionResult> Caixa()
    {
        await PrepararDadosCaixa();
        return View(new CaixaLancamentoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Caixa(CaixaLancamentoViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PrepararDadosCaixa(vm.CategoriaFinanceiraId);
            return View(vm);
        }

        var now = DateTime.Now;
        var entity = new LancamentoCaixa
        {
            DataHora = now,
            Ano = now.Year,
            Mes = now.Month,
            Tipo = vm.Tipo,
            OrigemVenda = vm.Tipo == TipoLancamento.Entrada ? vm.OrigemVenda : null,
            CategoriaFinanceiraId = vm.CategoriaFinanceiraId,
            Descricao = vm.Descricao,
            Valor = vm.Valor,
            FormaPagamento = vm.FormaPagamento,
            Observacao = vm.Observacao
        };
        _ctx.LancamentosCaixa.Add(entity);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Lançamento registrado.";
        return RedirectToAction(nameof(Caixa));
    }

    private async Task PopularViewBags(int? categoriaSelecionada = null)
    {
        var categorias = await _ctx.CategoriasFinanceiras
            .Where(c => c.Ativo)
            .OrderBy(c => c.Nome)
            .ToListAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", categoriaSelecionada);
    }

    private async Task PrepararDadosCaixa(int? categoriaSelecionada = null)
    {
        await PopularViewBags(categoriaSelecionada);
        var (list, cards) = await ObterCaixaDoMes();
        ViewBag.Lancamentos = list;
        ViewBag.Cards = cards;
    }

    private async Task<(List<LancamentoCaixa> list, (decimal entrada, decimal saida, decimal saldo) cards)> ObterCaixaDoMes()
    {
        var now = DateTime.Now;
        var lanc = await _ctx.LancamentosCaixa
            .Include(l => l.CategoriaFinanceira)
            .Where(l => l.Ano == now.Year && l.Mes == now.Month)
            .OrderByDescending(l => l.DataHora)
            .ToListAsync();
        var entrada = lanc.Where(l => l.Tipo == TipoLancamento.Entrada).Sum(l => l.Valor);
        var saida = lanc.Where(l => l.Tipo == TipoLancamento.Saida).Sum(l => l.Valor);
        return (lanc, (entrada, saida, entrada - saida));
    }

    public async Task<IActionResult> Categorias()
    {
        var list = await _ctx.CategoriasFinanceiras.AsNoTracking().OrderBy(c => c.Nome).ToListAsync();
        return View(list);
    }

    public IActionResult CriarCategoria() => View("CategoriaForm", new CategoriaFinanceiraViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarCategoria(CategoriaFinanceiraViewModel vm)
    {
        if (!ModelState.IsValid) return View("CategoriaForm", vm);
        var cat = new CategoriaFinanceira
        {
            Nome = vm.Nome,
            TipoPadrao = vm.TipoPadrao,
            Ativo = vm.Ativo
        };
        _ctx.CategoriasFinanceiras.Add(cat);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Categoria criada.";
        return RedirectToAction(nameof(Categorias));
    }

    public async Task<IActionResult> EditarCategoria(int id)
    {
        var cat = await _ctx.CategoriasFinanceiras.FindAsync(id);
        if (cat == null) return NotFound();
        var vm = new CategoriaFinanceiraViewModel
        {
            Id = cat.Id,
            Nome = cat.Nome,
            TipoPadrao = cat.TipoPadrao,
            Ativo = cat.Ativo
        };
        return View("CategoriaForm", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarCategoria(CategoriaFinanceiraViewModel vm)
    {
        if (!ModelState.IsValid) return View("CategoriaForm", vm);
        var cat = await _ctx.CategoriasFinanceiras.FindAsync(vm.Id);
        if (cat == null) return NotFound();
        cat.Nome = vm.Nome;
        cat.TipoPadrao = vm.TipoPadrao;
        cat.Ativo = vm.Ativo;
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Categoria atualizada.";
        return RedirectToAction(nameof(Categorias));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DesativarCategoria(int id)
    {
        var cat = await _ctx.CategoriasFinanceiras.FindAsync(id);
        if (cat != null)
        {
            cat.Ativo = false;
            await _ctx.SaveChangesAsync();
            TempData["Success"] = "Categoria desativada.";
        }
        return RedirectToAction(nameof(Categorias));
    }

    public IActionResult ContasAPagar() => View();
    public async Task<IActionResult> ContasAPagar(int? mes, int? ano, StatusContaPagar? status, AlocacaoCusto? alocacao)
    {
        await PopularViewBags();
        var filtroMes = mes ?? DateTime.Now.Month;
        var filtroAno = ano ?? DateTime.Now.Year;

        var query = _ctx.ContasAPagar
            .Include(c => c.CategoriaFinanceira)
            .Where(c => c.Vencimento.Month == filtroMes && c.Vencimento.Year == filtroAno);

        var today = DateTime.Today;

        if (status.HasValue)
        {
            if (status.Value == StatusContaPagar.Vencida)
            {
                query = query.Where(c => c.Status == StatusContaPagar.Aberta && c.Vencimento < today);
            }
            else
            {
                query = query.Where(c => c.Status == status.Value);
            }
        }

        if (alocacao.HasValue)
        {
            query = query.Where(c => c.AlocacaoCusto == alocacao.Value);
        }

        var list = await query
            .OrderByDescending(c => c.Vencimento)
            .Select(c => new
            {
                c.Id,
                c.Descricao,
                c.Valor,
                c.Vencimento,
                c.Status,
                c.AlocacaoCusto,
                Categoria = c.CategoriaFinanceira.Nome,
                c.DataPagamento,
                c.FormaPagamento
            }).ToListAsync();

        var display = list.Select(c =>
        {
            var statusExibicao = c.Status;
            if (c.Status == StatusContaPagar.Aberta && c.Vencimento.Date < today)
            {
                statusExibicao = StatusContaPagar.Vencida;
            }
            var podeEditar = c.Status != StatusContaPagar.Paga;
            return new
            {
                c.Id,
                c.Descricao,
                c.Valor,
                c.Vencimento,
                Status = statusExibicao,
                c.AlocacaoCusto,
                c.Categoria,
                c.DataPagamento,
                c.FormaPagamento,
                PodeEditar = podeEditar
            };
        }).ToList();

        var totalAberto = display.Where(c => c.Status == StatusContaPagar.Aberta || c.Status == StatusContaPagar.Vencida).Sum(c => c.Valor);
        var totalPago = display.Where(c => c.Status == StatusContaPagar.Paga).Sum(c => c.Valor);

        ViewBag.ItensAPagar = display;
        ViewBag.FiltroMes = filtroMes;
        ViewBag.FiltroAno = filtroAno;
        ViewBag.FiltroStatus = status;
        ViewBag.FiltroAlocacao = alocacao;
        ViewBag.TotalAberto = totalAberto;
        ViewBag.TotalPago = totalPago;

        return View(new ContaPagarCreateViewModel { Vencimento = new DateTime(filtroAno, filtroMes, DateTime.DaysInMonth(filtroAno, filtroMes)) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContasAPagar(ContaPagarCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopularViewBags();
            return await ContasAPagar(vm.Vencimento.Month, vm.Vencimento.Year, null, null);
        }

        var entity = new ContaAPagar
        {
            Descricao = vm.Descricao,
            CategoriaFinanceiraId = vm.CategoriaFinanceiraId,
            Valor = vm.Valor,
            Vencimento = vm.Vencimento,
            Status = StatusContaPagar.Aberta,
            AlocacaoCusto = vm.AlocacaoCusto,
            Observacao = vm.Observacao
        };
        _ctx.ContasAPagar.Add(entity);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Conta a pagar criada.";
        return RedirectToAction(nameof(ContasAPagar));
    }

    public async Task<IActionResult> EditarContaAPagar(int id)
    {
        var c = await _ctx.ContasAPagar.FindAsync(id);
        if (c == null || c.Status == StatusContaPagar.Paga) return RedirectToAction(nameof(ContasAPagar));
        var vm = new ContaPagarCreateViewModel
        {
            Id = c.Id,
            Descricao = c.Descricao,
            CategoriaFinanceiraId = c.CategoriaFinanceiraId,
            Valor = c.Valor,
            Vencimento = c.Vencimento,
            Status = c.Status,
            DataPagamento = c.DataPagamento,
            FormaPagamento = c.FormaPagamento,
            AlocacaoCusto = c.AlocacaoCusto,
            Observacao = c.Observacao
        };
        await PopularViewBags();
        return View("ContaPagarForm", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarContaAPagar(ContaPagarCreateViewModel vm)
    {
        var c = await _ctx.ContasAPagar.FindAsync(vm.Id);
        if (c == null) return RedirectToAction(nameof(ContasAPagar));
        if (c.Status == StatusContaPagar.Paga)
        {
            TempData["Success"] = "Conta já paga, não pode editar.";
            return RedirectToAction(nameof(ContasAPagar));
        }
        if (!ModelState.IsValid)
        {
            await PopularViewBags();
            return View("ContaPagarForm", vm);
        }
        c.Descricao = vm.Descricao;
        c.CategoriaFinanceiraId = vm.CategoriaFinanceiraId;
        c.Valor = vm.Valor;
        c.Vencimento = vm.Vencimento;
        c.AlocacaoCusto = vm.AlocacaoCusto;
        c.Observacao = vm.Observacao;
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Conta atualizada.";
        return RedirectToAction(nameof(ContasAPagar));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarContaPaga(int id, FormaPagamento? forma, bool gerarCaixa = false)
    {
        var c = await _ctx.ContasAPagar.FindAsync(id);
        if (c != null && c.Status != StatusContaPagar.Paga)
        {
            c.Status = StatusContaPagar.Paga;
            c.DataPagamento = DateTime.Now;
            if (forma.HasValue)
                c.FormaPagamento = forma.Value;

            if (gerarCaixa && c.LancamentoCaixaId == null && c.FormaPagamento.HasValue)
            {
                var now = DateTime.Now;
                var lanc = new LancamentoCaixa
                {
                    DataHora = now,
                    Ano = now.Year,
                    Mes = now.Month,
                    Tipo = TipoLancamento.Saida,
                    CategoriaFinanceiraId = c.CategoriaFinanceiraId,
                    Descricao = $"Pgto: {c.Descricao}",
                    Valor = c.Valor,
                    FormaPagamento = c.FormaPagamento.Value,
                    Observacao = c.Observacao
                };
                _ctx.LancamentosCaixa.Add(lanc);
                await _ctx.SaveChangesAsync();
                c.LancamentoCaixaId = lanc.Id;
            }
            await _ctx.SaveChangesAsync();
            TempData["Success"] = "Conta marcada como paga.";
        }
        return RedirectToAction(nameof(ContasAPagar));
    }

    public async Task<IActionResult> ContasAReceber(int? mes, int? ano, StatusContaReceber? status, OrigemVenda? origem)
    {
        var filtroMes = mes ?? DateTime.Now.Month;
        var filtroAno = ano ?? DateTime.Now.Year;
        var today = DateTime.Today;

        var query = _ctx.ContasAReceber
            .Where(c => c.Vencimento.Month == filtroMes && c.Vencimento.Year == filtroAno);

        if (status.HasValue)
        {
            if (status.Value == StatusContaReceber.Atrasada)
                query = query.Where(c => c.Status == StatusContaReceber.Aberta && c.Vencimento < today);
            else
                query = query.Where(c => c.Status == status.Value);
        }
        if (origem.HasValue)
            query = query.Where(c => c.OrigemVenda == origem.Value);

        var list = await query
            .OrderByDescending(c => c.Vencimento)
            .ToListAsync();

        var display = list.Select(c =>
        {
            var st = c.Status;
            if (c.Status == StatusContaReceber.Aberta && c.Vencimento.Date < today)
                st = StatusContaReceber.Atrasada;
            var podeEditar = c.Status != StatusContaReceber.Recebida;
            return new { c.Id, c.Descricao, c.Valor, c.Vencimento, Status = st, c.OrigemVenda, c.FormaRecebimento, c.DataRecebimento, PodeEditar = podeEditar };
        }).ToList();

        var totalAberto = display.Where(c => c.Status == StatusContaReceber.Aberta || c.Status == StatusContaReceber.Atrasada).Sum(c => c.Valor);
        var totalRecebido = display.Where(c => c.Status == StatusContaReceber.Recebida).Sum(c => c.Valor);

        ViewBag.ItensAReceber = display;
        ViewBag.FiltroMes = filtroMes;
        ViewBag.FiltroAno = filtroAno;
        ViewBag.FiltroStatus = status;
        ViewBag.FiltroOrigem = origem;
        ViewBag.TotalAberto = totalAberto;
        ViewBag.TotalRecebido = totalRecebido;

        return View(new ContaReceberCreateViewModel { Vencimento = new DateTime(filtroAno, filtroMes, DateTime.DaysInMonth(filtroAno, filtroMes)), FormaRecebimento = FormaPagamento.Pix });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContasAReceber(ContaReceberCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            return await ContasAReceber(vm.Vencimento.Month, vm.Vencimento.Year, null, null);
        }
        var c = new ContaAReceber
        {
            Descricao = vm.Descricao,
            Valor = vm.Valor,
            Vencimento = vm.Vencimento,
            Status = StatusContaReceber.Aberta,
            OrigemVenda = vm.OrigemVenda,
            FormaRecebimento = vm.FormaRecebimento,
            Observacao = vm.Observacao
        };
        _ctx.ContasAReceber.Add(c);
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Conta a receber criada.";
        return RedirectToAction(nameof(ContasAReceber));
    }

    public async Task<IActionResult> EditarContaAReceber(int id)
    {
        var c = await _ctx.ContasAReceber.FindAsync(id);
        if (c == null || c.Status == StatusContaReceber.Recebida) return RedirectToAction(nameof(ContasAReceber));
        var vm = new ContaReceberCreateViewModel
        {
            Id = c.Id,
            Descricao = c.Descricao,
            Valor = c.Valor,
            Vencimento = c.Vencimento,
            Status = c.Status,
            DataRecebimento = c.DataRecebimento,
            FormaRecebimento = c.FormaRecebimento,
            OrigemVenda = c.OrigemVenda,
            Observacao = c.Observacao
        };
        return View("ContaReceberForm", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarContaAReceber(ContaReceberCreateViewModel vm)
    {
        var c = await _ctx.ContasAReceber.FindAsync(vm.Id);
        if (c == null) return RedirectToAction(nameof(ContasAReceber));
        if (c.Status == StatusContaReceber.Recebida)
        {
            TempData["Success"] = "Conta já recebida, não pode editar.";
            return RedirectToAction(nameof(ContasAReceber));
        }
        if (!ModelState.IsValid) return View("ContaReceberForm", vm);
        c.Descricao = vm.Descricao;
        c.Valor = vm.Valor;
        c.Vencimento = vm.Vencimento;
        c.OrigemVenda = vm.OrigemVenda;
        c.FormaRecebimento = vm.FormaRecebimento;
        c.Observacao = vm.Observacao;
        await _ctx.SaveChangesAsync();
        TempData["Success"] = "Conta atualizada.";
        return RedirectToAction(nameof(ContasAReceber));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarContaRecebida(int id, FormaPagamento? forma, bool gerarCaixa = false)
    {
        var c = await _ctx.ContasAReceber.FindAsync(id);
        if (c != null && c.Status != StatusContaReceber.Recebida)
        {
            c.Status = StatusContaReceber.Recebida;
            c.DataRecebimento = DateTime.Now;
            if (forma.HasValue)
                c.FormaRecebimento = forma.Value;

            if (gerarCaixa && c.LancamentoCaixaId == null)
            {
                var catEntrada = await _ctx.CategoriasFinanceiras.Where(x => x.Ativo)
                    .OrderByDescending(x => x.TipoPadrao == TipoLancamento.Entrada)
                    .FirstOrDefaultAsync();
                if (catEntrada != null)
                {
                    var now = DateTime.Now;
                    var lanc = new LancamentoCaixa
                    {
                        DataHora = now,
                        Ano = now.Year,
                        Mes = now.Month,
                        Tipo = TipoLancamento.Entrada,
                        OrigemVenda = c.OrigemVenda,
                        CategoriaFinanceiraId = catEntrada.Id,
                        Descricao = $"Receb.: {c.Descricao}",
                        Valor = c.Valor,
                        FormaPagamento = c.FormaRecebimento,
                        Observacao = c.Observacao
                    };
                    _ctx.LancamentosCaixa.Add(lanc);
                    await _ctx.SaveChangesAsync();
                    c.LancamentoCaixaId = lanc.Id;
                }
            }
            await _ctx.SaveChangesAsync();
            TempData["Success"] = "Conta marcada como recebida.";
        }
        return RedirectToAction(nameof(ContasAReceber));
    }

    public async Task<IActionResult> Compras(int? mes, int? ano, AlocacaoCusto? alocacao, string? fornecedor, int? categoriaId)
    {
        await PrepararComprasViewData(mes, ano, alocacao, fornecedor, categoriaId);
        return View(new CompraCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CriarCompra(CompraCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PrepararComprasViewData(null, null, null, null, null);
            return View("Compras", vm);
        }
        var now = DateTime.Now;
        var compra = new Compra
        {
            DataHora = now,
            Fornecedor = vm.Fornecedor,
            ValorTotal = vm.ValorTotal,
            FormaPagamento = vm.FormaPagamento,
            AlocacaoCusto = vm.AlocacaoCusto,
            CategoriaFinanceiraId = vm.CategoriaFinanceiraId,
            Observacao = vm.Observacao
        };
        _ctx.Compras.Add(compra);
        await _ctx.SaveChangesAsync();

        if (vm.GerarLancamentoCaixa)
        {
            var obs = $"Alocação: {vm.AlocacaoCusto}";
            if (!string.IsNullOrWhiteSpace(vm.Observacao))
            {
                obs += $" | {vm.Observacao}";
            }

            var lanc = new LancamentoCaixa
            {
                DataHora = now,
                Ano = now.Year,
                Mes = now.Month,
                Tipo = TipoLancamento.Saida,
                CategoriaFinanceiraId = vm.CategoriaFinanceiraId,
                Descricao = $"Compra: {vm.Fornecedor}",
                Valor = vm.ValorTotal,
                FormaPagamento = vm.FormaPagamento,
                Observacao = obs
            };
            _ctx.LancamentosCaixa.Add(lanc);
            await _ctx.SaveChangesAsync();
            compra.LancamentoCaixaId = lanc.Id;
            await _ctx.SaveChangesAsync();
        }

        TempData["Success"] = "Compra registrada.";
        return RedirectToAction(nameof(Compras));
    }

    private async Task PrepararComprasViewData(int? mes, int? ano, AlocacaoCusto? alocacao, string? fornecedor, int? categoriaId)
    {
        var filtroMes = mes ?? DateTime.Now.Month;
        var filtroAno = ano ?? DateTime.Now.Year;
        var filtroFornecedor = fornecedor?.Trim() ?? string.Empty;

        await PopularViewBags(categoriaId);

        var query = _ctx.Compras
            .Include(c => c.CategoriaFinanceira)
            .Where(c => c.DataHora.Month == filtroMes && c.DataHora.Year == filtroAno);

        if (alocacao.HasValue)
            query = query.Where(c => c.AlocacaoCusto == alocacao.Value);

        if (categoriaId.HasValue)
            query = query.Where(c => c.CategoriaFinanceiraId == categoriaId.Value);

        if (!string.IsNullOrWhiteSpace(filtroFornecedor))
        {
            var term = filtroFornecedor.ToLower();
            query = query.Where(c => c.Fornecedor.ToLower().Contains(term));
        }

        var list = await query.OrderByDescending(c => c.DataHora).ToListAsync();

        var totalBuffet = list.Where(c => c.AlocacaoCusto == AlocacaoCusto.BuffetFeijoada).Sum(c => c.ValorTotal);
        var totalALaCarte = list.Where(c => c.AlocacaoCusto == AlocacaoCusto.ALaCarte).Sum(c => c.ValorTotal);
        var totalBebidas = list.Where(c => c.AlocacaoCusto == AlocacaoCusto.Bebidas).Sum(c => c.ValorTotal);
        var totalGeralAloc = list.Where(c => c.AlocacaoCusto == AlocacaoCusto.Geral).Sum(c => c.ValorTotal);
        var totalAdministrativo = list.Where(c => c.AlocacaoCusto == AlocacaoCusto.Administrativo).Sum(c => c.ValorTotal);
        var totalGeral = list.Sum(c => c.ValorTotal);

        ViewBag.FiltroMes = filtroMes;
        ViewBag.FiltroAno = filtroAno;
        ViewBag.FiltroAlocacao = alocacao;
        ViewBag.FiltroFornecedor = filtroFornecedor;
        ViewBag.FiltroCategoria = categoriaId;

        ViewBag.Compras = list.Select(c => new
        {
            c.Id,
            c.DataHora,
            c.Fornecedor,
            c.ValorTotal,
            c.FormaPagamento,
            c.AlocacaoCusto,
            c.Observacao,
            Categoria = c.CategoriaFinanceira.Nome
        }).ToList();

        ViewBag.Totais = new
        {
            Geral = totalGeral,
            BuffetFeijoada = totalBuffet,
            ALaCarte = totalALaCarte,
            Bebidas = totalBebidas,
            GeralOperacional = totalGeralAloc,
            Administrativo = totalAdministrativo
        };
    }

    public IActionResult ResumoMensal() => View();
}
