using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class DocumentModel
    {
        public int DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int Mandatory { get; set; }
        public List<DocumentModel> DocumentList { get; set; }
    }
}