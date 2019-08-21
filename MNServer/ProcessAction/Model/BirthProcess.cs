using System;
using System.Collections.Generic;
using MNServer.ProcessAction.Action;
using MNServer.ProcessAction.Model;
using System.Linq;
using System.Web;
using MNServer.Models;

namespace MNServer.ProcessAction.Model
{
    public class BirthProcess : IProcess
    {
        

        public DateTime creationTime { get; set; }

        public string deactivateBy { get; set; }

        public DateTime deactivatonTime { get; set; }
        public string initiatedBy { get; set; }

        public DateTime initiationTime { get; set; }

        public bool isActive { get; set; }

        public string process_desc { get; set; }

        public string process_name { get; set; }

        public int processID { get; set; }
        
        public string nextAction { get; set; }

        //public List<int> actionId { get; set; }



        #region other details related to BnD
        public int ID { get; set; }
        public string FormType { get; set; }
        public string Name_of_Informant { get; set; }
        public string Name_of_Child { get; set; }
        public String Gender_Of_Child { get; set; }
        public string Name_of_Father { get; set; }
        public string Name_of_Mother { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public string Place_of_Birth { get; set; }
        public string Address { get; set; }
        public string Gram_Panchayat_Unit { get; set; }
        public string Nationality { get; set; }
        public string City_name { get; set; }
        public String DocumentType { get; set; }
       // public HttpPostedFileBase files { get; set; }
        public string PassportNo { get; set; }
        public string Status { get; set; }
        public int count { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string DocumentName { get; set; }
        public List<string> Pending { get; set; }
        public List<BirthProcess> ShowallBirthRegis { get; set; }
        public List<CustomerMaster_TBl> ShowCustomerdetail { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGender { get; set; }

        public List<DocumentsAttachment> DocTypeList { get; set; }
        public List<Tbl_Document_List> DocList { get; set; }

        public string BnDType { get; set; }
        public string NameOfBnDperson { get; set; }
        public DateTime Date_of_Death { get; set; }
        public string PlaceofDeath { get; set; }
        public string AgeofDeceased { get; set; }
       



    }
    public class FileDetailsModel
    {
        public int ID { get; set; }
        public string Name_of_Informant { get; set; }
        public String FileName { get; set; }
        public byte[] FileContent { get; set; }
        public byte[] DocumentName { get; set; }


    }

    #endregion other details related to BnD


    public class DocumentsAttachment
    {
        public string DocumentTypeName { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
        public string ProcessId { get; set; }
        public List<string> DoctTypeList { get; set; }
        public string[] DocType { get; set; }
    }
}
