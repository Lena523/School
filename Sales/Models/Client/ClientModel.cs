using System.ComponentModel.DataAnnotations;

namespace Sales.Models.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Имя пользователя")]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Email")]
        public string? Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Номер телефона")]
        public string? PhoneNumber { get; set; }
    }
}
