using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Models
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        [Display(Name = "Kullanıcı")]
        public string Username { get; set; }
        public User User { get; set; }

        [Required]
        [Display(Name = "Yanıtınız")]
        public bool Response { get; set; }

        [Display(Name = "Not")]
        public string Note { get; set; }

        [Display(Name = "Tarih")]
        public DateTime ResponseDate { get; set; }
    }
}
