using Studizie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Studizie.Controllers
{
    public class AreasOfInterestController : Controller
    {
        private ApplicationDbContext _context;

        public AreasOfInterestController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: AreasOfInterest
        // List of all the areas of interest available
        public ActionResult Index()
        {
            var interests = _context.Interests.ToList();

            if (interests.Count == 0)
                ViewBag.message = "No areas of interest have been added";

            return View("List", interests);
        }

        // GET: AreasOfInterest/Create
        // Create Areas of Interest
        public ActionResult Create ()
        {
            return View();
        }

        // POST AreasOfInterest/Create
        // Save created or edited area of interest
        [HttpPost]
        public ActionResult Save (Interest interest)
        {
            var interestInDb = _context.Interests.SingleOrDefault(i => i.Id == interest.Id);

            if (!ModelState.IsValid)
                return View("Create",interest);

            if (interest.Id == 0)   // New interest
                _context.Interests.Add(interest);

            else
            {
                interestInDb.Name = interest.Name;
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Edit view:  AreasOfInterest/Create
        public ActionResult Edit (int id)
        {
            var interest = _context.Interests.SingleOrDefault(i => i.Id == id);

            return View(interest);
        }

        // Delete:  AreasOfInterest/Delete
        public ActionResult Delete (int id)
        {
            var interest = _context.Interests.SingleOrDefault(i => i.Id == id);

            _context.Interests.Remove(interest);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}