using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un flujo de trabajo automatizado (SmartFlow) asignado a un usuario.
    /// Define los pasos secuenciales de un proceso y su aprobador responsable.
    /// </summary>
    public class SmartFlow
    {
        /// <summary>
        /// Identificador único del flujo de trabajo.
        /// </summary>
        public int SmartFlow_id { get; set; }

        /// <summary>
        /// Nombre descriptivo del flujo de trabajo. Nulo si aún no ha sido asignado.
        /// </summary>
        public string? SmartFlow_name { get; set; }

        /// <summary>
        /// Identificador del usuario propietario o responsable del flujo de trabajo.
        /// </summary>
        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }

        /// <summary>
        /// Identificador del primer paso en la secuencia del flujo de trabajo.
        /// </summary>
        public int First_step_id { get; set; }

        /// <summary>
        /// Nombre descriptivo del primer paso del flujo de trabajo.
        /// </summary>
        public string? First_step_name { get; set; }

        /// <summary>
        /// Identificador del siguiente paso en la secuencia del flujo de trabajo.
        /// </summary>
        public int Next_step_id { get; set; }

        /// <summary>
        /// Nombre descriptivo del siguiente paso del flujo de trabajo.
        /// </summary>
        public string? Next_step_name { get; set; }

        /// <summary>
        /// Identificador del usuario aprobador responsable de validar el flujo.
        /// Nulo si el flujo no requiere aprobación.
        /// </summary>
        [ForeignKey("approver")]
        [Column("approver")]
        public string? Approver { get; set; }

        /// <summary>
        /// Usuario propietario del flujo de trabajo.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public User? User { get; set; }

        /// <summary>
        /// Usuario designado como aprobador del flujo de trabajo.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public User? Approver_user { get; set; }
    }
}