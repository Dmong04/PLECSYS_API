using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFRASTRUCTURE.Repositories
{
    public class VisitRepository : IVisitRepository
    {
        public Task<List<Visit>> GetAllVisits()
        {
            throw new NotImplementedException();
        }

        public Task<Visit> GetVisitById(int visit_id)
        {
            throw new NotImplementedException();
        }
    }
}
