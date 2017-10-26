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
using Munin.DAL.SQLite;

namespace Munin.Web.Controllers
{
    public class CadastersController : Controller
    {
        private MuninLiteContext db = new MuninLiteContext();
        //private MuninDb db = new MuninDb();

        // GET: Cadasters
        public ActionResult Index()
        {
            return View(db.Cadasters.ToList());
        }

        // GET: Cadasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadaster cadaster = db.Cadasters.Find(id);
            if (cadaster == null)
            {
                return HttpNotFound();
            }
            return View(cadaster);
        }

        // GET: Cadasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cadasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CadasterId,CadasterNb,Source,CreatedYear,Year,StamNr,User,Street,Number,Area,ZipNumber,City,Comment")] Cadaster cadaster)
        {
            if (ModelState.IsValid)
            {
                db.Cadasters.Add(cadaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cadaster);
        }

        // GET: Cadasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadaster cadaster = db.Cadasters.Find(id);
            if (cadaster == null)
            {
                return HttpNotFound();
            }
            return View(cadaster);
        }

        // POST: Cadasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CadasterId,CadasterNb,Source,CreatedYear,Year,StamNr,User,Street,Number,Area,ZipNumber,City,Comment")] Cadaster cadaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cadaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cadaster);
        }

        // GET: Cadasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadaster cadaster = db.Cadasters.Find(id);
            if (cadaster == null)
            {
                return HttpNotFound();
            }
            return View(cadaster);
        }

        // POST: Cadasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cadaster cadaster = db.Cadasters.Find(id);
            db.Cadasters.Remove(cadaster);
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
