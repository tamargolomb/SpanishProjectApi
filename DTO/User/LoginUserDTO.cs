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
        [StringLength(20, MinimumLength = 2, ErrorMessage = "user password must be between 2 and 20 characters")]
        public string UserPassword { get; set; } = string.Empty;
    }

}
