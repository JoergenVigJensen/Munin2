using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    /// <summary>
    /// Arkivalie
    /// </summary>
    /// 
    [Serializable]
    [Table(Name = "ArchiveDocuments")]
    public class ArchiveDocument
    {
        [Column(Name = "ArchiveDocumentId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ArchiveDocumentId { get; set; }
        public virtual Journal Journal { get; set; }
        public virtual Archive Archive { get; set; }

        [Column(Name = "BoxNumber", DbType = "INTEGER")]
        public int BoxNumber { get; set; }

        [Column(Name = "Signature", DbType = "INTEGER")]
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
