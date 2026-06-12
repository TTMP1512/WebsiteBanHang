using Microsoft.AspNetCore.Mvc;
using WebsiteBanHang.Models;
using WebsiteBanHang.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteBanHang.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Lấy giỏ hàng từ Session
        public List<CartItem> Carts
        {
            get
            {
                var data = HttpContext.Session.GetJson<List<CartItem>>("Cart");
                if (data == null)
                {
                    data = new List<CartItem>();
                }
                return data;
            }
        }

        // 2. Hiển thị trang Giỏ hàng
        public IActionResult Index()
        {
            return View(Carts);
        }

        // 3. Nút Thêm vào giỏ hàng
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.ProductId == id);

            if (item == null) // Nếu chưa có món này trong giỏ -> Thêm mới
            {
                var product = _context.Products.SingleOrDefault(p => p.Id == id);
                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm");
                }

                item = new CartItem
                {
                    ProductId = id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity,
                    ImageUrl = product.ImageUrl
                };
                myCart.Add(item);
            }
            else // Nếu có rồi -> Tăng số lượng
            {
                item.Quantity += quantity;
            }

            HttpContext.Session.SetJson("Cart", myCart); // Lưu lại vào Session
            return RedirectToAction("Index"); // Chuyển đến trang Giỏ hàng
        }

        // 4. Xóa 1 món khỏi giỏ
        public IActionResult RemoveCart(int id)
        {
            var myCart = Carts;
            var item = myCart.SingleOrDefault(p => p.ProductId == id);
            if (item != null)
            {
                myCart.Remove(item);
                HttpContext.Session.SetJson("Cart", myCart);
            }
            return RedirectToAction("Index");
        }
    }
}