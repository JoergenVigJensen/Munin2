//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ILABDb.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Journaler
    {
        public int JournalKey { get; set; }
        public string JournalID { get; set; }
        public Nullable<System.DateTime> Afleveret { get; set; }
        public string MedieArt { get; set; }
        public Nullable<double> Antal { get; set; }
        public Nullable<double> Regs { get; set; }
        public string Note { get; set; }
    }
}
