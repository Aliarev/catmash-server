using Catmash.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catmash.Model
{
    public class CatmashContext : DbContext
    {
        public CatmashContext(DbContextOptions<CatmashContext> options)
            : base(options)
        { }


        // Tables
        public DbSet<Cat> Cats { get; set; }
    }
}
