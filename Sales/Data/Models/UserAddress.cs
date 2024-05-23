using Sales.Data.Models.Base;

namespace Sales.Data.Models
{
    public class UserAddress : BaseEntity
    {
        public int UserId { get; set; }

        public ApplicationUser User { get; set; }

        public required string Address { get; set; }
    }
}
