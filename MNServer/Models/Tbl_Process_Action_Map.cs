//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MNServer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Process_Action_Map
    {
        public int ID { get; set; }
        public Nullable<int> Action_ID { get; set; }
        public Nullable<int> Process_ID { get; set; }
        public Nullable<System.DateTime> Action_date { get; set; }
        public string Acttion_by { get; set; }
        public string Login_ID { get; set; }
        public string Action_desc { get; set; }
        public Nullable<int> Seq_ID { get; set; }
    }
}
