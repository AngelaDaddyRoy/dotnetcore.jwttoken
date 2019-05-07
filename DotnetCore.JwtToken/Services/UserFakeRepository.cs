using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetCore.JwtToken.Models;

namespace DotnetCore.JwtToken.Services
{
    public class UserFakeRepository : IUserFakeRepository
    {
        private List<User> users = new List<User>();
        private int id = 1;
        public void AddUser(User user)
        {
            user.Id = id;
            users.Add(user);
            id++;
        }

        public User GetUserById(int id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public User GetUserByUsername(string username)
        {
            return users.FirstOrDefault(x => x.Username == username);
        }

        public void UpdateUser(User user)
        {
             var target = users.FirstOrDefault(x => x.Id == user.Id);
            if (target !=null)
            {
                users[users.IndexOf(target)] = user;
            }
        }
    }
}
