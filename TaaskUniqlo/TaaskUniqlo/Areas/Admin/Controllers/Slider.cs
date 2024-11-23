﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net;
using TaaskUniqlo.DataAccess;
using TaaskUniqlo.Extention;
using TaaskUniqlo.Models;
using TaaskUniqlo.ViewModel.Sliders;

namespace UniqIo.Areas.Admin.Controllers;


[Area("Admin")]
public class Slider(UniqloDbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Sliders.ToListAsync());
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(SliderCreateVM vm)
    {
        if(vm.File != null)
        {
            if (!vm.File.Name.IsValidType())
            {
                ModelState.AddModelError("File", "Cannot be anything else of type image");
            }
            if (!vm.File.Length.IsValidSize())
            {
                ModelState.AddModelError("File", "The file size must be less than 2 kb");
            }
        }

        if (!ModelState.IsValid) return View(vm);
        
        //_context.Sliders.Add(slider);
        //_context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return BadRequest();
        var vm =_context.Sliders.Where(x=>x.Id == id).FirstOrDefault();
        string imagePath = Path.Combine(_env.WebRootPath, "imgs", "sliders", vm.ImageUrl);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
        if (await _context.Sliders.AnyAsync(x => x.Id == id))
        {
            _context.Sliders.Remove(vm);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (id == null)
            return BadRequest();

        var slider = await _context.Sliders.FindAsync(id);
        if (slider == null)
            return NotFound();



        return View();
    }


    [HttpPost]
    public IActionResult Update(int? id, Slider slider, SliderCreateVM vm)
    {
        if (id == null) return BadRequest();
        var entity = _context.Sliders.Find(id.Value);
        if (entity == null) return NotFound();
        entity.Title = slider.Title;
        entity.Subtitle = slider.Subtitle;
        entity.Link = slider.Link;
        if (vm.File is not null)
        {
            string newFileName = vm.File.Upload(Path.Combine(_env.WebRootPath, "imgs", "sliders"));
            if (!string.IsNullOrEmpty(entity.ImageUrl))
            {
                string filePath = Path.Combine(_env.WebRootPath, "imgs", "sliders", entity.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            entity.ImageUrl = newFileName;
        }
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Hide(int? id)
    {
        var slider = await _context.Sliders.FindAsync(id);
        if(slider == null) return NotFound();
        slider.IsDeleted = true;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Show(int? id)
    {

        var slider = await _context.Sliders.FindAsync(id);
        if (slider == null) return NotFound();
        slider.IsDeleted = false;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
