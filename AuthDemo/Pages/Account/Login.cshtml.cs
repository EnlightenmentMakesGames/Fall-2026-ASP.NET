using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;
using AuthDemo.Models;

namespace AuthDemo.Pages.Account {
    public class LoginModel : PageModel {
        //Class Variables
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? Error { get; set; }

        //Default OnGet Command
        public void OnGet() {

        }

        //Async Task to handle login
        public async Task<IActionResult> OnPostAsync() {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            var usersFile = Path.Combine(dataDir, "users.json");
            var users = JsonSerializer.Deserialize<List<User>>(System.IO.File.ReadAllText(usersFile)) ?? new();

            //Implement Hashing
            using var sha = SHA256.Create();
            var hash = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(Password)));

            var user = users.FirstOrDefault(u => 
                u.Username.Equals(Username, StringComparison.OrdinalIgnoreCase)
                && u.PasswordHash == hash
            );

            Console.WriteLine($"Username Entered: {Username}");
            Console.WriteLine($"User found: {user != null}");

            //Handle invalid users or passwords
            if (user == null) {
                Error = "Invalid username or password.";
                return Page();
            }

            //Issue a new claim as a cookie
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username) };
            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Cookies", principal);

            Console.WriteLine("Authentication Successful");

            return RedirectToPage("/Admin");
        }
    }
}