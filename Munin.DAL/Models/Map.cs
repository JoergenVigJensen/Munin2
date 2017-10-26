using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "Maps")]
    public class Map
    {
        [Column(Name = "MapId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int MapId { get; set; }

        [Required]
        [Column(Name = "MapNb", DbType = "VARCHAR")]
        public string MapNb { get; set; }

        [Required]
        public virtual Journal Journal { get; set; }

        [Required]
        [Column(Name = "Title", DbType = "VARCHAR")]
        public string Title { get; set; }

        [Column(Name = "MapType", DbType = "INTEGER")]
        public MapType MapType { get; set; }

        [Column(Name = "Proportion", DbType = "VARCHAR")]
        public string Proportion { get; set; }

        [Column(Name = "Publisher", DbType = "VARCHAR")]
        public string Publisher { get; set; }

        [Column(Name = "PublishYear", DbType = "INTEGER")]
        public int PublishYear { get; set; }

        [Column(Name = "RevisionYear", DbType = "INTEGER")]
        public int RevisionYear { get; set; }

        [Column(Name = "Area", DbType = "VARCHAR")]
        public string Area { get; set; }

        [Column(Name = "Format", DbType = "VARCHAR")]
        public string Format { get; set; }

        [Column(Name = "Material", DbType = "INTEGER")]
        public MapMaterial Material { get; set; }

        [Column(Name = "Depot", DbType = "VARCHAR")]
        public string Depot { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }

    }
}


