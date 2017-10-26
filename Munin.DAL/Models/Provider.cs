using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "Providers")]
    public class Provider
    {
        [Column(Name = "ProviderId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int ProviderId { get; set; }
        public virtual Journal Journal { get; set; }

        [Column(Name = "Name", DbType = "VARCHAR")]
        public string Name { get; set; }

        [Column(Name = "Att", DbType = "VARCHAR")]
        public string Att { get; set; }

        [Column(Name = "Address", DbType = "VARCHAR")]
        public string Address { get; set; }

        [Column(Name = "ZipCode", DbType = "INTEGER")]
        public int ZipCode { get; set; }

        [Column(Name = "City", DbType = "VARCHAR")]
        public string City { get; set; }

        [Column(Name = "ProviderId", DbType = "DATETIME")]
        public DateTime Registrated { get; set; }

        [Column(Name = "ProviderId", DbType = "TEXT")]
        public string Comment { get; set; }

    }
}
