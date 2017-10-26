using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "Archives")]
    public class Archive
    {
        [Column(Name = "ArchiveId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ArchiveId { get; set; }

        [Column(Name = "ArchiveNb", DbType = "VARCHAR")]
        public string ArchiveNb { get; set; }

        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "Definition", DbType = "VARCHAR")]
        public string Definition { get; set; }

        [Column(Name = "ArchiveType", DbType = "INTEGER")]
        public ArchiveType ArchiveType { get; set; }

        [Column(Name = "Established", DbType = "DATETIME")]
        public Nullable<DateTime> Established { get; set; }

        [Column(Name = "ClosedDate", DbType = "DATETIME")]
        public Nullable<DateTime> ClosedDate { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }

    }
}
