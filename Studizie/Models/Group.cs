using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string About { get; set; }
        public string GroupType { get; set; }

        [Display(Name ="Number of Members")]
        public int NumberOfMembers { get; set; }
        public string EntryType { get; set; }

        [Display(Name = "Group Image")]
        public string GroupImage { get; set; }

        [Display(Name ="Meeting Point")]
        public string MeetingPoint { get; set; }

        [Display(Name = "Time of Meeting")]
        public string TimeOfMeeting { get; set; }

        [Display(Name = "Group Creator")]
        public string GroupCreator { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        public Member Member { get; set; }

        //public int MemberId { get; set; }

    }
}