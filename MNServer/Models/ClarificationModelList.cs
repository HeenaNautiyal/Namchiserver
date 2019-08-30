using MNServer.ProcessAction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class ClarificationModelList
    {
        public ActionTaken Attaken { get; set; }
        public string Clarification { get; set; }
        public List<string> CFlist { get; set; }
        public List<BirthProcess> RegistrationForm { get; set; }
        public List<DocumentsAttachment> DocTypeList { get; set; }
        public List<Tbl_Document_List> DocList { get; set; }
        public AppealDoc AppealDoc { get; set; }
    }
}