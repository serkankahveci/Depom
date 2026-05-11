using Depom.Application.Category.Services;
using Depom.Application.Product.DTOs;
using Depom.Application.Product.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Depom.Web.Controllers.Product;

[Authorize]
public class ProductController : Controller
{
    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;

    public ProductController(ProductService productService, CategoryService categoryService)
    {
        _productService  = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(dto);
        }
        await _productService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();
        var dto = new ProductUpdateDto
        {
            Id                 = product.Id,
            Name               = product.Name,
            Description        = product.Description,
            SKU                = product.SKU,
            Barcode            = product.Barcode,
            Price              = product.Price,
            LowStockThreshold  = product.LowStockThreshold,
            CategoryId         = product.CategoryId,
            Status             = product.Status
        };
        await LoadCategoriesAsync();
        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin,BranchManager")]
    public async Task<IActionResult> Edit(ProductUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync();
            return View(dto);
        }
        await _productService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SystemAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync()
    {
        var cats = await _categoryService.GetAllAsync();
        ViewBag.Categories = new SelectList(cats, "Id", "Name");
    }
}
