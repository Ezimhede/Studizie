using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class EntryType
    {
        public int Id { get; set; }

        [Display(Name = "Entry Type")]
        public string Name { get; set; }
    }
}