using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.SQLite.Models
{
    /// <summary>
    /// Arkivalie
    /// </summary>
    public class ArchiveDocument
    {
        [Column(Name = "ArchiveDocumentId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ArchiveDocumentId { get; set; }

        [Column(Name = "Journal_JournalId", DbType = "INTEGER")]
        public int Journal_JournalId { get; set; }
                
        public virtual Journal Journal { get; set; }

        [Column(Name = "AriArchive_AriArchiveId", DbType = "INTEGER")]
        public int AriArchive_AriArchiveId { get; set; }
                
        public virtual Archive AriArchive { get; set; }

        [Column(Name = "BoxNumber", DbType = "INTEGER")]
        public int BoxNumber { get; set; }

        [Column(Name = "Signature", DbType = "VARCHAR")]
        public string Signature { get; set; }

        [Column(Name = "Copyright", DbType = "BIT")]
        public bool Copyright { get; set; }

        [Column(Name = "YearStart", DbType = "INTEGER")]
        public int YearStart { get; set; }

        [Column(Name = "YearEnd", DbType = "INTEGER")]
        public int YearEnd { get; set; }

        [Column(Name = "Cover", DbType = "INTEGER")]
        public Cover Cover { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }
    }
}
