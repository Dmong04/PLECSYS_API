using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa una factura generada en el sistema entre dos empresas.
    /// Registra el monto, estado, moneda y las relaciones con pagos, reclamos e historial.
    /// </summary>
    public class Invoice
    {
        /// <summary>
        /// Identificador único de la factura. Clave primaria de la entidad.
        /// </summary>
        [Key]
        public int Invoice_id { get; set; }

        /// <summary>
        /// Número consecutivo de la factura para control interno.
        /// </summary>
        public int Consecutive { get; set; }

        /// <summary>
        /// Monto total del comprobante de la factura.
        /// </summary>
        public decimal Total_voucher { get; set; }

        /// <summary>
        /// Identificador del usuario que creó la factura.
        /// </summary>
        [ForeignKey("user_creator_id")]
        [Column("user_creator_id")]
        public required string User_creator_id { get; set; }

        /// <summary>
        /// Identificador de la empresa vendedora en la transacción.
        /// </summary>
        [ForeignKey("sell_company_id")]
        [Column("sell_company_id")]
        public required int Sell_company_id { get; set; }

        /// <summary>
        /// Identificador de la empresa compradora a quien se cobra la factura.
        /// </summary>
        [ForeignKey("charged_company_id")]
        [Column("charged_company_id")]
        public required int Charged_company_id { get; set; }

        /// <summary>
        /// Identificador de la moneda utilizada en la factura.
        /// </summary>
        [ForeignKey("currency_id")]
        [Column("currency_id")]
        public required int Currency_id { get; set; }

        /// <summary>
        /// Estado actual de la factura (ej. pendiente, pagada, anulada).
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Saldo pendiente de pago de la factura. Nulo si está completamente pagada.
        /// </summary>
        public decimal? Pending_balance { get; set; }

        /// <summary>
        /// Fecha y hora en que fue creada la factura.
        /// </summary>
        public DateTime Created_at { get; set; }

        /// <summary>
        /// Fecha de vencimiento de la factura.
        /// </summary>
        public DateTime Expiry_date { get; set; }

        /// <summary>
        /// Empresa compradora asociada a la factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Company? Charged_company { get; set; }

        /// <summary>
        /// Empresa vendedora asociada a la factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Company? Sell_company { get; set; }

        /// <summary>
        /// Usuario creador de la factura.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public User? User { get; set; }

        /// <summary>
        /// Colección de reclamos asociados a esta factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Claim>? Claims { get; set; }

        /// <summary>
        /// Colección de historiales de cambios de esta factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }

        /// <summary>
        /// Colección de registros de pago aplicados a esta factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }

        /// <summary>
        /// Moneda utilizada en la factura.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public Currency? Currency { get; set; }
    }
}