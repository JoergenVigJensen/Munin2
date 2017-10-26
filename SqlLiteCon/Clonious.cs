using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Munin.DAL;
using Munin.DAL.Models;
using Munin.DAL.SQLite;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.Entity;


namespace SqlLiteCon
{
    public class Clonious : IDisposable
    {



        public void CloneJournals(List<Journal> journals, MuninLiteContext db)
        {
            foreach (var journal in journals)
            {
                var j = Clone<Journal>(journal);
                db.Journals.Add(j);
                Console.WriteLine("Journal: " + j.JournalNb);
            }
            db.SaveChanges();
        }

        public void CloneArchiveDocs(List<ArchiveDocument> items, MuninLiteContext db)
        {
            foreach (var item in items)
            {
                var j = Clone(item);
                db.ArchiveDocuments.Add(item);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void CloneArchives(List<Archive> items, MuninLiteContext db)
        {
            foreach (var item in items)
            {
                var j = Clone<Archive>(item);
                db.Archives.Add(item);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void CloneBooks(List<Book> items, MuninLiteContext db)
        {

            var journals = db.Journals.ToList();

            foreach (var item in items)
            {
                var j = new Book()
                {
                    Attache = item.Attache,
                    Author = item.Author,
                    BookCode = item.BookCode,
                    BookId = item.BookId,
                    Comment = item.Comment,
                    DK5 = item.DK5,
                    Editor = item.Editor,
                    Journal = journals.FirstOrDefault(x=>x.JournalId == item.Journal.JournalId),
                    PublishYear = item.PublishYear,
                    SubTitle = item.SubTitle,
                    Tag = item.Tag,
                    Title = item.Title
                };
                db.Books.Add(j);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void Clonecadasters(List<Cadaster> items, MuninLiteContext db)
        {
            foreach (var item in items)
            {
                var j = Clone<Cadaster>(item);
                db.Cadasters.Add(j);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }


        public void CloneArchiveDocs(List<ChirchRecord> items, MuninLiteContext db)
        {
            foreach (var item in items)
            {
                var j = Clone<ChirchRecord>(item);
                db.ChirchRecords.Add(j);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void CloneArchiveDocs(List<Clip> items, MuninLiteContext db)
        {

            foreach (var item in items)
            {
                var j = new Clip() { 
                    ClipId = item.ClipId,
                    Attache = item.Attache,
                    Comment = item.Comment,
                    DateTime = item.DateTime,
                    Paper = item.Paper,
                    Title = item.Title,
                };
                var djd = db.Clips.Add(j);
                Console.WriteLine(item.ToString());
                db.SaveChanges();
            }

        }

        public void CloneArchiveDocs(List<Map> items, MuninLiteContext db)
        {
            var journals = db.Journals.ToList();

            foreach (var item in items)
            {
                var j = new Map() {
                    Area = item.Area,
                    Title = item.Title,
                    Comment = item.Comment,
                    Depot = item.Depot,
                    Format = item.Format,
                    Journal = journals.FirstOrDefault(x => x.JournalId == item.Journal.JournalId),
                    MapId = item.MapId,
                    MapNb = item.MapNb,
                    MapType = item.MapType,
                    Material = item.Material,
                    Proportion = item.Proportion,
                    Publisher = item.Publisher,
                    PublishYear = item.PublishYear,
                    RevisionYear = item.RevisionYear
                };
                db.Maps.Add(j);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void CloneArchiveDocs(List<Picture> items, MuninLiteContext db)
        {
            var journals = db.Journals.ToList();

            foreach (var item in items)
            {
                var j = new Picture() {
                    CDNb = item.CDNb,
                    Comment = item.Comment,
                    Copyright = item.Copyright,
                     DateTime = item.DateTime,
                     Journal = journals.FirstOrDefault(x=>x.JournalId == item.Journal.JournalId),
                     OrderNum = item.OrderNum,
                     Photograph = item.Photograph,
                     PictureId = item.PictureId,
                     PictureIndex = item.PictureIndex,
                     PictureMaterial = item.PictureMaterial,
                     Placed = item.Placed,
                     Provision = item.Provision,
                     Size = item.Size,
                     Tag = item.Tag                     
                };
                db.Pictures.Add(j);
                Console.WriteLine(item.ToString());
            }
            db.SaveChanges();
        }

        public void CloneArchiveDocs(List<Provider> items, MuninLiteContext db)
        {
            var journals = db.Journals.ToList();

            foreach (var item in items)
            {
                var j = new Provider()
                {
                    Address = item.Address,
                    Att = item.Att,
                    City = item.City,
                    Comment = item.Comment,
                    Journal = journals.FirstOrDefault(x=>x.JournalId == item.Journal.JournalId),
                    Name = item.Name,
                    ProviderId = item.ProviderId,
                    Registrated = item.Registrated,
                    ZipCode = item.ZipCode
                };
                db.Providers.Add(j);
                Console.WriteLine(item.ToString());
                db.SaveChanges();
            }
        }

        public void CloneArchiveDocs(List<Sequence> items, MuninLiteContext db)
        {
            var journals = db.Journals.ToList();

            foreach (var item in items)
            {
                var j = new Sequence()
                {
                    Comment = item.Comment,
                    Copyright = item.Copyright,
                    DateTime = item.DateTime,
                    Interviewer = item.Interviewer,
                    Journal = journals.FirstOrDefault(x=>x.JournalId == item.Journal.JournalId),
                    SequenceId = item.SequenceId,
                    SequenceNb = item.SequenceNb,
                    SequenceType = item.SequenceType,
                    Source = item.Source
                };
                db.Sequences.Add(j);
                Console.WriteLine(item.ToString());
                db.SaveChanges();
            }
        }



        private T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

    }
}
