using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmailService 
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var senderEmail = _configuration["Email:SenderEmail"];
            var username = _configuration["Email:Username"];
            var password = _configuration["Email:Password"];

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Spanish Learning Team", senderEmail!));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = body
            };

            // שימוש בשם המלא של הלקוח כדי למנוע התנגשות
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(username!, password!);

            // שליחת ההודעה
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}