using System;
using System.ComponentModel.DataAnnotations;

namespace Munin.DAL.Models
{
    /// <summary>
    /// billeder
    /// </summary>

    public class Picture
    {
        public int PictureId { get; set; }
        [Required]
        public Journal Journal { get; set; }

        //BilledIndex
        [Required(ErrorMessage = "Der skal angives billedindex")]
        [RegularExpression(@"^(?i)B.\d{4}$", ErrorMessage = "Billedindex skal opfylde formatet B.1234")]
        public string PictureIndex { get; set; }

        //Numordning
        public string OrderNum { get; set; }

        //Ordning
        public string Tag { get; set; }

        public string CDNb { get; set; }

        //Fotograf
        public string Photograph { get; set; }

        //Format
        public string Size { get; set; }

        [Required(ErrorMessage = "Der skal vælges materiale til billede.")]
        public PictureMaterial PictureMaterial { get; set; }

        //Ophavsret
        public bool Copyright { get; set; }

        //Klausul
        public bool Provision { get; set; }

        //Placering
        public string Placed { get; set; }

        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
    }
}
