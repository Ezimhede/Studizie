using Studizie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Studizie.ViewModels;

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
        public ActionResult Index()
        {
            var viewModel = new MembersViewModel()
            {
                Users = _context.Users.FirstOrDefault(u => u.Email.Equals(User.Identity.Name)),
                Groups = _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToList()
            };

            //var user = _context.Users.FirstOrDefault(u => u.Email.Equals(User.Identity.Name));

            return View("MembersHome", viewModel);
        }

        // Get all available groups based on member's interests
        [Authorize]
        public ActionResult JsonIndex()
        {
            var viewModel = new MembersViewModel()
            {
                Users = _context.Users.FirstOrDefault(u => u.Email.Equals(User.Identity.Name)),
                Groups = _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToList()
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
        public ActionResult JoinGroup(MembersViewModel viewModel, int groupId)
        {
            var userInDb = _context.Users.Include(u => u.ApplicationUserGroups).SingleOrDefault(u => u.Id == viewModel.Users.Id);
            var groupInDb = _context.Groups.SingleOrDefault(g => g.Id == groupId);

            viewModel = new MembersViewModel()
            {
                Users = _context.Users.SingleOrDefault(u => u.Id == viewModel.Users.Id),
                Groups = _context.Groups.Include(g => g.GroupTypes).Include(g => g.EntryTypes).Include(g => g.Interests).ToList()
            };

            // Redirect user if user has already joined the group
            var userGroups = _context.ApplicationUserGroups.ToList();

            foreach (var item in userGroups)
            {
                if ((item.ApplicationUserId == viewModel.Users.Id) && (item.GroupId == groupId))
                {
                    ViewBag.message = "You have already joined this group";

                    return View("MembersHome", viewModel);
                }
            }

            //create an instance of ApplicationUserGroup
            ApplicationUserGroup appUserGroup = new ApplicationUserGroup
            {
                ApplicationUserId = viewModel.Users.Id,
                GroupId = groupId
            };

            _context.ApplicationUserGroups.Add(appUserGroup);

            _context.SaveChanges();

            return RedirectToAction("JsonIndex");
        }

        //GET members/joinedgroups/
        //public ActionResult JoinedGroups(string Id)
        //{
        //    var groups = _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToList();
        //    return View("JoinedGroups", groups);
        //}


        // GET list of groups a user joined, as JSON
        public ActionResult JoinedGroups(string Id)
        {
            //_context.Configuration.ProxyCreationEnabled = false;
            var groups = _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToList();

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


        // Return a group image
        public ActionResult RenderImage(int groupId)
        {
            Group group = _context.Groups.Find(groupId);

            byte[] groupImage = group.GroupImage;

            return File(groupImage, "image/jpg");
        }


        //DELETE group
        public ActionResult DeleteGroup(string Id, int groupId)
        {
            var appUserGroup = _context.ApplicationUserGroups.Where(x => x.ApplicationUserId == Id && x.GroupId == groupId).SingleOrDefault();

            _context.ApplicationUserGroups.Remove(appUserGroup);
            _context.SaveChanges();

            return RedirectToAction("JoinedGroups", new { Id });
        }


    }
}