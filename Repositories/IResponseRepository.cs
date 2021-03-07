using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public interface IResponseRepository
    {
        void AddResponse(ResponseModel model);
        bool HasUserResponded(string username, int questionId);
        bool GetUserResponse(string username, int questionId);
        IList<ResponseModel> GetQuestionsAffirmativeResponses(int questionId);
        IList<ResponseModel> GetQuestionsRejectedResponses(int questionId);
    }
}
