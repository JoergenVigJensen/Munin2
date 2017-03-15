using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    /// <summary>
    /// billeder
    /// </summary>

    public class Picture
    {
        public int PictureId { get; set; }

        public Journal Journal { get; set; }

        //BilledIndex
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

        public PictureMaterial PictureMaterial { get; set; }

        //Ophavsret
        public bool Copyright { get; set; }

        //Klausul
        public bool Provision { get; set; }

        public DateTime DateTime { get; set; }
        public string Note { get; set; }
    }
}
