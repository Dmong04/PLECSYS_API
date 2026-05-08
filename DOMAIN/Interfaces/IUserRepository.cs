using DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOMAIN.Interfaces
{
    public interface IUserRepository
    {
        Task<User> VerifyUser(string email, string password);

        Task<User> RegisterUser(User user);

        Task<User> GetUserById(string email);

        Task<List<User>> SearchUsersByName(string query);
    }
}
