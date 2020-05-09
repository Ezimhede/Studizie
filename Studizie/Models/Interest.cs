using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class Interest
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="Area of Interest")]
        public string Name { get; set; }

    }
}