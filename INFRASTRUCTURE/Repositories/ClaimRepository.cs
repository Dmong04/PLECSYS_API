using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using INFRASTRUCTURE.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories
{
    public class ClaimRepository(AppDBContext _ctx) : IClaimRepository
    {
        public async Task<List<Claim>> GetAllClaims()
        {
            return await _ctx.Claims.Include(r => r.User)
                .Include(r => r.Invoice).ToListAsync();
        }

        public async Task<Claim> GetClaimById(int claim_id)
        {
            return await _ctx.Claims.Include(r => r.User)
                .Include(r => r.Invoice).FirstOrDefaultAsync(r => r.Claim_id == claim_id);
        }

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
