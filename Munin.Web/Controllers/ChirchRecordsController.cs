using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Munin.DAL;
using Munin.DAL.Models;

namespace Munin.Web.Controllers
{
    public class ChirchRecordsController : Controller
    {
        private MuninDb db = new MuninDb();

        // GET: ChirchRecords
        public ActionResult Index()
        {
            return View(db.ChirchRecords.ToList());
        }

        // GET: ChirchRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChirchRecord chirchRecord = db.ChirchRecords.Find(id);
            if (chirchRecord == null)
            {
                return HttpNotFound();
            }
            return View(chirchRecord);
        }

        // GET: ChirchRecords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChirchRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChirchRecordId,FirstName,LastName,Event,Date,PersonId")] ChirchRecord chirchRecord)
        {
            if (ModelState.IsValid)
            {
                db.ChirchRecords.Add(chirchRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chirchRecord);
        }

        // GET: ChirchRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChirchRecord chirchRecord = db.ChirchRecords.Find(id);
            if (chirchRecord == null)
            {
                return HttpNotFound();
            }
            return View(chirchRecord);
        }

        // POST: ChirchRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChirchRecordId,FirstName,LastName,Event,Date,PersonId")] ChirchRecord chirchRecord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chirchRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chirchRecord);
        }

        // GET: ChirchRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChirchRecord chirchRecord = db.ChirchRecords.Find(id);
            if (chirchRecord == null)
            {
                return HttpNotFound();
            }
            return View(chirchRecord);
        }

        // POST: ChirchRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChirchRecord chirchRecord = db.ChirchRecords.Find(id);
            db.ChirchRecords.Remove(chirchRecord);
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
