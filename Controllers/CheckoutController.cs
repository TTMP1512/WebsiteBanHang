using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteBanHang.Helpers;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Controllers
{
    [Authorize] // Bắt buộc người dùng phải đăng nhập mới được vào trang này
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // 1. Hiển thị Form thanh toán (GET)
        public async Task<IActionResult> Index()
        {
            // Lấy giỏ hàng, nếu trống thì đá về lại trang Giỏ hàng
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Lấy thông tin user đang đăng nhập để tự động điền sẵn Họ Tên và SĐT
            var user = await _userManager.GetUserAsync(User);
            var order = new Order
            {
                Name = user?.FullName,
                PhoneNumber = user?.PhoneNumber
            };

            ViewBag.Cart = cart; // Truyền giỏ hàng sang View để hiển thị tóm tắt
            return View(order);
        }

        // 2. Xử lý khi bấm nút "Đặt Hàng" (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Order order)
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            if (cart == null || cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var user = await _userManager.GetUserAsync(User);
            order.ApplicationUserId = user.Id;
            order.OrderDate = DateTime.Now;
            order.OrderStatus = "Chờ xác nhận"; // Trạng thái mặc định
            order.PaymentStatus = "Thanh toán khi nhận hàng (COD)";

            // Ép kiểu về double vì DB đang lưu dạng double
            order.OrderTotal = (double)cart.Sum(x => x.Total);

            // Loại bỏ các trường liên kết khỏi bộ kiểm tra lỗi (tránh báo lỗi ảo)
            ModelState.Remove("ApplicationUserId");
            ModelState.Remove("ApplicationUser");

            if (ModelState.IsValid)
            {
                // Hành động 1: Lưu Order (Hóa đơn tổng)
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); // Lưu để SQL cấp phát tự động cái Order.Id

                // Hành động 2: Lưu OrderDetail (Chi tiết từng món mua)
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderId = order.Id, // Lấy ID vừa được tạo ở trên
                        ProductId = item.ProductId,
                        Count = item.Quantity,
                        Price = (double)item.Price
                    };
                    _context.OrderDetails.Add(orderDetail);
                }
                await _context.SaveChangesAsync();

                // Hành động 3: Xóa giỏ hàng vì đã chốt đơn xong
                HttpContext.Session.Remove("Cart");

                // Hành động 4: Chuyển hướng tới trang Báo Thành Công
                return RedirectToAction("Success");
            }

            ViewBag.Cart = cart;
            return View(order);
        }

        // 3. Màn hình báo Đặt hàng thành công
        public IActionResult Success()
        {
            return View();
        }
    }
}