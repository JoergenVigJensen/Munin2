﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Munin.DAL;
using Munin.DAL.Models;
using Munin.Web.ViewModels;
using Newtonsoft.Json;
using Munin.DAL.SQLite;

namespace Munin.Web.Controllers
{
    public class SequencesController : Controller
    {
        private MuninLiteContext db = new MuninLiteContext();
        //private MuninDb db = new MuninDb();

        // GET: Sequences
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SequenceList(ListQuery query)
        {
            try
            {
                using (var dbmunin = new MuninDb())
                {

                    int flicks = query.P * query.Size;

                    var l = await dbmunin.Sequences.ToListAsync();

                    if (!string.IsNullOrEmpty(query.Q))
                    {
                        string[] sQuery = query.Q.Split(' ');
                        for (int i = 0; i < sQuery.Length; i++)
                        {
                            l = l.Where(x =>
                                (x.SequenceNb.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Comment != null && x.Comment.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.DateTime != null && x.DateTime.ToString().ToLower().Contains(sQuery[i].ToLower()))
                            ).ToList();
                        }
                    }


                    var column = typeof(Sequence).GetProperty(query.S,
                        BindingFlags.SetProperty | BindingFlags.IgnoreCase |
                        BindingFlags.Public | BindingFlags.Instance);


                    if (column != null)
                    {
                        if (query.O.ToUpper() == "DESC")
                        {
                            l = l.OrderByDescending(x => column.GetValue(x, null)).ToList();
                        }
                        else
                        {
                            l = l.OrderBy(x => column.GetValue(x, null)).ToList();
                        }
                    }

                    var pageResult = l.Select(x => new ListRowDto()
                    {
                        Id = x.SequenceId,
                        Date = x.DateTime.ToString("dd-MM-yyyy"),
                        Title = x.Comment,
                        Index = x.SequenceNb,
                        Ticks = x.DateTime.Ticks

                    }).Skip(1).Take(query.Size);

                    if (l.Count > flicks)
                    {
                        pageResult = l.Select(x => new ListRowDto()
                        {
                            Id = x.SequenceId,
                            Date = x.DateTime.ToString("dd-MM-yyyy"),
                            Title = x.Comment,
                            Index = x.SequenceNb,
                            Ticks = x.DateTime.Ticks
                        }).Skip(flicks).Take(query.Size);
                    }

                    var listResult = new ItemListVm<ListRowDto>()
                    {
                        Count = l.Count,
                        Pages = l.Count / query.Size,
                        Data = pageResult.ToList()
                    };

                    var result = JsonConvert.SerializeObject(listResult, Utils.JsonSettings());

                    return Content(result);
                }
            }
            catch (Exception ex)
            {
                return Content("Fejl:" + ex.Message);
            }
        }

        public async Task<ActionResult> Load(int id = 0)
        {
            SequenceVm vm = new SequenceVm();
            try
            {
                    vm.JournalList =
                        db.Journals.Where(x => x.JournalNb != null)
                            .Select(x => new UISelectItem() {Value = x.JournalId, Text = x.JournalNb})
                            .ToList();

                    vm.SequenceTypes = Utils.SelectListOf<SequenceType>();
                    vm.Model = new Sequence();

                    if (id > 0)
                    {
                        var item = await db.Sequences
                            .Include(x => x.Journal)
                            .FirstOrDefaultAsync(x => x.SequenceId == id);
                        if (item != null)
                        {
                            vm.Model.DateTime = item.DateTime;
                            vm.Model.Journal = item.Journal;
                            vm.Model.SequenceId = item.SequenceId;
                            vm.Model.Comment = item.Comment;
                            vm.Model.DateTime = item.DateTime;
                            vm.Model.SequenceType = item.SequenceType;
                            vm.Model.SequenceNb = item.SequenceNb;
                            vm.Model.Source = item.Source;
                            vm.Model.Interviewer = item.Interviewer;
                            vm.Model.Copyright = item.Copyright;
                        }
                    }
                    else
                    {                        
                        vm.Model.SequenceType = SequenceType.Lydbånd;
                        vm.Model.SequenceNb = GetNextIndex(vm.Model.SequenceType);
                        vm.Model.DateTime = DateTime.Now.Date;
                    }

                    var result = JsonConvert.SerializeObject(vm, Utils.JsonSettings());
                    return Content(result);
            }
            catch (Exception ex)
            {
                var result = JsonConvert.SerializeObject(ex.Message, Utils.JsonSettings());
                return Content(result);
            }
        }

