using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    public class Sequence
    {
        public int SequenceId { get; set; }
        public string SequenceNb { get; set; }
        public Journal Journal { get; set; }
        public DateTime DateTime { get; set; }
        public string Source { get; set; }
        public string Interviewer { get; set; }
        public bool Copyright { get; set; }
        public SequenceType SequenceType { get; set; }
        public string Comment { get; set; }
    }
}
