
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var senderEmail = _configuration["EmailSettings:SenderEmail"];
                var password = _configuration["EmailSettings:Password"];
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = _configuration.GetValue<int>("EmailSettings:Port");

                if (string.IsNullOrWhiteSpace(senderEmail) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(smtpServer) ||
                    smtpPort <= 0)
                {
                    throw new InvalidOperationException("Email settings are missing.");
                }

                var email = new MimeMessage();

                email.From.Add(
                    new MailboxAddress("HolaHub", senderEmail));

                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;

                email.Body = new TextPart("plain")
                {
                    Text = body
                };


                using var smtp = new SmtpClient();

                Console.WriteLine("STEP 1: Connecting to SMTP...");

                await smtp.ConnectAsync(
                   smtpServer,
                   smtpPort,
                   SecureSocketOptions.SslOnConnect);

                Console.WriteLine("STEP 2: Connected successfully");

                Console.WriteLine("STEP 3: Authenticating...");

                await smtp.AuthenticateAsync(senderEmail, password);

                Console.WriteLine("STEP 4: Authentication successful");

                Console.WriteLine("STEP 5: Sending email...");

                await smtp.SendAsync(email);

                Console.WriteLine("STEP 6: Email accepted by SMTP server");

                await smtp.DisconnectAsync(true);

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }
    }
}
