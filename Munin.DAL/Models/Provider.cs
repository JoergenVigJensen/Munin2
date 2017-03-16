using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Munin.DAL.Models
{
    public class Provider
    {
        public int ProviderId { get; set; }

        public virtual ICollection<Journal> Journals { get; set; } = new HashSet<Journal>();

        public string Name { get; set; }

        public string Art { get; set; }

        public string Address { get; set; }

        public int ZipCode { get; set; }

        public string City { get; set; }

        public DateTime Registrated { get; set; }

        public string Comment { get; set; }

    }
}
