using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un registro histórico de los cambios y acciones realizadas sobre una factura.
    /// Permite auditar pagos, cambios de estado y reclamos asociados.
    /// </summary>
    public class InvoiceHistory
    {
        /// <summary>
        /// Identificador único del registro histórico.
        /// </summary>
        public int Invoice_history_id { get; set; }

        /// <summary>
        /// Identificador de la factura a la que pertenece este historial.
        /// </summary>
        [ForeignKey("invoice_id")]
        [Column("invoice_id")]
        public required int Invoice_id { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró la acción en el historial.
        /// </summary>
        public DateTime Record_date { get; set; }

        /// <summary>
        /// Tipo de acción realizada (ej. pago, anulación, reclamo, cambio de estado).
        /// </summary>
        public required string Action { get; set; }

        /// <summary>
        /// Descripción detallada de la acción registrada.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Identificador del usuario que realizó la acción. Nulo si fue una acción del sistema.
        /// </summary>
        [ForeignKey("user_id")]
        [Column("user_id")]
        public string? User_id { get; set; }

        /// <summary>
        /// Estado de la factura antes de realizar la acción.
        /// </summary>
        public string? Previous_status { get; set; }

        /// <summary>
        /// Estado de la factura después de realizar la acción.
        /// </summary>
        public string? New_status { get; set; }

        /// <summary>
        /// Monto pagado en esta acción. Nulo si no corresponde a un pago.
        /// </summary>
        public decimal? Paid_amount { get; set; }

        /// <summary>
        /// Saldo pendiente de la factura tras esta acción. Nulo si no aplica.
        /// </summary>
        public decimal? Pending_balance { get; set; }

        /// <summary>
        /// Identificador del registro de pago asociado a esta entrada del historial. Nulo si no aplica.
        /// </summary>
        [ForeignKey("payment_record_id")]
        [Column("payment_record_id")]
        public int? Payment_record_id { get; set; }

        /// <summary>
        /// Identificador del reclamo asociado a esta entrada del historial. Nulo si no aplica.
        /// </summary>
        [ForeignKey("claim_id")]
        [Column("claim_id")]
        public int? Claim_id { get; set; }

        /// <summary>
        /// Usuario que realizó la acción registrada.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public User? User { get; set; }

        /// <summary>
        /// Factura a la que pertenece este registro histórico.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        /// <summary>
        /// Registro de pago relacionado con esta entrada del historial.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public PaymentRecord? PaymentRecord { get; set; }

        /// <summary>
        /// Reclamo relacionado con esta entrada del historial.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Claim? Claim { get; set; }
    }
}