using System.ComponentModel.DataAnnotations.Schema;

namespace DOMAIN.Entities
{
    /// <summary>
    /// Representa la relación muchos a muchos entre usuarios y empresas.
    /// Permite asociar un usuario a múltiples empresas y viceversa.
    /// </summary>
    public class UserCompany
    {
        /// <summary>
        /// Identificador del usuario vinculado a la empresa.
        /// </summary>
        [ForeignKey("user_id")]
        [Column("user_id")]
        public required string User_id { get; set; }

        /// <summary>
        /// Identificador de la empresa vinculada al usuario.
        /// </summary>
        [ForeignKey("company_id")]
        [Column("company_id")]
        public int Company_id { get; set; }

        /// <summary>
        /// Usuario asociado a esta relación.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Empresa asociada a esta relación.
        /// </summary>
        public Company? Company { get; set; }
    }
}