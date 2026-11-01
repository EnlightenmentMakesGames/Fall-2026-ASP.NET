using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace AuthDemo.Pages {
    [Authorize]
    public class AdminModel : PageModel {
        public void OnGet() {

        }
    }
}