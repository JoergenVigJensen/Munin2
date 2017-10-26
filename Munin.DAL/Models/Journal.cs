using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "Journals")]
    public class Journal
    {
        [Column(Name = "JournalId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int JournalId { get; set; }

        [Column(Name = "JournalNb", DbType = "VARCHAR")]
        [Display(Name = "Journalnummer: ")]
        public string JournalNb { get; set; }

        [Column(Name = "ReceiveDate", DbType = "DATETIME")]
        [Display(Name = "AfleveringsDato: ")]
        public DateTime ReceiveDate { get; set; }

        [Column(Name = "Material", DbType = "INTEGER")]
        [Display(Name = "Materiale: ")]
        public Material Material { get; set; }

        [Column(Name = "Count", DbType = "INTEGER")]
        public int Count { get; set; }

        [Column(Name = "Regs", DbType = "INTEGER")]
        public int Regs { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }

        //public virtual Provider Provider { get; set; }

        public ICollection<Picture> PirPictures = new HashSet<Picture>();

        public ICollection<Book> Books = new HashSet<Book>();

        public ICollection<Map> Maps = new HashSet<Map>();

        public ICollection<Sequence> Sequences = new HashSet<Sequence>();

        public ICollection<ArchiveDocument> ArchiveDocuments = new HashSet<ArchiveDocument>();

    }
}
