using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data;

public class BlogDataContext : DbContext
{
	public DbSet<Category> Categories { get; set; }
	public DbSet<Post> Posts { get; set; }
	public DbSet<User> Users { get; set; }

	override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
		optionsBuilder.UseSqlServer("Server=localhost,1433;Database=FluentBlog;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;");
}