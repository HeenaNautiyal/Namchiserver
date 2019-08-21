using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class RoleDesc
    {
        public int id { get; set; }

        public List<RoleDesc> Showall { get; set; }
    }
}