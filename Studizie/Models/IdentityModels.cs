using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Studizie.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        //public ApplicationUser()
        //{
        //    this.Groups = new HashSet<Group>();
        //}

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public bool IsScience { get; set; }
        public bool IsArts { get; set; }
        public bool IsTechnology { get; set; }
        public bool IsEducation { get; set; }
        public bool IsSports { get; set; }
        public bool IsReligious { get; set; }

        [Display(Name = "Skill Acquisition")]
        public bool IsSkillAcquisition { get; set; }
        public bool IsEntrepreneurship { get; set; }
        public bool IsGames { get; set; }
        public ICollection<ApplicationUserGroup> ApplicationUserGroups { get; set; }
        public ICollection<Notification> Notifications { get; set; }


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

        public DbSet<Group> Groups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<GroupType> GroupTypes { get; set; }
        public DbSet<EntryType> EntryTypes { get; set; }
        public DbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<Studizie.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}