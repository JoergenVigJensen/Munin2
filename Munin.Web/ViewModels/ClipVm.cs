using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munin.DAL.Models;

namespace Munin.Web.ViewModels
{
    public class ClipVm
    {
        public Clip Model { get; set; }

        public ICollection<UISelectItem> Papers { get; set; } = new HashSet<UISelectItem>();
    }
}