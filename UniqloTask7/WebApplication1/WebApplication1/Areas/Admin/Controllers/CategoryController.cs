using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.Models;

namespace WebApplication1.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController(WebApplication1DbContext _context) : Controller
{
    public async Task <IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category vm)
    {
        if (!ModelState.IsValid) 
            return View(vm);

        await _context.Categories.AddAsync(vm);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int? id)
    {
        if (null == id) return NotFound();
        var data = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
        
        if (data is null) return NotFound();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, Category vm)
    {
        if (null == id) 
            return NotFound();

        if (vm is null)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(vm);


        var data = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (data is null)
            return NotFound();

        data.Name = vm.Name;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (null == id)
            return NotFound();

        var data = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null)
            return NotFound();

        _context.Categories.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Toggle(int? id)
    {
        if (null == id)
            return NotFound();

        var data = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (data is null)
            return NotFound();

        data.IsDeleted = !data.IsDeleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
