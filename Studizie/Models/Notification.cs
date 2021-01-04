using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Studizie.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string MemberId { get; set; }
        public int GroupId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public string NotificationType { get; set; }

    }
}