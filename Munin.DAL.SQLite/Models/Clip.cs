using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.SQLite.Models
{
    /// <summary>
    /// Udklip
    /// </summary>
    public class Clip
    {
        [Column(Name = "ClipId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ClipId { get; set; }

        [Column(Name = "DateTime", DbType = "DATETIME")]
        public DateTime DateTime { get; set; }

        [Column(Name = "Attache", DbType = "VARCHAR")]
        public string Attache { get; set; }

        [Column(Name = "Paper", DbType = "INTEGER")]
        public PaperEnum Paper { get; set; }

        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }

    }
}
