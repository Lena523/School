using Sales.Data.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace Sales.Data.Models
{
    /// <summary>
    /// Заказы
    /// </summary>
    public class Order : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Order()
        {
            OrderedProducts = new HashSet<OrderedProduct>();
        }

        /// <summary>
        /// Id клиента, который заказал товары
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Клиент, который заказал товары
        /// </summary>
        [Display(Name = "Клиент")]
        public ApplicationUser Customer { get; set; }

        /// <summary>
        /// Дата создания заказа
        /// </summary>
        [Display(Name = "Дата заказа")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Id сотрудника
        /// </summary>
        public int? EmployeeId { get; set; }

        /// <summary>
        /// Сотрудник, который взял заказ в работу
        /// </summary>
        [Display(Name = "Принял заказ")]
        public ApplicationUser? Employee { get; set; }

        /// <summary>
        /// Когда взят в работу заказ
        /// </summary>
        [Display(Name = "Заказ в работе с")]
        public DateTime? AcceptedDate { get; set; }

        /// <summary>
        /// Адрес доставки
        /// </summary>
        [Display(Name = "Адрес доставки")]
        public string Address { get; set; }

        /// <summary>
        /// Закрыт ли заказ
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Список товаров в заказе
        /// </summary>
        public ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
