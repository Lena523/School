using Sales.Data.Models.Base;

namespace Sales.Data.Models
{
    /// <summary>
    /// Товар
    /// </summary>
    public class Product : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Product()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        /// <summary>
        /// Наименование товара
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Изображение
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Ширина
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Толщина
        /// </summary>
        public double Thickness { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Список заказов для товара
        /// </summary>
        public ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
