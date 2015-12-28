using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace Restaurant.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
 
        
        [Display(Name = "E-Mail")]
        public override string Email
        {
            get
            {
                return base.Email;
            }
            set
            {
                base.Email = value;
            }
        }

        public virtual Collection<UserAddress> Addresses { get; private set; }

        public virtual Collection<Order> UserOrders { get; private set; }

        public ApplicationUser()
        {
            Addresses = new Collection<UserAddress>();
            UserOrders = new Collection<Order>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Restaurant.Models.MenuItem> MenuItems { get; set; }

        public System.Data.Entity.DbSet<Restaurant.Models.Order> Orders { get; set; }
        public System.Data.Entity.DbSet<Restaurant.Models.OrderStatus> OrderStatuses { get; set; }

        public System.Data.Entity.DbSet<Restaurant.Models.UserAddress> UserAddresses { get; set; }
    }
}