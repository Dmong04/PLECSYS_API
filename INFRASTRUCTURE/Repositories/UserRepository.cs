using DOMAIN;
using DOMAIN.Entities;
using DOMAIN.Entities.GPS;
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
    public class UserRepository(AppDBContext _ctx) : IUserRepository
    {
        public async Task<User> RegisterUser(User user)
        {
            var created = await _ctx.Users.AddAsync(user);
            await _ctx.SaveChangesAsync();
            return await _ctx.Users.FindAsync(created.Entity.Email);
        }

        public async Task<User> GetUserById(string email)
        {
            return await _ctx.Users.FindAsync(email);
        }

        public async Task<User> VerifyUser(string email, string password)
        {
            return await _ctx.Users.Include(u => u.Linked_companies).ThenInclude(uc => uc.Company)
                .Include(u => u.Linked_processes)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public async Task<List<User>> SearchUsersByName(string query)
        {
            query = query.ToLower();
            return await _ctx.Users.Where(u => (u.Name + " " + u.First_lastname + " " + u.Second_lastname).ToLower()
            .Contains(query)).OrderBy(u => !(u.Name + " " + u.First_lastname + " " + u.Second_lastname)
            .ToLower().StartsWith(query)).ThenBy(u => u.Name).Take(3).ToListAsync();
        }
    }
}
