using MicroBlog.Services;
using MicroBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MicroBlog.Pages;

public class IndexModel : PageModel {
    private readonly IBlogRepository _repo;
    public List<Post> Posts { get; private set; } = new();
    public IndexModel(IBlogRepository repo) => _repo = repo;

    public void OnGet(){
        Posts = _repo.GetAll().ToList();
    }
}
