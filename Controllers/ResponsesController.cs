using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuestionApp.Extensions;
using QuestionApp.Models;
using QuestionApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QuestionApp.Controllers
{
    public class ResponsesController : Controller
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IResponseRepository responseRepository;
        private readonly IEmailService emailService;

        public ResponsesController(IQuestionRepository questionRepository, IResponseRepository responseRepository, IEmailService emailService)
        {
            this.questionRepository = questionRepository;
            this.responseRepository = responseRepository;
            this.emailService = emailService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Create(int questionId)
        {
            ResponseModel response = new ResponseModel
            {
                Question = questionRepository.GetQuestion(questionId),
                Username = User.GetUsername(),
                ResponseDate = DateTime.Now


            };
            return View(response);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ResponseModel model)
        {
            ResponseModel res = new ResponseModel
            {
                QuestionId = model.QuestionId,
                Username = model.Username,
                Response = model.Response,
                ResponseDate = model.ResponseDate,
                Note = model.Note
            };

            if (ModelState.IsValid)
            {
                responseRepository.AddResponse(res);
                if (res.Response)
                {
                    string questionTitle = questionRepository.GetQuestion(res.QuestionId).Title;
                    emailService.SendEmailForAffirmative(res.Username, questionTitle, res.Note);
                    if (IsRequiredProvided(res.QuestionId)) emailService.SendEmailForReachedNumberOfAffirmatives(res.Question.Title);
                }
            }
            return Redirect("/Questions/List");
        }

       


        private bool IsRequiredProvided(int questionId)
        {
            return questionRepository.IsQuestionApproved(questionId);
        }
       

        public IActionResult Affirmatives(int questionId)
        {
            var responseList = responseRepository.GetQuestionsAffirmativeResponses(questionId);
            return View(responseList);
        }
        public IActionResult Negatives(int questionId)
        {
            var responseList = responseRepository.GetQuestionsRejectedResponses(questionId);
            return View(responseList);
        }

        [HttpPost]
        public IActionResult ResponseControl(int questionId)
        {
            string username = User.GetUsername();
            var result = responseRepository.HasUserResponded(username, questionId);
            return Json(result);
        }


        [HttpPost]
        public IActionResult MyResponse(int questionId)
        {
            string username = User.GetUsername();
            var result = responseRepository.GetUserResponse(username, questionId);
            if (result)
            {
                return Json("Evet");
            }
            return Json("Hayır");
        }
    }
}
