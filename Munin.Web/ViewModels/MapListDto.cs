using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Munin.Web.ViewModels
{
    public class MapListDto
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Title { get; set; }
        public string Area { get; set; }
        public int PublishYear  { get; set; }
        public int ErrorCode { get; set; }
    }
}