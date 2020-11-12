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
        public async Task<ActionResult> Index()
        {
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
        public async Task<ActionResult> JoinGroup(MembersViewModel viewModel, int groupId)
        {
            var userInDb = await _context.Users.Include(u => u.ApplicationUserGroups).SingleOrDefaultAsync(u => u.Id == viewModel.Users.Id);
            var groupInDb = await _context.Groups.SingleOrDefaultAsync(g => g.Id == groupId);

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
                    ViewBag.message = "You have already joined this group";

                    return View("MembersHome", viewModel);
                }
            }
            //Increase the number of members each time a user joins a group
            groupInDb.NumberOfMembers++;

            //create an instance of ApplicationUserGroup
            ApplicationUserGroup appUserGroup = new ApplicationUserGroup
            {
                ApplicationUserId = viewModel.Users.Id,
                GroupId = groupId
            };

            _context.ApplicationUserGroups.Add(appUserGroup);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //GET members/joinedgroupslist/
        //Get the list of all the groups a member has joined
        public ActionResult JoinedGroupsList(string Id)
        {
            //var groups = _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToList();

            var viewModel = new MembersViewModel()
            {
                Users = _context.Users.FirstOrDefault(u => u.Email.Equals(User.Identity.Name)),
                Groups = _context.ApplicationUserGroups.Include(g => g.Group).Where(g => g.ApplicationUserId == Id).Select(g => g.Group).ToList()
            };

            return View("MembersHome", viewModel);
        }


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

        // GET list of groups a user created
        public ActionResult MyGroups(string Id)
        {
            var groups = _context.Groups.Where(g => g.ApplicationUserId == Id).ToList();

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
            if(groupId == 0)
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
        public ActionResult LeaveGroup(string Id, int groupId)
        {
            var groupInDb = _context.Groups.SingleOrDefault(g => g.Id == groupId);
            var appUserGroup = _context.ApplicationUserGroups.Where(x => x.ApplicationUserId == Id && x.GroupId == groupId).SingleOrDefault();
            
            //Decrease the number of members each time a user leaves a group
            groupInDb.NumberOfMembers--;

            _context.ApplicationUserGroups.Remove(appUserGroup);
            _context.SaveChanges();

            return RedirectToAction("JoinedGroupsList", new { Id });
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
        public ActionResult CreateGroup(string id)
        {
            var viewModel = new GroupsViewModel()
            {
                Groups = new Group(),
                Interests = _context.Interests.ToList(),
                GroupTypes = _context.GroupTypes.ToList(),
                EntryTypes = _context.EntryTypes.ToList()
            };

            ViewBag.user = _context.Users.SingleOrDefault(m => m.Id == id);

            return View(viewModel);
        }

        //GET: members/editGroup
        // edit a group a member created
        public ActionResult EditGroup(int id, string userId)
        {
            var viewModel = new GroupsViewModel()
            {
                Groups = _context.Groups.Include(g => g.ApplicationUser).SingleOrDefault(g => g.Id == id),
                Interests = _context.Interests.ToList(),
                GroupTypes = _context.GroupTypes.ToList(),
                EntryTypes = _context.EntryTypes.ToList()
            };

            ViewBag.user = _context.Users.SingleOrDefault(m => m.Id == userId);

            return View("CreateGroup",viewModel);
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
        public ActionResult SaveGroup(GroupsViewModel group, HttpPostedFileBase File)
        {

            if (!ModelState.IsValid)
            {
                var viewModel = new GroupsViewModel()
                {
                    Groups = group.Groups,
                    Interests = _context.Interests.ToList(),
                    GroupTypes = _context.GroupTypes.ToList(),
                    EntryTypes = _context.EntryTypes.ToList()
                };

                ViewBag.user = _context.Users.SingleOrDefault(m => m.Id == group.Groups.ApplicationUserId);

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
                var groupInDb = _context.Groups.SingleOrDefault(g => g.Id == group.Groups.Id);

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

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}