namespace OptionsWebSite.Migrations.IdentityMigrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<OptionsWebSite.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\IdentityMigrations";
        }

        protected override void Seed(OptionsWebSite.Models.ApplicationDbContext context)
        {

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);


            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);
            }
            if (!context.Roles.Any(r => r.Name == "Student"))
            {
                var role = new IdentityRole { Name = "Student" };
                roleManager.Create(role);
            }
            if (!context.Users.Any(a => a.UserName == "A0011111"))
            {
                var user = new ApplicationUser
                {
                    UserName = "A00111111",
                    Email = "a@a.a"
                };
                userManager.Create(user, "P@$$w0rd");
                userManager.AddToRole(user.Id, "Admin");
            }
            if (!context.Users.Any(s => s.UserName == "A00222222"))
            {
                var user = new ApplicationUser
                {
                    UserName = "A00222222",
                    Email = "s@s.s"
                };
                userManager.Create(user, "P@$$w0rd");
                userManager.AddToRole(user.Id, "Student");
            }
        }
    }
}