using Depom.Application.Branch.Services;
using Depom.Application.Product.Services;
using Depom.Application.Transfer.DTOs;
using Depom.Application.Transfer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Depom.Web.Controllers.Transfer;

[Authorize]
public class TransferController : Controller
{
    private readonly TransferService _transferService;
    private readonly BranchService   _branchService;
    private readonly ProductService  _productService;

    public TransferController(TransferService transferService, BranchService branchService, ProductService productService)
    {
        _transferService = transferService;
        _branchService   = branchService;
        _productService  = productService;
    }

    public async Task<IActionResult> Index()
    {
        var branchIdClaim = User.FindFirstValue("BranchId");
        List<TransferListDto> transfers;

        if (User.IsInRole("SystemAdmin") || branchIdClaim == null)
            transfers = await _transferService.GetAllAsync();
        else
            transfers = await _transferService.GetByBranchAsync(int.Parse(branchIdClaim));

        return View(transfers);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var transfer = await _transferService.GetByIdAsync(id);
        if (transfer == null) return NotFound();
        return View(transfer);
    }

    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Create()
    {
        await LoadDropdownsAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Create(TransferCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDropdownsAsync();
            return View(dto);
        }
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var id = await _transferService.CreateAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Submit(TransferActionDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _transferService.SubmitAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id = dto.TransferId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Approve(TransferActionDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _transferService.ApproveAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id = dto.TransferId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Ship(TransferActionDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _transferService.ShipAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id = dto.TransferId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Receive(TransferActionDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _transferService.ReceiveAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id = dto.TransferId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(TransferActionDto dto)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _transferService.CancelAsync(dto, userId);
        return RedirectToAction(nameof(Detail), new { id = dto.TransferId });
    }

    private async Task LoadDropdownsAsync()
    {
        var branches = await _branchService.GetAllAsync();
        var products = await _productService.GetAllAsync();
        ViewBag.Branches = new SelectList(branches, "Id", "Name");
        ViewBag.Products = new SelectList(products, "Id", "Name");
    }
}
