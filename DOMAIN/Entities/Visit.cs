using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Entities
{
    public class Visit
    {
        public int Visit_id { get; set; }

        public DateOnly Visit_date { get; set; }

        public TimeOnly Visit_time { get; set; }

        public required string Gps_point { get; set; } // Primaria de ruta de Mongo sincronizada

        public required string Gps_point_new { get; set; }

        public required string Description { get; set; }
    }
}
