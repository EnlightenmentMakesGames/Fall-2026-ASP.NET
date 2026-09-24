using MicroBlog.Services;
using MicroBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Pages;

public class IndexModel : PageModel
{

    private readonly PostStore _store;
    public List<Post> Posts { get; private set; } = new();
    public IndexModel(PostStore store) => _store = store;

    public void OnGet()
    {
        Posts = _store.GetAll().ToList();
    }
}
