using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLocal.Web.Controllers;

public class ItensController : Controller
{
    private readonly IItemService _itemService;

    public ItensController(IItemService itemService) => _itemService = itemService;

    public async Task<IActionResult> Index()
    {
        var itens = await _itemService.ListarAsync();
        return View(itens);
    }

    public IActionResult Create() => View(new ItemViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ItemViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _itemService.CriarAsync(vm);
        TempData["Success"] = "Item criado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _itemService.ObterAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ItemViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _itemService.AtualizarAsync(vm);
        TempData["Success"] = "Item atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var vm = await _itemService.ObterAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _itemService.RemoverAsync(id);
        TempData["Success"] = "Item removido.";
        return RedirectToAction(nameof(Index));
    }
}
