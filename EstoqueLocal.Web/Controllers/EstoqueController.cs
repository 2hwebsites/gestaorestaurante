using EstoqueLocal.Application.Interfaces;
using EstoqueLocal.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueLocal.Web.Controllers;

public class EstoqueController : Controller
{
    private readonly IEstoqueService _estoqueService;

    public EstoqueController(IEstoqueService estoqueService) => _estoqueService = estoqueService;

    public async Task<IActionResult> Atual()
    {
        var vm = await _estoqueService.ObterEstoqueAtualAsync();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtualizarInicial(int itemId, decimal quantidadeInicial)
    {
        await _estoqueService.AtualizarQuantidadeInicialAsync(itemId, quantidadeInicial);
        TempData["Success"] = "Quantidade inicial atualizada.";
        return RedirectToAction(nameof(Atual));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtualizarConsumo(int itemId, ConsumoMedio consumoMedio)
    {
        await _estoqueService.AtualizarConsumoMedioAsync(itemId, consumoMedio);
        TempData["Success"] = "Consumo médio atualizado.";
        return RedirectToAction(nameof(Atual));
    }
}
