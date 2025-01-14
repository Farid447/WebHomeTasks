using AB108Uniqlo.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.Models;
using WebApplication1.ViewModels.Sliders;

namespace WebApplication1.Areas.Admin.Controllers;

[Area("Admin")]
public class SliderController(WebApplication1DbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        var data = await _context.Sliders.ToListAsync();
        return View(data);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SliderVM vm)
    {   
        if(vm is null) 
            return BadRequest();

        if (vm.File is null)
        {
            ModelState.AddModelError("File", "Şəkil seçilməyib");
            return View(vm);
        }

        string FileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "Sliders"); 


        if(FileName.Length >= 26 )  // Errorun uzunlugu minimum 26-di, eger qaytardigi sey 26-dan boyukdurse demeli error qaytarib. Eks halda faylin adini qaytarib.
            ModelState.AddModelError("File", FileName);

        if (!ModelState.IsValid)
            return View(vm);


        Slider slider = new()
        {
            Title = vm.Title,
            Subtitle = vm.Subtitle,
            Link = vm.Link,
            ImageUrl = FileName
        };
        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int? id)
    {
        if (null == id) return NotFound();
        var data = _context.Sliders.Where(x => x.Id == id).Select(x=> new SliderVM
        {
            Title = x.Title,
            Subtitle = x.Subtitle,
            Link = x.Link!,
            FilePath = x.ImageUrl,
        }).FirstOrDefault();
        if (data is null) return NotFound();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, SliderVM vm)
    {
        if (null == id) return NotFound();

        if (vm is null)
            return BadRequest();


        string FileName = null!;

        if (vm.File != null)
        {
            FileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "Sliders");

            if (FileName.Length >= 26)  // Errorun uzunlugu minimum 26-di, eger qaytardigi sey 26-dan boyukdurse demeli error qaytarib. Eks halda faylin adini qaytarib.
                ModelState.AddModelError("File", FileName);
        }
        
        if (!ModelState.IsValid)
            return View(vm);
        
        var data = await _context.Sliders.Where(x => x.Id == id).FirstOrDefaultAsync();
        
        if (data is null) 
            return NotFound();
        
        if(FileName is not null)  // null deyilse, demeli, vm.File null deyil ve UploadAsync işleyib. Ona gorede evvelki sekili silsin.
        {
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "imgs", "Sliders", data.ImageUrl)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "imgs", "Sliders", data.ImageUrl));
            }
        }
        else  // FileName nulldursa, demeli, vm.File-da nulldur. Ona gorede evvelki adini versin FileName-ə.
        {
            FileName = data.ImageUrl;
        }
        
        data.Title = vm.Title;
        data.Subtitle = vm.Subtitle!;
        data.Link = vm.Link;
        data.ImageUrl = FileName;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (null == id) 
            return NotFound();

        var data = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null) 
            return NotFound();

        if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "imgs", "Sliders", data.ImageUrl)))
        {
            System.IO.File.Delete(Path.Combine(_env.WebRootPath, "imgs", "Sliders", data.ImageUrl));
        }

        _context.Sliders.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Toggle(int? id)
    {
        if (null == id) 
            return NotFound();

        var data = await _context.Sliders.FirstOrDefaultAsync(x => x.Id == id);

        if (data is null) 
            return NotFound();

        data.IsDeleted = !data.IsDeleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
