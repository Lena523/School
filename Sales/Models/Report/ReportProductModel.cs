using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Report
{
    public class ReportProductModel
    {
        [Display(Name = "Наименование товара")]
        public string Name { get; set; }

        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        [Display(Name = "Общая сумма (BYN)")]
        public decimal Summ { get; set; }
    }
}
