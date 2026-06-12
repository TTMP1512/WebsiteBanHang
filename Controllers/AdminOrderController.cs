using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    // Cấp quyền: Chỉ có tài khoản Admin mới được phép vào Controller này
    [Authorize(Roles = SD.Role_Admin)]
    public class AdminOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị danh sách TẤT CẢ đơn hàng
        public async Task<IActionResult> Index()
        {
            // Lấy tất cả hóa đơn, sắp xếp từ mới nhất đến cũ nhất
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }

        // 2. Chức năng cập nhật trạng thái đơn hàng
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.OrderStatus = status; // Đổi trạng thái mới
                await _context.SaveChangesAsync(); // Lưu vào Database
            }
            return RedirectToAction(nameof(Index)); // Load lại trang
        }
    }
}