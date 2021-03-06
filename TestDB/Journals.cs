//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Journals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Journals()
        {
            this.Books = new HashSet<Books>();
            this.Maps = new HashSet<Maps>();
            this.Pictures = new HashSet<Pictures>();
            this.Providers = new HashSet<Providers>();
            this.Sequences = new HashSet<Sequences>();
        }
    
        public int JournalId { get; set; }
        public string JournalNb { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public int Material { get; set; }
        public int Count { get; set; }
        public int Regs { get; set; }
        public string Comment { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Books> Books { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maps> Maps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pictures> Pictures { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Providers> Providers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sequences> Sequences { get; set; }
    }
}
