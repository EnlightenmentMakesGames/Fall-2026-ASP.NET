using ApiDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Data {
	public class BlogDbContext : DbContext {
		public BlogDbContext(DbContextOptions<BlogDbContext> options)
			: base(options) { }

		public DbSet<Post> Posts => Set<Post>();
	}
}