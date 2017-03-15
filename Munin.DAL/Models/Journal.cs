using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Munin.DAL;

namespace Munin.DAL.Models
{
    public class Journal
    {
        public int JournalId { get; set; }

        [Display(Name = "Journalnummer: ")]
        public string JournalNb { get; set; }

        [Display(Name = "AfleveringsDato: ")]
        public DateTime ReceiveDate { get; set; }

        [Display(Name = "Materiale: ")]
        public Material Material { get; set; }

        public int Count { get; set; }

        public int Regs { get; set; }
        
    }
}
