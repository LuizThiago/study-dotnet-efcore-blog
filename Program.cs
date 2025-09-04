using Blog.Data;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog;

class Program
{
	static void Main(string[] args)
	{
		using var context = new BlogDataContext();

		//CreateUserAndPost(context);

		//ListPosts(context);

		//UpdatePost(context);
	}

	private static void CreateUserAndPost(BlogDataContext context)
	{
		var user = new User
		{
			Name = "Luiz",
			Email = "luiz.thiago@gmail.com",
			PasswordHash = "12345_HASH",
			Bio = "Programador .NET",
			Image = "http://luizthiago.com",
			Slug = "luiz"
		};

		var category = new Category
		{
			Name = "Backend",
			Slug = "backend"
		};

		var post = new Post
		{
			Author = user,
			Category = category,
			Title = "My first post",
			Slug = "my-first-post",
			Summary = "This is my first post",
			Body = "This is the body of my first post",
			CreateDate = DateTime.Now,
			LastUpdateDate = DateTime.Now
		};

		context.Posts.Add(post);
		context.SaveChanges();
	}

	private static void ListPosts(BlogDataContext context)
	{
		var posts = context
			.Posts
			.AsNoTracking()
			.Include(x => x.Author)
			.Include(x => x.Category)
			.OrderBy(x => x.LastUpdateDate)
			.ToList();

		foreach (var post in posts)
		{
			Console.WriteLine($"----- {post.Title} -----");
			Console.WriteLine($"created: {post.CreateDate} - last updated: {post.LastUpdateDate}");
			Console.WriteLine($"Author: {post.Author?.Name} - Category: {post.Category?.Name}");
			Console.WriteLine(post.Summary);
			Console.WriteLine(post.Body);
			Console.WriteLine();
		}
	}

	private static void UpdatePost(BlogDataContext context)
	{
		var post = context
			.Posts
			.Include(x => x.Author)
			.Include(x => x.Category)
			.OrderBy(x => x.LastUpdateDate)
			.FirstOrDefault();

		if (post != null)
		{
			post.LastUpdateDate = DateTime.Now;
			post.Author.Name = "Edited Author";

			context.Posts.Update(post);
			context.SaveChanges();
		}
	}
}