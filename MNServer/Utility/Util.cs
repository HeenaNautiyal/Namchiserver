using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

using MNServer.Models;
using MNServer.ProcessAction.Action;
using MNServer.ProcessAction.Model;

namespace MNServer.Utility
{
    public class Util
    {

        public interface INotificationService
        {
            bool Notify();
        }

        public static string base64String = null;

        private static SqlConnection con;

        public static object Request { get; private set; }

        public static SqlConnection Connection(String dbEntities)
        {
            if (con == null)
                con = new SqlConnection(ConfigurationManager.ConnectionStrings[dbEntities].ToString());

            return con;
        }

        public static SqlConnection openConnection(SqlConnection connection)
        {
            if (connection != null)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
            }
            return connection;
        }

        public static SortedDictionary<int, string> GetActionSequenceold(int PID, string sp_name)
        {
            SortedDictionary<int, string> ObjbirthProcessActionSeq = new SortedDictionary<int, string>();
            DataTable DTable = new DataTable();
            //string NextAction = null;

            con = Utility.Util.Connection("DBEntities");

            DataTable dt = null;

            try
            {
                #region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_BirthProcessAction_1", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PID", PID);
                cmd.Parameters.AddWithValue("@strproc", sp_name);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(DTable);
                #endregion

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            finally
            {
                con.Close();
            }
            //foreach (DataRow row in DTable.Rows)
            //{
            //    ObjbirthProcessActionSeq.Add(Convert.ToInt16(row["Seq_ID"]), row["ActionName"].ToString());
            //}

            return ObjbirthProcessActionSeq;

        }

        #region[SP_BirthProcessAction]
        //public static string GetActionSequence(int PID, string sp_name)
        //{
        //    string Next_Action = null;
        //    DataSet Dset = new DataSet();
        //    List<RoletoAction> SeqList = null;
        //    //string NextAction = null;

        //    con = Utility.Util.Connection("DBEntities");



        //    try
        //    {
        //        #region Birthregistration
        //        SqlCommand cmd = new SqlCommand("SP_BirthProcessAction", con);
        //        con.Open();

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@PID", PID);
        //        cmd.Parameters.AddWithValue("@strproc", sp_name);
        //        SqlDataAdapter da = new SqlDataAdapter();
        //        da.SelectCommand = cmd;
        //        da.Fill(Dset);

        //        SeqList = new List<RoletoAction>();

        //        #region For Display role in Table
        //        for (int i = 0; i < Dset.Tables[0].Rows.Count; i++)
        //        {
        //            RoletoAction TAM = new RoletoAction();
        //            TAM.NextAction = Convert.ToInt16(Dset.Tables[0].Rows[i]["NextAction"].ToString());
        //            TAM.ActionDesc = Dset.Tables[0].Rows[i]["ActionDesc"].ToString();
        //            TAM.NextDesc = Dset.Tables[0].Rows[i]["NextDesc"].ToString();
        //            TAM.ID= Convert.ToInt16(Dset.Tables[0].Rows[i]["ID"].ToString());

        //            Next_Action = TAM.NextDesc; 
        //            SeqList.Add(TAM);
        //        }
        //        #endregion

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.StackTrace.ToString();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    //foreach (DataRow row in DTable.Rows)
        //    //{
        //    //    ObjbirthProcessActionSeq.Add(Convert.ToInt16(row["Seq_ID"]), row["ActionName"].ToString());
        //    //}

        //    return Next_Action;
        //    #endregion
        //}
        #endregion

        public static string GetActionSequence(int PID, string sp_name)
        {
            string Next_Action_Name = null;
            DataSet Dset = new DataSet();
            List<RoletoAction> SeqList = null;
            //string NextAction = null;

            con = Utility.Util.Connection("DBEntities");



            try
            {
                #region Birthregistration
                SqlCommand cmd = new SqlCommand("SP_BirthProcessAction_1", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PID", PID);
                cmd.Parameters.AddWithValue("@strproc", sp_name);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(Dset);

                SeqList = new List<RoletoAction>();

                #region For Display role in Table
                for (int i = 0; i < Dset.Tables[0].Rows.Count; i++)
                {
                    RoletoAction TAM = new RoletoAction();
                    TAM.NextAction = Convert.ToInt16(Dset.Tables[0].Rows[i]["NextAction"].ToString());
                    TAM.ActionDesc = Dset.Tables[0].Rows[i]["ActionDesc"].ToString();
                    TAM.NextDesc = Dset.Tables[0].Rows[i]["NextDesc"].ToString();
                    TAM.ID = Convert.ToInt16(Dset.Tables[0].Rows[i]["ID"].ToString());

                    Next_Action_Name = TAM.NextDesc.Trim()+'|'+TAM.NextAction.ToString();
                    SeqList.Add(TAM);
                }
                #endregion

            }
            catch (Exception ex)
            {
                ex.StackTrace.ToString();
            }
            finally
            {
                con.Close();
            }
            //foreach (DataRow row in DTable.Rows)
            //{
            //    ObjbirthProcessActionSeq.Add(Convert.ToInt16(row["Seq_ID"]), row["ActionName"].ToString());
            //}

            return Next_Action_Name;
            #endregion
        }
       

        public static string ImageToBase64(HttpPostedFileBase files)
        {

            string FileExt = Path.GetExtension(files.FileName).ToUpper();

            if (FileExt == ".PDF")
            {
                Stream str = files.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                base64String = Convert.ToBase64String(FileDet);

                //Tbl_BirthRegistration Fd = new Tbl_BirthRegistration();
                //Fd.FileName = files.FileName;
                //Fd.FileContent = FileDet.ToString();
                return base64String;
            }

            return base64String;
        }

        public static string AltMsg(string Message)
        {
            return string.Format(Message);
        }

        public static void Clear()
        {

            
        }
        
        public static string AddDocuments(HttpRequestBase Request, List<Tbl_Document_List> Doclist, 
            string fileSavePath, string Appno, int Custid )
        {
            string results = "";
            if (Request.Files.AllKeys.Length > 0)
            {
                BirthProcessAction obj = new BirthProcessAction();
                for (int k = 0; k < Doclist.Count; k++)
                {
                    var DocumentTmp = Doclist[k];
                    var files = Request.Files;
                    
                        var file = files[k];
                        if (file != null)
                        {
                            var fileName = file.FileName;
                            if (fileName != null && fileName.Trim().Length > 0)
                            {
                                if (fileName.Contains(@"\"))
                                {
                                    fileName = fileName.Substring(fileName.LastIndexOf(@"\"));
                                }
                                string Docname = DocumentTmp.Docname;
                                
                                if (Docname.Equals("Identity Document"))
                                {
                                    Docname = Docname.Replace(" ", "_");
                                }
                                else
                                {
                                string[] temp = Appno.Split('_');
                                Docname = temp[0];
                                }

                                int idx = fileName.LastIndexOf(".");
                                string fileNameTemp = fileName.Substring(0, idx);
                                string extemp = fileName.Substring(idx);
                                fileNameTemp = fileNameTemp + "_" + Appno;
                                fileName = fileNameTemp + extemp;

                                fileSavePath = fileSavePath + Docname;
                                if (!Directory.Exists(fileSavePath))
                                {
                                    Directory.CreateDirectory(fileSavePath);
                                }
                           
                                fileSavePath = fileSavePath + "\\" + fileName;
                            
                                
                                //populate DocumentTmp
                                DocumentTmp.DocFilePath = fileSavePath;
                                DocumentTmp.IsActive = true;
                                DocumentTmp.BR_ID = Appno;
                                DocumentTmp.Doctype = Doclist[k].Doctype;
                                results = results + obj.InsertDocumentAttachment
                                (DocumentTmp.BR_ID, fileName, DocumentTmp.DocFilePath, DocumentTmp.Doctype, DocumentTmp.IsActive,Custid);
                                file.SaveAs(fileSavePath);
                            }
                        }
                    
                }
            }
            return results;
        }


        #region EmailIntegration
        public class MailService : INotificationService
        {
            readonly string address;
            readonly string subject;
            readonly string message;
            private string v1;
            private string v2;
            private SmsSender smsSender;

            public MailService(string adrss, string subject, string mesg)
            {
                this.address = adrss;
                this.subject = subject;
                this.message = mesg;
            }

            public MailService(string v1, string v2, SmsSender smsSender)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.smsSender = smsSender;
            }

            public bool Notify()
            {
                string fromAddress = "smartcitygangtok@gmail.com";

                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(address);
                    //mail.CC.Add("ccid@hotmail.com");
                    mail.From = new MailAddress(fromAddress);
                    mail.Subject = subject;

                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential(fromAddress, "smartcity");
                    smtp.Send(mail);
                    Console.WriteLine("finish");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                    Console.WriteLine(ex.StackTrace);
                }

                return true;

            }
        }
        
        public class SmsService : INotificationService
        {
            readonly string phoneNumber;
            readonly string message;
            readonly ISmsSender sender;
            public SmsService(string phone, string msg, ISmsSender send)
            {
                this.phoneNumber = phone;
                this.message = msg;
                this.sender = send;
            }

            public bool Notify()
            {
                Console.WriteLine("Sms service invoked" + phoneNumber, message);
                return sender.SendSms(message);

            }
        }
        public interface ISmsSender
        {
            bool SendSms(string msg);
        }

        public class SmsSender : ISmsSender
        {
            public bool SendSms(string msg)
            {
                return true;
            }
        }
        #endregion

    }
}