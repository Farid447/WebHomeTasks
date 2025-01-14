using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.DataAccess;
using WebApplication1.Models;
using WebApplication1.ViewModels.Commons;

namespace WebApplication1.Controllers;

public class HomeController(WebApplication1DbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        HomeVM vm = new();
        vm.Sliders = await _context.Sliders.Where(x=> !x.IsDeleted).ToListAsync();
        vm.Products = await _context.Products.Where(x => !x.IsDeleted).ToListAsync();

        return View(vm);
    }

}