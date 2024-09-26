using Microsoft.EntityFrameworkCore;

namespace ProductManagament_MVC.Models
{
    public class PM_Context : DbContext
    {
        public PM_Context(DbContextOptions<PM_Context> options) : base(options)
        {
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
    }
}