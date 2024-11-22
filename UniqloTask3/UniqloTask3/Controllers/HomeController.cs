using Microsoft.AspNetCore.Mvc;
using UniqloTask3.DAL;

namespace UniqIo.Controllers;

public class HomeController(UniqIoDbContext _context) : Controller
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
