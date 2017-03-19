using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munin.Web.ViewModels
{
    public class ItemListVm
    {
        public int Count { get; set; }

        public int Pages { get; set; }

        public ICollection<ListRowDto> Data { get; set; } = new HashSet<ListRowDto>();
    }
}