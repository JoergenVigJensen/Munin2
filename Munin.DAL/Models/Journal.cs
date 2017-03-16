using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace Munin.DAL.Models
{
    public class Journal
    {
        public int JournalId { get; set; }

        [Display(Name = "Journalnummer: ")]
        public string JournalNb { get; set; }

        [Display(Name = "AfleveringsDato: ")]
        public DateTime ReceiveDate { get; set; }

        [Display(Name = "Materiale: ")]
        public Material Material { get; set; }

        public int Count { get; set; }

        public int Regs { get; set; }

        public string Comment { get; set; }

        //public virtual Provider Provider { get; set; }

        public ICollection<Picture> PirPictures = new HashSet<Picture>();

        public ICollection<Book> Books = new HashSet<Book>();

        public ICollection<Map> Maps = new HashSet<Map>();

        public ICollection<Sequence> Sequences = new HashSet<Sequence>();

        public ICollection<ArchiveDocument> ArchiveDocuments = new HashSet<ArchiveDocument>();

    }
}
