using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstoqueLocal.Web.Controllers;

public class SaidasController : Controller
{
    private readonly ISaidaService _saidaService;
    private readonly IItemService _itemService;

    public SaidasController(ISaidaService saidaService, IItemService itemService)
    {
        _saidaService = saidaService;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        await PopularItens();
        ViewBag.Saidas = await _saidaService.ListarAsync();
        return View(new SaidaCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(SaidaCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopularItens();
            ViewBag.Saidas = await _saidaService.ListarAsync();
            return View(vm);
        }
        await _saidaService.CriarAsync(vm);
        TempData["Success"] = "Saída registrada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var saida = await _saidaService.ObterAsync(id);
        if (saida == null) return NotFound();
        await PopularItens();
        var vm = new SaidaCreateViewModel
        {
            Id = saida.Id,
            ItemId = saida.ItemId,
            Quantidade = saida.Quantidade,
            Motivo = saida.Motivo
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(SaidaCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopularItens();
            return View(vm);
        }
        await _saidaService.AtualizarAsync(vm);
        TempData["Success"] = "Saída atualizada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var saida = await _saidaService.ObterAsync(id);
        if (saida == null) return NotFound();
        return View(saida);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _saidaService.RemoverAsync(id);
        TempData["Success"] = "Saída removida.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopularItens()
    {
        var itens = await _itemService.ListarAsync();
        ViewBag.Itens = new SelectList(itens, "Id", "Nome");
    }
}
