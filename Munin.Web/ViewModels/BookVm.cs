using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Munin.DAL.Models;

namespace Munin.Web.ViewModels
{
    public class BookVm
    {
        public Book Model { get; set; }

        public ICollection<UISelectItem> JournalList { get; set; }

    }
}