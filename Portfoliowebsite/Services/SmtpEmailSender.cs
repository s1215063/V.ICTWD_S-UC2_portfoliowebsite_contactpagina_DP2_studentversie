using System.Net;
using System.Net.Mail;

namespace Portfoliowebsite.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public async Task SendAsync(string Name, string Email, string Subject, string Message)
        {
            // verbinding leggen mailtrap SMTP server
            var smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("e05ab38ce8c9e2", "d6868a5b46ee8b")
            };

            // mail samenstellen
            var mail = new MailMessage();
            mail.From = new MailAddress("noreply@example.com", "Website");

            mail.To.Add("contact@example.com");
            mail.Subject = $"Contact: {Subject}";
            mail.Body = $"Naam: {Name}\nEmail: {Email}\nBericht:\n{Message}";

            await smtp.SendMailAsync(mail);
        }
    }
}
