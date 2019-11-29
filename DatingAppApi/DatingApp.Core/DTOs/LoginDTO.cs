using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DatingApp.Core.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "This field is required")]
        public string username { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }
    }
}