        public async Task<ActionResult> Save(Sequence model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    message = ModelState.Keys.SelectMany(k => ModelState[k].Errors).First().ErrorMessage
                });

            try
            {

                if (model.Journal == null)
                {
                    throw new Exception("Der skal vælges en Journal.");
                }

                using (var db = new MuninDb())
                {

                    var dbModel = new Sequence();

                    if (model.SequenceId > 0)
                    {
                        dbModel =
                            await db.Sequences.Include(x => x.Journal)
                                .FirstOrDefaultAsync(x => x.SequenceId == model.SequenceId);
                        if (dbModel == null)
                            throw new Exception(string.Format("Udklip med id {0} blev ikke fundet", model.SequenceId));
                    }

                    var journal = db.Journals.FirstOrDefault(x => x.JournalId == model.Journal.JournalId);
                    if (journal == null)
                        throw new Exception(string.Format("Journal '{0}' blev ikke fundet", model.Journal.JournalNb));

                    dbModel.DateTime = model.DateTime;
                    dbModel.SequenceId = model.SequenceId;
                    dbModel.Comment = model.Comment;
                    dbModel.DateTime = model.DateTime;
                    dbModel.SequenceType = model.SequenceType;
                    dbModel.SequenceNb = model.SequenceNb;
                    dbModel.Source = model.Source;
                    dbModel.Interviewer = model.Interviewer;
                    dbModel.Copyright = model.Copyright;
                    dbModel.Journal = journal;

                    if (model.SequenceId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Sequences.Add(dbModel);
                    }

                    await db.SaveChangesAsync();
                    return Json(new {success = true, message = ""});
                }
            }
            catch (Exception ex)
            {
                //brug extensible funktion
                string err_message = ex.Message;

                if (ex.InnerException != null)
                {
                    err_message = ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                        err_message = ex.InnerException.InnerException.Message;
                }

                return Json(new
                {
                    success = false,
                    message = err_message
                });
            }

        }

        [HttpPost]
        public ActionResult GetSesquenceNb(int seqType)
        {
            try
            {
                string output = GetNextIndex((SequenceType)seqType);

                //string output = GetNextIndex(seqNumber);

                return Json(new {success = true, output});
            }
            catch (Exception ex)
            {
                return Json(new {success = false, message = ex.Message});
            }

        }

        private string GetNextIndex(string seqNumber)
        {
            using (var db = new MuninDb())
            {
                if (seqNumber.Length < 2)
                        throw new Exception("Sekevenstypen er skal starte med mindst 2 bogstaver.");

                string preFix = seqNumber.Split('.')[0];
                int counter = 0;
                string[] index;

                var item =
                    db.Sequences.Where(x => x.SequenceNb.ToLower().StartsWith(preFix.ToLower()))
                        .OrderByDescending(x => x.
                            SequenceNb);

                if (item.ToList().Count == 0)
                {
                    index = seqNumber.Split('.');
                }
                else
                {
                    index = item.First().SequenceNb.Split('.');
                }

                if (index.Length == 2)
                {
                    if (Int32.TryParse(index[1], out counter))
                        counter++;
                    else
                    {
                        counter = 1;
                    }
                }

                return string.Format("{0}.{1}", preFix, counter.ToString().PadLeft(4, '0'));
            }
        }

        private string GetNextIndex(SequenceType seqType)
        {
            switch (seqType)
            {
                case SequenceType.Lydbånd:
                case SequenceType.LY:
                case SequenceType.MC:
                case SequenceType.MC2x:
                case SequenceType.VoiceRecorder:
                {
                    return GetNextIndex("Ly.0000");
                }                
                case SequenceType.VHS:
                case SequenceType.video:
                {
                    return GetNextIndex("Vi.000");
                }
                case SequenceType.DVD:
                {
                    return GetNextIndex("Dv.0000");
                }
                case SequenceType.Filmfarve8mm:
                case SequenceType.SmalfilmSH8mm:
                case SequenceType.Smalfilm8mm:
                case SequenceType.mmSmalfilm95mm:
                case SequenceType.Superfilm8mm:
                {
                    return GetNextIndex("Fi.0000");
                }
                default:
                {
                    return GetNextIndex("An.0000");
                }
            }
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
            var vm = new SequenceVm();
            vm.Model = new Sequence();
            vm.Model.SequenceId = 0;

            return View(vm.Model);
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
