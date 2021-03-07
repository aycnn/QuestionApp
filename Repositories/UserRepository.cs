using QuestionApp.Data;
using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUser(User user)
        {
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }

        public bool IsUserExist(string username)
        {
            return dbContext.Users.Any(u => u.Username == username);
        }

        public User ValidUser(string username, string password)
        {
            return dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
