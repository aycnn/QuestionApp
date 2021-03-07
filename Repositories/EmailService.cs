using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QuestionApp.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;
        public EmailService(IConfiguration config)
        {
            this.config = config;
        }
        public void SendEmailForAffirmative(string username, string questionTitle, string note)
        {
            MailMessage myMail = PrepareMail(questionTitle);
            myMail.Body = $"'{username}' adlı kullanıcı '{questionTitle}' başlıklı sorunuza olumlu yanıt verdi!<br> Not: {note}";
            SmtpSettings().Send(myMail);

        }
        public void SendEmailForReachedNumberOfAffirmatives(string questionTitle)
        {
            MailMessage myMail = PrepareMail(questionTitle);
            myMail.Body = $"'{questionTitle}' başlıklı soru gereken onay sayısına ulaştı!";
            SmtpSettings().Send(myMail);
        }

        private SmtpClient SmtpSettings()
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(config["Eposta"], config["EpostaPassword"]);
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            return smtp;
        }

        private MailMessage PrepareMail(string questionTitle)
        {
            MailMessage myMail = new MailMessage();
            myMail.To.Add(config["AdminEposta"]);
            myMail.From = new MailAddress(config["Eposta"], "QuestionApp");
            myMail.Subject = questionTitle;
            myMail.IsBodyHtml = true;
            return myMail;
        }
    }
}
