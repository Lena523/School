using Sales.Data.Models.Base;

namespace Sales.Data.Models
{
    /// <summary>
    /// Товары в заказе
    /// </summary>
    public class OrderedProduct : BaseEntity
    {
        /// <summary>
        /// Id заказа
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Заказ
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Id товара
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Товар
        /// </summary>
        public Product Product { get; set; }

        /// <summary>
        /// Количество
        /// </summary>
        public int Quantity { get; set; }
    }
}
