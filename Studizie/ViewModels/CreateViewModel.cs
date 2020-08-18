using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Studizie.Models;

namespace Studizie.ViewModels
{
    public class CreateViewModel
    {
        public Group Groups { get; set; }
        public IEnumerable<Interest> Interests { get; set; }
        public IEnumerable<GroupType> GroupTypes { get; set; }
        public IEnumerable<EntryType> EntryTypes { get; set; }

    }
}