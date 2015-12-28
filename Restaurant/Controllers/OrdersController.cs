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
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Authorize(Roles = "Clients,Cashiers,Kitchen,Deliveries")]
        public ActionResult Index()
        {
            var ordersList = new List<Order>();
            if (User.IsInRole("Clients"))
            {
                string uID = User.Identity.GetUserId();
                ordersList = db.Orders.Where(o => o.Client_Id == uID).ToList();
            }
            else if (User.IsInRole("Kitchen"))
            {
                string uID = User.Identity.GetUserId();
                ordersList = db.Orders.Where(o => o.Status.ID == 0 || o.Status.ID == 1).ToList();
            }
            else if (User.IsInRole("Deliveries"))
            {
                string uID = User.Identity.GetUserId();
                ordersList = db.Orders.Where(o => o.Status.ID == 2).ToList();
            }
            else if (User.IsInRole("Cashiers"))
            {
                string uID = User.Identity.GetUserId();
                ordersList = db.Orders.Where(o => o.Status.ID == 3).ToList();
            }


            return View(ordersList);
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Client_Id = User.Identity.GetUserId();
            ViewBag.MenuItems = db.MenuItems.ToList();
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            //var errs = ModelState.ToDictionary(kvp => kvp.Key,
            //   kvp => kvp.Value.Errors
            //                   .Select(e => e.ErrorMessage).ToArray())
            //                   .Where(m => m.Value.Count() > 0);
            if (order.OrderMenuItems != null)
            {
                for (int i = order.OrderMenuItems.Count - 1; i >= 0; i--)
                {
                    if (order.OrderMenuItems[i].MenuItem_Id == 0 || order.OrderMenuItems[i].Count < 1)
                    {
                        order.OrderMenuItems.RemoveAt(i);
                        ModelState["OrderMenuItems[" + i + "].Count"].Errors.Clear();
                    }
                    else
                    {
                        MenuItem mi = db.MenuItems.Find(order.OrderMenuItems[i].MenuItem_Id);
                        order.OrderMenuItems[i].Price = mi.Price;
                    }
                }
            }
            if (ModelState.IsValid)
            {
                OrderStatus os = db.OrderStatuses.Find(0);
                order.Status = os;
                order.Client_Id = User.Identity.GetUserId();

                db.Orders.Add(order);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.MenuItems = db.MenuItems.ToList();
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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

        public ActionResult OrderStatus(int orderId, int statusID)
        {
            Order o = db.Orders.Find(orderId);
            OrderStatus os = db.OrderStatuses.Find(statusID);
            if (o != null && os != null)
            {
                o.Status = os;
               int x = db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
