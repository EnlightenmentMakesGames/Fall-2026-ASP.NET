using System.ComponentModel.DataAnnotations;
using MicroBlog.Services;
using MicroBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Pages {
    public class CreateModel : PageModel {
        private readonly PostStore _store;
        public CreateModel(PostStore store) => _store = store;

        [BindProperty]
        public InputModel Form { get; set; } = new();

        public class InputModel {
            [Required, StringLength(100)]
            public string Title { get; set; } = string.Empty;

            [Required]
            public string Body { get; set; } = string.Empty;
        }

        public void OnGet() { }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }
            var post = new Post {
                Title = Form.Title.Trim(),
                Body = Form.Body.Trim()
            };
            post = _store.Add(post);
            return RedirectToPage("/Details", new {id = post.Id});
        }
    }
}