using Microsoft.AspNetCore.Http;
using Sales.Models.Cart;
using Sales.Models.Product;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sales.Services
{
    public class CartService : CartModel
    {
        private readonly string sessionKey = "cart";

        [JsonIgnore]
        ISession Session { get; set; }
        public static CartModel GetCart(IServiceProvider sp)
        {
            // получить объект сессии
            var session = sp
           .GetRequiredService<IHttpContextAccessor>()
            .HttpContext!
            .Session;
            // получить CartService из сессии
            // или создать новый для возможности тестирования

            var cartString = session.GetString("cart");


            CartService cart;

            if (string.IsNullOrEmpty(cartString))
            {
                cart = new CartService();
            }
            else
            {
                cart = JsonSerializer.Deserialize<CartService>(cartString)!;
            }
            
            cart.Session = session;
            return cart;
        }

       
        public override void AddToCart(ProductModel product)
        {
            base.AddToCart(product);
            Session.SetString(sessionKey, JsonSerializer.Serialize(this));
        }
        public override void RemoveFromCart(int id)
        {
            base.RemoveFromCart(id);
            Session.SetString(sessionKey, JsonSerializer.Serialize(this));
        }
        public override void ClearAll()
        {
            base.ClearAll();
            Session.SetString(sessionKey, JsonSerializer.Serialize(this));
        }
    }
}