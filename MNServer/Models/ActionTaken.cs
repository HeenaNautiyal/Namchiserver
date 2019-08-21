using MNServer.ProcessAction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MNServer.Models
{
    public class ActionTaken
    {
        public List<BirthProcess> Showall { get; set; }
        public List<Tbl_Birth_Process_Action> ShowAllComments { get; set; }
        public string Comments { get; set; }
        public string Action { get; set; }
        public string Filemessage { get; set; }
        public List<string> ShowallMessage { get; set; }
        public List<Tbl_Clarification_Master> Allrecomendedvarification { get; set; }
        public string SelectedAnswer { get; set; }
        public string RoleName { get; set; }
        public string BnDType { get; set; }
        public List<Tbl_Document_Identity_List> viewIdentityDocument { get; set; }
        public List<Tbl_Document_BnD_List> viewBnDDocument { get; set; }

    }

}