using Depom.Application.Branch.Services;
using Depom.Application.User.DTOs;
using Depom.Application.User.Services;
using Depom.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Depom.Web.Controllers.User;

[Authorize(Roles = nameof(AppRole.SystemAdmin))]
public class UserController : Controller
{
    private readonly UserService   _userService;
    private readonly BranchService _branchService;

    public UserController(UserService userService, BranchService branchService)
    {
        _userService   = userService;
        _branchService = branchService;
    }

    // GET /User
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    // GET /User/Detail/5
    public async Task<IActionResult> Detail(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    // GET /User/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View();
    }

    // POST /User/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync();
            return View(dto);
        }

        var error = await _userService.CreateAsync(dto);
        if (error != null)
        {
            ModelState.AddModelError(nameof(dto.Username), error);
            await PopulateDropdownsAsync();
            return View(dto);
        }

        TempData["Success"] = "KullanÄ±cÄ± baÅŸarÄ±yla oluÅŸturuldu.";
        return RedirectToAction(nameof(Index));
    }

    // GET /User/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var dto = new UserUpdateDto
        {
            Id       = user.Id,
            FullName = user.FullName,
            Username = user.Username,
            Role     = user.Role,
            BranchId = user.BranchId,
            IsActive = user.IsActive
        };

        await PopulateDropdownsAsync();
        return View(dto);
    }

    // POST /User/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest();

        if (!ModelState.IsValid)
        {
            await PopulateDropdownsAsync();
            return View(dto);
        }

        var error = await _userService.UpdateAsync(dto);
        if (error != null)
        {
            ModelState.AddModelError(string.Empty, error);
            await PopulateDropdownsAsync();
            return View(dto);
        }

        TempData["Success"] = "KullanÄ±cÄ± gÃ¼ncellendi.";
        return RedirectToAction(nameof(Index));
    }

    // POST /User/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var currentUserId = int.Parse(
            User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

        if (id == currentUserId)
        {
            TempData["Error"] = "Kendi hesabÄ±nÄ±zÄ± silemezsiniz.";
            return RedirectToAction(nameof(Index));
        }

        await _userService.DeleteAsync(id);
        TempData["Success"] = "KullanÄ±cÄ± silindi.";
        return RedirectToAction(nameof(Index));
    }

    // â”€â”€ Helpers â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€

    private async Task PopulateDropdownsAsync()
    {
        var branches = await _branchService.GetAllAsync();

        ViewBag.Branches = new SelectList(branches, "Id", "Name");
        ViewBag.Roles    = new SelectList(
            Enum.GetValues<AppRole>().Select(r => new
            {
                Value = (int)r,
                Text  = r switch
                {
                    AppRole.SystemAdmin   => "Sistem YÃ¶neticisi",
                    AppRole.BranchManager => "Åube MÃ¼dÃ¼rÃ¼",
                    AppRole.StockClerk    => "Depo Personeli",
                    AppRole.Viewer        => "Ä°zleyici",
                    _                     => r.ToString()
                }
            }),
            "Value", "Text");
    }
}