using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionApp.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {

            DateTime datetime = (DateTime)value;
            return (datetime > DateTime.Now);

        }
    }
}
