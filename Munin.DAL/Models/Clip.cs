using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    /// <summary>
    /// Udklip
    /// </summary>
    /// 
    [Serializable]
    [Table(Name = "Clips")]
    public class Clip
    {
        [Key]
        [Column(Name = "ClipId",IsDbGenerated =true, IsPrimaryKey = true, DbType = "INTEGER")]
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
