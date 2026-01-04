using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLocal.Web.Controllers;

public class ConferenciasController : Controller
{
    private readonly IConferenciaService _conferenciaService;

    public ConferenciasController(IConferenciaService conferenciaService)
    {
        _conferenciaService = conferenciaService;
    }

    public async Task<IActionResult> Index()
    {
        var conferencias = await _conferenciaService.ListarAsync();
        return View(conferencias);
    }

    public async Task<IActionResult> Nova()
    {
        var vm = await _conferenciaService.PrepararNovaAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nova(ConferenciaNovaViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _conferenciaService.CriarAsync(vm);
        TempData["Success"] = "Conferência salva.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var vm = await _conferenciaService.ObterParaEdicaoAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ConferenciaNovaViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _conferenciaService.AtualizarAsync(vm);
        TempData["Success"] = "Conferência atualizada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var vm = await _conferenciaService.ObterParaEdicaoAsync(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _conferenciaService.RemoverAsync(id);
        TempData["Success"] = "Conferência removida.";
        return RedirectToAction(nameof(Index));
    }
}
