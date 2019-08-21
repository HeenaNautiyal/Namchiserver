using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using MNServer.Controllers.Login;
using MNServer.Models;
using MNServer.ProcessAction.Action;
using MNServer.ProcessAction.Model;
using System.Web.Mvc.Html;
using System.Web;
using System.IO;

namespace MNServer.Controllers.BirthRegistration
{
    public class BirthRegistController : Controller
    {
        BirthProcessAction obj= new BirthProcessAction();
        public System.Web.UI.ClientScriptManager ClientScript { get; }

        #region View BirthRegistration
        [HttpGet]
        public ActionResult BirthRegistration(string FormType)
        {
                      

            string[] IE= Request.Params.AllKeys;
            BirthProcess BprocessView = new BirthProcess();
            BprocessView.Date_of_Birth = System.DateTime.Today;//For Current Date
            BprocessView.Date_of_Death = System.DateTime.Today;
            int CustID = Convert.ToInt32 (Session["Cust_ID"].ToString());
            BprocessView.CustomerName = obj.CustomerDetails(CustID);
            BprocessView.CustomerAddress = obj.CustomerAddress(CustID);
            BprocessView.CustomerGender = obj.CustomerGender(CustID);
            // BprocessView.DocTypeList = objdocument.DocumentList();
            if (FormType == "BR")
            {
                BprocessView.DocList = obj.DocumentList(1); //1 is process id
            }
            else
                BprocessView.DocList = obj.DocumentList(2); //2 is process id

            BprocessView.FormType = FormType;
           
            return View(BprocessView);
        }
        #endregion

        #region Save BirthRegistrationData
        [HttpPost]
        public ActionResult BirthRegistration(BirthProcess BRobject, HttpPostedFileBase httpPostedFileBase)
        {

                int id = BRobject.ID;
                if (string.IsNullOrEmpty(BRobject.DocumentType))
                    {
                        ModelState.AddModelError("DocumentType", "Please Select Document Type");
                    }

                obj = new BirthProcessAction();
                int Custid = Convert.ToInt32(Session["Cust_ID"]);

                if (BRobject.CustomerGender == "Female")
                    {
                        BRobject.Name_of_Mother = BRobject.CustomerName;
                    }
                else
                    {
                        BRobject.Name_of_Father = BRobject.CustomerName;
                    }
                    string result = obj.create(BRobject, 1, Custid, BRobject.FormType);
                    string[] line = result.Split('|');
                    string Rt = line[0].ToString();
                    string Appno = line[1].ToString();
                    string FormType_1 = BRobject.FormType;
                    Session["AppNo"] = FormType_1 + "_" + Appno;

                    Clear();
                    int Roleid = Convert.ToInt16(Session["SortedList1"]);
                    int CustID = Convert.ToInt32(Session["Cust_ID"].ToString());

            if (Rt == "Registered Successfully")
            {
                string fileSavePath = HttpContext.Server.MapPath("~/UploadedFiles/");
                string rt=Utility.Util.AddDocuments(Request, BRobject.DocList, fileSavePath,
                    FormType_1 + "_"+ Appno, CustID);

                string AppNo = FormType_1 + "_" + Appno;
                List<MailNotification> MailInfo = obj.getMailInfo("BirthApplied", AppNo);
                for(int i = 0; i < MailInfo.Count; i++)
                {
                    Utility.Util.INotificationService mailNotification = new Utility.Util.MailService
                   ("heena.nautiyal@caritaseco.in", MailInfo[i].Subject, MailInfo[i].MailAppNo);
                    mailNotification.Notify();
                }
               

             

                Message();
              

            }
                       
            TempData["Viewstr"] = Session["ViewData"].ToString();
            TempData["ErrorMessage"] = "Your application has been register succesfully." +
            "\n Your application id no:" + Appno;
            return RedirectToAction("Index", "Login");
        }

        private ContentResult Message()
        {
        return Content("<script language='javascript' type='text/javascript'>alert     ('Requested Successfully ');</script>");
         }

        #endregion

        #region Clear window
        private void Clear()
        {
            ModelState.Remove("Name_of_Child");
            ModelState.Remove("Name_of_Father");
            ModelState.Remove("Name_of_Mother");
            ModelState.Remove("Place_of_Birth");
            ModelState.Remove("Gram_Panchayat_Unit");
            ModelState.Remove("files");
            ModelState.Remove("Nationality");              
        }
        #endregion

    }

}