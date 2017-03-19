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
    public class ClipsController : Controller
    {
        private MuninDb db = new MuninDb();

        // GET: Clips
        public ActionResult Index()
        {
            return View(db.Clips.ToList());
        }

        // GET: Clips/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clip clip = db.Clips.Find(id);
            if (clip == null)
            {
                return HttpNotFound();
            }
            return View(clip);
        }

        public async Task<ActionResult> ClipsList(ListQuery query)
        {
            try
            {
                using (var dbmunin = new MuninDb())
                {

                    int flicks = query.P * query.Size;

                    var l = await dbmunin.Clips.ToListAsync();

                    if (!string.IsNullOrEmpty(query.Q))
                    {
                        string[] sQuery = query.Q.Split(' ');
                        for (int i = 0; i < sQuery.Length; i++)
                        {
                            l = l.Where(x =>
                                (x.Attache.ToLower().Contains(sQuery[0].ToLower())) ||
                                (x.Title.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Comment != null && x.Comment.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.DateTime.ToString().Contains(sQuery[i]))
                                ).ToList();
                        }
                    }


                    var column = typeof(Clip).GetProperty(query.S,
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
                        Id = x.ClipId,
                        Date = x.DateTime.ToString("dd-MM-yyyy"),
                        Title = x.Title,
                        Index = x.Attache,
                        Ticks = x.DateTime.Ticks
                    }).Skip(1).Take(query.Size);

                    if (l.Count > flicks)
                    {
                        pageResult = l.Select(x => new ListRowDto()
                        {
                            Id = x.ClipId,
                            Index = x.Attache,
                            Date = x.DateTime.ToString("dd-MM-yyyy"),
                            Title = x.Title,
                            Ticks = x.DateTime.Ticks

                        }).Skip(flicks).Take(query.Size);
                    }

                    var listResult = new ItemListVm()
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
            ClipVm vm = new ClipVm();
            try
            {
                using (MuninDb db = new MuninDb())
                {
                    vm.Papers = Utils.SelectListOf<PaperEnum>();
                    vm.Model = new Clip();

                    if (id > 0)
                    {
                        var item = await db.Clips
                            .FirstOrDefaultAsync(x => x.ClipId == id);
                        if (item != null)
                        {
                            vm.Model.DateTime = item.DateTime;

                            vm.Model.ClipId = item.ClipId;
                            vm.Model.Comment = item.Comment;
                            vm.Model.DateTime = item.DateTime;
                            vm.Model.Paper = item.Paper;
                            vm.Model.Title = item.Title;
                            vm.Model.Attache = item.Attache;
                        }
                    }
                    else
                    {
                        var pictureIndex = db.Pictures.OrderByDescending(x => x.PictureIndex).First().PictureIndex;
                        int bindex = Int32.Parse(pictureIndex.Split('.')[1]);
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


        public async Task<ActionResult> Save(Clip model)
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

                    var dbModel = new Clip();

                    if (model.ClipId > 0)
                    {
                        dbModel = await db.Clips.FirstOrDefaultAsync(x => x.ClipId == model.ClipId);
                        if (dbModel == null)
                            throw new Exception(string.Format("Udklip med id {0} blev ikke fundet", model.ClipId));
                    }
                    dbModel.DateTime = model.DateTime;
                    dbModel.Comment = model.Comment;
                    dbModel.Title = model.Title;
                    dbModel.Attache = model.Attache;
                    dbModel.Paper = model.Paper;

                    if (model.ClipId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Clips.Add(dbModel);
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


        // GET: Clips/Create
        public ActionResult Create()
        {
            var vm = new ClipVm();
            vm.Model = new Clip();
            vm.Model.ClipId = 0;

            return View(vm.Model);
        }

        // GET: Clips/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clip clip = db.Clips.Find(id);
            if (clip == null)
            {
                return HttpNotFound();
            }
            return View(clip);
        }

        // GET: Clips/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clip clip = db.Clips.Find(id);
            if (clip == null)
            {
                return HttpNotFound();
            }
            return View(clip);
        }

        // POST: Clips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clip clip = db.Clips.Find(id);
            db.Clips.Remove(clip);
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
