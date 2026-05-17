using Depom.Application.Branch.Services;
using Depom.Application.Product.Services;
using Depom.Application.Stock.Services;
using Depom.Application.Transfer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Depom.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly BranchService _branchService;
    private readonly ProductService _productService;
    private readonly StockService _stockService;
    private readonly TransferService _transferService;

    public HomeController(
        BranchService branchService,
        ProductService productService,
        StockService stockService,
        TransferService transferService)
    {
        _branchService = branchService;
        _productService = productService;
        _stockService = stockService;
        _transferService = transferService;
    }

    public async Task<IActionResult> Index()
    {
        var branches = await _branchService.GetAllAsync();
        var products = await _productService.GetAllAsync();
        var transfers = await _transferService.GetAllAsync();

        // Her branch'in stoklarini topla
        var allStocks = new List<Depom.Application.Stock.DTOs.StockItemDto>();
        foreach (var b in branches)
        {
            var s = await _stockService.GetByBranchAsync(b.Id);
            allStocks.AddRange(s);
        }

        ViewBag.BranchCount = branches.Count;
        ViewBag.ProductCount = products.Count;
        ViewBag.StockCount = allStocks.Count;
        ViewBag.ActiveTransferCount = transfers
            .Count(t => t.Status.ToString() is "Pending" or "Approved" or "InTransit");

        ViewBag.RecentTransfers = transfers
            .OrderByDescending(t => t.CreatedAt)
            .Take(6)
            .ToList();

        ViewBag.LowStockItems = allStocks
            .Where(s => s.Quantity <= 5)
            .OrderBy(s => s.Quantity)
            .Take(8)
            .ToList();

        return View();
    }
}