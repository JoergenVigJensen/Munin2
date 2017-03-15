using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    /// <summary>
    /// Udklip
    /// </summary>
    public class Clip
    {
        public int ClipId { get; set; }
        public DateTime DateTime { get; set; }
        public string Attache { get; set; }
        public PaperEnum Paper { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

    }
}
