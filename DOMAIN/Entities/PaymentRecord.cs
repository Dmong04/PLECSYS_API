using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un registro de pago aplicado a una factura en el sistema.
    /// Almacena el detalle de la transacción, método de pago, moneda y montos.
    /// </summary>
    public class PaymentRecord
    {
        /// <summary>
        /// Identificador único del registro de pago. Clave primaria de la entidad.
        /// </summary>
        [Key]
        public int Payment_record_id { get; set; }

        /// <summary>
        /// Identificador de la factura origen a la que se aplica este pago.
        /// </summary>
        [ForeignKey("source_id")]
        [Column("source_id")]
        public int Source_id { get; set; }

        /// <summary>
        /// Identificador de la moneda utilizada en el pago.
        /// </summary>
        [ForeignKey("currency_id")]
        [Column("currency_id")]
        public required int Currency_id { get; set; }

        /// <summary>
        /// Identificador del método de pago utilizado en la transacción.
        /// </summary>
        [ForeignKey("payment_method_id")]
        [Column("payment_method_id")]
        public required int Payment_method_id { get; set; }

        /// <summary>
        /// Detalle adicional del método de pago (ej. últimos 4 dígitos de tarjeta, banco).
        /// Nulo si no aplica para el método seleccionado.
        /// </summary>
        public string? Detail_payment_method { get; set; }

        /// <summary>
        /// Monto total pagado en esta transacción.
        /// </summary>
        public required decimal Paid_amount { get; set; }

        /// <summary>
        /// Fecha y hora en que se realizó el pago.
        /// </summary>
        public required DateTime Payment_date { get; set; }

        /// <summary>
        /// Descripción o detalle interno del pago registrado.
        /// </summary>
        public required string Payment_detail { get; set; }

        /// <summary>
        /// Identificador de la transacción generado por el sistema de pago externo.
        /// </summary>
        public required string Third_party_transaction_id { get; set; }

        /// <summary>
        /// Factura origen asociada a este registro de pago.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Invoice? Source { get; set; }

        /// <summary>
        /// Moneda utilizada en el pago.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Currency? Currency { get; set; }

        /// <summary>
        /// Método de pago utilizado en la transacción.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public PaymentMethod? Payment_method { get; set; }

        /// <summary>
        /// Colección de historiales de factura relacionados con este pago.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}