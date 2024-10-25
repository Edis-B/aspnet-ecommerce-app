using TechStoreApp.Web.ViewModels.Products;

namespace TechStoreApp.Web.ViewModels.Cart
{
    public class CartItemViewModel
    {
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public ProductViewModel Product { get; set; }
    }
}
