using Microsoft.EntityFrameworkCore;
using WebsiteBanHang.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký DbContext (Cấu hình kết nối SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Đăng ký Repository (Tạm thời vẫn giữ Mock Data, lát nữa tạo xong EF Repository ta sẽ sửa chỗ này sau)
builder.Services.AddScoped<WebsiteBanHang.Repositories.IProductRepository, WebsiteBanHang.Repositories.EFProductRepository>();
builder.Services.AddScoped<WebsiteBanHang.Repositories.ICategoryRepository, WebsiteBanHang.Repositories.EFCategoryRepository>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();