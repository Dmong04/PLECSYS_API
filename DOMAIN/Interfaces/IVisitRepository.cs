using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IVisitRepository
    {
        Task<List<Visit>> GetAllVisits();

        Task<Visit> GetVisitById(int visit_id);
    }
}
