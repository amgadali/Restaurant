using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;
using System.IO;

  
namespace Restaurant.Controllers
{
 
    public class MenuItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MenuItems
        public ActionResult Index()
        {
            return View(db.MenuItems.ToList());
        }

        // GET: MenuItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem MenuItem = db.MenuItems.Find(id);
            if (MenuItem == null)
            {
                return HttpNotFound();
            }
            return View(MenuItem);
        }

        // GET: MenuItems/Create
        [Authorize(Roles = "Admins")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MenuItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Price")] MenuItem MenuItem)
        {
            if (ModelState.IsValid)
            {
                var upload = Request.Files["ImageFile"];
                if (upload != null && upload.ContentLength > 0)
                {
                    string path = HttpContext.Server.MapPath("~/content/Images");
                    string savedFileName = Path.Combine(path, MenuItem.Name + ".jpg");
                    upload.SaveAs(savedFileName);
                    MenuItem.Image = MenuItem.Name + ".jpg";
                }
               
                db.MenuItems.Add(MenuItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(MenuItem);
        }

        // GET: MenuItems/Edit/5
        [Authorize(Roles = "Admins")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem MenuItem = db.MenuItems.Find(id);
            if (MenuItem == null)
            {
                return HttpNotFound();
            }
            return View(MenuItem);
        }

        // POST: MenuItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Image,Price")] MenuItem MenuItem)
        {
            if (ModelState.IsValid)
            {
                var upload = Request.Files["ImageFile"];
                if (upload != null && upload.ContentLength > 0)
                {
                    string path = HttpContext.Server.MapPath("~/content/Images");
                    string savedFileName = Path.Combine(path, MenuItem.Name + ".jpg");
                     upload.SaveAs(savedFileName);
                    MenuItem.Image = MenuItem.Name + ".jpg";
                }
                db.Entry(MenuItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(MenuItem);
        }

        // GET: MenuItems/Delete/5
        [Authorize(Roles = "Admins")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MenuItem MenuItem = db.MenuItems.Find(id);
            if (MenuItem == null)
            {
                return HttpNotFound();
            }
            return View(MenuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admins")]
        public ActionResult DeleteConfirmed(int id)
        {
            MenuItem MenuItem = db.MenuItems.Find(id);
            db.MenuItems.Remove(MenuItem);
            db.SaveChanges();
            return RedirectToAction("Index");
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
