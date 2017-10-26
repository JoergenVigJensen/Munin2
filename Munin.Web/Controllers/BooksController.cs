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
using Munin.DAL.SQLite;

namespace Munin.Web.Controllers
{
    public class BooksController : Controller
    {
        private MuninLiteContext db = new MuninLiteContext();
        //private MuninDb db = new MuninDb();

        // GET: Books
        public ActionResult Index()
        {
            return View(db.Books.ToList());
        }

        public async Task<ActionResult> BooksList(ListQuery query)
        {
            try
            {
                using (var dbmunin = new MuninDb())
                {

                    int flicks = query.P * query.Size;

                    var l = await dbmunin.Books.ToListAsync();

                    if (!string.IsNullOrEmpty(query.Q))
                    {
                        string[] sQuery = query.Q.Split(' ');
                        for (int i = 0; i < sQuery.Length; i++)
                        {
                            l = l.Where(x =>
                                (x.BookCode.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Title != null && x.Title.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.SubTitle != null && x.SubTitle.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Author != null && x.Author.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Editor != null && x.Editor.ToLower().Contains(sQuery[i].ToLower())) ||
                                (x.Comment != null && x.Comment.ToLower().Contains(sQuery[i].ToLower()))
                                ).ToList();
                        }
                    }


                    var column = typeof(Book).GetProperty(query.S,
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

                    var pageResult = l.Select(x => new BookListRowDto()
                    {
                        Id = x.BookId,
                        Index = x.BookCode,
                        Title = x.Title,
                        Author = x.Author
                    }).Skip(1).Take(query.Size);

                    if (l.Count > flicks)
                    {
                        pageResult = l.Select(x => new BookListRowDto()
                        {
                            Id = x.BookId,
                            Index = x.BookCode,
                            Title = x.Title,
                            Author = x.Author

                        }).Skip(flicks).Take(query.Size);
                    }

                    var listResult = new ItemListVm<BookListRowDto>()
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
            BookVm vm = new BookVm();
            try
            {
                vm.JournalList =
                    db.Journals.Where(x => x.JournalNb != null).Select(x => new UISelectItem() { Value = x.JournalId, Text = x.JournalNb }).ToList();
                vm.Model = new Book();

                if (id > 0)
                {
                    var book = await db.Books
                        .Include(x => x.Journal)
                        .FirstOrDefaultAsync(x => x.BookId == id);
                    if (book != null)
                    {
                        vm.Model.BookId = book.BookId;
                        vm.Model.DK5 = book.DK5;
                        vm.Model.Author = book.Author;
                        vm.Model.BookCode = book.BookCode;
                        vm.Model.Editor = book.Editor;
                        vm.Model.Title = book.Title;
                        vm.Model.SubTitle = book.SubTitle;
                        vm.Model.PublishYear = book.PublishYear;
                        vm.Model.Journal = book.Journal;
                        vm.Model.Comment = book.Comment;
                        vm.Model.Tag = book.Tag;
                    }
                }
                else
                {
                    var bookcode = db.Books.OrderByDescending(x => x.BookCode).First().BookCode;
                    int bindex = Int32.Parse(bookcode.Split('.')[1]);
                    bindex++;
                    vm.Model.BookCode = "B." + bindex.ToString().PadLeft(4, '0');
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
        public async Task<ActionResult> Save(Book model)
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

                    var dbModel = new Book();

                    if (model.BookId > 0)
                    {
                        dbModel = await db.Books.FirstOrDefaultAsync(x => x.BookId == model.BookId);
                        if (dbModel == null)
                            throw new Exception(string.Format("Bog med id {0} blev ikke fundet", model.BookId));
                    }
                    var journal = db.Journals.FirstOrDefault(x => x.JournalId == model.Journal.JournalId);
                    if (journal == null)
                        throw new Exception(string.Format("Journal '{0}' blev ikke fundet", model.Journal.JournalNb));

                    dbModel.BookId = model.BookId;
                    dbModel.DK5 = model.DK5;
                    dbModel.Author = model.Author;
                    dbModel.BookCode = model.BookCode;
                    dbModel.Editor = model.Editor;
                    dbModel.Title = model.Title;
                    dbModel.SubTitle = model.SubTitle;
                    dbModel.PublishYear = model.PublishYear;
                    dbModel.Journal = model.Journal;
                    dbModel.Tag = model.Tag;


                    if (model.BookId > 0)
                        db.Entry(dbModel).State = EntityState.Modified;
                    else
                    {
                        db.Books.Add(dbModel);
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

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            var vm = new BookVm();
            vm.Model = new Book();
            vm.Model.BookId = 0;

            return View(vm.Model);
        }

  
        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

 
        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
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
