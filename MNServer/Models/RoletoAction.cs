using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class RoletoAction
    {
        public int NextAction { get; set; }
        public int ID { get; set; }
        public string ActionDesc { get; set; }
        public string NextDesc { get; set; }
      
    }
}