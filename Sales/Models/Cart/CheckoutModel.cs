using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Cart
{
    public class CheckoutModel
    {
        [Display(Name = "Выберите адрес доставки из списка или введите его вручную")]
        public string? SelectedAddress { get; set; }

        [Display(Name = "Использовать адрес, введенный вручную")]
        public bool UseManualAddress { get; set; }

        [Display(Name = "Адрес доставки")]
        public string? Address { get; set; }
    }
}
