namespace Restaurant.Migrations
{
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Restaurant.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Restaurant.Models.ApplicationDbContext";
        }

        protected override void Seed(Restaurant.Models.ApplicationDbContext context)
        {

            string[] orderStatusList = new string[]{"new", "preparing","delivering", "Delevered" ,"canceled" };
            for (int i = 0; i < orderStatusList.Length; i++)
            {
                string statusName = orderStatusList[i];
                OrderStatus os = context.OrderStatuses.Where(o => o.Name == statusName).FirstOrDefault();
                if (os == null)
                {
                     os = new OrderStatus();
                    os.ID = i;
                    os.Name = orderStatusList[i];
                    context.OrderStatuses.Add(os);
                    context.SaveChanges();
                }
            }

            //  This method will be called after migrating to the latest version.
            var UserStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(UserStore);
            string[] systemRoles = new string[] { "Admins", "Cashiers", "Deliveries", "Clients", "Kitchen" };
            string[] userNames = new string[] { "Admin", "Cashier", "Delivery", "Client", "Kitchen"};

            for (int i = 0; i < systemRoles.Count(); i++)
            {
                string sysRoelName = systemRoles[i];
                IdentityRole sysRole = context.Roles.Where(m => m.Name == sysRoelName).FirstOrDefault();
                if (sysRole == null)
                {
                    sysRole = new IdentityRole(sysRoelName);
                    context.Roles.AddOrUpdate(sysRole);
                    context.SaveChanges();
                    sysRole = context.Roles.Where(m => m.Name == sysRoelName).FirstOrDefault();
                }

                string userName = userNames[i];

                ApplicationUser myUser = new ApplicationUser { UserName = userName, Email = userName + "@Restaurant.com", FullName = userName, PhoneNumber = "01201234567", LockoutEnabled = true };
                var result = UserManager.Create(myUser, "P@ssw0rd");
                if (result.Succeeded && sysRole != null)
                {
                    var roelResult = UserManager.AddToRoles(myUser.Id, sysRole.Name);
                }
            }
        }
    }
}
