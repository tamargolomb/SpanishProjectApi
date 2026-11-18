using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.User
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;
    }
}
