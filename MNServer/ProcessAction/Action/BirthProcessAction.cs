using System;
using System.Collections.Generic;
using MNServer.ProcessAction.Model;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using MNServer.Utility;
using MNServer.Models;

namespace MNServer.ProcessAction.Action
{
    public class BirthProcessAction:IProcessAction
    {
        SqlConnection con = null;
        BirthProcess bRobject = null;
        int authvalue = 0;
        public string roleDesc { set; get; }
       
        public string create(IProcess bRobject1,int P_Id,int custid,string BnDType)
        {
            bRobject = (BirthProcess)bRobject1;
            //bRobject.processID = P_Id;
            ProcessActionseq(P_Id);


            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string result2 = "";
            
            string pageName = "Birth Registration";
            try
            { 
                #region Birthregistration
                SqlCommand cmd = new SqlCommand("Tbl_BirthRegistration_SP", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;

                
                    bRobject.Status = "BirthApplied";
                    cmd.Parameters.AddWithValue("@Name_of_Child", checkAndGetValue( bRobject.Name_of_Child));

                    cmd.Parameters.AddWithValue("@Gender_Of_Child", checkAndGetValue(bRobject.Gender_Of_Child));
                    cmd.Parameters.AddWithValue("@Name_of_Mother", checkAndGetValue( bRobject.Name_of_Mother));

                    cmd.Parameters.AddWithValue("@Date_of_Birth", checkAndGetDate(bRobject.Date_of_Birth));
                    cmd.Parameters.AddWithValue("@Place_of_Birth", checkAndGetValue( bRobject.Place_of_Birth));
                    cmd.Parameters.AddWithValue("@Gram_Panchayat_Unit", checkAndGetValue( bRobject.Gram_Panchayat_Unit));
                    cmd.Parameters.AddWithValue("@Nationality", checkAndGetValue( bRobject.Nationality));
                 


                    cmd.Parameters.AddWithValue("@NameofBnDPerson", checkAndGetValue(bRobject.NameOfBnDperson));
                   
                        cmd.Parameters.AddWithValue("@Date_of_Death", checkAndGetDate(bRobject.Date_of_Birth));
                   
                   
                    cmd.Parameters.AddWithValue("@PlaceofDeath", checkAndGetValue(bRobject.PlaceofDeath));
                    cmd.Parameters.AddWithValue("@AgeofDeceased", checkAndGetValue(bRobject.AgeofDeceased));


               
                cmd.Parameters.AddWithValue("@Name_of_Father", checkAndGetValue( bRobject.Name_of_Father));
                cmd.Parameters.AddWithValue("@Name_of_Informant", checkAndGetValue(bRobject.CustomerName));
                
                cmd.Parameters.AddWithValue("@Address", checkAndGetValue( bRobject.CustomerAddress));
               
                
                cmd.Parameters.AddWithValue("@FileName","");
                cmd.Parameters.AddWithValue("@DocumentName","ABC");
                cmd.Parameters.AddWithValue("@DocumentType", checkAndGetValue(bRobject.DocumentType));
                cmd.Parameters.AddWithValue("@PassportNo", "");

                cmd.Parameters.AddWithValue("@FileContent", "");
                cmd.Parameters.AddWithValue("@UserID", "");
                cmd.Parameters.AddWithValue("@ModifyDate", "");
                cmd.Parameters.AddWithValue("@Status", checkAndGetValue( bRobject.Status));
                cmd.Parameters.AddWithValue("@NextAction", bRobject.nextAction);

                cmd.Parameters.AddWithValue("@IsActive", 1);
                cmd.Parameters.AddWithValue("@Custid", custid);
                cmd.Parameters.AddWithValue("@BnDType", BnDType);

                cmd.Parameters.AddWithValue("@Msg", "");

                result = cmd.ExecuteScalar().ToString();
                string[] lines = result.Split('|');
                authvalue = lines[0].Contains("Registered Successfully") ? 1 : 0;

                int BirthID = Convert.ToInt16(lines[1]);

               #endregion

                #region AuditTable
                result2 = AuditData.AduitFile.FillAudit(pageName, bRobject.CustomerName, authvalue);

                #endregion

                return result;
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();

                return result;
            }
            finally
            {
                con.Close();
            }
        }

        public List<MailNotification> getMailInfo(string AcName,string Appno)
        {
            List<MailNotification> Mailinf = new List<MailNotification>();
            try
            {
                con = Utility.Util.Connection("DBEntities");
                string result = "";
                SqlCommand cmd = new SqlCommand("SP_Mail_Content", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionN", AcName);
            
                result = cmd.ExecuteNonQuery().ToString();

                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    MailNotification MailItems = new MailNotification();
                    MailItems.Subject = ds.Tables[0].Rows[i]["Subject"].ToString();
                    MailItems.Message = ds.Tables[0].Rows[i]["Message"].ToString();
                    MailItems.MailAppNo = MailItems.Message.Replace("<AppNo>",Appno);
                    Mailinf.Add(MailItems);
                }

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }


            return Mailinf;
        }

        public List<Tbl_Document_List> DocumentList(int P_Id)
        {
            List<Tbl_Document_List> Tbl_DocumentTypeList = new List<Tbl_Document_List>();
            ActionTaken RoleAccess = new ActionTaken();
            try
            {
                con = Utility.Util.Connection("DBEntities");
                string result = "";
                SqlCommand cmd = new SqlCommand("SP_DocumentType", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_Id", P_Id);
                cmd.Parameters.AddWithValue("@Msg", "");
                result = cmd.ExecuteScalar().ToString();
                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Tbl_Document_List DocAttachment = new Tbl_Document_List();
                    DocAttachment.ID =( i + 1);

                    DocAttachment.Docname= ds.Tables[0].Rows[i]["DocumentTypeName"].ToString();
                    DocAttachment.Doctype= ds.Tables[0].Rows[i]["DocumentType"].ToString();
                    DocAttachment.DoctypeArr = DocAttachment.Doctype.Split('|');

                    Tbl_DocumentTypeList.Add(DocAttachment);
                }

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }


            return Tbl_DocumentTypeList;
        }

        public List<Tbl_Document_Identity_List> ShowIdentityDocumentList(string custid)
        {
            List<Tbl_Document_Identity_List> Tbl_DocumentViewList = new List<Tbl_Document_Identity_List>();
            ActionTaken RoleAccess = new ActionTaken();
            try
            {
                con = Utility.Util.Connection("DBEntities");
                string result = "";
                SqlCommand cmd = new SqlCommand("SP_ViewIdentityDocuments", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cust_id", custid);
                cmd.Parameters.AddWithValue("@Msg", "");
                result = cmd.ExecuteScalar().ToString();
                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Tbl_Document_Identity_List DocViewAttachment = new Tbl_Document_Identity_List();
                    DocViewAttachment.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);

                    DocViewAttachment.Docname = ds.Tables[0].Rows[i]["Docname"].ToString();
                    DocViewAttachment.Doctype = ds.Tables[0].Rows[i]["Doctype"].ToString();
                    DocViewAttachment.DocFilePath = ds.Tables[0].Rows[i]["DocFilePath"].ToString();
                    DocViewAttachment.Custid = Convert.ToInt32(ds.Tables[0].Rows[i]["Custid"]);

                    Tbl_DocumentViewList.Add(DocViewAttachment);
                }

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }


            return Tbl_DocumentViewList;
        }

