using Microsoft.AspNetCore.Mvc;
using UniqloTask3.ViewModel.Sliders;
using UniqloTask3.Models;
using UniqloTask3.DAL;
using UniqloTask3.Extention;


namespace UniqloTask3.Areas.Admin.Controllers;

[Area("Admin")]
public class SliderController(UniqIoDbContext _context, IWebHostEnvironment _env) : Controller
{
    public IActionResult Index()
    {
        return View(_context.Sliders.ToList());
    }
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Create(SCreateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        if (!vm.File.ContentType.IsValidType())
        {
            ModelState.AddModelError("sekil", "sec");
            return View(vm);
        }
        if (vm.File.Length.IsValidSize())
        {
            ModelState.AddModelError("sekil", "2kb dan az olmalidir");
            return View(vm);
        }


        var FileName = vm.File.Upload(Path.Combine(_env.WebRootPath, "imgs", "sliders"));

        Slider slider = new Slider
        {
            ImageUrl = FileName,
            Title = vm.Title,
            Subtitle = vm.Subtitle!,
            Link = vm.Link
        };
        _context.Sliders.Add(slider);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Delete(int id)
    {
        Slider obj =_context.Sliders.Where(x=>x.Id == id).FirstOrDefault()!;
        string image = Path.Combine(_env.WebRootPath, "imgs", "sliders", obj!.ImageUrl);

        if (_context.Sliders.Any(x => x.Id == id))
        {
            System.IO.File.Delete(image);

            _context.Sliders.Remove(obj);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
