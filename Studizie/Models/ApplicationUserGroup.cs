using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class ApplicationUserGroup
    {
        public int ApplicationUserGroupId { get; set; }
        public string ApplicationUserId { get; set; }
        public int GroupId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Group Group { get; set; }

    }
}