using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Category
{
    /// <summary>
    /// Добавить категорию
    /// </summary>
    public class CreateCategoryModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Наименование категории")]
        public string Name { get; set; } = null!;

        [Display(Name = "Изображение категории")]
        public IFormFile? Image { get; set; }
    }
}