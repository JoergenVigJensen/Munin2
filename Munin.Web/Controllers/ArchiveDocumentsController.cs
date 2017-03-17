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
    public class ArchiveDocumentsController : Controller
    {
        private MuninDb db = new MuninDb();

        // GET: ArchiveDocuments
        public ActionResult Index()
        {
            return View(db.ArchiveDocuments.ToList());
        }

        // GET: ArchiveDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveDocument archiveDocument = db.ArchiveDocuments.Find(id);
            if (archiveDocument == null)
            {
                return HttpNotFound();
            }
            return View(archiveDocument);
        }

        // GET: ArchiveDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ArchiveDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ArchiveDocumentId,BoxNumber,Signature,Copyright,YearStart,YearEnd,Cover,Comment")] ArchiveDocument archiveDocument)
        {
            if (ModelState.IsValid)
            {
                db.ArchiveDocuments.Add(archiveDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(archiveDocument);
        }

        // GET: ArchiveDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveDocument archiveDocument = db.ArchiveDocuments.Find(id);
            if (archiveDocument == null)
            {
                return HttpNotFound();
            }
            return View(archiveDocument);
        }

        // POST: ArchiveDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ArchiveDocumentId,BoxNumber,Signature,Copyright,YearStart,YearEnd,Cover,Comment")] ArchiveDocument archiveDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archiveDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(archiveDocument);
        }

        // GET: ArchiveDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchiveDocument archiveDocument = db.ArchiveDocuments.Find(id);
            if (archiveDocument == null)
            {
                return HttpNotFound();
            }
            return View(archiveDocument);
        }

        // POST: ArchiveDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArchiveDocument archiveDocument = db.ArchiveDocuments.Find(id);
            db.ArchiveDocuments.Remove(archiveDocument);
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
