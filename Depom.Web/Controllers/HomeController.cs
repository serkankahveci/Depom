using Depom.Application.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Depom.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly UserService _userService;

    public HomeController(UserService userService)
    {
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        await _userService.SeedAdminAsync();
        return View();
    }
}
