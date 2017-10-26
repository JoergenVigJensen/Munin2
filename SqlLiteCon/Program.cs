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
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Så kører toget....");
            try
            {
                using (var dbLite = new MuninLiteContext())
                {

                    using (var dbms = new MuninDb())
                    {
                        using (var cl = new Clonious())
                        {
                            Console.WriteLine("Kopierer Arkivfonde...");
                            //var ardocs = dbms.ArchiveDocuments.ToList();
                            //var journals = dbms.Journals.ToList();
                            //CloneJournals(journals, dbLite);
                            //var arclist = dbms.Archives.ToList();
                            //cl.CloneArchives(arclist, dbLite);
                            //var booklist = dbms.Books.ToList();
                            //cl.CloneBooks(booklist, dbLite);
                            //var cadlist = dbms.Cadasters.ToList();
                            //cl.Clonecadasters(cadlist, dbLite);
                            //var chirch = dbms.ChirchRecords.ToList();
                            //cl.CloneArchiveDocs(chirch, dbLite);
                            //var clips = dbms.Clips.ToList();
                            //cl.CloneArchiveDocs(clips, dbLite);
                            //var maps = dbms.Maps.ToList();
                            //cl.CloneArchiveDocs(maps, dbLite);
                            //var pictures = dbms.Pictures.ToList();
                            //cl.CloneArchiveDocs(pictures, dbLite);
                            //var provs = dbms.Providers.ToList();
                            //cl.CloneArchiveDocs(provs, dbLite);

                            var seqs = dbms.Sequences.ToList();
                            cl.CloneArchiveDocs(seqs, dbLite);

                            //dbLite.SaveChanges();

                            Console.WriteLine("Finito");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        Console.WriteLine("Inner exception: " + ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    }
                    
                }
                else
                {
                    Console.WriteLine("Fejl: " + ex.Message);
                }
                
            }
            Console.ReadLine();
        }


  

    }
}
