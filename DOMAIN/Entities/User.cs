using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa un usuario registrado en el sistema.
    /// Actúa como actor principal en facturas, órdenes, reclamos y flujos de trabajo.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Correo electrónico del usuario. Clave primaria y credencial de acceso al sistema.
        /// </summary>
        [Key]
        public required string Email { get; set; }

        /// <summary>
        /// Nombre de pila del usuario.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Primer apellido del usuario.
        /// </summary>
        public required string First_lastname { get; set; }

        /// <summary>
        /// Segundo apellido del usuario.
        /// </summary>
        public required string Second_lastname { get; set; }

        /// <summary>
        /// Número de teléfono de contacto del usuario.
        /// </summary>
        public required string Phone { get; set; }

        /// <summary>
        /// Contraseña del usuario almacenada de forma segura.
        /// Nula si el usuario aún no ha configurado su contraseña.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Fecha y hora en que fue creado el usuario en el sistema.
        /// Nula si no fue registrada al momento de la creación.
        /// </summary>
        public DateTime? Created_at { get; set; }

        /// <summary>
        /// Colección de empresas a las que está vinculado el usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<UserCompany>? Linked_companies { get; set; }

        /// <summary>
        /// Colección de flujos de trabajo asignados a este usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<SmartFlow>? Linked_processes { get; set; }

        /// <summary>
        /// Flujo de trabajo en el que este usuario figura como aprobador.
        /// Ignorado en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public SmartFlow? Approved_Process { get; set; }

        /// <summary>
        /// Colección de facturas creadas por este usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Invoice>? Invoices { get; set; }

        /// <summary>
        /// Colección de órdenes de venta generadas por este usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<SaleOrder>? Orders { get; set; }

        /// <summary>
        /// Colección de reclamos realizados por este usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<Claim>? Claims { get; set; }

        /// <summary>
        /// Colección de entradas del historial de facturas generadas por este usuario.
        /// Ignorada en la serialización JSON para evitar referencias circulares.
        /// </summary>
        [JsonIgnore]
        public ICollection<InvoiceHistory>? Invoice_histories { get; set; }
    }
}