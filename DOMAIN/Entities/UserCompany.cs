
using System.ComponentModel.DataAnnotations.Schema;

namespace DOMAIN.Entities
{
    public class UserCompany
    {
        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }

        [ForeignKey("company_id")]
        [Column("company_id")]
        public int Company_id { get; set; }

        public User? User { get; set; }
        public Company? Company { get; set; }
    }
}
