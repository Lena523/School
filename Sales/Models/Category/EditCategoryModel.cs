using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Category
{
    /// <summary>
    /// Изменить категорию
    /// </summary>
    public class EditCategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Наименование категории")]
        public string Name { get; set; } = null!;
        [Display(Name = "Изображение категории")]
        public IFormFile? Image { get; set; }
    }
}
