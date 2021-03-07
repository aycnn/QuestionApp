using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public interface IQuestionRepository
    {
        void AddQuestion(Question question);
        IList<Question> GetQuestions();
        Question GetQuestion(int id);
        bool IsQuestionValid(int id);

        IList<Question> GetQuestionsWithResponses();
        bool IsQuestionApproved(int id);
        void CancelQuestion(int id);
    }
}
