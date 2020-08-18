using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class GroupType
    {
        public int Id { get; set; }

        [Display (Name = "Group Type")]
        public string Name { get; set; }

    }
}