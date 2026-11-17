using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
