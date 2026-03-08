using Microsoft.AspNetCore.Mvc;
using Portfoliowebsite.Services;

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
            [FromForm] string? Subject,      // Nullable - optioneel
            [FromForm] string Message,
            [FromForm] string? website)      // Honeypot - nullable
        {
            // Honeypot check (niet valideren, alleen checken of ingevuld)
            if (!string.IsNullOrEmpty(website))
            {
                return BadRequest();
            }

            // Handmatige validatie
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

            try
            {
                await _email.SendAsync(Name, Email, Subject ?? "", Message);

                TempData["ThanksName"] = Name;
                TempData["ThanksEmail"] = Email;
                TempData["ThanksMessage"] = Message;

                return RedirectToAction(nameof(Thanks));
            }
            catch
            {
                ModelState.AddModelError("", "Er ging iets mis. Probeer het later opnieuw.");
                return View();
            }
        }

        public IActionResult Thanks() => View();
    }
}
