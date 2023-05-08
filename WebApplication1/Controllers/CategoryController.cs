using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Category> objCategoryList = await _db.Categories.ToListAsync();

        return View(objCategoryList);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder)
            ModelState.AddModelError("CustomError", "Display order can not be the same as name");


        if (!ModelState.IsValid) return View(obj);

        _db.Categories.Add(obj);
        await _db.SaveChangesAsync();
        TempData["success"] = "Category created successfully";

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = await _db.Categories.FindAsync(id);

        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category obj)
    {
        if (obj.Name == obj.DisplayOrder)
            ModelState.AddModelError("CustomError", "Display order can not be the same as name");

        if (!ModelState.IsValid) return View(obj);

        _db.Categories.Update(obj);
        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int? id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePost(int? id)
    {
        var category = await _db.Categories.FindAsync(id);

        if (category == null) return NotFound();

        _db.Categories.Remove(category);

        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}