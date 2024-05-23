using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Sales.Models.Employee
{
    /// <summary>
    /// 
    /// </summary>
    public class EditEmployeeModel
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

        //[Required]
        //[StringLength(100, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Пароль")]
        //public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //[Display(Name = "Подтвердить пароль")]
        //[Compare("Password", ErrorMessage = "Пароли не совпадают")]
        //public string ConfirmPassword { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }
    }
}
