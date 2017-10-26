using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace Munin.DAL.Models
{
    /// <summary>
    /// billeder
    /// </summary>
    [Serializable]
    [Table(Name = "Pictures")]
    public class Picture
    {
        [Column(Name = "PictureId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int PictureId { get; set; }

        [Required]
        public virtual Journal Journal { get; set; }

        //BilledIndex
        [Column(Name = "PictureIndex", DbType = "VARCHAR")]
        [Required(ErrorMessage = "Der skal angives billedindex")]
        [RegularExpression(@"^(?i)B.\d{4}$", ErrorMessage = "Billedindex skal opfylde formatet B.1234")]
        public string PictureIndex { get; set; }

        //Numordning
        [Column(Name = "OrderNum", DbType = "VARCHAR")]
        public string OrderNum { get; set; }

        //Ordning
        [Column(Name = "Tag", DbType = "VARCHAR")]
        public string Tag { get; set; }

        [Column(Name = "CDNb", DbType = "VARCHAR")]
        public string CDNb { get; set; }

        //Fotograf
        [Column(Name = "Photograph", DbType = "VARCHAR")]
        public string Photograph { get; set; }

        //Format
        [Column(Name = "Size", DbType = "VARCHAR")]
        public string Size { get; set; }

        [Column(Name = "PictureMaterial", DbType = "INTEGER")]
        [Required(ErrorMessage = "Der skal vælges materiale til billede.")]
        public PictureMaterial PictureMaterial { get; set; }

        //Ophavsret
        [Column(Name = "Copyright", DbType = "BIT")]
        public bool Copyright { get; set; }

        //Klausul
        [Column(Name = "Provision", DbType = "BIT")]
        public bool Provision { get; set; }

        //Placering
        [Column(Name = "Placed", DbType = "VARCHAR")]
        public string Placed { get; set; }

        [Column(Name = "DateTime", DbType = "DATETIME")]
        public DateTime DateTime { get; set; }

        [Column(Name = "Comment", DbType = "TEXT")]
        public string Comment { get; set; }
    }
}
