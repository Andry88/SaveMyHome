namespace SaveMyHome.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using SaveMyHome.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using SaveMyHome.Helpers;
    using SaveMyHome.DAL;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //Заполняем таблицу Apartments
            var apartments = new List<Apartment>();
            for (int i = 1; i <= House.ApartmentsAmount; i++)
                apartments.Add(new Apartment { Number = i });

            apartments.ForEach(a => context.Apartments.AddOrUpdate(p => p.Number, a));
            context.SaveChanges();

            //Заполняем таблицы AspNetRoles и AspNetUsers
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // создаем две роли
            string roleUser = "user";
            string roleAdmin = "admin";

            // добавляем роли в бд
            if (!roleManager.RoleExists(roleAdmin))
                roleManager.Create(new IdentityRole(roleAdmin));

            if (!roleManager.RoleExists(roleUser))
                roleManager.Create(new IdentityRole(roleUser));

            // создаем пользователей
            var users = new List<ApplicationUser>();
            
            for (int i = 1; i <= House.ApartmentsAmount; i++)
            {
                users.Add(new ApplicationUser
                {
                    UserName = $"apart{i}@mail.ru",
                    Email = $"apart{i}@mail.ru",
                    PhoneNumber = "80291111111"
                });
            }


            IdentityResult result = null;
            //Добавление администратора
            ApplicationUser userAdmin = users.FirstOrDefault(u => u.Email == "apart60@mail.ru");
            if (userManager.FindByName(userAdmin.UserName) == null)
            {
                result = userManager.Create(userAdmin, "123" + userAdmin.Email.Split('@').First());
                if (!userManager.IsInRole(userAdmin.Id, roleAdmin))
                    userManager.AddToRole(userAdmin.Id, roleAdmin);
            }

            //Добавление остальных пользователей
            foreach (ApplicationUser user in users)
            {
                if(userManager.FindByName(user.UserName) == null)
                {
                    result = userManager.Create(user, "123" + user.Email.Split('@').First());
                    if (result.Succeeded && !userManager.IsInRole(user.Id, roleUser))

                        userManager.AddToRole(user.Id, roleUser);
                }
            }

            //context.SaveChanges();
            
            //Заполняем таблицу ClientProfiles
            var profiles = new List<Models.ClientProfile>();

            for (int i = 1; i <= House.ApartmentsAmount; i++)
            {
                profiles.Add(new Models.ClientProfile
                {
                    Id = users[i-1].Id,
                    FirstName = $"Имя{i}",
                    LastName = $"Фамилия{i}",
                    Age = 28,
                    ApartmentNumber = i,
                });
            }

            profiles.ForEach(p => context.ClientProfiles.AddOrUpdate(a => a.Id, p));
            context.SaveChanges();

            //Заполняем таблицу Problems
            List<Problem> problems = new List<Problem>
            {
                new Problem{ Name = "Потоп" },
                new Problem{ Name = "Пожар" }
            };

            problems.ForEach(p => context.Problems.AddOrUpdate(a => a.Name, p));
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
