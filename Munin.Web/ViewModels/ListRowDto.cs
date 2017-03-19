using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munin.Web.ViewModels
{
    public class ListRowDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
        public long Ticks { get; set; }
        public string Index { get; set; }
        public int ErrorCode { get; set; }
    }
}