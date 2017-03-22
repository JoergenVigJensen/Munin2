using System;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Munin.DAL;
using Munin.DAL.Models;
using ILABDb.DAL;

namespace DBMigration
{
    class Program
    {


        private static StreamWriter writer;

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

        private static DateTime SQLSrvMinDate = new DateTime(1799,1,1);

        static void Main(string[] args)
        {
            bool running = false;

            Archive ar = new Archive();

            Archive ar2 = new Archive();

            ArchiveDocument adoc = new ArchiveDocument();

            var t = ar.GetType();

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

            Console.WriteLine("Starter kørsel");

            Console.WriteLine("Starter Journaler");
            CreateJournals();

            Console.WriteLine("Starter Giver");
            CreateProviders();

            Console.WriteLine("Starter Arkivfonde");
            CreateArchives();

            Console.WriteLine("Starter Arkivalier ");
            CreateArhiveDocs();

            Console.WriteLine("Starter Kort");
            CreateArhiveMaps();

            Console.WriteLine("Starter Bøger");
            CreateArhiveBooks();

            Console.WriteLine("Starter Matrikler");
            CreateCadaster();

            Console.WriteLine("Starter Udklip");
            CreateClips();

            Console.WriteLine("Starter Billeder");
            CreatePicture();

            Console.WriteLine("Starter Sekvenser");
            CreateSequence();

            Console.ReadLine();
        }

