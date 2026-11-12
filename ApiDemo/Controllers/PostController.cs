using ApiDemo.Data;
using ApiDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase {
        //Class variables
        private readonly BlogDbContext _db;

        //Constructor
        public PostsController(BlogDbContext db) => _db = db;

        //APIs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAll() {
            var posts = await _db.Posts
                .OrderByDescending(p => p.CreatedUtc)
                .ToListAsync();
            return Ok(posts);
        }

        // GET: /api/posts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetById(int id) {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return Ok(post);
        }

        // POST: /api/posts
        [HttpPost]
        public async Task<ActionResult<Post>> Create(Post post) {
            //Validate the model
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //Create and save the post
            post.CreatedUtc = DateTime.UtcNow;
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            //Return 201 Created
            return CreatedAtAction(nameof(GetById), new {id = post.Id}, post);
        }
    }
}