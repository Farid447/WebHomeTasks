using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaaskUniqlo.DataAccess;
using TaaskUniqlo.Models;
using TaaskUniqlo.ViewModel.Brand;
//using TaaskUniqlo.ViewModel.BrandCreateVM;

namespace TaaskUniqlo.Areas.Admin.Controllers;



[Area("Admin")]
public class Brand(UniqloDbContext _context) : Controller
{
    public IActionResult Index()
    {
        var brands = _context.Brands.ToList();
        return View(brands);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]

    //public IActionResult Create(BrandCreateVM vm)
    //{
        
    //    if (!ModelState.IsValid) return View(vm);
        
    //    _context.Add();
    //    _context.SaveChanges();
    //    return RedirectToAction(nameof(Index));

    //}

    public IActionResult Delete(int? id)
    {
        if (id == null) return BadRequest();
        if (_context.Brands.Any(x => x.Id == id))
        {
            var brand = _context.Brands.Where(x => x.Id == id).FirstOrDefault();
            _context.Brands.Remove(brand);
            _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int? id)
    {
        if (id == null) return BadRequest();
            var data = _context.Brands.Where(x => x.Id == id).FirstOrDefault();
            if (data == null) return NotFound();
            return View(data);
    }

    [HttpPost]
    public IActionResult Update(int? id,BrandCreateVM vm)
    {
        if (id == null) return BadRequest();
        var entity = _context.Brands.Find(id);
        if (entity == null) return NotFound();
        entity.Name = vm.Name;
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
