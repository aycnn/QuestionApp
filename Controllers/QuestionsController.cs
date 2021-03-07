using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestionApp.Models;
using QuestionApp.Repositories;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository questionRepository;

        public QuestionsController(IQuestionRepository questionRepository)
        {
            this.questionRepository = questionRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Create(Question model)
        {
            if (ModelState.IsValid)
            {
                questionRepository.AddQuestion(model);
                return RedirectToAction("List", "Questions");
            }
            return View(model);
        }

        public IActionResult List()
        {
            var questions = questionRepository.GetQuestionsWithResponses();
            return View(questions);
        }

       
        [HttpPost]

        public IActionResult DeadlineControl(int questionId)
        {
            var result = questionRepository.IsQuestionValid(questionId);
            return Json(result);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DownloadDocument(int questionId)
        {
            var questionTitle = questionRepository.GetQuestion(questionId).Title;
            WordDocument document = GenerateDocument(questionId);
            //Saves the Word document to  MemoryStream
            MemoryStream stream = new MemoryStream();
            document.Save(stream, FormatType.Docx);
            stream.Position = 0;

            //Download Word document in the browser
            return File(stream, "application/msword", $"{questionTitle}.docx");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult QuestionDetails(int questionId)
        {
            var question = questionRepository.GetQuestion(questionId);
            return View(question);
        }


        [Authorize(Roles ="Admin")]
        public IActionResult Cancel(int questionId)
        {
            questionRepository.CancelQuestion(questionId);
            return Redirect("/Questions/List");
        }

        private WordDocument GenerateDocument(int questionId)
        {
            var question = questionRepository.GetQuestion(questionId);
            WordDocument document = new WordDocument();
            //Adding a new section to the document.
            WSection section = document.AddSection() as WSection;
            //Set Margin of the section
            section.PageSetup.Margins.All = 72;
            //Set page size of the section
            section.PageSetup.PageSize = new Syncfusion.Drawing.SizeF(612, 792);
            IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
            paragraph.ApplyStyle("Normal");
            paragraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
            WTextRange textRange = paragraph.AppendText($"'{question.Title}' başlıklı soru yanıtları") as WTextRange;
            textRange.CharacterFormat.FontSize = 12f;
            textRange.CharacterFormat.FontName = "Calibri";
            textRange.CharacterFormat.TextColor = Syncfusion.Drawing.Color.Black;

            var list = question.Responses.OrderByDescending(r => r.ResponseDate);

            foreach (var response in list)
            {
                paragraph = section.AddParagraph();
                paragraph.BreakCharacterFormat.FontSize = 12f;
                textRange = paragraph.AppendText(response.Username + ": " + (response.Response == true ? "Evet" : "Hayır")) as WTextRange;
                textRange = paragraph.AppendText($"  ({response.ResponseDate})") as WTextRange;
                paragraph = section.AddParagraph();
                textRange = paragraph.AppendText("Not: " + (response.Note == null ? "-" : response.Note)) as WTextRange;
                paragraph = section.AddParagraph();
                textRange = paragraph.AppendText("----------------------") as WTextRange;
                textRange.CharacterFormat.FontSize = 12f;
            }
            section.AddParagraph();

            return document;
        }
    }
}
