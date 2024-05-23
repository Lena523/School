using Sales.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Cart
{
    public class CartItemModel
    {
        public ProductModel Product { get; set; } = null!;

       
        [Display(Name = "Количество")]
        public int Quantity { get; set; }
    }
}
