using Microsoft.EntityFrameworkCore;

namespace ideas.Models
{
    public class IdeaContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public IdeaContext(DbContextOptions<IdeaContext> options) : base(options) { }

	    public DbSet<User> users {get; set;}
    	public DbSet<Post> posts {get; set;}
        public DbSet<Like> likes {get; set;}
    }
}