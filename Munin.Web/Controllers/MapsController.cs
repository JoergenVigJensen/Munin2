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
    public class MapsController : Controller
    {
        private MuninDb db = new MuninDb();

        // GET: Maps
        public ActionResult Index()
        {
            return View();
        }


        public async Task<ActionResult> MapList(ListQuery query)
        {
            try
            {
                using (var dbmunin = new MuninDb())
                {

                    int flicks = query.P * query.Size;

                    var l = await dbmunin.Maps.ToListAsync();

                    if (!string.IsNullOrEmpty(query.Q))
                    {
                        string[] sQuery = query.Q.Split(' ');
                        for (int i = 0; i < sQuery.Length; i++)
                        {
                            l = l.Where(x =>
                                (x.MapNb.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Title != null && x.Title.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.PublishYear.ToString().Contains(sQuery[i].ToLower())) ||
                                (x.RevisionYear.ToString().Contains(sQuery[i].ToLower())) ||
                                (x.Area != null && x.Area.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Depot != null && x.Depot.Contains(sQuery[i].ToLower())) ||
                                (x.Comment != null && x.Comment.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Publisher != null && x.Publisher.ToLower().Contains(sQuery[i].ToLower()))
                                ).ToList();
                        }
                    }


                    var column = typeof(Map).GetProperty(query.S,
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

                    var pageResult = l.Select(x => new MapListDto()
                    {
                        Id = x.MapId,
                        Title = x.Title,
                        Index = x.MapNb,
                        PublishYear = x.PublishYear,
                        Area = x.Area
                    }).Skip(1).Take(query.Size);

                    if (l.Count > flicks)
                    {
                        pageResult = l.Select(x => new MapListDto()
                        {
                            Id = x.MapId,
                            Title = x.Title,
                            Index = x.MapNb,
                            PublishYear = x.PublishYear,
                            Area = x.Area
                        }).Skip(flicks).Take(query.Size);
                    }

                    var listResult = new ItemListVm<MapListDto>()
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
            MapVm vm = new MapVm();
            try
            {
                using (MuninDb db = new MuninDb())
                {
                    vm.JournalList =
                        db.Journals.Where(x => x.JournalNb != null).Select(x => new UISelectItem() { Value = x.JournalId, Text = x.JournalNb }).ToList();

                    vm.MaterialList = Utils.SelectListOf<MapMaterial>();
                    vm.MapTypes = Utils.SelectListOf<MapType>();

                    vm.Model = new Map();

                    if (id > 0)
                    {
                        var map = await db.Maps
                            .Include(x => x.Journal)
                            .FirstOrDefaultAsync(x => x.MapId == id);
                        if (map != null)
                        {
                            vm.Model.MapId = map.MapId;
                            vm.Model.MapNb = map.MapNb;
                            vm.Model.Journal = map.Journal;
                            vm.Model.Area = map.Area;
                            vm.Model.PublishYear = map.PublishYear;
                            vm.Model.Publisher = map.Publisher;
                            vm.Model.Depot = map.Depot;
                            vm.Model.Title = map.Title;
                            vm.Model.Format = map.Format;
                            vm.Model.MapType = map.MapType;
                            vm.Model.Proportion = map.Proportion;
                            vm.Model.Material = map.Material;
                            vm.Model.Comment = map.Comment;
                        }
                    }
                    else
                    {
                        var mapIndex = db.Maps.OrderByDescending(x => x.MapNb).First().MapNb;
                        string indexCount = mapIndex.Split('.')[1];
                        string indexNumber = "";
                        for (int i = 0; i < indexCount.Length; i++)
                        {
                            if ("0123456789".IndexOf(indexCount[i]) < 0)
                                indexNumber += indexCount[i];
                        }
                        int bindex = Int32.Parse(indexNumber);
                        bindex++;
                        vm.Model.MapNb = "K." + bindex.ToString().PadLeft(4, '0');
                    }

                    var result = JsonConvert.SerializeObject(vm, Utils.JsonSettings());
                    return Content(result);

                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(Map model)
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

                    var dbModel = new Map();

                    if (model.MapId > 0)
                    {
                        dbModel = await db.Maps.FirstOrDefaultAsync(x => x.MapId == model.MapId);
                        if (dbModel == null)
                            throw new Exception(string.Format("Kort med id {0} blev ikke fundet", model.MapId));
                    }
                    var journal = db.Journals.FirstOrDefault(x => x.JournalId == model.Journal.JournalId);
                    if (journal == null)
                        throw new Exception(string.Format("Journal '{0}' blev ikke fundet", model.Journal.JournalNb));

                    dbModel.MapId = model.MapId;
                    dbModel.Journal = model.Journal;
                    dbModel.Area = model.Area;
                    dbModel.PublishYear = model.PublishYear;
                    dbModel.Publisher = model.Publisher;
                    dbModel.Depot = model.Depot;
                    dbModel.Title = model.Title;
                    dbModel.Format = model.Format;
                    dbModel.MapType = model.MapType;
                    dbModel.Proportion = model.Proportion;
                    dbModel.Material = model.Material;
                    dbModel.Comment = model.Comment;

                    if (model.MapId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Maps.Add(dbModel);
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

        // GET: Maps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // GET: Maps/Create
        public ActionResult Create()
        {
            return View();
        }


        // GET: Maps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }


        // GET: Maps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Map map = db.Maps.Find(id);
            if (map == null)
            {
                return HttpNotFound();
            }
            return View(map);
        }

        // POST: Maps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Map map = db.Maps.Find(id);
            db.Maps.Remove(map);
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
