using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.User
{
    public class ResetPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 8)] 
        public string NewPassword { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; }
    }
}
