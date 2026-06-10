using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un reclamo asociado a una factura dentro del sistema.
    /// </summary>
    public class Claim
    {
        /// <summary>
        /// Identificador único del reclamo.
        /// </summary>
        public int Claim_id { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró el reclamo.
        /// </summary>
        public DateTime Record_date { get; set; }

        /// <summary>
        /// Identificador del usuario que generó el reclamo.
        /// </summary>
        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }

        /// <summary>
        /// Descripción detallada del motivo del reclamo.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Identificador de la factura asociada al reclamo.
        /// </summary>
        [ForeignKey("invoice_id")]
        [Column("invoice_id")]
        public required int Invoice_id { get; set; }

        /// <summary>
        /// Monto económico reclamado.
        /// </summary>
        public decimal Claim_amount { get; set; }

        /// <summary>
        /// Usuario relacionado con el reclamo.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public User? User { get; set; }

        /// <summary>
        /// Factura relacionada con el reclamo.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        /// <summary>
        /// Colección de historiales de factura vinculados a este reclamo.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}