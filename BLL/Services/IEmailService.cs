using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public interface IEmailService
    {
        //void SendEmail(string to, string subject, string body);
         Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
