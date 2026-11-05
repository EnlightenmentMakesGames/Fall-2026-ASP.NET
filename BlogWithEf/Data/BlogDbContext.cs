using BlogWithEf.Models;
using Microsoft.EntityFrameworkCore;
namespace BlogWithEf.Data {
    public class BlogDbContext : DbContext {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) {}
        public DbSet<Post> Posts => Set<Post>();
    }
}