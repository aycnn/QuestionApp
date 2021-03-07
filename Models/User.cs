using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.Models
{
    public class User
    {
        [Key]
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        [MinLength(5, ErrorMessage = "Şifreniz en az 5 karakterden oluşmalı.")]
        public string Password { get; set; }
        public UserType Role { get; set; }
        public IList<ResponseModel> Responses { get; set; }

        public User()
        {
            this.Role = UserType.Member;
        }
    }
}
