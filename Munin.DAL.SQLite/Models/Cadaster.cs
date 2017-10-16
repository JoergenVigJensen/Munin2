using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.SQLite.Models
{
    /// <summary>
    /// Matrikler
    /// </summary>
    public class Cadaster
    {
        [Column(Name = "CadasterId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int CadasterId { get; set; }

        //Matrikelnummer
        [Column(Name = "CadasterNb", DbType = "VARCHAR")]
        public string CadasterNb { get; set; }

        [Column(Name = "Source", DbType = "VARCHAR")]
        public string Source { get; set; }

        [Column(Name = "CreatedYear", DbType = "INTEGER")]
        public int CreatedYear { get; set; }

        [Column(Name = "Year", DbType = "INTEGER")]
        public int Year { get; set; }

        [Column(Name = "StamNr", DbType = "VARCHAR")]
        public string StamNr { get; set; }

        [Column(Name = "User", DbType = "VARCHAR")]
        public string User { get; set; }

        [Column(Name = "Street", DbType = "VARCHAR")]
        public string Street { get; set; }

        [Column(Name = "Number", DbType = "VARCHAR")]
        public string Number { get; set; }

        [Column(Name = "Area", DbType = "VARCHAR")]
        public string Area { get; set; }

        [Column(Name = "ZipNumber", DbType = "INTEGER")]
        public int ZipNumber { get; set; }

        [Column(Name = "City", DbType = "VARCHAR")]
        public string City { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }
    }
}
