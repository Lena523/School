using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sales.Data.Models;

namespace Sales.Data;

/// <summary>
/// Контекст базы данных
/// </summary>
/// <param name="options"></param>
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)
{
    /// <summary>
    /// Адреса пользователей
    /// </summary>
    public DbSet<UserAddress> UserAddresses { get; set; }

    /// <summary>
    /// Категории
    /// </summary>
    public DbSet<Category> Categories { get; set; }

    /// <summary>
    /// Товары
    /// </summary>
    public DbSet<Product> Products { get; set; }

    /// <summary>
    /// Товары в заказе
    /// </summary>
    public DbSet<OrderedProduct> OrderedProducts { get; set; }

    /// <summary>
    /// Заказы
    /// </summary>
    public DbSet<Order> Orders { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserAddress>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.UserAddresses)
            .HasForeignKey(ua => ua.UserId)
            .IsRequired(true);

        builder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Entity<Product>()
            .Property(c => c.Name)
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired(true);


        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2)
            .IsRequired(true);

        builder.Entity<Product>()
            .Property(p => p.Length)
            .HasPrecision(18, 2)
            .IsRequired(true);

        builder.Entity<Product>()
            .Property(p => p.Width)
            .HasPrecision(18, 2)
            .IsRequired(true);

        builder.Entity<Product>()
            .Property(p => p.Thickness)
            .HasPrecision(18, 2)
            .IsRequired(true);

        builder.Entity<OrderedProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderedProducts)
            .HasForeignKey(op => op.ProductId)
            .IsRequired(true);

        builder.Entity<OrderedProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderedProducts)
            .HasForeignKey(op => op.OrderId)
            .IsRequired(true);

        builder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(true);

        builder.Entity<Order>()
            .HasOne(o => o.Employee)
            .WithMany(c => c.AcceptedOrders)
            .HasForeignKey(o => o.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired(false);


        builder.Entity<IdentityRole<int>>()
            .HasData(new List<IdentityRole<int>>
            {
                new() {
                    Id = 1,
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new() {
                    Id = 2,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new() {
                    Id = 3,
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            });


        builder.Entity<Category>().HasData(new List<Category>
        {
        });

        builder.Entity<Product>().HasData(new List<Product>
        {
          
        });
    }
}