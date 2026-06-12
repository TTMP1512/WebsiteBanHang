using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebsiteBanHang.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // --- CÁC DBSET CŨ CỦA BẠN ---
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        // --- CÁC DBSET MỚI VỪA THÊM (BƯỚC 1.3) ---
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Giữ lại 6 Danh mục mẫu của bạn từ Bài 3
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Laptop" },
                new Category { Id = 2, Name = "PC" },
                new Category { Id = 3, Name = "VGA" },
                new Category { Id = 4, Name = "PSU" },
                new Category { Id = 5, Name = "RAM" },
                new Category { Id = 6, Name = "CPU" }
            );
        }
    }
}