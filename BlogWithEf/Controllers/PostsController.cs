using BlogWithEf.Data;
using BlogWithEf.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogWithEf.Controllers {
    public class PostsController : Controller {
        private readonly BlogDbContext _db;

        //Constructor
        public PostsController(BlogDbContext db) => _db = db;

        // GET: /Posts
        public async Task<IActionResult> Index() {
            var posts = await _db.Posts
                .OrderByDescending(p => p.CreatedUtc)
                .ToListAsync();
            return View(posts);
        }

        // GET: /Posts/Details/5
        public async Task<IActionResult> Details(int id) {
            var post = await _db.Posts.FindAsync(id);
            if (post is null) return NotFound();
            return View(post);
        }

        // GET: /Posts/Create
        public IActionResult Create() => View();

        // POST: /Posts/Create
        [HttpPost]
        public async Task<IActionResult> Create(Post post) {
            if (!ModelState.IsValid) return View(post);

            post.CreatedUtc = DateTime.UtcNow;
            _db.Add(post);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Posts/Edit/5
        public async Task<IActionResult> Edit(int id) {
            var post = await _db.Posts.FindAsync(id);
            if (post is null) return NotFound();
            return View(post);
        }

        // POST: /Posts/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Post form) {
            //Filter for bad changes
            if (id != form.Id) return BadRequest();
            if (!ModelState.IsValid) return View(form);

            //Try to update, catch if it fails
            try {
                _db.Entry(form).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch (DbUpdateConcurrencyException) {
                if (!await _db.Posts.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }
        }

        // GET: /Posts/Delete/5
        public async Task<IActionResult> Delete(int id) {
            var post = await _db.Posts.FindAsync(id);
            if (post is null) return NotFound();
            return View(post);
        }

        // POST /Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var post = await _db.Posts.FindAsync(id);
            if (post is not null) {
                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}