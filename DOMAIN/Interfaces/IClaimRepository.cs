using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IClaimRepository
    {
        Task<List<Claim>> GetAllClaims();

        Task<Claim> GetClaimById(int claim_id);

        Task<Claim> CreateClaim(Claim claim);
    }
}
