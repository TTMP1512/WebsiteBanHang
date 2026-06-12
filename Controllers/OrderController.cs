using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize] // Bắt buộc đăng nhập mới xem được lịch sử
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Màn hình Lịch sử mua hàng
        public async Task<IActionResult> History()
        {
            // 1. Lấy thông tin user đang đăng nhập
            var user = await _userManager.GetUserAsync(User);

            // 2. Lấy danh sách hóa đơn của đúng user này, sắp xếp đơn mới nhất lên đầu
            var orders = await _context.Orders
                .Where(o => o.ApplicationUserId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return View(orders);
        }
        // Màn hình Chi tiết đơn hàng
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            // Lấy thông tin đơn hàng
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.ApplicationUserId == user.Id);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }

            // Lấy danh sách sản phẩm trong đơn hàng đó (Dùng Include để JOIN bảng Product lấy Tên và Hình)
            var orderDetails = await _context.OrderDetails
                .Include(od => od.Product)
                .Where(od => od.OrderId == id)
                .ToListAsync();

            ViewBag.OrderDetails = orderDetails; // Gửi danh sách món hàng qua View
            return View(order);
        }
    }
}