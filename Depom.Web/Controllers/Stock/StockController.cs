using Depom.Application.Branch.Services;
using Depom.Application.Product.Services;
using Depom.Application.Stock.DTOs;
using Depom.Application.Stock.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Depom.Web.Controllers.Stock;

[Authorize]
public class StockController : Controller
{
    private readonly StockService   _stockService;
    private readonly BranchService  _branchService;
    private readonly ProductService _productService;

    public StockController(StockService stockService, BranchService branchService, ProductService productService)
    {
        _stockService   = stockService;
        _branchService  = branchService;
        _productService = productService;
    }

    public async Task<IActionResult> Index(int? branchId)
    {
        var branches = await _branchService.GetAllAsync();
        ViewBag.Branches  = new SelectList(branches, "Id", "Name", branchId);
        ViewBag.BranchId  = branchId;

        if (branchId == null) return View(new List<StockItemDto>());

        var items = await _stockService.GetByBranchAsync(branchId.Value);
        return View(items);
    }

    [Authorize(Roles = "SystemAdmin,BranchManager,StockClerk")]
    public async Task<IActionResult> Movement(int? branchId)
    {
        await LoadDropdownsAsync(branchId);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager,StockClerk")]
    public async Task<IActionResult> Movement(StockMovementCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync(dto.BranchId);
            return View(dto);
        }
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _stockService.AddMovementAsync(dto, userId);
        return RedirectToAction(nameof(Index), new { branchId = dto.BranchId });
    }

    [Authorize(Roles = "SystemAdmin,BranchManager,StockClerk")]
    public async Task<IActionResult> Adjust(int? branchId)
    {
        await LoadDropdownsAsync(branchId);
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager,StockClerk")]
    public async Task<IActionResult> Adjust(StockAdjustDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _stockService.AdjustAsync(dto, userId);
        return RedirectToAction(nameof(Index), new { branchId = dto.BranchId });
    }

    public async Task<IActionResult> Movements(int branchId, int? productId)
    {
        var movements = await _stockService.GetMovementsAsync(branchId, productId);
        var branches  = await _branchService.GetAllAsync();
        ViewBag.Branches  = new SelectList(branches, "Id", "Name", branchId);
        ViewBag.BranchId  = branchId;
        return View(movements);
    }

    private async Task LoadDropdownsAsync(int? branchId = null)
    {
        var branches = await _branchService.GetAllAsync();
        var products = await _productService.GetAllAsync();
        ViewBag.Branches = new SelectList(branches, "Id", "Name", branchId);
        ViewBag.Products = new SelectList(products, "Id", "Name");
    }
}
