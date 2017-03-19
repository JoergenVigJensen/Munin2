using System;
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

namespace Munin.Web.Controllers
{
    public class SequencesController : Controller
    {
        private MuninDb db = new MuninDb();

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
                using (MuninDb db = new MuninDb())
                {
                    vm.JournalList =
                       db.Journals.Where(x => x.JournalNb != null).Select(x => new UISelectItem() { Value = x.JournalId, Text = x.JournalNb }).ToList();

                    vm.SequenceTypes = Utils.SelectListOf<SequenceType>();
                    vm.Model = new Sequence();

                    if (id > 0)
                    {
                        var item = await db.Sequences
                            .Include(x=>x.Journal)
                            .FirstOrDefaultAsync(x => x.SequenceId == id);
                        if (item != null)
                        {
                            vm.Model.DateTime = item.DateTime;

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
                        var seqIndex = db.Sequences.OrderByDescending(x => x.SequenceNb).First().SequenceNb;
                        int bindex = Int32.Parse(seqIndex.Split('.')[1]);
                        bindex++;
                        vm.Model.DateTime = DateTime.Now.Date;
                    }

                    var result = JsonConvert.SerializeObject(vm, Utils.JsonSettings());
                    return Content(result);

                }
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
                using (var db = new MuninDb())
                {

                    var dbModel = new Sequence();

                    if (model.SequenceId > 0)
                    {
                        dbModel = await db.Sequences.Include(x=>x.Journal).FirstOrDefaultAsync(x => x.SequenceId == model.SequenceId);
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

                    if (model.SequenceId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Sequences.Add(dbModel);
                    }

                    await db.SaveChangesAsync();
                    return Json(new { success = true, message = "" });
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
