using Microsoft.AspNetCore.Identity;

namespace Sales.Data.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
            AcceptedOrders = new HashSet<Order>();
            UserAddresses = new HashSet<UserAddress>();
        }

        /// <summary>
        /// ФИО
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Заказы
        /// </summary>
        public ICollection<Order> Orders { get; set; }

        /// <summary>
        /// Подтвержденные заказы
        /// </summary>
        public ICollection<Order> AcceptedOrders { get; set; }

        /// <summary>
        /// Адреса пользователей
        /// </summary>
        public ICollection<UserAddress> UserAddresses { get; set; }
    }
}
