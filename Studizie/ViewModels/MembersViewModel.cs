using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Studizie.Models;

namespace Studizie.ViewModels
{
    public class MembersViewModel
    {
        public ApplicationUser Users { get; set; }
        public IEnumerable<Group> Groups { get; set; }

    }
}