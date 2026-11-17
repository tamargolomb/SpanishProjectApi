using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{

    public class LoginUserDTO
    {
        [Required(ErrorMessage = "user email is required")]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "user password is required")]
        public string UserPassword { get; set; } = string.Empty;
    }

}
