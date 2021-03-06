﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    [Serializable]
    [Table(Name = "Sequences")]
    public class Sequence
    {
        [Column(Name = "SequenceId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int SequenceId { get; set; }

        [Required]
        [Column(Name = "SequenceNb", DbType = "VARCHAR")]
        public string SequenceNb { get; set; }

        public virtual Journal Journal { get; set; }

        [Column(Name = "DateTime", DbType = "DATETIME")]
        public DateTime DateTime { get; set; }

        [Column(Name = "Source", DbType = "VARCHAR")]
        public string Source { get; set; }

        [Column(Name = "Interviewer", DbType = "VARCHAR")]
        public string Interviewer { get; set; }

        [Column(Name = "Copyright", DbType = "BIT")]
        public bool Copyright { get; set; }

        [Column(Name = "SequenceType", DbType = "INTEGER")]
        public SequenceType SequenceType { get; set; }

        [Column(Name = "Comment", DbType = "INTEGER")]
        public string Comment { get; set; }

    }
}
