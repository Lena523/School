using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Sales.Models.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class EditClientModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "ФИО пользователя")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}
