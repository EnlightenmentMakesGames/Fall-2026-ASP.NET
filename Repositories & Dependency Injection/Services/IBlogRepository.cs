using MicroBlog.Models;

namespace MicroBlog.Services {
    public interface IBlogRepository {
        IEnumerable<Post> GetAll();
        Post? GetById(int id);
        void Add(Post post);
        void Save();
    }
}