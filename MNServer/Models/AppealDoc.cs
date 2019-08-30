using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class AppealDoc
    {
        public string AppealComment { get; set; }
        public string AppealFileName { get; set; }
        public string AppealFilePath { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public int ID { get; set; }
        public string BR_ID { get; set; }
    }
}