using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa una moneda utilizada en las transacciones del sistema.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Identificador único de la moneda.
        /// </summary>
        public int Currency_id { get; set; }

        /// <summary>
        /// Código ISO de la moneda (ej. CRC, USD, EUR).
        /// </summary>
        public required string Currency_ISO { get; set; }

        /// <summary>
        /// Código abreviado de la moneda (ej. ₡, $, €).
        /// </summary>
        public required string Currency_code { get; set; }

        /// <summary>
        /// Nombre completo de la moneda (ej. Colón Costarricense, Dólar Estadounidense).
        /// </summary>
        public required string Currency_name { get; set; }

        /// <summary>
        /// Colección de registros de pago realizados con esta moneda.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }

        /// <summary>
        /// Colección de facturas asociadas a esta moneda.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Invoice>? Invoices { get; set; }
    }
}