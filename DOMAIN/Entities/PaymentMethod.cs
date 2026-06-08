using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un método de pago disponible en el sistema.
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Identificador único del método de pago.
        /// </summary>
        public int Payment_method_id { get; set; }

        /// <summary>
        /// Nombre descriptivo del método de pago (ej. Transferencia, Tarjeta, Efectivo).
        /// </summary>
        public required string Payment_method_name { get; set; }

        /// <summary>
        /// Código numérico que identifica el método de pago para integraciones externas.
        /// </summary>
        public required int Payment_method_code { get; set; }

        /// <summary>
        /// Colección de registros de pago que utilizaron este método.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<PaymentRecord>? Payment_records { get; set; }
    }
}