using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "ChirchRecords")]
    public class ChirchRecord
    {
        [Column(Name = "ChirchRecordId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ChirchRecordId { get; set; }

        [Column(Name = "FirstName", DbType = "VARCHAR")]
        public string FirstName { get; set; }

        [Column(Name = "LastName", DbType = "VARCHAR")]
        public string LastName { get; set; }

        [Column(Name = "Event", DbType = "INTEGER")]
        public ChirchEvent Event { get; set; }

        [Column(Name = "Date", DbType = "DATETIME")]
        public DateTime Date { get; set; }

        [Column(Name = "PersonId", DbType = "INTEGER")]
        public int PersonId { get; set; }

    }
}
