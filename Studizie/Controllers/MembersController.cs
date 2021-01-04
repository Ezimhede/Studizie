using Studizie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Studizie.ViewModels;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Studizie.Controllers
{
    //[Authorize(Roles = Roles.Member)]
    public class MembersController : Controller
    {
        private ApplicationDbContext _context;

        public MembersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Members
        [Authorize]
        public async Task<ActionResult> Index(bool joinSuccess = false, bool? leaveSuccess = false)
        {
            ViewBag.joinSuccess = joinSuccess; // display a success message when a user successfully joins a group
            ViewBag.leaveSuccess = leaveSuccess;
            ViewBag.alreadyJoined = false;

            var viewModel = new MembersViewModel()
            {
                Users = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name)),
                Groups = await _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToListAsync()
            };

            //var user = _context.Users.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));

            return View("MembersHome", viewModel);
        }

        // Get all available groups based on member's interests
        [Authorize]
        public async Task<ActionResult> JsonIndex()
        {
            var viewModel = new MembersViewModel()
            {
                Users = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name)),
                Groups = await _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToListAsync()
            };

            //_context.Configuration.ProxyCreationEnabled = false;

            return new JsonResult()
            {
                Data = viewModel,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        //Save a group that a member joins
        [HttpPost]
        public async Task<ActionResult> JoinGroup(MembersViewModel viewModel, int groupId)
        {
            var userInDb = await _context.Users.Include(u => u.ApplicationUserGroups).SingleOrDefaultAsync(u => u.Id == viewModel.Users.Id);
            var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

            // Notify group creator when a new member joins their group
            if (groupInDb.GroupTypesId == 1) // send join notification if it's an open group
            {
                var notification = new Notification()
                {
                    ApplicationUserId = groupInDb.ApplicationUserId,
                    Date = DateTime.Now,
                    Message = userInDb.FirstName + " " + userInDb.LastName + " joined " + groupInDb.Name,
                    NotificationType = "Plain",
                    MemberId = viewModel.Users.Id,
                    GroupId = groupId
                };

                //Increase the number of members each time a user joins a group
                groupInDb.NumberOfMembers++;

                _context.Notifications.Add(notification);
            }

            if (groupInDb.GroupTypesId == 2) // send request notification if it's a close group
            {
                var notification = new Notification()
                {
                    ApplicationUserId = groupInDb.ApplicationUserId,
                    Date = DateTime.Now,
                    Message = userInDb.FirstName + " " + userInDb.LastName + " requested to join " + groupInDb.Name,
                    NotificationType = "Request",
                    MemberId = viewModel.Users.Id,
                    GroupId = groupId
                };

                _context.Notifications.Add(notification);
            }

            viewModel = new MembersViewModel()
            {
                Users = await _context.Users.SingleOrDefaultAsync(u => u.Id == viewModel.Users.Id),
                Groups = await _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToListAsync()
            };

            // Redirect user if user has already joined the group
            var userGroups = await _context.ApplicationUserGroups.ToListAsync();

            foreach (var item in userGroups)
            {
                if ((item.ApplicationUserId == viewModel.Users.Id) && (item.GroupId == groupId))
                {
                    ViewBag.alreadyJoined = true;
                    ViewBag.joinSuccess = false;
                    ViewBag.leaveSuccess = false;

                    return View("MembersHome", viewModel);
                }
            }


            //create an instance of ApplicationUserGroup
            ApplicationUserGroup appUserGroup = new ApplicationUserGroup
            {
                ApplicationUserId = viewModel.Users.Id,
                GroupId = groupId
            };

            // Save group if group type is "open"
            if (groupInDb.GroupTypesId == 1)
            {
                _context.ApplicationUserGroups.Add(appUserGroup);
            }

            // Redirect to payment page for paid groups
            if (groupInDb.EntryTypes.Name == "Paid")
            {
                return RedirectToAction("Payment", new { customerEmail = viewModel.Users.Email, firstName = viewModel.Users.FirstName, lastName = viewModel.Users.LastName });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { joinSuccess = true});
        }


        public ActionResult Payment(string customerEmail, string firstName, string lastName)
        {
            ViewBag.customerEmail = customerEmail;
            ViewBag.firstName = firstName;
            ViewBag.lastName = lastName;

            return View();
        }


        // Save groups that were accepted by group creator
        [HttpPost]
        public async Task<ActionResult> SaveAcceptedGroup(string Id, string memberId, int groupId, int notificationId)
        {
            //create an instance of ApplicationUserGroup
            ApplicationUserGroup appUserGroup = new ApplicationUserGroup
            {
                ApplicationUserId = memberId,
                GroupId = groupId
            };

            var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

            // Notify the user that sent the group request when the request is accepted
            var notification = new Notification()
            {
                ApplicationUserId = memberId,
                Date = DateTime.Now,
                Message = "Your request to join " + groupInDb.Name + " was accepted",
                NotificationType = "Plain",
                MemberId = Id,
                GroupId = groupId
            };

            // Delete the notification message after it has been accepted
            var notifMessage = await _context.Notifications.Where(n => n.NotificationId == notificationId).SingleOrDefaultAsync();
            _context.Notifications.Remove(notifMessage);

            // Redirect user if user has already joined the group
            //var userGroups = _context.ApplicationUserGroups.ToList();

            //foreach (var item in userGroups)
            //{
            //    if ((item.ApplicationUserId == memberId) && (item.GroupId == groupId))
            //    {
            //        ViewBag.alreadyJoined = "You have already joined this group";

            //        return View("MembersHome");
            //    }
            //}

            //ViewBag.alreadyJoined = true;

            //Increase the number of members each time a user joins a group
            groupInDb.NumberOfMembers++;

            _context.Notifications.Add(notification);

            _context.ApplicationUserGroups.Add(appUserGroup);
            await _context.SaveChangesAsync();

            //return new JsonResult()
            //{
            //    Data = "data",
            //    MaxJsonLength = 86753090,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};

            return RedirectToAction("Index", new { joinSuccess = false });
        }


        // Send notification when a group request is rejected
        [HttpPost]
        public async Task<ActionResult> RejectRequest(string Id, string memberId, int groupId, int notificationId)
        {
            
            var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

            // Notify the user that sent the group request when the request is accepted
            var notification = new Notification()
            {
                ApplicationUserId = memberId,
                Date = DateTime.Now,
                Message = "Your request to join " + groupInDb.Name + " was rejected",
                NotificationType = "Plain",
                MemberId = Id,
                GroupId = groupId
            };

            // Delete the notification message after it has been accepted
            var notifMessage = await _context.Notifications.Where(n => n.NotificationId == notificationId).SingleOrDefaultAsync();
            _context.Notifications.Remove(notifMessage);

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { joinSuccess = false });
        }

        //GET members/joinedgroupslist/
        //Get the list of all the groups a member has joined
        public async Task<ActionResult> JoinedGroupsList(string Id, bool leaveSuccess = false, bool joinSuccess = false)
        {
            //var groups = _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToList();

            ViewBag.leaveSuccess = leaveSuccess;
            ViewBag.joinSuccess = joinSuccess;

            var viewModel = new MembersViewModel()
            {
                Users = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(User.Identity.Name)),
                Groups = await _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToListAsync()
            };

            return View("MembersHome", viewModel);
        }


        // GET list of groups a user joined, as JSON
        public async Task<ActionResult> JoinedGroups(string Id)
        {
            //_context.Configuration.ProxyCreationEnabled = false;
            var groups = await _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToListAsync();

            //return Json(new { data = groups}, JsonRequestBehavior.AllowGet);

            //return Json(groups.Select(s => new
            //{
            //    id = s.Id,
            //    name = s.Name,
            //    about = s.About,
            //    numberOfMembers = s.NumberOfMembers,
            //    groupImage = s.GroupImage,
            //    meetingPoint = s.MeetingPoint,
            //    timeOfMeeting = s.TimeOfMeeting,
            //    groupCreator = s.GroupCreator,
            //    dateCreated = s.DateCreated,
            //    entryType = s.EntryTypes,
            //    groupType = s.GroupTypes
            //}), JsonRequestBehavior.AllowGet);

            return new JsonResult()
            {
                Data = groups,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // GET list of groups a user created
        public async Task<ActionResult> MyGroups(string Id)
        {
            var groups = await _context.Groups.Where(g => g.ApplicationUserId == Id).ToListAsync();

            return new JsonResult()
            {
                Data = groups,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        // Return a group image
        public ActionResult RenderImage(int groupId)
        {
            if (groupId == 0)
            {
                return null;
            }
            else
            {
                Group group = _context.Groups.Find(groupId);

                byte[] groupImage = group.GroupImage;

                return File(groupImage, "image/jpg");
            }

        }


        //Leave a joined group
        public async Task<ActionResult> LeaveGroup(string Id, int groupId)
        {
            var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);
            var appUserGroup = await _context.ApplicationUserGroups.Where(x => x.ApplicationUserId == Id && x.GroupId == groupId).SingleOrDefaultAsync();

            //Decrease the number of members each time a user leaves a group
            groupInDb.NumberOfMembers--;

            // Notify group creator when a member leaves their group
            var notification = new Notification()
            {
                ApplicationUserId = groupInDb.ApplicationUserId,
                Date = DateTime.Now,
                Message = appUserGroup.ApplicationUser.FirstName + " " + appUserGroup.ApplicationUser.LastName + " left " + groupInDb.Name
            };

            _context.ApplicationUserGroups.Remove(appUserGroup);
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();

            return RedirectToAction("JoinedGroupsList", new { Id, leaveSuccess = true, joinSuccess = false });
        }

        //Delete a group a member created
        public async Task<ActionResult> DeleteGroup(int groupId)
        {
            var group = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //GET: members/creategroup
        [Authorize]
        public async Task<ActionResult> CreateGroup(string id)
        {
            var viewModel = new GroupsViewModel()
            {
                Groups = new Group(),
                Interests = await _context.Interests.ToListAsync(),
                GroupTypes = await _context.GroupTypes.ToListAsync(),
                EntryTypes = await _context.EntryTypes.ToListAsync()
            };

            ViewBag.user = await _context.Users.SingleOrDefaultAsync(m => m.Id == id);

            return View(viewModel);
        }

        //GET: members/editGroup
        // edit a group a member created
        public async Task<ActionResult> EditGroup(int id, string userId)
        {
            var viewModel = new GroupsViewModel()
            {
                Groups = await _context.Groups.Include(g => g.ApplicationUser).SingleOrDefaultAsync(g => g.Id == id),
                Interests = await _context.Interests.ToListAsync(),
                GroupTypes = await _context.GroupTypes.ToListAsync(),
                EntryTypes = await _context.EntryTypes.ToListAsync()
            };

            ViewBag.user = await _context.Users.SingleOrDefaultAsync(m => m.Id == userId);

            return View("CreateGroup", viewModel);
            //return new JsonResult()
            //{
            //    Data = viewModel,
            //    MaxJsonLength = 86753090,
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
        }


        // POST: members/savegroup
        // Save created or edited groups
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveGroup(GroupsViewModel group, HttpPostedFileBase File)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new GroupsViewModel()
                {
                    Groups = group.Groups,
                    Interests = await _context.Interests.ToListAsync(),
                    GroupTypes = await _context.GroupTypes.ToListAsync(),
                    EntryTypes = await _context.EntryTypes.ToListAsync()
                };

                ViewBag.user = await _context.Users.SingleOrDefaultAsync(m => m.Id == group.Groups.ApplicationUserId);

                return View("CreateGroup", viewModel);
            }

            if (group.Groups.Id == 0)  // New group
            {
                group.Groups.DateCreated = DateTime.Now;

                using (var binaryReader = new BinaryReader(File.InputStream)) //convert image to binary and save
                    group.Groups.GroupImage = binaryReader.ReadBytes(File.ContentLength);

                _context.Groups.Add(group.Groups);
            }

            // For editing existing group
            else
            {
                var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == group.Groups.Id);

                if (File == null)
                    groupInDb.GroupImage = groupInDb.GroupImage;

                if (File != null)
                {
                    using (var binaryReader = new BinaryReader(File.InputStream)) //convert image to binary and save
                        groupInDb.GroupImage = binaryReader.ReadBytes(File.ContentLength);
                }

                groupInDb.Name = group.Groups.Name;
                groupInDb.About = group.Groups.About;
                groupInDb.MeetingPoint = group.Groups.MeetingPoint;
                groupInDb.TimeOfMeeting = group.Groups.TimeOfMeeting;
                groupInDb.GroupCreator = group.Groups.GroupCreator;
                groupInDb.GroupTypesId = group.Groups.GroupTypesId;
                groupInDb.EntryTypesId = group.Groups.EntryTypesId;
                groupInDb.InterestsId = group.Groups.InterestsId;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // Get all the notifications for the logged in members
        public async Task<ActionResult> GetNotifications(string id)
        {
            var notificationsInDb = await _context.Notifications.Where(n => n.ApplicationUserId == id).ToListAsync();
            notificationsInDb.Select(n => n.Date);

            return new JsonResult()
            {
                Data = notificationsInDb,
                MaxJsonLength = 86753090,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}