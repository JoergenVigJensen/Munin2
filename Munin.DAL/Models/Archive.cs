using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    public class Archive
    {
        public int ArchiveId { get; set; }
        public string ArchiveNb { get; set; }

        public string Title { get; set; }

        public string Definition { get; set; }
        public ArchiveType ArchiveType { get; set; }

        public Nullable<DateTime> Established { get; set; }
        public Nullable<DateTime> ClosedDate { get; set; }
       
        public string Comment { get; set; }

    }
}
