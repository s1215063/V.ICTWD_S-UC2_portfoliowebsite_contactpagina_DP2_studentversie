using Microsoft.AspNetCore.Mvc;
using Portfoliowebsite.Services;
using System.Text.RegularExpressions;
using System.Web;

namespace Portfoliowebsite.Controllers
{
    public class ContactController : Controller
    {
        private readonly IEmailSender _email;

        public ContactController(IEmailSender email) => _email = email;

        public IActionResult Index() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(
            [FromForm] string Name,
            [FromForm] string Email,
            [FromForm] string? Subject,
            [FromForm] string Message,
            [FromForm] string? website)
        {
            if (!string.IsNullOrEmpty(website))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(Name) || Name.Length < 2)
            {
                ModelState.AddModelError("Name", "Naam moet minimaal 2 tekens zijn");
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.Contains("@"))
            {
                ModelState.AddModelError("Email", "Voer een geldig e-mailadres in");
            }

            if (string.IsNullOrWhiteSpace(Message) || Message.Length < 10)
            {
                ModelState.AddModelError("Message", "Bericht moet minimaal 10 tekens zijn");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            // Sanitize alle input
            var safeName = SanitizeInput(Name);
            var safeSubject = SanitizeInput(Subject ?? "");
            var safeMessage = SanitizeInput(Message);

            try
            {
                await _email.SendAsync(safeName, Email, safeSubject, safeMessage);

                // Sanitized data opslaan
                TempData["ThanksName"] = safeName;
                TempData["ThanksEmail"] = Email;
                TempData["ThanksMessage"] = safeMessage;

                return RedirectToAction(nameof(Thanks));
            }
            catch
            {
                ModelState.AddModelError("", "Er ging iets mis. Probeer het later opnieuw.");
                return View();
            }
        }

        // Sanitization methode
        private static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Verwijder HTML tags
            var noHtml = Regex.Replace(input, "<[^>]*>", string.Empty);
            
            // HTML encode speciale karakters
            var encoded = HttpUtility.HtmlEncode(noHtml);
            
            return encoded.Trim();
        }

        public IActionResult Thanks() => View();
    }
}
