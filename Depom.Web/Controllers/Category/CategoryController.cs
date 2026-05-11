using Depom.Application.Category.DTOs;
using Depom.Application.Category.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Depom.Web.Controllers.Category;

[Authorize(Roles = "SystemAdmin")]
public class CategoryController : Controller
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _categoryService.GetAllAsync();
        return View(list);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CategoryCreateDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _categoryService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }
}
