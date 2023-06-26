using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrdersController : Controller
    {
        private sneakerShopEntities db = new sneakerShopEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var order = db.Orders.Include(o => o.Cart).Include(o => o.paymentType1).Include(o => o.AspNetUser);
            return View(order.ToList());
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
            ViewBag.cartID = new SelectList(db.Carts, "cartId", "cartId");
            ViewBag.paymentType = new SelectList(db.paymentTypes, "paymentTypeID", "paymentTypeName");
            ViewBag.userID = new SelectList(db.AspNetUsers, "userId", "username");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "orderID,userID,cartID,orderDate,status,shipping,totalPay,paymentType,address")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cartID = new SelectList(db.Carts, "cartId", "cartId", order.cartID);
            ViewBag.paymentType = new SelectList(db.paymentTypes, "paymentTypeID", "paymentTypeName", order.paymentType);
            ViewBag.userID = new SelectList(db.AspNetUsers, "userId", "username", order.userID);
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
            ViewBag.cartID = new SelectList(db.Carts, "cartId", "cartId", order.cartID);
            ViewBag.paymentType = new SelectList(db.paymentTypes, "paymentTypeID", "paymentTypeName", order.paymentType);
            ViewBag.userID = new SelectList(db.AspNetUsers, "userId", "username", order.userID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "orderID,userID,cartID,orderDate,status,shipping,totalPay,paymentType,address")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cartID = new SelectList(db.Carts, "cartId", "cartId", order.cartID);
            ViewBag.paymentType = new SelectList(db.paymentTypes, "paymentTypeID", "paymentTypeName", order.paymentType);
            ViewBag.userID = new SelectList(db.AspNetUsers, "userId", "username", order.userID);
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
    }
}
