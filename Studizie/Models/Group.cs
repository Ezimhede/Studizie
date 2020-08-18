using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class Group
    {
        //public Group()
        //{
        //    this.ApplicationUsers = new HashSet<ApplicationUser>();
        //}
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string About { get; set; }

        [Display(Name = "Number of Members")]
        public int NumberOfMembers { get; set; }

        [Display(Name = "Meeting Point")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Time of Meeting")]
        public string TimeOfMeeting { get; set; }

        [Display(Name = "Group Creator")]
        public string GroupCreator { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public Member Member { get; set; }

        //public int MemberId { get; set; }
        public Interest Interests { get; set; }
        public int? InterestsId { get; set; }

        [Display(Name = "Group Type")]
        public virtual GroupType GroupTypes { get; set; }
        public int GroupTypesId { get; set; }

        [Display(Name = "Entry Type")]
        public virtual EntryType EntryTypes { get; set; }
        public int EntryTypesId { get; set; }

        [Display(Name = "Group Image")]
        public byte[] GroupImage { get; set; }

        public ICollection<ApplicationUserGroup> ApplicationUserGroups { get; set; }

        [NotMapped]
        //[ValidateFile(ErrorMessage = "Please select a PNG/JPEG image smaller than 20kb")]
        public HttpPostedFileBase File
        {
            get
            {
                return null;
            }
            set
            {
                try
                {
                    var target = new MemoryStream();

                    if (value.InputStream == null)
                        return;

                    value.InputStream.CopyTo(target);
                    GroupImage = target.ToArray();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }

    }
}