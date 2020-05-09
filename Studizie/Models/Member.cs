using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Interest Interest { get; set; }
        public int InterestId { get; set; }

    }
}