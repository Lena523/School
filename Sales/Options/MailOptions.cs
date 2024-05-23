namespace Sales.Options
{
    /// <summary>
    /// Настройки Smtp
    /// </summary>
    public class MailOptions
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Smtp хост
        /// </summary>
        public string SmtpHost { get; set; } = null!;

        /// <summary>
        /// Smtp порт
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Smtp пользователь
        /// </summary>
        public string SmtpUser { get; set; } = null!;

        /// <summary>
        /// Smtp пароль
        /// </summary>
        public string SmtpPassword { get; set; } = null!;
    }
}
