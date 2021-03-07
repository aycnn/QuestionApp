using QuestionApp.Data;
using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public class ResponseRepository : IResponseRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ResponseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddResponse(ResponseModel model)
        {
            dbContext.Responses.Add(model);
            dbContext.SaveChanges();
        }

        public IList<ResponseModel> GetQuestionsAffirmativeResponses(int questionId)
        {
            return dbContext.Responses.Where(r => r.QuestionId == questionId && r.Response).ToList();
        }

        public IList<ResponseModel> GetQuestionsRejectedResponses(int questionId)
        {
            return dbContext.Responses.Where(r => r.QuestionId == questionId && !r.Response).ToList();
        }

        public bool GetUserResponse(string username, int questionId)
        {
            return dbContext.Responses.First(r => r.Username == username && r.QuestionId == questionId).Response;
        }


        public bool HasUserResponded(string username, int questionId)
        {
            return dbContext.Responses.Any(r => r.Username == username && r.QuestionId == questionId);
        }
    }
}
