using Microsoft.EntityFrameworkCore;

namespace ProductManagament_MVC.Models;

public class PM_Context : DbContext
{
    public DbSet<Products> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Cart> Carts { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            "Server=localhost,1433;Database=PM;User ID=SA;Password=reallyStrongPwd123;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;";
        optionsBuilder.UseSqlServer(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
}