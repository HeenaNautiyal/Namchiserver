using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    using System;
    public class RoleAction
    {
        public int RoleId { get; set; }
        public int ActionId { get; set;}
        public string ActionDesc { get; set; }
        public string NextDesc { get; set; }
        public List<RoleAction> ShowRolltoAction { get; set; }
        public string RoleDesc { get; set; }
    }
}