using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Guldrullen.Models
{
    public class UsersRegisterViewModel
    {
        [Display(Name = "User name")]
        [Required(ErrorMessage = "*Required")]
        [MaxLength(50, ErrorMessage ="Max length is 50 characters")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "*Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