        public List<Tbl_Document_BnD_List> ShowBnDDocumentList()
        {
            List<Tbl_Document_BnD_List> Tbl_DocumentViewList = new List<Tbl_Document_BnD_List>();
            ActionTaken RoleAccess = new ActionTaken();
            try
            {
                con = Utility.Util.Connection("DBEntities");
                string result = "";
                SqlCommand cmd = new SqlCommand("SP_ViewBndDocuments", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
               
                result = cmd.ExecuteScalar().ToString();
                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Tbl_Document_BnD_List DocViewBnDAttachment = new Tbl_Document_BnD_List();
                    DocViewBnDAttachment.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]);

                    DocViewBnDAttachment.Docname = ds.Tables[0].Rows[i]["Docname"].ToString();
                    DocViewBnDAttachment.Doctype = ds.Tables[0].Rows[i]["Doctype"].ToString();
                    DocViewBnDAttachment.DocFilePath = ds.Tables[0].Rows[i]["DocFilePath"].ToString();
                 

                    Tbl_DocumentViewList.Add(DocViewBnDAttachment);
                }

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }


            return Tbl_DocumentViewList;
        }

        public string InsertDocumentAttachment(string bR_ID, string fileName, string docFilePath,string Doctype, bool? isActive, int custid)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string Name = "";
            List<CustomerMaster_TBl> CMList = new List<CustomerMaster_TBl>();
            try
            {
                //#region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_InsertDocumentList_1", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BR_ID", bR_ID);
                cmd.Parameters.AddWithValue("@Docname", fileName);
                cmd.Parameters.AddWithValue("@Doctype", Doctype);
                cmd.Parameters.AddWithValue("@DocFilePath", docFilePath);
                cmd.Parameters.AddWithValue("@IsActive", isActive);
                cmd.Parameters.AddWithValue("@Custid", custid);
                cmd.Parameters.AddWithValue("@Msg", "");
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();}
            finally
            {
                con.Close();
            }
            return result;
        }

        public string CustomerDetails(int cust_ID)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string Name = "";
            List<CustomerMaster_TBl> CMList= new List<CustomerMaster_TBl>();
            try
            {
                //#region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_Customer_Details", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", cust_ID);
                result = cmd.ExecuteScalar().ToString();

                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BirthProcess CustomerTbl = new BirthProcess();
                    CustomerTbl.CustomerName = ds.Tables[0].Rows[i]["Name"].ToString();
                    CustomerTbl.CustomerAddress = ds.Tables[0].Rows[i]["Address"].ToString();
                     Name = ds.Tables[0].Rows[i]["Name"].ToString();
                }

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();

                return Name;
            }
            finally
            {
                con.Close();
            }
            return Name;
        }

        public string CustomerAddress(int cust_ID)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string Address = "";
            List<CustomerMaster_TBl> CMList = new List<CustomerMaster_TBl>();
            try
            {
                //#region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_Customer_Details", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", cust_ID);
                result = cmd.ExecuteScalar().ToString();

                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BirthProcess CustomerTbl = new BirthProcess();
                    CustomerTbl.CustomerAddress = ds.Tables[0].Rows[i]["Address"].ToString();
                    Address = ds.Tables[0].Rows[i]["Address"].ToString();
                }

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();

                return Address;
            }
            finally
            {
                con.Close();
            }
            return Address;
        }

        public string CustomerGender(int cust_ID)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string Address = "";
            List<CustomerMaster_TBl> CMList = new List<CustomerMaster_TBl>();
            try
            {
                //#region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_Customer_Details", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", cust_ID);
                result = cmd.ExecuteScalar().ToString();

                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BirthProcess CustomerTbl = new BirthProcess();
                    CustomerTbl.CustomerAddress = ds.Tables[0].Rows[i]["Gender"].ToString();
                    Address = ds.Tables[0].Rows[i]["Gender"].ToString();
                }

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();

                return Address;
            }
            finally
            {
                con.Close();
            }
            return Address;
        }

        public string GetAllUsersPending(int userrole)
        {
            string InsertedResult = null;
            con = Utility.Util.Connection("DBEntities");
            SqlCommand cmd = new SqlCommand("SP_Pending_verification_User", con);
            con.Open();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", userrole);
                cmd.Parameters.AddWithValue("@Msg", "");
                InsertedResult = cmd.ExecuteScalar().ToString();

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return InsertedResult;
        }

        public List<RoleAction> GetTbl_Action_MasterId(int R_ID, int PId)
        {
            List<RoleAction> Tbl_Action_MasterList = new List<RoleAction>();
            ActionTaken RoleAccess = new ActionTaken();
            try
            {
                con = Utility.Util.Connection("DBEntities");
                string result = "";
                SqlCommand cmd = new SqlCommand("SP_ChkRoleAction", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", R_ID);
                cmd.Parameters.AddWithValue("@ProcessId", PId);
                result = cmd.ExecuteScalar().ToString();
                if (result.Equals(null))
                {
                    return null;
                }
                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    RoleAction RoleAction = new RoleAction();
                    RoleAction.ActionId = Convert.ToInt32(ds.Tables[0].Rows[i]["ActionId"].ToString());
                    RoleAction.RoleId = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"].ToString());
                    RoleAction.ActionDesc = ds.Tables[0].Rows[i]["ActionDesc"].ToString();
                    roleDesc= ds.Tables[0].Rows[i]["RoleName"].ToString();
                    Tbl_Action_MasterList.Add(RoleAction);
                }

            }

            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }
        
            
            return Tbl_Action_MasterList;
        }

        public string CheckClarification(int id, int custId)
        {
           string result = null;
                try
                {
                    con = Utility.Util.Connection("DBEntities");
                    SqlCommand cmd = new SqlCommand("SP_User_Rejection/Clarification", con);
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@B_ID", id);
                    cmd.Parameters.AddWithValue("@UID", custId);
                    result = cmd.ExecuteScalar().ToString();
            }
                catch (Exception ex)
                { ex.Message.ToString(); }

                finally
                { con.Close(); }
            
           return result;
        }

        public List<string> FileLoadMessage(string message)
        {
            List<string> Msg = new List<string>();
            Msg.Add(message);
            return Msg;
        }

        public string AddDataComments(int bR_ID, string userName, string action, string comments,int U_ID)
        {
            string InsertedResult = null;
            con = Utility.Util.Connection("DBEntities");
            List<Tbl_Birth_Process_Action> CommentList = new List<Tbl_Birth_Process_Action>();
            SqlCommand cmd = new SqlCommand("SP_InsertNextActionData_1", con);
            //SqlCommand cmd = new SqlCommand("SP_InsertNextActionData_v1", con);
            con.Open();
            if (comments == null)
            {
                comments = "";
            }
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BR_ID", bR_ID);
                cmd.Parameters.AddWithValue("@Comment", comments);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@ActionSection", "");
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@UID", U_ID);
                cmd.Parameters.AddWithValue("@Msg", "");
                InsertedResult = cmd.ExecuteScalar().ToString();
             
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return InsertedResult;
        }

        public string delete()
        {
            throw new NotImplementedException();
        }

        public string edit()
        {
            throw new NotImplementedException();
        }

        public SortedDictionary<int, string> ProcessActionseq(int pId)
        {
            string NextAction = null;
            string FirstAction = "BirthApplied";
            NextAction = Util.GetActionSequence(pId, FirstAction);

            bRobject.nextAction = NextAction.Trim();
          
            return null;
        }

        public List<IProcess> view()
        {
            throw new NotImplementedException();
        }

        public List<BirthProcess> CountPending()
        {
            DataSet ds = null;
            List<BirthProcess> Birth_Process_list = null;
            try
            {
                con = Utility.Util.Connection("DBEntities");
                SqlCommand cmd = new SqlCommand("[SP_PendingAction]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                ds = new DataSet();
                da.Fill(ds);
                Birth_Process_list = new List<BirthProcess>();

                #region For Display role in Table
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BirthProcess Birth_P = new BirthProcess();
                    Birth_P.count = Convert.ToInt32(ds.Tables[0].Rows[i]["No_Of_Action"].ToString());
                    Birth_P.nextAction = ds.Tables[0].Rows[i]["NextAction"].ToString().Trim();
                    Birth_Process_list.Add(Birth_P);
                }
                #endregion

            }
            catch (Exception ex)
            { ex.Message.ToString(); }

            finally
            { con.Close(); }

            return Birth_Process_list;
        }

        public List<BirthProcess> CountPending(string getAction_ID,string getAction,int custId)
        {
            DataSet ds = null;
            string result = null;
            List<BirthProcess> Birth_Process_list = null;

            if (getAction_ID == null || getAction == null)
            {
                BirthProcess Birth_P = new BirthProcess();
                Birth_P.count = 0;
                Birth_P.nextAction = "Page Is Unavailable";
                Birth_Process_list.Add(Birth_P);
            }
            else
            {
                try
                {
                    con = Utility.Util.Connection("DBEntities");
                    //SqlCommand cmd = new SqlCommand("SP_PendingAction_Selection", con);
                    SqlCommand cmd = new SqlCommand("SP_look", con);
                    con.Open();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionID", getAction_ID);
                    cmd.Parameters.AddWithValue("@Cust_Id", custId);
                    result = Convert.ToString(cmd.ExecuteScalar());

                    #region List Of roles
                    //if (result == "")
                    //{
                    //    cmd = new SqlCommand("SP_UserPending", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@CustId", custId);
                    //    result = Convert.ToString(cmd.ExecuteScalar());


                    //    if (result=="")
                    //    {
                    //        BirthProcess Birth_P = new BirthProcess();
                    //        Birth_P.count = 0;
                    //        Birth_P.nextAction = "Not Assign";
                    //        Birth_Process_list.Add(Birth_P);
                    //    }

                    //    else { 
                    //    SqlDataAdapter da = new SqlDataAdapter();
                    //    da.SelectCommand = cmd;
                    //    ds = new DataSet();
                    //    da.Fill(ds);
                    //    Birth_Process_list = new List<BirthProcess>();


                    //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //            {
                    //                BirthProcess Birth_P = new BirthProcess();
                    //                Birth_P.count = Convert.ToInt32(ds.Tables[0].Rows[i]["No_Of_Action"].ToString());
                    //                Birth_P.nextAction = ds.Tables[0].Rows[i]["NextAction"].ToString().Trim();
                    //                Birth_Process_list.Add(Birth_P);
                    //            }
                    //        }
                    //}
                    //else
                    //{
                    #endregion

                    SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        ds = new DataSet();
                        da.Fill(ds);
                        Birth_Process_list = new List<BirthProcess>();

                        #region For Display role in Table
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            BirthProcess Birth_P = new BirthProcess();
                            Birth_P.count = Convert.ToInt32(ds.Tables[0].Rows[i]["No_Of_Action"].ToString());
                            Birth_P.nextAction = ds.Tables[0].Rows[i]["NextAction"].ToString().Trim();
                            Birth_P.BnDType= ds.Tables[0].Rows[i]["BnDType"].ToString().Trim();
                        Birth_Process_list.Add(Birth_P);
                        }
                    #endregion
                    //}
                    if (Birth_Process_list.Count == 0)
                    {
                        BirthProcess Birth_P = new BirthProcess();
                        Birth_P.count = 0;
                        Birth_P.nextAction ="No_Pending_action";
                        Birth_P.BnDType ="";
                        Birth_Process_list.Add(Birth_P);
                    }

                }
                catch (Exception ex)
                { ex.Message.ToString(); }

                finally
                { con.Close(); }
            }
            return Birth_Process_list;
        }

        public List<Tbl_Clarification_Master> AddrecomendVerification()
        {
            DataSet ds = new DataSet();
            List<Tbl_Clarification_Master> RecomVerifyList = new List<Tbl_Clarification_Master>();
            try { 
            con = Utility.Util.Connection("DBEntities");
            SqlCommand cmd = new SqlCommand("SP_Clarification_Master", con);

            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            ds = new DataSet();
            da.Fill(ds);
                RecomVerifyList = new List<Tbl_Clarification_Master>();

                #region For Display role in Table
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Tbl_Clarification_Master CM = new Tbl_Clarification_Master();
                    CM.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                    CM.ClarificationOption = ds.Tables[0].Rows[i]["ClarificationOption"].ToString().Trim();
                    RecomVerifyList.Add(CM);
                }
                #endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return RecomVerifyList;
        }

        public List<BirthProcess> GetAllEmployees(string actionid,string type)
        {
            con = Utility.Util.Connection("DBEntities");
            List<BirthProcess> BirthList = new List<BirthProcess>();
            SqlCommand cmd = new SqlCommand("Tbl_GetBirthRegistration_SP", con);
            con.Open();

            try {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nextAction", actionid);
                cmd.Parameters.AddWithValue("@BnDType", type);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                BirthList = (from DataRow dr in dt.Rows

                             select new BirthProcess()
                             {
                                 ID=Convert.ToInt16(dr["ID"]),
                                 Name_of_Informant = Convert.ToString(dr["Name_of_Informant"]),
                                 Name_of_Child = Convert.ToString(dr["Name_of_Child"]),
                                 Gender_Of_Child = Convert.ToString(dr["Gender_Of_Child"]),
                                 Date_of_Birth = Convert.ToDateTime(dr["Date_of_Birth"]),
                                 BnDType= Convert.ToString(dr["BnDType"]),
                                 //Date_of_Birth  = DateTime.ParseExact(dr["Date_of_Birth"].ToString()),

                             }).ToList();
               
            }
          catch(Exception ex)
            {
                ex.Message.ToString();
            }
            finally{
                con.Close();
            }
            return BirthList;

        }

        public List<BirthProcess> GetDataofparticularuser(int BR_Id)
        {
            con = Utility.Util.Connection("DBEntities");
            List<BirthProcess> BirthList = new List<BirthProcess>();
            SqlCommand cmd = new SqlCommand("SP_GetDetailofSingleUser", con);
            con.Open();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BR_ID", BR_Id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                BirthList = (from DataRow dr in dt.Rows

                             select new BirthProcess()
                             {
                                 ID = Convert.ToInt16(dr["ID"]),
                                 Name_of_Informant = Convert.ToString(dr["Name_of_Informant"]),
                                 Name_of_Child = Convert.ToString(dr["Name_of_Child"]),
                                 NameOfBnDperson = Convert.ToString(dr["NameOfBnDperson"]),
                                 Name_of_Father = Convert.ToString(dr["Name_of_Father"]),
                                 Name_of_Mother = Convert.ToString(dr["Name_of_Mother"]),
                                 Address = Convert.ToString(dr["Address"]),
                                 Gender_Of_Child = Convert.ToString(dr["Gender_Of_Child"]),
                                 Date_of_Birth = Convert.ToDateTime(dr["Date_of_Birth"]),
                                 Date_of_Death = Convert.ToDateTime(dr["Date_of_Death"]),
                                 Place_of_Birth = Convert.ToString(dr["Place_of_Birth"]),
                                 PlaceofDeath = Convert.ToString(dr["PlaceofDeath"]),
                                 AgeofDeceased = Convert.ToString(dr["AgeofDeceased"]),
                                 Gram_Panchayat_Unit = Convert.ToString(dr["Gram_Panchayat_Unit"]),
                                 DocumentType = Convert.ToString(dr["DocumentType"]),
                                 FileName=Convert.ToString(dr["FileName"]),
                                 DocumentName = Convert.ToString(dr["DocumentName"]),
                                 Nationality = Convert.ToString(dr["Nationality"]),
                                 BnDType= Convert.ToString(dr["BnDType"]),
                             }).ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return BirthList;
        }

        public List<Tbl_Birth_Process_Action> GetAllCommentsdata(int B_ID)
        {
            con = Utility.Util.Connection("DBEntities");
            List<Tbl_Birth_Process_Action> CommentList = new List<Tbl_Birth_Process_Action>();
            SqlCommand cmd = new SqlCommand("SP_Select_Next_Action_Data", con);
            con.Open();
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BR_ID", B_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CommentList = (from DataRow dr in dt.Rows

                             select new Tbl_Birth_Process_Action()
                             {
                                 ID = Convert.ToInt16(dr["ID"]),
                                 B_ID = Convert.ToInt16(dr["B_ID"]),

                                 Comments = Convert.ToString(dr["Comments"]),
                                 Action = Convert.ToString(dr["Action"]),
                                 Username = Convert.ToString(dr["Username"]),
                                 DateTime = Convert.ToDateTime(dr["DateTime"]),
                                
                             }).ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return CommentList;

        }

        public string AddRejectrecomendVerification(int bR_ID, int userid,string userName, string action,string RejectAction, string comments)
        {
            DataSet ds = new DataSet();
            string InsertedResult = null;

            con = Utility.Util.Connection("DBEntities");
            SqlCommand cmd = new SqlCommand("SP_InsertNextActionData_1", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            if (comments == null)
            {
                comments = "";
            }

            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BR_ID", bR_ID);
                cmd.Parameters.AddWithValue("@Comment", comments);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@ActionSection", RejectAction);
                cmd.Parameters.AddWithValue("@username", userName);
                cmd.Parameters.AddWithValue("@UID", userid);
                cmd.Parameters.AddWithValue("@Msg", "");
                InsertedResult = cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }
            return InsertedResult;
        }


         private string checkAndGetValue(string param)
        {
            if (string.IsNullOrEmpty(param))
                param = "";

            return param;
        }
        private DateTime checkAndGetDate(DateTime da)
        {
            string date = Convert.ToString(da);
            if (date.Equals("01-01-0001 00:00:00"))
            {
                da = DateTime.Now;
            }
               

            return da;
        }

        
    }


}