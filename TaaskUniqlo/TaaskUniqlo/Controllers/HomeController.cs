using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaaskUniqlo.Models;
using TaaskUniqlo.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace TaaskUniqlo.Controllers;

public class HomeController(UniqloDbContext _context) : Controller
{
    public IActionResult Index()
    {
        return View(_context.Sliders.ToList());
    }
    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

}