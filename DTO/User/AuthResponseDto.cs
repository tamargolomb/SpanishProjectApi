using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }
}
