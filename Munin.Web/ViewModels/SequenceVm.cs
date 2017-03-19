using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munin.DAL.Models;

namespace Munin.Web.ViewModels
{
    public class SequenceVm
    {
        public Sequence Model { get; set; }

        public ICollection<UISelectItem> JournalList { get; set; }

        public ICollection<UISelectItem> SequenceTypes { get; set; }
    }
}