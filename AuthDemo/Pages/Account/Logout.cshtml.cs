using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace AuthDemo.Pages.Account {
    public class LogoutModel : PageModel {
        public async Task<IActionResult> OnGetAsync() {
            await HttpContext.SignOutAsync("Cookies");
            return RedirectToPage("/Admin");
        }
    }
}