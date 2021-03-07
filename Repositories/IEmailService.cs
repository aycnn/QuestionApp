using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public interface IEmailService
    {
        void SendEmailForAffirmative(string username, string questionTitle, string note);
        void SendEmailForReachedNumberOfAffirmatives(string questionTitle);
    }
}
