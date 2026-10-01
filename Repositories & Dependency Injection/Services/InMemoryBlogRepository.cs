using MicroBlog.Models;

namespace MicroBlog.Services {
    public class InMemoryBlogRepository : IBlogRepository {
        private readonly List<Post> _posts = new();
        private int _nextId = 1;

        public IEnumerable<Post> GetAll() => _posts.OrderByDescending(p => p.Id);
        public Post? GetById(int id) => _posts.FirstOrDefault(p => p.Id == id);
        public void Add(Post post) {
            post.Id = _nextId++;
            post.CreatedUtc = DateTime.UtcNow;
            _posts.Add(post);
        }
        public void Save() {
            /* unusable for storing in RAM */
        }
    }
}