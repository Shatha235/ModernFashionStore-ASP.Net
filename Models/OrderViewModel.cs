namespace OnlineStore.Models
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public string CouponCode { get; set; }
        public decimal? CouponDiscount { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}
