using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Restaurant.Controllers
{
    public class UserAddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserAddresses/Index/e
        public ActionResult List(string uName)
        {
            var userAddresses = new List<UserAddress>();
            if (User.IsInRole("Clients"))
                userAddresses = db.UserAddresses.Include(u => u.ApplicationUser).Where(u => u.ApplicationUser.UserName.ToLower() == User.Identity.Name.ToLower()).ToList();

            else if (uName != null)
                userAddresses = db.UserAddresses.Include(u => u.ApplicationUser).Where(u => u.ApplicationUser.UserName.ToLower() == uName.ToLower()).ToList();


            return View(userAddresses);
        }

        // GET: UserAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAddress userAddress = db.UserAddresses.Find(id);
            if (userAddress == null)
            {
                return HttpNotFound();
            }
            return View(userAddress);
        }

        // GET: UserAddresses/Create
        [Authorize(Roles = "Clients")]
        public ActionResult Create()
        {
            var UserStore = new UserStore<ApplicationUser>(db);
            var UserManager = new UserManager<ApplicationUser>(UserStore);
            ViewBag.ApplicationUser_Id = new SelectList(UserManager.Users, "Id", "FullName");
            ViewBag.UserID = User.Identity.GetUserId();
            return View();
        }

        // POST: UserAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clients")]
        public ActionResult Create([Bind(Include = "ID,Address,lat,lon,ApplicationUser_Id")] UserAddress userAddress)
        {
            userAddress.ApplicationUser_Id = User.Identity.GetUserId();
            
            if (ModelState.IsValid)
            {
                db.UserAddresses.Add(userAddress);
                db.SaveChanges();
                return RedirectToAction("list");
            }
            
            //var UserStore = new UserStore<ApplicationUser>(db);
            //var UserManager = new UserManager<ApplicationUser>(UserStore);
            //ViewBag.ApplicationUser_Id = new SelectList(UserManager.Users, "Id", "FullName", userAddress.ApplicationUser_Id);
            return View(userAddress);
        }

        // GET: UserAddresses/Edit/5
        [Authorize(Roles = "Clients")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAddress userAddress = db.UserAddresses.Find(id);
            if (userAddress == null)
            {
                return HttpNotFound();
            }
            else if (userAddress.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
         
            return View(userAddress);
        }

        // POST: UserAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clients")]
        public ActionResult Edit([Bind(Include = "ID,Address,lat,lon,ApplicationUser_Id")] UserAddress userAddress)
        {
            if (ModelState.IsValid)
            {
             if (userAddress.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
                db.Entry(userAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("list");
            }
            
            return View(userAddress);
        }

        // GET: UserAddresses/Delete/5
        [Authorize(Roles = "Clients")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAddress userAddress = db.UserAddresses.Find(id);
            if (userAddress == null)
            {
                return HttpNotFound();
            }
            else if (userAddress.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            return View(userAddress);
        }

        // POST: UserAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Clients")]
        public ActionResult DeleteConfirmed(int id)
        {
            UserAddress userAddress = db.UserAddresses.Find(id);
            if (userAddress.ApplicationUser_Id != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.UserAddresses.Remove(userAddress);
            db.SaveChanges();
            return RedirectToAction("list");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
