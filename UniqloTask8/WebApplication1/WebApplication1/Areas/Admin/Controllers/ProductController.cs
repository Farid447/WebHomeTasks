using AB108Uniqlo.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataAccess;
using WebApplication1.Models;
using WebApplication1.ViewModels.Products;

namespace WebApplication1.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController(WebApplication1DbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        var data = await _context.Products.Include(x=> x.Category).ToListAsync();
        return View(data);
    }
    public async Task <IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.Where(x=> !x.IsDeleted).ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
    {
        if (vm is null)
            return BadRequest();

        if (vm.File is null)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ModelState.AddModelError("File", "Şəkil seçilməyib");
            return View(vm);
        }



        string FileName;
        string error = "";

        FileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "Products");

        if (FileName.Length >= 26)
        {
            error = FileName;
            ModelState.AddModelError("File", error);
        }


        List<string> otherfiles = [];

        if (vm.OtherFiles != null && vm.OtherFiles.Count > 0)
        {
            foreach (var item in vm.OtherFiles)
            {
                string filename = await item.UploadAsync(_env.WebRootPath, "imgs", "Products");
                if (filename.Length >= 26)
                {
                    error = filename;
                    ModelState.AddModelError("OtherFiles", error);
                    break;
                }
                otherfiles.Add(filename);
            }
        }

        if (!await _context.Categories.Where(x=> !x.IsDeleted).AnyAsync(x => x.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Kateqoriya düzgün seçilməyib");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(vm);
        }


        Product product = vm;
        product.CoverImage = FileName;

        if (vm.OtherFiles != null && vm.OtherFiles.Count > 0)
        {
            product.Images = otherfiles.Select(x => new ProductImage
            {
                ImageUrl = x,
            }).ToList();
        }

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int? id)
    {
        if (null == id)
            return NotFound();
        var data = await _context.Products.Where(x=>x.Id == id).Select(x => new ProductUpdateVM
        {
            Id = x.Id,
            Name = x.Name,
            CategoryId = x.CategoryId ?? 0,
            CostPrice = x.CostPrice,
            Description = x.Description,
            Discount = x.Discount,
            FileUrl = x.CoverImage,
            Quantity = x.Quantity,
            SellPrice = x.SellPrice,
            OtherFilesUrls = x.Images.Select(y => y.ImageUrl)

        }).FirstOrDefaultAsync();
        if (data is null)
            return NotFound();

        ViewBag.Categories = await _context.Categories.Where(x=> !x.IsDeleted).ToListAsync();
        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
    {
        var data = await _context.Products.Include(x => x.Images)
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();

        
        string FileName = data.CoverImage;
        string error = "";
        
        if(data.CoverImage != vm.FileUrl)
        {
            FileName = await vm.File.UploadAsync(_env.WebRootPath, "imgs", "Products");

            if (FileName.Length >= 26)
            {
                error = FileName;
                ModelState.AddModelError("File", error);
            }

        }

        List<string> otherfiles = [];

        if (vm.OtherFiles != null && vm.OtherFiles.Count > 0)
        {
            foreach (var item in vm.OtherFiles)
            {
                string filename = await item.UploadAsync(_env.WebRootPath, "imgs", "Products");
                if (filename.Length >= 26)
                {
                    error = filename;
                    ModelState.AddModelError("OtherFiles", error);
                    break;
                }
                otherfiles.Add(filename);
            }
        }

        if (!await _context.Categories.Where(x => !x.IsDeleted).AnyAsync(x => x.Id == vm.CategoryId))
        {
            ModelState.AddModelError("CategoryId", "Kateqoriya düzgün seçilməyib");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(vm);
        }


        Product product = vm;
        product.CoverImage = FileName;

        if (vm.OtherFiles != null && vm.OtherFiles.Count > 0)
        {
            product.Images = otherfiles.Select(x => new ProductImage
            {
                ImageUrl = x,
            }).ToList();
        }

        await _context.Products.AddAsync(product);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (null == id)
            return NotFound();

        var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (data is null)
            return NotFound();

        if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "imgs", "Products", data.CoverImage)))
        {
            System.IO.File.Delete(Path.Combine(_env.WebRootPath, "imgs", "Products", data.CoverImage));
        }

        if (data.Images != null && data.Images.Count > 0)
        {
            foreach (var item in data.Images)
            {
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "imgs", "Products", item.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "imgs", "Products", item.ImageUrl));
                }
            }
        }

        _context.Products.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Toggle(int? id)
    {
        if (null == id)
            return NotFound();

        var data = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

        if (data is null)
            return NotFound();

        data.IsDeleted = !data.IsDeleted;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
