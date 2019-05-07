using DotnetCore.JwtToken.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetCore.JwtToken.Services
{
    public interface IUserFakeRepository
    {
        void AddUser(User user);
        User GetUserById(int id);

        User GetUserByUsername(string username);
        void UpdateUser(User user);
    }
}
