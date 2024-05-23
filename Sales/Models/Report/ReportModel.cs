using Sales.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Report
{
    /// <summary>
    /// Отчет по продажам
    /// </summary>
    public class ReportModel
    {
        /// <summary>
        /// Дата начала периода
        /// </summary>
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала периода")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания периода
        /// </summary>
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата окончания периода обязательна для заполнения")]
        [Display(Name = "Дата окончания периода")]
        public DateTime EndDate { get; set; } = DateTime.Now.Date;

        /// <summary>
        /// Количество проданных товаров
        /// </summary>
        [Display(Name = "Количество проданных товаров")]
        public int Count { get; set; }

        /// <summary>
        /// Сумма проданных товаров
        /// </summary>
        [Display(Name = "Сумма проданных товаров")]
        public decimal Summ { get; set; }

        [Display(Name = "Проданные товары")]
        public List<ReportProductModel> ReportProducts { get; set; } = [];
    }
}