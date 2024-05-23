using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Display(Name = "Наименование категории")]
        public string Name { get; set; } = null!;

        [Display(Name = "Изображение категории")]
        public string? Image { get; set; }
    }
}
