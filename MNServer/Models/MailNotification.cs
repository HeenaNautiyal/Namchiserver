using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class MailNotification
    {
        public string ActionName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string MailAppNo { get; set; }
    }
}