using Sales.Models.Category;
using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Product
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required(ErrorMessage = "Наименование товара обязательно для заполнения")]
        [Display(Name = "Наименование товара")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Изображение товара")]
        public string? Image { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        [Display(Name = "Категория товара")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Ширина
        /// </summary>
        [Display(Name = "Ширина")]
        public double Width { get; set; }

        /// <summary>
        /// Длина
        /// </summary>
        [Display(Name = "Длина")]
        public double Length { get; set; }

        /// <summary>
        /// Толщина
        /// </summary>
        [Display(Name = "Толщина")]
        public double Thickness { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Категория")]
        public CategoryModel Category { get; set; } = null!;
    }
}
