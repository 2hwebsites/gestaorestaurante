using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EstoqueLocal.Web.Controllers;

public class EntradasController : Controller
{
    private readonly IEntradaService _entradaService;
    private readonly IItemService _itemService;

    public EntradasController(IEntradaService entradaService, IItemService itemService)
    {
        _entradaService = entradaService;
        _itemService = itemService;
    }

    public async Task<IActionResult> Index()
    {
        await PopularItens();
        ViewBag.Entradas = await _entradaService.ListarAsync();
        return View(new EntradaCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(EntradaCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopularItens();
            ViewBag.Entradas = await _entradaService.ListarAsync();
            return View(vm);
        }
        await _entradaService.CriarAsync(vm);
        TempData["Success"] = "Entrada registrada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entrada = await _entradaService.ObterAsync(id);
        if (entrada == null) return NotFound();
        await PopularItens();
        var vm = new EntradaCreateViewModel
        {
            Id = entrada.Id,
            ItemId = entrada.ItemId,
            Quantidade = entrada.Quantidade,
            ValorTotal = entrada.ValorTotal,
            Fornecedor = entrada.Fornecedor
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EntradaCreateViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            await PopularItens();
            return View(vm);
        }
        await _entradaService.AtualizarAsync(vm);
        TempData["Success"] = "Entrada atualizada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var entrada = await _entradaService.ObterAsync(id);
        if (entrada == null) return NotFound();
        return View(entrada);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _entradaService.RemoverAsync(id);
        TempData["Success"] = "Entrada removida.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopularItens()
    {
        var itens = await _itemService.ListarAsync();
        ViewBag.Itens = new SelectList(itens, "Id", "Nome");
    }
}
