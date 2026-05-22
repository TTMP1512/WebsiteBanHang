var builder = WebApplication.CreateBuilder(args);
// Đăng ký Repository để Controller có thể gọi được dữ liệu
builder.Services.AddSingleton<WebsiteBanHang.Repositories.IProductRepository, WebsiteBanHang.Repositories.MockProductRepository>();
builder.Services.AddScoped<WebsiteBanHang.Repositories.ICategoryRepository, WebsiteBanHang.Repositories.MockCategoryRepository>();
builder.Services.AddScoped<WebsiteBanHang.Repositories.ICategoryRepository, WebsiteBanHang.Repositories.MockCategoryRepository>();

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
