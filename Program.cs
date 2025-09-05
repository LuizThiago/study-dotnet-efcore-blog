using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog;

class Program
{
	static async Task Main(string[] args)
	{
		using var context = new Data.BlogDataContext();

		// CREATE A USER
		// CreateUser("Luiz", "luiz.thiago@gmail.com", "HASH");
		// var user = GetFirstUser();
		// if (user != null) Console.WriteLine($"User: {user.Name}");

		// CREATE A CATEGORY
		// CreateCategory("Backend");
		// var category = GetFirstCategory();
		// if (category != null) Console.WriteLine($"Category: {category.Name}");

		// CREATE A POST
		// var user = GetFirstUser(context);
		// var category = GetFirstCategory(context);
		// CreatePost(context,
		// 	"My first post",
		// 	"This is the body of my first post",
		// 	"Summary of my first post",
		// 	user,
		// 	category);

		// READ AND PRINT POSTS
		var posts = await GetPostsAsync(context);
		foreach (var post in posts) PrintPost(post);
	}

	#region Post CRUD

	public static async Task<IEnumerable<Post>> GetPostsAsync(Data.BlogDataContext context, int skip = 0, int take = 25)
	{
		return await context.Posts
			.AsNoTracking()
			.Skip(skip)
			.Take(take)
			.Include(x => x.Author)
			.Include(x => x.Category)
			.ToListAsync();
	}

	public static void CreatePost(Data.BlogDataContext context, string title, string body,
		string summary, User author, Category category)
	{
		var post = new Post
		{
			Title = title,
			Body = body,
			Author = author,
			Summary = summary,
			Category = category,
			LastUpdateDate = DateTime.Now,
			Slug = title.ToLower().Replace(" ", "-"),
		};

		context.Posts.Add(post);
		context.SaveChanges();
	}

	public static void PrintPost(Post post)
	{
		if (post == null) return;

		var user = post.Author;
		var category = post.Category;

		Console.WriteLine($"---------Post: {post.Title} --------------------------------------------");
		Console.WriteLine($"Author: {post.Author?.Name} - Category: {post.Category?.Name}");
		Console.WriteLine($"CreateDate: {post.CreateDate} - LastUpdateDate: {post.LastUpdateDate}");
		Console.WriteLine($"Summary: {post.Summary}");
		Console.WriteLine($"Body: {post.Body}");
		Console.WriteLine("-----------------------------------------------------");
	}

	#endregion

	#region User CRUD

	public static User GetFirstUser(Data.BlogDataContext context)
	{
		return context.Users.FirstOrDefault()!;
	}

	public static User? GetUserById(Data.BlogDataContext context, int id)
	{
		return context.Users.FirstOrDefault(x => x.Id == id);
	}

	public static void CreateUser(Data.BlogDataContext context, string name, string email, string passwordHash)
	{
		var user = new User
		{
			Name = name,
			Email = email,
			PasswordHash = passwordHash,
			Bio = string.Empty,
			Image = string.Empty,
			Slug = name.ToLower().Replace(" ", "-")
		};

		context.Users.Add(user);
		context.SaveChanges();
	}

	#endregion

	#region Category CRUD

	public static void CreateCategory(Data.BlogDataContext context, string name)
	{
		var category = new Category
		{
			Name = name,
			Slug = name.ToLower().Replace(" ", "-")
		};

		context.Categories.Add(category);
		context.SaveChanges();
	}

	public static Category GetFirstCategory(Data.BlogDataContext context)
	{
		return context.Categories.FirstOrDefault()!;
	}

	public static Category? GetCategoryById(Data.BlogDataContext context, int id)
	{
		return context.Categories.FirstOrDefault(x => x.Id == id);
	}

	#endregion
}