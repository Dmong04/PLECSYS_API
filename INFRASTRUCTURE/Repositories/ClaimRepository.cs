using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;

namespace INFRASTRUCTURE.Repositories
{
    /// <summary>
    /// Repositorio para la gestión de reclamos en la base de datos SQL Server.
    /// Implementa <see cref="IClaimRepository"/> usando Entity Framework Core.
    /// </summary>
    /// <param name="_ctx">Contexto de base de datos de la aplicación.</param>
    public class ClaimRepository(AppDBContext _ctx) : IClaimRepository
    {
        /// <summary>
        /// Obtiene todos los reclamos registrados en el sistema,
        /// incluyendo el usuario y la factura asociada a cada uno.
        /// </summary>
        /// <returns>Lista de todos los <see cref="Claim"/> con sus relaciones cargadas.</returns>
        public async Task<List<Claim>> GetAllClaims()
        {
            return await _ctx.Claims.Include(r => r.User)
                .Include(r => r.Invoice).ToListAsync();
        }

        /// <summary>
        /// Obtiene un reclamo específico por su identificador,
        /// incluyendo el usuario y la factura asociada.
        /// </summary>
        /// <param name="claim_id">Identificador único del reclamo a buscar.</param>
        /// <returns>
        /// El <see cref="Claim"/> con sus relaciones cargadas,
        /// o <c>null</c> si no existe ningún reclamo con ese identificador.
        /// </returns>
        public async Task<Claim> GetClaimById(int claim_id)
        {
            return await _ctx.Claims.Include(r => r.User)
                .Include(r => r.Invoice).FirstOrDefaultAsync(r => r.Claim_id == claim_id);
        }

        /// <summary>
        /// Crea un nuevo reclamo en la base de datos y retorna el registro
        /// completo con sus relaciones cargadas.
        /// </summary>
        /// <param name="claim">Objeto <see cref="Claim"/> con los datos del reclamo a crear.</param>
        /// <returns>
        /// El <see cref="Claim"/> recién creado con el usuario y la factura asociada cargados.
        /// </returns>
        public async Task<Claim> CreateClaim(Claim claim)
        {
            var created = await _ctx.Claims.AddAsync(claim);
            await _ctx.SaveChangesAsync();
            var success = await _ctx.Claims.Include(r => r.User)
                .Include(r => r.Invoice).FirstOrDefaultAsync(r => r.Claim_id == created.Entity.Claim_id);
            return success;
        }
    }
}