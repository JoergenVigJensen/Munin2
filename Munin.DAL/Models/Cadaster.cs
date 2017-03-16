using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    /// <summary>
    /// Matrikler
    /// </summary>
    public class Cadaster
    {
        public int CadasterId { get; set; }

        //Matrikelnummer
        public string CadasterNb { get; set; }

        public string Source { get; set; }

        public int CreatedYear { get; set; }

        public int Year { get; set; }

        public string StamNr { get; set; }

        public string User { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string Area { get; set; }

        public int ZipNumber { get; set; }

        public string City { get; set; }

        public string Comment { get; set; }
    }
}
