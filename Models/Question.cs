using QuestionApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Models
{
    public class Question
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "Geçerli bir tarih giriniz.")]
        [Display(Name = "Son Geçerlilik Tarihi")]
        public DateTime Deadline { get; set; }

        public DateTime CreateDate { get; set; }

        [Required]
        [Display(Name = "Gereken Onay Sayısı")]
        [Range(0, int.MaxValue, ErrorMessage = "Onay sayısı negatif olamaz.")]
        public int NumberofAffirmativesRequired { get; set; }
        public IList<ResponseModel> Responses { get; set; }
        public bool IsActive { get; set; }

        public Question()
        {
            this.CreateDate = DateTime.Now;
            this.IsActive = true;
        }
    }
}
