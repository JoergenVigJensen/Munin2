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
    public class SequencesController : Controller
    {
        private MuninDb db = new MuninDb();

        // GET: Sequences
        public ActionResult Index()
        {
            return View(db.Sequences.ToList());
        }

        // GET: Sequences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sequence sequence = db.Sequences.Find(id);
            if (sequence == null)
            {
                return HttpNotFound();
            }
            return View(sequence);
        }

        // GET: Sequences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sequences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SequenceId,SequenceNb,DateTime,Source,Interviewer,Copyright,SequenceType,Comment")] Sequence sequence)
        {
            if (ModelState.IsValid)
            {
                db.Sequences.Add(sequence);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sequence);
        }

        // GET: Sequences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sequence sequence = db.Sequences.Find(id);
            if (sequence == null)
            {
                return HttpNotFound();
            }
            return View(sequence);
        }

        // POST: Sequences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SequenceId,SequenceNb,DateTime,Source,Interviewer,Copyright,SequenceType,Comment")] Sequence sequence)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sequence).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sequence);
        }

        // GET: Sequences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sequence sequence = db.Sequences.Find(id);
            if (sequence == null)
            {
                return HttpNotFound();
            }
            return View(sequence);
        }

        // POST: Sequences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sequence sequence = db.Sequences.Find(id);
            db.Sequences.Remove(sequence);
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
