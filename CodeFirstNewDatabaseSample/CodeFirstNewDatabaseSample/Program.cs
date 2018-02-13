using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstNewDatabaseSample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
           
            using (var db = new BloggingContext())
            {

                // Create and save a new Blog 
                //Console.Write("Enter a name for a new Blog: ");
                //var name = Console.ReadLine();

                //var blog = new Blog {Name = name};

                //db.Blogs.Add(blog);
                //db.SaveChanges();

                //Create and save a new organization
                Console.WriteLine("Enter a name for a new organization");
                var orgname = Console.ReadLine();
                var organization = new Organization { OrganizationName = orgname };

                db.Organizations.Add(organization);
                db.SaveChanges();

                //create a user for
                Console.WriteLine("Enter a username");
                var username = Console.ReadLine();
                var user = new User { Username = username, Organization = organization };

                db.Users.Add(user);
                db.SaveChanges();

                // Display all Blogs from the database 
                //var query = from b in db.Blogs
                //    orderby b.Name
                //    select b;

                //Console.WriteLine("All blogs in the database:");
                //foreach (var item in query)
                //{
                //    Console.WriteLine(item.Name);
                //}

                var userQuery = from u in db.Users
                    orderby u.Username
                    select u;

                Console.WriteLine("All users in the database");
                foreach (var VARIABLE in userQuery)
                {
                    Console.WriteLine(VARIABLE.Username + " belongs to " + VARIABLE.Organization);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public class Blog
        {
            public int BlogId { get; set; }

            public string Name { get; set; }

            public string Url { get; set; }

            /*You’ll notice that we’re making the two navigation properties (Blog.Posts and Post.Blog) virtual. This enables the Lazy Loading feature of Entity Framework.
             Lazy Loading means that the contents of these properties will be automatically loaded from the database when you try to access them.*/
            public virtual List<Post> Posts { get; set; }
        }

        public class Post
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }

            public int BlogId { get; set; }
            public virtual Blog Blog { get; set; }
        }

        public class BloggingContext : DbContext
        {
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<User> Users { get; set; }
            public DbSet<Organization> Organizations { get; set; }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>()
                    .Property(u => u.DisplayName)
                    .HasColumnName("display_name");
            }
        }

        public class User
        {
            [Key]
            public string Username { get; set; }
            public string DisplayName { get; set; }
            public virtual Organization Organization{ get; set; }
        }

        public class Organization
        {
            [Key]
            public string OrganizationName { get; set; }
            //test
        }
    }
}