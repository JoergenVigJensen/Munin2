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
    
    public partial class UserTbl
    {
        public int UserID { get; set; }
        public string Initials { get; set; }
        public string Username { get; set; }
        public string UserPwd { get; set; }
        public Nullable<double> Accesslevel { get; set; }
    }
}
