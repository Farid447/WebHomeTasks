using Microsoft.AspNetCore.Mvc;
using TaaskUniqlo.Models;
using TaaskUniqlo.Extention;
using TaaskUniqlo.DataAccess;
using TaaskUniqlo.ViewModel.Sliders;
using Microsoft.EntityFrameworkCore;
using TaaskUniqlo.ViewModel.Product;
namespace TaaskUniqlo.Areas.Admin.Controllers;


    [Area("Admin")]
    public class Product(IWebHostEnvironment _env, UniqloDbContext _context) : Controller
    {
    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        
        return View(products);
    }
    public IActionResult Create()
        {
            ViewBag.Categories = _context.Brands.Where(x => !x.IsDeleted).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductCreateVM vm)
        {
            if (vm.File != null)
            {
                if (!vm.File.Length.IsValidSize())
                    ModelState.AddModelError("File", "File must be less than 2 kb");
                if (!vm.File.FileName.IsValidType())
                    ModelState.AddModelError("File", "File must be an image");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Brands.Where(x => !x.IsDeleted).ToList();
                return View(vm);
            }
            if (!_context.Products.Any(x => x.Id == vm.BrandId))
            {
                ViewBag.Categories = _context.Brands.Where(x => !x.IsDeleted).ToList();
                ModelState.AddModelError("BrandId", "Brain not found");
                return View();
            }
           
            //product. = vm.File.Upload(_env.WebRootPath, "imgs", "products");
            //_context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    public IActionResult Delete(int? id)
    {
        if (id == null) return BadRequest();
        var vm = _context.Products.Where(x => x.Id == id).FirstOrDefault();
        string imagePath = Path.Combine(_env.WebRootPath, "imgs", "sliders", vm.Image);
        if (System.IO.File.Exists(imagePath))
        {
            System.IO.File.Delete(imagePath);
        }
        if (_context.Products.Any(x => x.Id == id))
        {
            _context.Products.Remove(vm);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Update(int? id)
    {
        if (id == null)
            return BadRequest();

        var slider = _context.Sliders.Find(id);
        if (slider == null)
            return NotFound();


        return View();
    }


    [HttpPost]
    public IActionResult Update(int? id, Product product, ProductCreateVM vm)
    {
        if (id == null) return BadRequest();
        var entity = _context.Products.Find(id);
        if (entity == null) return NotFound();
        entity.Name = product.Name;
        entity.Description = product.Description;
        entity.CostPrice = product.CostPrice;
        entity.SellPrice = product.SellPrice;
        entity.PCount = product.ProductCount;
        entity.Discount = product.Discount;
        entity.BrandId = product.BrandId;
        entity.Image = product.Image;
        if (vm.File is not null)
        {
            string newFileName = vm.File.Upload(Path.Combine(_env.WebRootPath, "imgs", "sliders"));
            if (!string.IsNullOrEmpty(entity.Image))
            {
                string filePath = Path.Combine(_env.WebRootPath, "imgs", "sliders", entity.Image);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            entity.Image = newFileName;
        }
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
