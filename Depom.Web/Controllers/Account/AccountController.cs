using Depom.Application.User.DTOs;
using Depom.Application.User.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Depom.Web.Controllers.Account;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(dto);

        var user = await _userService.ValidateAsync(dto.Username, dto.Password);
        if (user == null)
        {
            ModelState.AddModelError("", "Kullanici adi veya sifre hatali.");
            return View(dto);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,           user.FullName),
            new(ClaimTypes.Role,           user.Role.ToString()),
        };

        if (user.BranchId.HasValue)
            claims.Add(new Claim("BranchId", user.BranchId.Value.ToString()));

        var identity  = new ClaimsIdentity(claims, "DepomCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("DepomCookie", principal,
            new AuthenticationProperties { IsPersistent = dto.RememberMe });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("DepomCookie");
        return RedirectToAction("Login");
    }
}
