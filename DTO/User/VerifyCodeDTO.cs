using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class VerifyCodeDTO
    {
        public string UserEmail { get; set; }
        public string Code { get; set; }
    }
}
