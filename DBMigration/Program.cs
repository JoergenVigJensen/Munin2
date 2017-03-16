using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Utilities;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using Munin.DAL;
using Munin.DAL.Models;
using ILABDb.DAL;

namespace DBMigration
{
    class Program
    {

        private static StreamWriter writer = File.AppendText("Logfil.txt");

        private static string ArchiveGUID;
        private static string ArchiveDocGUID;
        private static string BookGUID;
        private static string CadasterGUID;
        private static string ChirchGUID;
        private static string clipGUID;
        private static string journalGUID;
        private static string MapGUID;
        private static string pictureGUID;
        private static string ProviderGUID;
        private static string SeqGUID;

        static void Main(string[] args)
        {
            bool running = false;

            Archive ar = new Archive();

            Archive ar2 = new Archive();

            ArchiveDocument adoc = new ArchiveDocument();

            var t = ar.GetType();

            var s = Console.ReadLine();

            running = CreateProviders();

            ArchiveGUID = typeof(Archive).GUID.ToString();
            ArchiveDocGUID = typeof(ArchiveDocument).GUID.ToString();
            BookGUID = typeof(Book).GUID.ToString();
            CadasterGUID = typeof(Cadaster).GUID.ToString();
            ChirchGUID = typeof(ChirchRecord).GUID.ToString();
            clipGUID = typeof(Clip).GUID.ToString();
            journalGUID = typeof(Munin.DAL.Models.Journal).GUID.ToString();
            MapGUID = typeof(Map).GUID.ToString();
            pictureGUID = typeof(Picture).GUID.ToString();
            ProviderGUID = typeof(Provider).GUID.ToString();
            SeqGUID = typeof(Sequence).GUID.ToString();

            if (running)
                running = CreateJournals();

            CreateArchives();

            CreateArhiveDocs();

            CreateArhiveMaps();

            CreateArhiveBooks();

            CreateCadaster();

            CreateClips();

            CreatePicture();

            CreateSequence();
        }

        private static void CreateSequence()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Sekvenser.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Map>(item.Journal, muninDb);
                        var seq = new Sequence()
                        {
                            SequenceNb = item.SekvensNr,
                            Journal = journal,
                            Copyright = item.Klausul,
                            Source = item.Kilde,
                            Interviewer = item.Interviewer,                           
                            Comment = item.Note                            
                        };

                        seq.SequenceType = GetSeqType(item.Type);
                        if (seq.SequenceType == SequenceType.Andet)
                            message += String.Format("Sekvens {0} har et andet format", item.SekvensNr);

                        DateTime dt = mergeDatetime(item.DateringAar, item.DateringMrd, item.DateringDag);

                        if (dt == DateTime.MinValue)
                            message += string.Format("Sekvens {0} har ikke angivet dato", item.SekvensNr);

                        seq.DateTime = dt;

