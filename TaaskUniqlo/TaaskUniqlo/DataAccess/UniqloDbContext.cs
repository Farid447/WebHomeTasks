using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaaskUniqlo.Models;

namespace TaaskUniqlo.DataAccess;

public class UniqloDbContext : DbContext
{
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Brand> Brands { get; set; }

    public UniqloDbContext(DbContextOptions opt) : base(opt){}

}