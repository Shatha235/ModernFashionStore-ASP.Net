namespace OnlineStore.Models
{
    public class UserShoppingCartItem
    {
        public Shoppingcart CartItem { get; set; }

        public Product Products { get; set; }

        public Category Categories { get; set; }

        public Productcolor Colors { get; set; }

        public Productsize Sizes { get; set; }

        public static decimal CalculateTotalPrice(IEnumerable<UserShoppingCartItem> cartItems)
        {
            return (decimal)cartItems.Sum(item => item.Products.Price * item.CartItem.Quantity);
        }

        public static decimal CalculateGrandPrice(IEnumerable<UserShoppingCartItem> cartItems)
        {
            return (decimal)CalculateTotalPrice(cartItems) + 4;
        }

    }

}
