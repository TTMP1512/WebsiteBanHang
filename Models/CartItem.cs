namespace WebsiteBanHang.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        // Sửa double thành decimal ở đây
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }

        // Sửa double thành decimal ở đây
        public decimal Total => Quantity * Price;
    }
}