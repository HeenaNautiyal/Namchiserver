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
    
    public partial class Tbl_Login
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MailId { get; set; }
        public Nullable<long> MobileNumber { get; set; }
        public string Created_by { get; set; }
        public string Modified_by { get; set; }
    }
}
