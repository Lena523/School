using Sales.Data.Models.Base;

namespace Sales.Data.Models
{
    /// <summary>
    /// Категория товаров
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Category()
        {
            Products = new HashSet<Product>();
        }

        /// <summary>
        /// Наименование категории
        /// </summary>
        public required string Name { get; set; } 

        /// <summary>
        /// Изображение
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Список товаров в категории
        /// </summary>
        public ICollection<Product> Products { get; set; }
    }
}
