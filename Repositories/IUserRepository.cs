using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public interface IUserRepository
    {
        User ValidUser(string username, string password);
        void AddUser(User user);
        bool IsUserExist(string username);
    }
}
