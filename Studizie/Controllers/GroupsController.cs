using Studizie.Models;
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
            return View();
        }

        // POST Groups/Create
        // Save created or edited groups
        [HttpPost]
        public ActionResult Save(Group group, HttpPostedFileBase image)
        {
          
            if (!ModelState.IsValid)
                return View("Create", group);

            if (group.Id == 0)  // New group
            {
                // Saving uploaded image to folder

                //string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                ////string fName = Path.GetFileName(image.FileName);
                //string extension = Path.GetExtension(image.FileName);
                //fileName = fileName + extension;
                //var fileNamePath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                //image.SaveAs(fileNamePath);

                //group.GroupImage = fileName;
                group.DateCreated = DateTime.Now;

                _context.Groups.Add(group);
            }

            // For editing existing group
            else
            {
                var groupInDb = _context.Groups.SingleOrDefault(g => g.Id == group.Id);

                groupInDb.Name = group.Name;
                groupInDb.About = group.About;
                groupInDb.MeetingPoint = group.MeetingPoint;
                groupInDb.TimeOfMeeting = group.TimeOfMeeting;
                groupInDb.GroupCreator = group.GroupCreator;
                groupInDb.GroupType = group.GroupType;
                groupInDb.EntryType = group.EntryType;
                groupInDb.GroupImage = group.GroupImage;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Edit view:  Groups/edit
        public ActionResult Edit(int id)
        {
            var group = _context.Groups.SingleOrDefault(i => i.Id == id);

            return View(group);
        }

        // Details: Groups/details/1
        public ActionResult Details(int id)
        {
            var group = _context.Groups.SingleOrDefault(g => g.Id == id);

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