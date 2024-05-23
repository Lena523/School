using Sales.Models.Product;

namespace Sales.Models.Cart
{
    
    public class CartModel
    {
        public Dictionary<int, CartItemModel> Items { get; set; }

        public CartModel()
        {
            Items = [];
        }
        /// <summary>
        /// Количество объектов в корзине
        /// </summary>
        public int Count => Items.Sum(item => item.Value.Quantity);
        /// <summary>
        /// Количество калорий
        /// </summary>
        public decimal Summ => Items.Sum(item => item.Value.Quantity * item.Value.Product.Price);

        /// <summary>
        /// Добавление в корзину
        /// </summary>
        /// <param name="product">добавляемый объект</param>
        public virtual void AddToCart(ProductModel product)
        {
            // если объект есть в корзине
            // то увеличить количество
            if (Items.TryGetValue(product.Id, out CartItemModel? value))
                value.Quantity++;
            // иначе - добавить объект в корзину
            else
                Items.Add(product.Id, new CartItemModel
                {
                    Product = product,
                    Quantity = 1
                });
        }

        /// <summary>
        /// Удалить объект из корзины
        /// </summary>
        /// <param name="id">id удаляемого объекта</param>
        public virtual void RemoveFromCart(int id)
        {
            Items.Remove(id);
        }

        /// <summary>
        /// Очистить корзину
        /// </summary>
        public virtual void ClearAll()
        {
            Items.Clear();
        }
    }
}