                        muninDb.Sequences.Add(seq);
                        muninDb.SaveChanges();


                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.SekvensNr, ex.Message);
                    }
                    Log(message);
                }
            }
        }

        private static SequenceType GetSeqType(string sekvenstype)
        {
            if (sekvenstype.ToUpper().IndexOf("SUPER") > 0) return SequenceType.Superfilm8mm;

            if (sekvenstype.ToUpper().IndexOf("FARVE") > 0) return SequenceType.Filmfarve8mm;

            if (sekvenstype.ToUpper().IndexOf("SH") > 0) return SequenceType.SmalfilmSH8mm;

            if (sekvenstype.IndexOf("8") > 0) return SequenceType.Smalfilm8mm;

            if (sekvenstype.ToUpper().IndexOf("9.5") > 0 || sekvenstype.ToUpper().IndexOf("9,5") > 0) return SequenceType.mmSmalfilm95mm;

            if (sekvenstype.ToUpper().IndexOf("VHS") > 0) return SequenceType.VHS;

            if (sekvenstype.ToUpper().IndexOf("LYDBÅND") > 0) return SequenceType.Lydbånd;

            if (sekvenstype.ToUpper().IndexOf("LY") > 0) return SequenceType.LY;

            if (sekvenstype.ToUpper().IndexOf("X") > 0 || sekvenstype.ToUpper().IndexOf("2 BÅND") > 0) return SequenceType.MC2x;

            if (sekvenstype.ToUpper().IndexOf("MC") > 0) return SequenceType.MC;

            if (sekvenstype.ToUpper().IndexOf("DVD") > 0) return SequenceType.DVD;

            if (sekvenstype.ToUpper().IndexOf("DVS") > 0) return SequenceType.DVS;

            if (sekvenstype.ToUpper().IndexOf("FOREDRAG") > 0) return SequenceType.Foredrag;

            if (sekvenstype.ToUpper().IndexOf("PROD") > 0) return SequenceType.Produktion;

            if (sekvenstype.ToUpper().IndexOf("VOICE") > 0) return SequenceType.VoiceRecorder;

            if (sekvenstype.ToUpper().IndexOf("ENAKTER") > 0) return SequenceType.SmåEnakter;

            if (sekvenstype.ToUpper().IndexOf("VIDEO") > 0) return SequenceType.video;

            return SequenceType.Andet;
        }

        private static void CreatePicture()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Billeder.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Picture>(item.Journal, muninDb);
                        var pic = new Picture()
                        {
                            PictureIndex = item.Billedindex,
                            CDNb = item.CDnr,
                            Copyright = item.Ophavsret,
                            Journal = journal,
                            Photograph = item.Fotograf,
                            OrderNum = item.Numordning,
                            Provision = item.Klausul,
                            Size = item.Format,
                            Tag = item.Ordning,
                            Placed = item.Placering,                                                        
                            Comment = item.Note                            
                        };
                        pic.PictureMaterial = (PictureMaterial) ((item.Materiale == null) ? 6 : (int) item.Materiale.Value);

                        muninDb.Pictures.Add(pic);
                        muninDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.Billedindex, ex.Message);
                    }
                    Log(message);
                }
            }
        }

   

        private static void CreateClips()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.udklip.ToList())
                {
                    string message = "";

                    try
                    {
                        var clip = new Clip()
                        {
                            Attache = item.Mappe,
                            Paper = GetPaperEnum(item.Aviskode),
                            Comment = item.Note,
                            Title = item.Overskrift
                        };

                        DateTime dt;

                        if (!DateTime.TryParse(item.Datering, out dt))
                            message += string.Format("Udklip {0} har ikke angivet aflevering",item.UdklipsID);

                        clip.DateTime = dt;

                        muninDb.Clips.Add(clip);
                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.UdklipsID, ex.Message);
                    }
                    Log(message);
                }
            }
        }

        private static PaperEnum GetPaperEnum(string aviskode)
        {
            switch (aviskode)
            {
                case "UA":
                    return PaperEnum.UgeAvisen;
                case "OP":
                    return PaperEnum.OdensePosten;
                case "BE":
                    return PaperEnum.BerlingskeTidende;
                case "EB":
                    return PaperEnum.EkstraBladet;
                case "MP":
                    return PaperEnum.MorgenPosten;
                case "DH":
                    return PaperEnum.DalumHallese;
                case "UL":
                    return PaperEnum.UgeskriftForLaeger;
                case "FS":
                    return PaperEnum.FyensStiftsTidende;
                case "PO":
                    return PaperEnum.PostalPost;
                case "FT":
                    return PaperEnum.FyensTidende;
                case "FA":
                    return PaperEnum.FyensAmtsAvis;
                default:
                    return PaperEnum.AndenAvis;
            }
        }

        private static void CreateCadaster()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Matrikler.ToList())
                {
                    string message = "";

                    try
                    {
                        var cadaster = new Cadaster()
                        {
                            CadasterNb = item.MatrikelNr,
                            Source = item.Kilde,
                            CreatedYear = Int32.Parse(item.Oprettet),
                            Year = Int32.Parse(item.Dato),
                            StamNr = item.Stamnr,
                            User = item.Bruger,
                            Street = item.Vejnavn,
                            Number = item.Vejnr,
                            ZipNumber = Int32.Parse(item.Postnr),
                            City = item.By,
                            Comment = item.Note                            
                        };

                        muninDb.Cadasters.Add(cadaster);
                        muninDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.MatrikelNr, ex.Message);
                    }
                    Log(message);
                }
            }
        }

        private static void CreateArhiveBooks()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Bibliotek.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Book>(item.Journal, muninDb);

                        var book = new Book()
                        {
                            Journal = journal,
                            Title = item.Titel,
                            Attache = item.Samlemappe,
                            DK5 = item.DK5,
                            Author = item.Forfatter,
                            BookCode = item.Bogkode,
                            Editor = item.Redaktor,
                            PublishYear = (item.Udgivet == null)?0 : (int)item.Udgivet.Value,
                            SubTitle = item.Undertitel,
                            Comment = item.Note,
                            Tag = item.Ordningsord
                        };

                        if (book.PublishYear == 0)
                            message += string.Format("Bog: {0} mangler udgivelsesår", item.Bogkode);

                        muninDb.Books.Add(book);
                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.Bogkode, ex.Message);
                    }
                    Log(message);
                }
            }
        }

        private static void CreateArhiveMaps()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Kort.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Map>(item.Journal,muninDb);

                        var map = new Map()
                        {
                            Journal = journal,
                            MapNb = item.Kortnr,
                            Title = item.Navn,
                            MapType = GetMapType(item.KortType),
                            Comment = item.Note,
                            Format = item.Format,
                            Area = item.Omraade,
                            Publisher = item.Udgiver,
                            PublishYear = (item.DateringAar == null)?0 : item.DateringAar.Value,
                            RevisionYear = (item.RevAar == null) ? 0 : item.RevAar.Value,
                            Depot = item.Depot

                        };
                        if (map.MapType == MapType.Udefineret)
                            message += string.Format("Kort {0} har ikke korrekt format angivet");

                        muninDb.Maps.Add(map);
                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.Kortnr, ex.Message);
                    }
                    Log(message);
                }
            }
        }

        private static void CreateArhiveDocs()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Arkivalie.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Archive>(item.Journal, muninDb);
                        var archive = GetArchive(item.Arkivfondnr, muninDb);
                        ArchiveDocument doc = new ArchiveDocument()
                        {
                            Journal = journal,
                            AriArchive = archive,
                            Copyright = item.Klausul,
                            Signature = item.Signatur,
                            Cover = GetCover(item.Omfang)
                        };
                        doc.YearStart = (item.YderAarStart == null) ? 0 : (int) item.YderAarStart.Value;
                        doc.YearEnd = (item.YderAarSlut == null) ? 0 : (int) item.YderAarSlut.Value;
                        doc.Comment = item.Note;

                        muninDb.ArchiveDocuments.Add(doc);
                        muninDb.SaveChanges();
                        
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message);
                    }
                }
            }

        }

        private static MapType GetMapType(string korttype)
        {
            switch (korttype)
            {
                case "A4":
                    return MapType.A4;
                case "Atlasblad":
                    return MapType.Atlasblad;
                case "Grundtegning":
                    return MapType.Grundtegning;
                case "Lodret Luftfoto":
                    return MapType.Luftfoto;
                case "Matrikel":
                    return MapType.Matrikelkort;
                case "Matrikel- perspektivkort":
                    return MapType.Matrikelkort;
                case "Matrikelkort":
                    return MapType.Matrikelkort;
                case "Matrikelkort (Original 2)":
                    return MapType.Matrikelkort;
                case "Matrikelkort m. gadenavne":
                    return MapType.Matrikelkort;
                case "Matrikelkort- Transparent":
                    return MapType.Matrikelkort;
                case "Matrikelkort -Transparent":
                    return MapType.Matrikelkort;
                case "Målebordsblad":
                    return MapType.Maalebordsblad;
                case "Tegning":
                    return MapType.Tegning;
                case "Tematisk":
                    return MapType.Tematisk;
                case "Tematisk kort":
                    return MapType.Tematisk;
                case "Tematisk kort/":
                    return MapType.Tematisk;
                case "Tematisk kort/planche":
                    return MapType.Tematisk;
                case "Tematisk/Planche":
                    return MapType.Planche;
                case "Tematist":
                    return MapType.Tematisk;
                case "Topografisk":
                    return MapType.Topografisk;
                case "Topografisk kort":
                    return MapType.Topografisk;
                case "UTM":
                    return MapType.UTM;
                case "UTM-kort":
                    return MapType.UTM;
                default:
                    return MapType.Udefineret;
            }
        }

        private static Cover GetCover(string omfang)
        {
            switch (omfang)
            {
                case "O":
                    return Cover.O;
                case "K":
                    return Cover.K;
                case "L":
                    return Cover.L;
                case "B":
                    return Cover.B;
                default:
                    return Cover.Andet;
            }
        }

        private static Archive GetArchive(string arkivnummer, MuninDb db)
        {
            var archive = db.Archives.FirstOrDefault(x => x.ArchiveNb == arkivnummer);

            if (archive != null)
                return archive;
            archive = new Archive()
            {
                ArchiveNb = arkivnummer,
                ArchiveType = ArchiveType.Andet
            };
            Log(string.Format("Arkiv {0} var ikke oprettet i forvejen",arkivnummer));

            return db.Archives.Add(archive);
        }


        //http://stackoverflow.com/questions/708911/using-case-switch-and-gettype-to-determine-the-object
        private static Munin.DAL.Models.Journal GetJournal<T>(string journalnr, MuninDb db)
        {
            var journal = db.Journals.FirstOrDefault(x => x.JournalNb == journalnr);
            if (journal != null)
                return journal;

            journal = new Journal()
            {
                JournalNb = journalnr
            };

            if (typeof(T).GUID.ToString() == BookGUID)
                journal.Material = Material.Book;
            if (typeof(T).GUID.ToString() == ArchiveDocGUID)
                journal.Material = Material.Document;
            if (typeof(T).GUID.ToString() == pictureGUID)
                journal.Material = Material.Picture;
            if (typeof(T).GUID.ToString() == SeqGUID)
                journal.Material = Material.Sequence;

            journal.ReceiveDate = DateTime.Now;
            journal.Count = 1;
            journal.Regs = 1;

            return db.Journals.Add(journal);
        }


        private static void CreateArchives()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.ArkivFond.ToList())
                {
                    string message = "";

                    try
                    {
                        var archive = new Archive()
                        {
                            ArchiveNb = item.ArkivNr,
                        };
                        archive.ArchiveType = getArchiveType(item.ArkivType);
                        if (archive.ArchiveType == ArchiveType.Andet)
                            message += string.Format("Arkfond {0} har ikke valid arkivfondtype");

                        var dt = mergeDatetime(item.StiftetYear, item.StiftetMonth, item.StiftetDay);
                        if (dt != DateTime.MinValue)
                            archive.Established = dt;

                        dt = mergeDatetime(item.AfsluttetYear, item.AfsluttetMonth, item.AfsluttetDay);
                        if (dt != DateTime.MinValue)
                        archive.ClosedDate = dt;

                        archive.Comment = item.Note;
                        muninDb.Archives.Add(archive);
                        muninDb.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        message += "\r\n" + ex.Message;
                    }
                    Log(message);
                }
            }
        }

        private static DateTime mergeDatetime(double? stiftetYear, double? stiftetMonth, double? stiftetDay)
        {
            int y, m = 1, d = 1;

            if (stiftetYear == null)
                return DateTime.MinValue;
            y = (int)stiftetDay.Value;

            if (stiftetMonth > 0)
                m = (int)stiftetMonth.Value;
            if (stiftetDay > 0)
                d = (int)stiftetDay.Value;

            return new DateTime(y,m,d);
        }

        private static ArchiveType getArchiveType(string aType)
        {
            if (aType == "")
                return ArchiveType.Andet;

            switch (aType.ToUpper())
            {
                case "E":
                    return ArchiveType.E;
                case "F":
                    return ArchiveType.F;
                case "P":
                    return ArchiveType.P;
                case "I":
                    return ArchiveType.I;
                default:
                    return ArchiveType.Andet;
            }
        }

            private static bool CreateJournals()
        {
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var journal in db.Journaler.ToList())
                {
                    bool newJournal = false;
                    string message = "";

                    try
                    {
                        var muninJournal = muninDb.Journals.FirstOrDefault(x => x.JournalNb == journal.JournalID);
                        if (muninJournal == null)
                        {
                            newJournal = true;
                            muninJournal = new Munin.DAL.Models.Journal()
                            {
                                JournalNb = journal.JournalID
                            };
                        }
                        int regs = 1;
                        if (!Int32.TryParse(journal.Regs.ToString(), out regs))
                            message += string.Format("Journal {0} har ikke angivet Regs", journal.JournalID);

                        muninJournal.Regs = regs;

                        if (!Int32.TryParse(journal.Antal.ToString(), out regs))
                            message += string.Format("Journal {0} har ikke angivet Antal", journal.JournalID);

                        muninJournal.Count = regs;

                        DateTime dt;

                        if (!DateTime.TryParse(journal.Afleveret.ToString(), out dt))
                            message += string.Format("Journal {0} har ikke angivet aflevering", journal.JournalID);

                        muninJournal.ReceiveDate = dt;
                        muninJournal.Comment = journal.Note;
                        if (newJournal)
                            muninDb.Journals.Add(muninJournal);
                        else
                        {
                            muninDb.Entry(muninJournal).State = EntityState.Modified;
                        }
                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += "\r\n" + ex.Message;
                        Log(message);
                        return false;
                    }
                    Log(message);
                }
            }
            return true;
        }

        private static bool CreateProviders()
        {
            var muninDb = new MuninDb();

            using (var db = new ILABClassic())
            {
                foreach (var giver in db.Giver.ToList())
                {
                    string message = "";

                    try
                    {
                        var muninJournal = new Munin.DAL.Models.Journal()
                        {
                            JournalNb = giver.Journal
                        };

                        var s = muninDb.Journals.Add(muninJournal);
                        muninDb.SaveChanges();

                        var provider = new Provider()
                        {
                            Journal = muninJournal,
                            Address = giver.Adresse,
                            Att = giver.Att,
                            Name = giver.Navn,
                            City = giver.By
                        };

                        int zipCode;

                        if (!Int32.TryParse(giver.Postnr, out zipCode))
                            message += string.Format("Giver {0} Postnummer blev sat til 0. ", giver.GiverID);

                        provider.ZipCode = zipCode;

                        DateTime dt = DateTime.Today;

                        if (giver.AflevDato == null)
                            message += string.Format("Giver {0} har ikke afleveringsdato. ", giver.GiverID);
                        else
                        {
                            dt = giver.AflevDato.Value;
                        }

                        provider.Registrated = dt;

                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += string.Format("Giver {0} fejlede med: {1}", giver.GiverID, ex.Message);
                        return false;
                    }

                    Log(message);
                }
                
            }
            return true;
        }

        private static void Log(string message)
        {
            if (message == "")
                return;

            if (writer == null)
                writer = File.AppendText("Logfil.txt");

            writer.WriteLine(message);
        }

    }
}
