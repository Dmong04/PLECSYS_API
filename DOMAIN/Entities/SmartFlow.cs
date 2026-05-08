using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    public class SmartFlow
    {
        public int SmartFlow_id { get; set; }

        public string? SmartFlow_name { get; set; }

        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }
        
        public int First_step_id { get; set; }
        
        public string? First_step_name { get; set; }
        
        public int Next_step_id { get; set; }
        
        public string? Next_step_name { get; set; }

        [ForeignKey("approver")]
        [Column("approver")]
        public string? Approver { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public User? Approver_user { get; set; }
    }
}
