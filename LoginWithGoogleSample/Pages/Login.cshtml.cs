using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginWithGoogleSample.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Credential { get; set; }

        [BindProperty(Name = "g_csrf_token")]
        public string G_CSRF_Token { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var csrfTokenCookie = Request.Cookies["g_csrf_token"];
            Console.WriteLine("CSRF Token from Cookie: " + csrfTokenCookie);

            if (string.IsNullOrEmpty(csrfTokenCookie))
            {
                return BadRequest("No CSRF token in Cookie.");
            }

            if (string.IsNullOrEmpty(G_CSRF_Token))
            {
                return BadRequest("No CSRF token in post body.");
            }

            Console.WriteLine("CSRF Token from Body: " + G_CSRF_Token);

            if (csrfTokenCookie != G_CSRF_Token)
            {
                return BadRequest("Failed to verify double submit cookie.");
            }

            // Your logic to handle the JWT token after verifying CSRF tokens
            // For example, validate the Credential (JWT token) with Google's API

            return RedirectToPage("/Index");
        }
    }
}