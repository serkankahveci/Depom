using Depom.Application.Branch.DTOs;
using Depom.Application.Branch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Depom.Web.Controllers.Branch;

[Authorize]
public class BranchController : Controller
{
    private readonly BranchService _branchService;

    public BranchController(BranchService branchService)
    {
        _branchService = branchService;
    }

    public async Task<IActionResult> Index()
    {
        var branches = await _branchService.GetAllAsync();
        return View(branches);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var branch = await _branchService.GetByIdAsync(id);
        if (branch == null) return NotFound();
        return View(branch);
    }

    [Authorize(Roles = "SystemAdmin")]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> Create(BranchCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _branchService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var branch = await _branchService.GetByIdAsync(id);
        if (branch == null) return NotFound();
        var dto = new BranchUpdateDto
        {
            Id      = branch.Id,
            Name    = branch.Name,
            Address = branch.Address,
            Phone   = branch.Phone
        };
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> Edit(BranchUpdateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _branchService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _branchService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
