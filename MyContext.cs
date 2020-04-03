using System.ComponentModel.DataAnnotations;
using mfbcustomizerserver.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace mfbcustomizerserver
{
    public class MyContext : DbContext
    {
        public static MyContext New()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlite("Data Source=./SqliteDB.db");
            return new MyContext(optionsBuilder.Options);
        }
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {

        }
        
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<FoodBag> FoodBags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ObjectMeta> ObjectMetas { get; set; }
        public DbSet<ObjectStack> ObjectStacks { get; set; }
    }
}