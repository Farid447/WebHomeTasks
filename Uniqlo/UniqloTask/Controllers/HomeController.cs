using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace UniqloTask.Controllers;

public class HomeController : Controller
{   

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
    public IActionResult Contact()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

}
