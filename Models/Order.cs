using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebsiteBanHang.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Liên kết với tài khoản người dùng đã đăng nhập
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } // Ngày đặt hàng
        public DateTime ShippingDate { get; set; } // Ngày giao hàng
        public double OrderTotal { get; set; } // Tổng tiền của toàn đơn hàng

        public string? OrderStatus { get; set; } // Trạng thái đơn hàng (Chờ duyệt, Đã gửi hàng, Đang vận chuyển, Đã giao)
        public string? PaymentStatus { get; set; } // Trạng thái thanh toán (Chưa thanh toán, Đã thanh toán, Bị từ chối)

        // Các trường thu thập thông tin vận chuyển (Ráp chính xác theo Module 1 - Màn hình Thanh toán)
        [Required(ErrorMessage = "Vui lòng nhập Họ tên người nhận")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại nhận hàng")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ cụ thể (Số nhà, tên đường...)")]
        [Display(Name = "Địa chỉ chi tiết")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
        [Display(Name = "Tỉnh/Thành phố")]
        public string City { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
        [Display(Name = "Quận/Huyện")]
        public string District { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
        [Display(Name = "Phường/Xã")]
        public string Ward { get; set; }
    }
}