        private static void CreateSequence()
        {
            var muninDb = new MuninDb();
            int i = 1;

            using (var db = new ILABClassic())
            {
                foreach (var item in db.Sekvenser.ToList())
                {
                    string message = "";

                    Console.Write("\r sekvens {0}   ", i);
                    i++;


                    try
                    {
                        var journal = GetJournal<Sequence>(item.Journal, muninDb);
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
                        {
                            message += string.Format("Sekvens {0} har ikke angivet dato", item.SekvensNr);
                            dt = SQLSrvMinDate;
                        }

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

            if (string.IsNullOrEmpty(sekvenstype))
                return SequenceType.Andet;

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
            int i = 1;
            using (var db = new ILABClassic())
            {
                foreach (var item in db.Billeder.ToList())
                {
                    string message = "";
                    Console.Write("\r billede {0}   ", i);
                    i++;
                    try
                    {
                        var journal = GetJournal<Picture>(item.Journal, muninDb);
                        var pic = new Picture()
                        {
                            PictureIndex = item.Billedindex,
                            CDNb =   (string.IsNullOrEmpty(item.CDnr))? "" : item.CDnr,
                            Copyright =  item.Ophavsret,
                            Journal = journal,
                            Photograph = (string.IsNullOrEmpty(item.Fotograf)) ? "" : item.Fotograf,
                            OrderNum = item.Numordning,
                            Provision = item.Klausul,
                            Size = item.Format,
                            Tag = (string.IsNullOrEmpty(item.Ordning)) ? "" : item.Ordning,
                            Placed = item.Placering,                                                                                    
                            Comment = item.Note                            
                        };
                        pic.PictureMaterial = (PictureMaterial) ((item.Materiale == null) ? 6 : (int) item.Materiale.Value);

                        DateTime dt = SQLSrvMinDate;
                        if (item.Datering != null)
                            dt = item.Datering.Value;
                        else
                        {
                            message += string.Format("\n\r Kort {0} har ikke korrekt datering", item.Billedindex);
                        }

                        pic.DateTime = dt;


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
            int i = 1;
            var muninDb = new MuninDb();
            using (var db = new ILABClassic())
            {
                foreach (var item in db.udklip.ToList())
                {
                    string message = "";

                    Console.Write("\r udklip {0}   ", i);
                    i++;

                    try
                    {
                        var clip = new Clip()
                        {
                            Attache = item.Mappe,
                            Paper = GetPaperEnum(item.Aviskode),
                            Comment = item.Note,
                            DateTime = new DateTime(1799,1,1),
                            Title = item.Overskrift
                        };

                        //datetime sat til default min date iflg. Datetime format i database

                        DateTime dt;

                        if (!DateTime.TryParse(item.Datering, out dt))
                            message += string.Format("\n\r Udklip {0} har ikke angivet aflevering", item.UdklipsID);

                        if (dt > SQLSrvMinDate)
                            clip.DateTime = dt;
                        else
                        {
                            message += string.Format("\n\r Udklip {0} er ikke korrekt dateret", item.UdklipsID);
                        }

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
                            StamNr = item.Stamnr,
                            User = item.Bruger,
                            Comment = item.Note                            
                        };

                        if (string.IsNullOrEmpty(item.Vejnavn))
                            message += string.Format("\n\r Matrikel {0} har ikke angivet vejnavn", item.MatrikelNr);
                        else
                        {
                            cadaster.Street = item.Vejnavn;
                        }

                        if (string.IsNullOrEmpty(item.Vejnr))
                            message += string.Format("\n\r Matrikel {0} har ikke angivet vejnummer", item.MatrikelNr);
                        else
                        {
                            cadaster.Street = item.Vejnr;
                        }

                        if (string.IsNullOrEmpty(item.By))
                            message += string.Format("\n\r Matrikel {0} har ikke angivet by", item.MatrikelNr);
                        else
                        {
                            cadaster.City = item.By;
                        }




                        if (string.IsNullOrEmpty(item.Postnr))
                            message += string.Format("\n\r Matrikel {0} har ikke korrekt postnr", item.MatrikelNr);
                        else
                        {
                            int z;
                            if (Int32.TryParse(item.Postnr, out z))
                                cadaster.ZipNumber = z;
                            else
                            {
                                message += string.Format("\n\r Matrikel {0} har ikke korrekt postnr", item.MatrikelNr);
                            }
                        }

                        if (string.IsNullOrEmpty(item.Oprettet) || item.Oprettet.Length < 4)
                            message += string.Format("String {0} har ikke angivet oprettelsesår", item.MatrikelNr);
                        else
                        {
                            cadaster.CreatedYear = Int32.Parse(item.Oprettet.Substring(0,4));
                        }

                        if (string.IsNullOrEmpty(item.Dato) || item.Dato.Length < 4)
                            message += string.Format("\n\r Matrikel {0} har ikke korrekt år", item.MatrikelNr);
                        else
                        {
                            int d;
                            if (Int32.TryParse(item.Dato.Substring(0, 4), out d))
                                cadaster.Year = Int32.Parse(item.Dato.Substring(0, 4));
                            else
                            {
                                message += string.Format("\n\r Matrikel {0} har ikke korrekt år", item.MatrikelNr);
                            }
                        }

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

                        string bogkode = item.Bogkode;

                        if (string.IsNullOrEmpty(bogkode))
                            continue;
                        
                        if (book.PublishYear == 0)
                            message += string.Format("Bog: {0} mangler udgivelsesår", bogkode);

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
                            Material = GetMaterial(item.Materiale),
                            MapType = GetMapType(item.KortType),
                            Comment = item.Note,
                            Format = item.Format,
                            Area = item.Omraade,
                            Publisher = item.Udgiver,
                            PublishYear = (item.DateringAar == null)?0 : item.DateringAar.Value,
                            RevisionYear = (item.RevAar == null) ? 0 : item.RevAar.Value,
                            Depot = item.Depot,
                            Proportion = item.Propertion

                        };
                        if (map.MapType == MapType.Udefineret)
                            message += string.Format("Kort {0} har ikke korrekt format angivet", item.Kortnr);

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
                        message += string.Format("\n\r Kort {0} fejler med {1}", item.ArkivalieID, ex.Message);
                    }
                    Log(message);
                }
            }

        }


        private static MapMaterial GetMaterial(string material)
        {

                if (material.ToLower().Contains("fotok. på plast"))
                    return MapMaterial.FotokopiPlast;

                if (material.ToLower().Contains("kopi"))
                {
                    if (material.ToLower().Contains("foto") || material.ToLower().Contains("papir"))
                        return MapMaterial.Fotokopi;

                    if (material.ToLower().Contains("lys"))
                        return MapMaterial.Lyskopi;
                }

                if (material.ToLower().Contains("ohp"))
                {
                    if (material.ToLower().Contains("transparent"))
                        return MapMaterial.OHPTransparent;
                    if (material.ToLower().Contains("a4"))
                        return MapMaterial.OHPA4;
                    if (material.ToLower().Contains("folie"))
                        return MapMaterial.OHPFolie;
                }

                if (material.ToLower().Contains("a4"))
                    return MapMaterial.A4;


                if (material.ToLower().Contains("negativ"))
                {
                    if (material.ToLower().Contains("96"))
                        return MapMaterial.NegativReol96;

                    if (material.ToLower().Contains("97"))
                        return MapMaterial.NegativReol97;
                }

                if (material.ToLower().Contains("karton"))
                {
                    if (material.ToLower().Contains("82"))
                        return MapMaterial.KartonBag82A;
                    if (material.ToLower().Contains("96"))
                        return MapMaterial.KartonReol96a;
                    if (material.ToLower().Contains("97"))
                        return MapMaterial.KartonReol97;
                    if (material.ToLower().Contains("oplæbet"))
                        return MapMaterial.KartonOpklaebet;
                    if (material.ToLower().Contains("kortskab"))
                        return MapMaterial.KartonKortskab;

                    return MapMaterial.Karton;
                }

                if (material.ToLower().Contains("hylde"))
                    return MapMaterial.HyldeLoft85;

                if (material.ToLower().Contains("96"))
                    return MapMaterial.KartonReol96a;

                if (material.ToLower().Contains("97"))
                    return MapMaterial.KartonReol97;

                if (material.ToLower().Contains("luftfoto bag 82"))
                    return MapMaterial.LuftfotoBag82;

                if (material.ToLower().Contains("82"))
                    return MapMaterial.KartonBag82A;

                if (material.ToLower().Contains("litografi"))
                    return MapMaterial.FotoLitografi;

                if (material.ToLower().Contains("lærred"))
                    return MapMaterial.Kortlærred;

                if (material.ToLower().Contains("tryk"))
                {
                    if (material.ToLower().Contains("lys"))
                        return MapMaterial.Lystryk;

                    if (material.ToLower().Contains("farve"))
                        return MapMaterial.Farvetryk;
                }

                if (material.ToLower().Contains("gram"))
                    return MapMaterial.Fotogrametrisk;

                if (material.ToLower().Contains("prod"))
                    return MapMaterial.Farvereprodukt;

                if (material.ToLower().Contains("klæb"))
                    return MapMaterial.Opklæbet;

                if (material.ToLower().Contains("trans"))
                    return MapMaterial.Transparent;

                if (material.ToLower().Contains("99"))
                {
                    if (material.ToLower().Contains("skab"))
                        return MapMaterial.OriginalSkab99;

                    return MapMaterial.OriginalSe99;
                }

                if (material.ToLower().Contains("planche"))
                {
                    if (material.ToLower().Contains("samle"))
                        return MapMaterial.SamletPlanche;

                    return MapMaterial.planche;
                }

            if (material.ToLower().Contains("papir"))
                return MapMaterial.Papir;


            if (material.ToLower().Contains("plast"))
                    return MapMaterial.Plast;

                return MapMaterial.Andet;
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

            int year = DateTime.Today.Year;

            if (string.IsNullOrEmpty(journalnr))
            {
                journal =
                    db.Journals.Where(x => x.JournalNb.Contains(year.ToString()))
                        .OrderByDescending(x => x.JournalNb)
                        .First();

                int count = 1;
                if (journal != null)
                {
                    Int32.TryParse(journal.JournalNb.Split('/')[1], out count);
                    if (count < 1)
                        count = 1;
                }
                journalnr = string.Format("{0}/{1}", year.ToString(), count.ToString().PadLeft(3, '0'));

            }

            if (string.IsNullOrEmpty(journalnr))
                

            if (journalnr != null)
                year = Int32.Parse(journalnr.Split('/')[0]);


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

            journal.ReceiveDate = new DateTime(year, 1, 1);
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
                            message += string.Format("Arkfond {0} har ikke valid arkivfondtype", item.ArkivNr);

                        var dt = mergeDatetime(item.StiftetYear, item.StiftetMonth, item.StiftetDay);
                        if (dt != DateTime.MinValue)
                            archive.Established = dt;

                        dt = mergeDatetime(item.AfsluttetYear, item.AfsluttetMonth, item.AfsluttetDay);
                        archive.Comment = item.Note;
                        if (dt > DateTime.MinValue)
                            archive.ClosedDate = dt;

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
            y = (int)stiftetYear.Value;

            if (stiftetMonth > 0)
                m = (int)stiftetMonth.Value;

            if (stiftetDay > 0)
                d = (int)stiftetDay.Value;

            return new DateTime(y,m,d);
        }

        private static ArchiveType getArchiveType(string aType)
        {
            if (string.IsNullOrEmpty(aType))
                return ArchiveType.Andet;

            switch (aType.ToUpper())
            {
                case "A":
                    return ArchiveType.A;
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

        private static void CreateJournals()
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

                        int year = Int32.Parse(journal.JournalID.Split('/')[0]);

                        if (dt == DateTime.MinValue)
                            dt = new DateTime(year,1,1);

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
                    }
                    Log(message);
                }
            }
        }

        private static void CreateProviders()
        {
            var muninDb = new MuninDb();

            using (var db = new ILABClassic())
            {
                foreach (var giver in db.Giver.ToList())
                {
                    string message = "";

                    try
                    {
                        var journal = GetJournal<Book>(giver.Journal,muninDb);

                        var provider = new Provider()
                        {
                            Journal = journal,
                            Address = giver.Adresse,
                            Att = giver.Att,
                            Name = giver.Navn,
                            City = giver.By
                        };

                        int zipCode;

                        if (!Int32.TryParse(giver.Postnr, out zipCode))
                            message += string.Format("Giver {0} Postnummer blev sat til 0. ", giver.GiverID);

                        provider.ZipCode = zipCode;

                        DateTime dt = journal.ReceiveDate;

                        if (giver.AflevDato == null)
                            message += string.Format("Giver {0} har ikke afleveringsdato. ", giver.GiverID);
                        else
                        {
                            dt = giver.AflevDato.Value;
                        }

                        provider.Registrated = dt;

                        muninDb.Providers.Add(provider);

                        muninDb.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        message += string.Format("Giver {0} fejlede med: {1}", giver.GiverID, ex.Message);
                    }

                    Log(message);
                }
                
            }
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
