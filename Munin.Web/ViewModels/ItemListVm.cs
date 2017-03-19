using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munin.Web.ViewModels
{
    public class ItemListVm<T>
    {
        public int Count { get; set; }

        public int Pages { get; set; }

        public ICollection<T> Data { get; set; } = new HashSet<T>();
    }
}