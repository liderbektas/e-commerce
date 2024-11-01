using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                userName = "admin",
                password = "Ads3129110.",
                email = "admin@example.com",
                role = "Manager",
                CreatedAt = DateTime.Now
            });
        }
    }
}