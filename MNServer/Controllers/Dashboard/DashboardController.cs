﻿using MNServer.Models;
using MNServer.ProcessAction.Action;
using MNServer.ProcessAction.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Dapper;
using System.Web.Security;
using System.Web;
using System.Net;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace MNServer.Controllers.Dashboard
{
    public class DashboardController : Controller
    {

        #region class
        SqlConnection con = null;
        BirthProcessAction Bprocess = new BirthProcessAction();
        #endregion


        #region variable
        string RejectList = null;
        string RejectComments = null;
        string RejectActionSection = null;
        int BR_ID = 0;
        string UserName = null;
        int UserId = 0;
        string InsertResult = null;
        string ActionID_Name = null;
        public string ActionId = null;
        public string ActionName = null;
        public int CustomerID = 0;

      
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            #region variabels
            string strTmp = null;
            string getAction_ID = null;
            string getAction = null;
            string UserPending = null;
            List<int> roleidlist = new List<int>();
            List<RoleMap> RoleMapList = new List<RoleMap>();
            #endregion

            #region Session variable
            int CustId = Convert.ToInt32(Session["Cust_ID"]);
            int UserToRole = Convert.ToInt16(Session["SortedList1"]);
            int Userrole = Convert.ToInt16(Session["U_ID"]);
            #endregion

            #region Objects
            RoleAction RoletoAction = new RoleAction();
            BirthProcessAction objDB = new BirthProcessAction();
            BirthProcess Bprocess = new BirthProcess();
            #endregion

            RoletoAction.ShowRolltoAction = objDB.GetTbl_Action_MasterId(Userrole, 1);
            Session["RoleDesc"] = objDB.roleDesc;

            if (RoletoAction.ShowRolltoAction.Count==0)
            {
                UserPending = objDB.GetAllUsersPending(Userrole);
                return View("Dummy");
            }

            UserPending = objDB.GetAllUsersPending(Userrole);

            TempData["Viewstr"] = strTmp;

            #region Differentiate Each role to Action
            foreach (RoleAction ActionRole in RoletoAction.ShowRolltoAction)
            {
                if (UserToRole == ActionRole.RoleId)
                {
                    getAction_ID = ActionRole.ActionId.ToString();
                    getAction_ID = getAction_ID.Trim();
                    getAction = ActionRole.ActionDesc;
                }
            }
            #endregion
           
            Bprocess.ShowallBirthRegis = objDB.CountPending(getAction_ID,getAction, CustId);
            Session["ActionId"] = getAction_ID;
            Session["ActionName"] = getAction;
            Session["CustomerID"] = CustId;


            ActionID_Name = getAction + "|" + getAction_ID;
            if (Bprocess.ShowallBirthRegis==null)
            {
                return View("Dummy");
            }
          
            return View(Bprocess);
        }

        public ActionResult EditPending(string id,string BndType)
        {
            BirthProcessAction EmpRepo = new BirthProcessAction();
                       
            ModelState.Clear();

            return RedirectToAction("ShowData", new RouteValueDictionary
                (new { controller = "Dashboard", action = "ShowData", actionname = id ,BnDType = BndType }));
        }
               
        public ActionResult ShowData( string actionname,string BnDType)
            {
            string name = actionname;
            string type = BnDType;
            BirthProcess Bprofile = new BirthProcess();
            string[] line = actionname.Split('|');
            string sts = line[0].ToString();
            Bprofile.Status = sts;

            Bprofile.nextAction = actionname;

            BirthProcessAction objDB = new BirthProcessAction(); //calling class DBdata
            Bprofile.ShowallBirthRegis = objDB.GetAllEmployees(name, type);
            return View(Bprofile);
            }

        public ActionResult ClickAction( int Id,string BnDType)
        {
            string strmsg = null;
          return RedirectToAction("AddAction", new RouteValueDictionary(new { controller = "Dashboard", action = "AddAction", BR_ID = Id,Type=BnDType, Message= strmsg  }));
        }

        public ActionResult PaymentClickAction(int Id, string BnDType)
        {
            string strmsg = null;
            return RedirectToAction("PaymentAddAction", new RouteValueDictionary(new { controller = "Dashboard", action = "PaymentAddAction", BR_ID = Id, Type = BnDType, Message = strmsg }));
        }

        public ActionResult AddAction(int BR_ID,string Message,string Type)
        {
            ActionTaken At = new ActionTaken();
            string custid = Session["Cust_ID"].ToString();
            if (Message != null)
            {
                Session["BirthRegister_ID"] = BR_ID;
                BirthProcessAction Bprocess = new BirthProcessAction();
                At.Showall = Bprocess.GetDataofparticularuser(BR_ID);
                At.ShowAllComments = Bprocess.GetAllCommentsdata(BR_ID);
                At.ShowallMessage = Bprocess.FileLoadMessage(Message);
                At.Allrecomendedvarification = Bprocess.AddrecomendVerification();
                At.BnDType = Type;
                return View(At);
                
            }
            else
            {
                Session["BirthRegister_ID"] = BR_ID;
                BirthProcessAction Bprocess = new BirthProcessAction();
                At.Showall = Bprocess.GetDataofparticularuser(BR_ID);
                At.ShowAllComments = Bprocess.GetAllCommentsdata(BR_ID);
                At.Allrecomendedvarification = Bprocess.AddrecomendVerification();
                At.viewIdentityDocument = Bprocess.ShowIdentityDocumentList(custid);
                At.viewBnDDocument = Bprocess.ShowBnDDocumentList();
                At.RoleName = Session["RoleDesc"].ToString();
                return View(At);
            }

        }

        [HttpGet]
        public ActionResult PaymentAddAction(int BR_ID, string Message,string Type)
        {
            ActionTaken At = new ActionTaken();
            if (Message != null)
            {
                Session["BirthRegister_ID"] = BR_ID;
                BirthProcessAction Bprocess = new BirthProcessAction();
                At.Showall = Bprocess.GetDataofparticularuser(BR_ID);
                At.ShowAllComments = Bprocess.GetAllCommentsdata(BR_ID);
                At.ShowallMessage = Bprocess.FileLoadMessage(Message);
                return View(At);

            }
            else
            {
                Session["BirthRegister_ID"] = BR_ID;
                BirthProcessAction Bprocess = new BirthProcessAction();
                At.Showall = Bprocess.GetDataofparticularuser(BR_ID);
                At.ShowAllComments = Bprocess.GetAllCommentsdata(BR_ID);
                At.BnDType = Type;
                return View(At);
            }

        }

        public ActionResult AjaxPostCall(ActionTaken CommentData)
        {
            int BR_ID = Convert.ToInt16(Session["BirthRegister_ID"]);
            string UserName = Session["U_Name"].ToString();
            int UserId = Convert.ToInt16(Session["U_ID"]);
            string Action = CommentData.Action;
            string comments = CommentData.Comments;
            BirthProcessAction Bprocess = new BirthProcessAction();
          
            string Insertresult = Bprocess.AddDataComments(BR_ID,UserName,Action,comments, UserId);
           

          
           

            //TempData.Remove("Message");
            //TempData["Message"] = Utility.Util.AltMsg("Verification has been done");

            return RedirectToAction("Dashboard", "DashBoard");
           
            //return View();
            
        }

        public ActionResult AddPayment( string DType)
        {
            string BR_IDstr = DType + "_"+Session["BirthRegister_ID"].ToString();
            
            List<MailNotification> MailInfo = Bprocess.getMailInfo("Payment", BR_IDstr.ToString());
            for (int i = 0; i < MailInfo.Count; i++)
            {
                Utility.Util.INotificationService mailNotification = new Utility.Util.MailService
               ("heena.nautiyal@caritaseco.in", MailInfo[i].Subject, MailInfo[i].MailAppNo);
                mailNotification.Notify();
            }
            return View();
        }

        [HttpGet]
        public ActionResult DownLoadFile(int id)
        {
           // string FileMsg = null;
            ActionTaken At = new ActionTaken();
            List<BirthProcess> ObjFiles = GetFileList(id);

            var FileById = (from FC in ObjFiles
                          
            select new { FC.DocumentName, FC.FileName }).ToList().FirstOrDefault();

            byte[] data = Convert.FromBase64String(FileById.FileName);
            string imreBase64Data = Convert.ToBase64String(data);
            string imgDataURL = string.Format( imreBase64Data);
            //Passing image data in viewbag to view  
            ViewBag.ImageData = imgDataURL;
            return View("ViewDocument", imgDataURL,id);

            #region for save the file in path   
            // string strpath = "C:\\Users\\Caritas-Heena\\Desktop\\";
            //if (Directory.Exists(strpath))
            //    {
            //        strpath = strpath + FileById.DocumentName;
            //        using (FileStream Writer = new System.IO.FileStream(strpath, FileMode.Create, FileAccess.Write))
            //        {

            //            Writer.Write(data, 0, data.Length);
            //            FileMsg = "FileDownload";
            //        }
            //    }
            //    else
            //    {
            //        throw new System.Exception("PDF Shared Location not found");
            //    }
            //if (FileMsg.Contains("FileDownload"))
            //{
            //    At.Filemessage = "FileDownload";
            //    return RedirectToAction("AddAction",new { BR_ID=id ,Message=At.Filemessage.ToString()});
            //}
            //else
            //{
            //    At.Filemessage = "Unable to download";
            //    //TempData["shortMessage"] = String.Format("Unable to download");
            //    return RedirectToAction("AddAction", new { BR_ID = id });
            //}
            #endregion
        }

        //get View Document
        public ActionResult ViewDocument()
        {
            return View();
        }

        private List<BirthProcess> GetFileList( int id)
        {
            con = Utility.Util.Connection("DBEntities");
            List<BirthProcess> DetList = new List<BirthProcess>();

            con = Utility.Util.Connection("DBEntities");
            con.Open();
            var Selectionid = new DynamicParameters();
            Selectionid.Add("@BR_ID", id);

            DetList = SqlMapper.Query<BirthProcess>(con, "SP_GetDetailofSingleUser", Selectionid, commandType: CommandType.StoredProcedure).ToList();
            con.Close();
            return DetList;
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public JsonResult GetCountries()
        {
           // ActionTaken At = new ActionTaken();
            BirthProcessAction Bprocess = new BirthProcessAction();
            //At.Allrecomendedvarification = 
            List<Tbl_Clarification_Master> objCnt= Bprocess.AddrecomendVerification();
            return Json(objCnt);
        }

        [HttpPost]
        public JsonResult AjaxRecomendRejectCall(RecommendedRejects RR)
        {
            RejectList = RR.Action;
            RejectComments = RR.Comments;
            RejectActionSection = RR.ActionSection;

            BR_ID = Convert.ToInt16(Session["BirthRegister_ID"]);
            UserName = Session["U_Name"].ToString();
            UserId = Convert.ToInt16(Session["U_ID"]);
            Bprocess = new BirthProcessAction();
            InsertResult = Bprocess.AddRejectrecomendVerification(BR_ID,UserId, UserName, RejectActionSection, RejectList,RejectComments);
            if(InsertResult!= "Approve"|| InsertResult != "Clarification" || InsertResult != "RecommendRejection")
            {
               ViewBag.Message = Utility.Util.AltMsg("Verification has been done");
            }
            string BR_IDstr = "_" + Session["BirthRegister_ID"].ToString();

            if (RejectActionSection.Equals("Clarification"))
            {
                List<MailNotification> MailInfo = Bprocess.getMailInfo("Clarification", BR_IDstr.ToString());
                for (int i = 0; i < MailInfo.Count; i++)
                {
                    Utility.Util.INotificationService mailNotification = new Utility.Util.MailService
                   ("heena.nautiyal@caritaseco.in", MailInfo[i].Subject, MailInfo[i].MailAppNo);
                    mailNotification.Notify();
                }
            }
            else if (RejectActionSection.Contains("Rejection"))
            {
                List<MailNotification> MailInfo = Bprocess.getMailInfo("Rejection", BR_IDstr.ToString());
                for (int i = 0; i < MailInfo.Count; i++)
                {
                    Utility.Util.INotificationService mailNotification = new Utility.Util.MailService
                   ("heena.nautiyal@caritaseco.in", MailInfo[i].Subject, MailInfo[i].MailAppNo);
                    mailNotification.Notify();
                }
            }
            return Json(RejectList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Clarification( int id)
        {
            ClarificationModelList CmodelList = new ClarificationModelList();
            int CustId = Convert.ToInt32(Session["Cust_ID"]);
            BirthProcessAction Bprocess = new BirthProcessAction();
            string Clarifications= Bprocess.CheckClarification(id, CustId);
            string[] SplitStringLine = Clarifications.Split(',', '|');
            List<string> ClarificationList = new List<string>();
            foreach(string List in SplitStringLine)
            {
                ClarificationList.Add(List);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in ClarificationList)
            {
                sb.Append(item + "\n");
            }
            var str = new HtmlString(sb.ToString());
            CmodelList.Clarification = str.ToString();

            return View(CmodelList);
        }

        public ActionResult DownloadFile_I(string DocName,string Doctype)
        {
            string path = null;
            if (Doctype.Equals("Pan")|| Doctype.Equals("AdhaarCard") || Doctype.Equals("Passport") || Doctype.Equals("ElectoralCard")
                || Doctype.Equals("SSC/COI/CitizenshipCertificate"))
            {
                 path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles\\Identity_Document\\";
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + DocName);
            string fileName = DocName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult DownloadFile_BnD(string DocName, string Doctype)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles\\Identity_Document\\";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + DocName);
            string fileName = DocName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}