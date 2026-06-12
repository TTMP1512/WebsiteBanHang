using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebsiteBanHang.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // Liên kết nối với bảng Đơn hàng chính (Order)
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public Order Order { get; set; }

        // Liên kết nối với bảng Sản phẩm (Product)
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [Required]
        public int Count { get; set; } // Số lượng sản phẩm mua

        [Required]
        public double Price { get; set; } // Giá sản phẩm tại thời điểm mua
    }
}
