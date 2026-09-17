using ContactApp.Models;
using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContactApp.Pages; 
public class ContactModel : PageModel {
    //Create Form Variables
    private readonly ContactStore _store;

    [BindProperty]
    public Contact Form { get; set; }

    public ContactModel(ContactStore store) => _store = store;

    //Form Logic to Load
    public void OnGet() { /* display empty form */ }

    //Form Logic to Post
    public IActionResult OnPost() {
        //Validate the model
        if (!ModelState.IsValid) {
            //Redisplay the form with validation messages
            return Page();
        }

        //Add the Submission to the Store
        Form.SubmittedUtc = DateTime.UtcNow;
        _store.Add(Form);

        //Redirect to the thank you page
        return RedirectToPage("/ThankYou");
    }
}