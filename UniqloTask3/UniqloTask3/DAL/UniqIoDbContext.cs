using Microsoft.EntityFrameworkCore;
using UniqloTask3.Models;

namespace UniqloTask3.DAL;

public class UniqIoDbContext : DbContext
{
    public DbSet<Slider> Sliders { get; set; }

        public UniqIoDbContext(DbContextOptions option) :base(option)
        {
            
        }

}
