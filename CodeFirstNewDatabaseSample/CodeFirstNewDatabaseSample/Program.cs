using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace CodeFirstNewDatabaseSample
{


    internal class Program
    {
        
        public class BlogLogic
        {
            private BloggingContext db = new BloggingContext();
            public void createAndSaveBlock()
            {
                
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();
                var blog = new Blog { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();
            }

            public void DisplayBlogs()
            {
                //Display all Blogs from the database
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }

        public class OrganizationLogic
        {
            private BloggingContext db = new BloggingContext();

            public void MakeOrganization()
            {
                Console.WriteLine("write the organization name");
                string orgname = Console.ReadLine();
                Organization temp = db.Organizations.Find(orgname);
                //search org or add
                if (temp == null)
                {
                    temp = db.Organizations.Add(new Organization {OrganizationName = orgname});
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("organization already exists!");
                }
                
            }

            public void NewUserForOrg()
            {
                Console.WriteLine("type username");
                var username = Console.ReadLine();
                Console.WriteLine("type organization name");
                var orgname = Console.ReadLine();
                var org = db.Organizations.Find(orgname);
                if (org != null)
                {
                    var user = new User { Username = username, Organization = org };
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("no such organization, feel free to try again");
                }
            }

            public void PrintOrganizaionsAndTheirUsers()
            {
                var userQuery = from u in db.Users.Include("Organization")
                                orderby u.Organization.OrganizationName
                                select u;

                Console.WriteLine("All users in the database, and the organization they belong to");
                foreach (var VARIABLE in userQuery)
                {
                    Console.WriteLine(VARIABLE.Organization.OrganizationName +"\t\t" + VARIABLE.Username + " belongs to ");
                }
               
            }

            public void PrintOrganizations()
            {
                var orgQuery = from o in db.Organizations
                    orderby o.OrganizationName
                    select o;

                Console.WriteLine("All organizations in the database");

                foreach (var VARIABLE in orgQuery)
                {
                    Console.WriteLine(VARIABLE.OrganizationName);
                }
            }

        }

        public class CountryLogic
        {
            private BloggingContext db = new BloggingContext();

            public void PrintCountriesAndOrgs()
            {
                var countryQuery = from c in db.Countries.Include("OrgsInCountry")
                    orderby c.CountryName
                    select c;
        
                Console.WriteLine("All Countries, orderes by name, and their orgs");
                foreach (var VARIABLE in countryQuery)
                {
                    Console.Write(VARIABLE.CountryName + "\t\twith id: " +  VARIABLE.CountryCode + "\t has the orgs:\t");
                    foreach (var org in VARIABLE.OrgsInCountry)
                    {
                        Console.Write(org.OrganizationName + " ");
                    }
                    Console.WriteLine();
                }
            }
        }

        private static void Main(string[] args)
        {

            using (var db = new BloggingContext())
            {
                BlogLogic newBlog = new BlogLogic();
                //newBlog.createAndSaveBlock();
                //displaying all the blogs
                //newBlog.DisplayBlogs();

                OrganizationLogic newOrg = new OrganizationLogic();
                //newOrg.NewUserForOrg();
                //newOrg.PrintOrganizations();
                //newOrg.PrintOrganizaionsAndTheirUsers();
                //newOrg.MakeOrganization();
                
                CountryLogic newCountryLogic = new CountryLogic();
                //newCountryLogic.PrintCountriesAndOrgs();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        public class Blog
        {
            public int BlogId { get; set; }

            public string Name { get; set; }

            public string Url { get; set; }
           
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
            public DbSet<Country> Countries { get; set; }

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
            public Organization Organization{ get; set; } // virtual ?
        }

        public class Organization
        {
            [Key]
            public string OrganizationName { get; set; }
            public virtual List<Country> HomeLands { get; set; }
        }

        public class Country
        {
            [Key]
            public int CountryId { get; set; }
            public string CountryName { get; set;}
            public int CountryCode { get; set; }
            public virtual List<Organization> OrgsInCountry { get; set; }
        }
    }
}