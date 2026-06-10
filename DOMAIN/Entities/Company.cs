using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa una empresa registrada en el sistema.
    /// Puede actuar como compradora o vendedora en las transacciones.
    /// </summary>
    public class Company
    {
        /// <summary>
        /// Identificador único de la empresa.
        /// </summary>
        public int Company_id { get; set; }

        /// <summary>
        /// Nombre oficial de la empresa.
        /// </summary>
        public required string Company_name { get; set; }

        /// <summary>
        /// Dirección física de la empresa.
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Número de teléfono de contacto de la empresa.
        /// </summary>
        public required string Phone { get; set; }

        /// <summary>
        /// Correo electrónico de contacto de la empresa.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Colección de usuarios asociados a esta empresa.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<UserCompany>? Company_users { get; set; }

        /// <summary>
        /// Colección de proveedores vinculados a esta empresa.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Supplier>? Suppliers { get; set; }

        /// <summary>
        /// Colección de órdenes de venta generadas por esta empresa.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<SaleOrder>? Orders { get; set; }

        /// <summary>
        /// Colección de facturas en las que esta empresa figura como compradora.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Invoice>? Charged_invoices { get; set; }

        /// <summary>
        /// Colección de facturas en las que esta empresa figura como vendedora.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Invoice>? Sell_invoices { get; set; }
    }
}