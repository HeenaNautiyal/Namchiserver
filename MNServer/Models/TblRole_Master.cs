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
    
    public partial class TblRole_Master
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public Nullable<bool> IsSystemUser { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string Created_By { get; set; }
        public Nullable<System.DateTime> Created_Time { get; set; }
        public string Deactivate_By { get; set; }
        public Nullable<System.DateTime> Deactivated_Time { get; set; }
    }
}
