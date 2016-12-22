namespace AnnouncementsWebApp1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using AnnouncementsWebApp1.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<AnnouncementsWebApp1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AnnouncementsWebApp1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            AddRoles(context);
            AddUsers(context);
        }

        void AddUsers(AnnouncementsWebApp1.Models.ApplicationDbContext context)
        {
            var userLecturer = new ApplicationUser { UserName = "Lecturer1@email.com" };
            var userStudent1 = new ApplicationUser { UserName = "Student1@email.com" };
            var userStudent2 = new ApplicationUser { UserName = "Student2@email.com" };
            var userStudent3 = new ApplicationUser { UserName = "Student3@email.com" };
            var userStudent4 = new ApplicationUser { UserName = "Student4@email.com" };
            var userStudent5 = new ApplicationUser { UserName = "Student5@email.com" };


            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            um.Create(userLecturer, "password");
            um.Create(userStudent1, "password");
            um.Create(userStudent2, "password");
            um.Create(userStudent3, "password");
            um.Create(userStudent4, "password");
            um.Create(userStudent5, "password");

            //Lectuer User
            var userLecturerId = um.FindByName(userLecturer.UserName);
            userLecturerId.FirstName = "The";
            userLecturerId.LastName = "Lecturer";
            um.Update(userLecturerId);

            // Student User
            var userStudent1Id = um.FindByName(userStudent1.UserName);
            userStudent1Id.FirstName = "John";
            userStudent1Id.LastName = "Smith";
            um.Update(userStudent1Id);

            // Student User
            var userStudent2Id = um.FindByName(userStudent2.UserName);
            userStudent2Id.FirstName = "Paula";
            userStudent2Id.LastName = "Murphy";
            um.Update(userStudent2Id);

            // Student User
            var userStudent3Id = um.FindByName(userStudent3.UserName);
            userStudent3Id.FirstName = "Andrew";
            userStudent3Id.LastName = "Finch";
            um.Update(userStudent3Id);

            // Student User
            var userStudent4Id = um.FindByName(userStudent4.UserName);
            userStudent4Id.FirstName = "Joseph";
            userStudent4Id.LastName = "Pratt";
            um.Update(userStudent4Id);

            var userStudent5Id = um.FindByName(userStudent5.UserName);
            userStudent5Id.FirstName = "Josh";
            userStudent5Id.LastName = "Patterson";
            um.Update(userStudent5Id);


            um.AddToRole(userLecturerId.Id, "Lecturer");
            um.AddToRole(userStudent1Id.Id, "Student");
            um.AddToRole(userStudent2Id.Id, "Student");
            um.AddToRole(userStudent3Id.Id, "Student");
            um.AddToRole(userStudent4Id.Id, "Student");
            um.AddToRole(userStudent5Id.Id, "Student");
        }

        void AddRoles(AnnouncementsWebApp1.Models.ApplicationDbContext context)
        {
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string roleNameLecturer = "Lecturer";
            string roleNameStudent = "Student";
            IdentityResult roleResult;
            if (!RoleManager.RoleExists(roleNameLecturer))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleNameLecturer));
            }
            if (!RoleManager.RoleExists(roleNameStudent))
            {
                roleResult = RoleManager.Create(new IdentityRole(roleNameStudent));
            }
        }
    }
}
