using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DTOs
{
    // I used DisplayName because I want to display a correct and meaningful name in Swagger Schemas
    [DisplayName("Register")]
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                           + "@"
                           + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", 
                            ErrorMessage = "The Email field is not a valid e-mail address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.{8,16}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9]).*$", ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 Number, 1 non Alphanumeric and at least 6 characters.")]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
