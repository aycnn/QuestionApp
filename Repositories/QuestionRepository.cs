using Microsoft.EntityFrameworkCore;
using QuestionApp.Data;
using QuestionApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public QuestionRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void AddQuestion(Question question)
        {
            dbContext.Questions.Add(question);
            dbContext.SaveChanges();
        }

        public Question GetQuestion(int id)
        {
            return dbContext.Questions.Include(s => s.Responses).FirstOrDefault(s => s.Id == id);
        }

        public IList<Question> GetQuestions()
        {
            return dbContext.Questions.ToList();
        }

        public IList<Question> GetQuestionsWithResponses()
        {
            return dbContext.Questions.Include(s => s.Responses)
                 .OrderByDescending(s => s.CreateDate)
                 .Where(s => s.IsActive)
                 .ToList();

        }

        public bool IsQuestionApproved(int id)
        {
            var survey = GetQuestion(id);
            return survey.Responses.Where(r => r.Response).Count() == survey.NumberofAffirmativesRequired;
        }

        public bool IsQuestionValid(int id)
        {
            return dbContext.Questions.First(s => s.Id == id).Deadline > DateTime.Now;
        }

        public void CancelQuestion(int id)
        {
            var survey = GetQuestion(id);
            survey.IsActive = false;
            dbContext.Entry(survey).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
    }
}
