using Studizie.Models;
using Studizie.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studizie.Controllers
{
    //[Authorize(Roles = "CanManageGroups")]
    public class GroupsController : Controller
    {
        private ApplicationDbContext _context;

        public GroupsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Groups
        // List of all groups
        public ActionResult Index()
        {
            var groups = _context.Groups.ToList();

            if (groups.Count == 0)
                ViewBag.message = "No groups have been added";

            return View("Groups", groups);
        }

        // GET: Groups/Create
        // Create Groups
        public ActionResult Create()
        {
            var viewModel = new CreateViewModel()
            {
                Groups = new Group(),
                Interests = _context.Interests.ToList(),
                GroupTypes = _context.GroupTypes.ToList(),
                EntryTypes = _context.EntryTypes.ToList()
            };

            return View(viewModel);
        }

        // POST Groups/Create
        // Save created or edited groups
        [HttpPost]
        public ActionResult Save(CreateViewModel group, HttpPostedFileBase File)
        {

            if (!ModelState.IsValid)
                return View("Edit", group);

            if (group.Groups.Id == 0)  // New group
            {
                // Saving uploaded image to folder

                //string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                ////string fName = Path.GetFileName(image.FileName);
                //string extension = Path.GetExtension(image.FileName);
                //fileName = fileName + extension;
                //var fileNamePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                //image.SaveAs(fileNamePath);

                //group.GroupImage = fileName;
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
                //group.Groups.GroupImage = new byte[File.ContentLength];
                //File.InputStream.Read(group.Groups.GroupImage, 0, File.ContentLength);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Edit view:  Groups/edit
        public ActionResult Edit(int id)
        {
            var viewModel = new CreateViewModel()
            {
                Groups = _context.Groups.SingleOrDefault(i => i.Id == id),
                Interests = _context.Interests.ToList(),
                GroupTypes = _context.GroupTypes.ToList(),
                EntryTypes = _context.EntryTypes.ToList()
            };

            //var group = _context.Groups.SingleOrDefault(i => i.Id == id);

            return View(viewModel);
        }

        // Details: Groups/details/1
        public ActionResult Details(int id)
        {
            var group = _context.Groups.Include(s => s.Interests)
                .Include(s => s.GroupTypes).Include(s => s.EntryTypes).SingleOrDefault(g => g.Id == id);

            // Get the number of members for each category
            ViewBag.scienceCount = _context.Users.Count(s => s.IsScience);
            ViewBag.artsCount = _context.Users.Count(s => s.IsArts);
            ViewBag.technologyCount = _context.Users.Count(s => s.IsTechnology);
            ViewBag.educationCount = _context.Users.Count(s => s.IsEducation);
            ViewBag.sportsCount = _context.Users.Count(s => s.IsSports);
            ViewBag.religiousCount = _context.Users.Count(s => s.IsReligious);
            ViewBag.skillAcquisitionCount = _context.Users.Count(s => s.IsSkillAcquisition);
            ViewBag.entrepreneurshipCount = _context.Users.Count(s => s.IsEntrepreneurship);
            ViewBag.gamesCount = _context.Users.Count(s => s.IsGames);

            return View(group);
        }

        // Delete:  Groups/Delete/id
        public ActionResult Delete(int id)
        {
            var group = _context.Groups.SingleOrDefault(i => i.Id == id);

            _context.Groups.Remove(group);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}