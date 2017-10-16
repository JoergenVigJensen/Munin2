using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.SQLite.Models
{
    public class Book
    {
        [Column(Name = "BookId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        [Required]
        public int BookId { get; set; }

        [Column(Name = "BookCode", DbType = "VARCHAR")]
        [Required(ErrorMessage = "Der skal angives billedindex")]
        [RegularExpression(@"^(?i)BB.\d{4}$", ErrorMessage = "Bogkode skal opfylde formatet B.1234")]
        public string BookCode { get; set; }

        [Required]
        [Column(Name = "Journal_JournalId", DbType = "VARCHAR")]
        public int Journal_JournalId { get; set; }

        public virtual Journal Journal { get; set; }

        [Column(Name = "DK5", DbType = "VARCHAR")]
        public string DK5 { get; set; }

        //Ordningsord
        [Column(Name = "Tag", DbType = "VARCHAR")]
        public string Tag { get; set; }

        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "SubTitle", DbType = "VARCHAR")]
        public string SubTitle { get; set; }

        [Column(Name = "Author", DbType = "VARCHAR")]
        public string Author { get; set; }

        //Redaktør
        [Column(Name = "Editor", DbType = "VARCHAR")]
        public string Editor { get; set; }

        [Column(Name = "PublishYear", DbType = "INTEGER")]
        public int PublishYear { get; set; }

        [Column(Name = "Attache", DbType = "VARCHAR")]
        public string Attache { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }

    }
}
