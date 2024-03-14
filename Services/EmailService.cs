using MailKit.Net.Smtp;
using MimeKit;
using TonaWebApp.Interface;
namespace TonaWebApp.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Kantinan Boonchalee", "65010065@kmitl.ac.th"));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart("html")
            {
                Text = htmlMessage
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);
            await client.AuthenticateAsync("65010065@kmitl.ac.th", "3ypassisYOU");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}

