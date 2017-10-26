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
using WebGrease.Css.Extensions;
using Munin.DAL.SQLite;

namespace Munin.Web.Controllers
{
    public class PicturesController : Controller
    {
        private MuninLiteContext db = new MuninLiteContext();
        //private MuninDb db = new MuninDb();

        // GET: Pictures
        public ActionResult Index()
        {
            return View(db.Pictures.ToList());
        }

        public async Task<ActionResult> PictureList(ListQuery query)
        {
            try
            {
                using (var dbmunin = new MuninDb())
                {

                    int flicks = query.P * query.Size;

                    var l = await dbmunin.Pictures.ToListAsync();

                    if (!string.IsNullOrEmpty(query.Q))
                    {
                        string[] sQuery = query.Q.Split(' ');
                        for (int i = 0; i < sQuery.Length; i++)
                        {
                            l = l.Where(x =>
                                (x.PictureIndex.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Comment != null && x.Comment.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.OrderNum != null && x.OrderNum.ToLower().Contains(sQuery[i].ToLower()))
                                ).ToList();
                        }
                    }
                    

                    var column = typeof(Picture).GetProperty(query.S,
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
                        Id = x.PictureId,
                        Date = x.DateTime.ToString("dd-MM-yyyy"),
                        Title = x.Comment,
                        Index = x.PictureIndex,
                        Ticks = x.DateTime.Ticks
                    }).Skip(1).Take(query.Size);

                    if (l.Count > flicks)
                    {
                        pageResult = l.Select(x => new ListRowDto()
                        {
                            Id = x.PictureId,
                            Date = x.DateTime.ToString("dd-MM-yyyy"),
                            Title = x.Comment,
                            Index = x.PictureIndex,
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
            PictureVm vm = new PictureVm();
            try
            {
                vm.JournalList =
                    db.Journals.Where(x=>x.JournalNb != null).Select(x => new UISelectItem() {Value = x.JournalId, Text = x.JournalNb}).ToList();
                vm.MaterialList = Utils.SelectListOf<PictureMaterial>();
                vm.Model = new Picture();

                if (id > 0)
                {
                    var picture = await db.Pictures
                        .Include(x=>x.Journal)
                        .FirstOrDefaultAsync(x => x.PictureId == id);
                    if (picture != null)
                    {
                        vm.Model.DateTime = picture.DateTime;
                        vm.Model.PictureId = picture.PictureId;
                        vm.Model.CDNb = picture.CDNb;
                        vm.Model.Comment = picture.Comment;
                        vm.Model.Copyright = picture.Copyright;
                        vm.Model.Journal = picture.Journal;
                        vm.Model.Photograph = picture.Photograph;
                        vm.Model.PictureIndex = picture.PictureIndex;
                        vm.Model.PictureMaterial = picture.PictureMaterial;
                        vm.Model.OrderNum = picture.OrderNum;
                        vm.Model.Tag = picture.Tag;
                        vm.Model.Provision = picture.Provision;
                        vm.Model.Size = picture.Size;
                        vm.Model.Placed = picture.Placed;
                    }
                }
                else
                {
                    var pictureIndex = db.Pictures.OrderByDescending(x => x.PictureIndex).First().PictureIndex;
                    int bindex = Int32.Parse(pictureIndex.Split('.')[1]);
                    bindex++;
                    vm.Model.PictureIndex = "B." + bindex.ToString().PadLeft(4, '0');
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

        [HttpPost]
        public async Task<ActionResult> Save(Picture model)
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

                    var dbModel = new Picture();

                    if (model.PictureId > 0)
                    {
                        dbModel = await db.Pictures.FirstOrDefaultAsync(x => x.PictureId == model.PictureId);
                        if (dbModel == null)
                            throw new Exception(string.Format("Billede med id {0} blev ikke fundet", model.PictureId));
                    }
                    var journal = db.Journals.FirstOrDefault(x => x.JournalId == model.Journal.JournalId);
                    if (journal == null)
                        throw new Exception(string.Format("Journal '{0}' blev ikke fundet", model.Journal.JournalNb));

                    dbModel.DateTime = model.DateTime;
                    dbModel.PictureIndex = model.PictureIndex;
                    dbModel.CDNb = model.CDNb;
                    dbModel.Size = model.Size;
                    dbModel.Photograph = model.Photograph;
                    dbModel.Journal = journal;
                    dbModel.PictureId = model.PictureId;
                    dbModel.Provision = model.Provision;
                    dbModel.PictureMaterial = model.PictureMaterial;
                    dbModel.Comment = model.Comment;
                    dbModel.OrderNum = model.OrderNum;
                    dbModel.Copyright = model.Copyright;
                    dbModel.Tag = model.Tag;
                    dbModel.Placed = model.Placed;

                    if (model.PictureId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Pictures.Add(dbModel);
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

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            var vm = new PictureVm();
            vm.Model = new Picture();
            vm.Model.PictureId = 0;

            return View(vm.Model);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
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
