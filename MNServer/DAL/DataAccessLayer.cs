using MNServer.Models;
using System;
using System.Data.SqlClient;
using MNServer.Utility;
using System.Data;
using System.Collections.Generic;
using MNServer.AuditData;
using System.Text.RegularExpressions;
using System.Configuration;
using MNServer.ProcessAction.Model;

namespace MNServer.Controllers.Login
{
    public class DataAccessLayer
    {
        SqlConnection con = null;
       
        List<RoleDesc> RoleList;

        public string getId { get; set; }
        public int customerid { get; set; }
        public string custType { get; set; }
        public string Custgender { get; set; }

        public string getName { get; set; }
       

        #region Login
        public int  LoginDetail(Tbl_UserMaster Uobject)
        {

            //List<RoleDesc> sessionrole = new List<RoleDesc>();

            int sessionrole = 0;
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string result2 = "";
            string pageName = "Login";

            try
            {
                #region CheckLogin
                SqlCommand cmd = new SqlCommand("SP_Login", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userName", Uobject.UserName);
                cmd.Parameters.AddWithValue("@Password", Uobject.Password);
                cmd.Parameters.AddWithValue("@Msg", "");
                result = cmd.ExecuteScalar().ToString();
                string[] lines = result.Split('-');
                int authvalue = lines[0].Contains("Login Successfull") ? 1 : 0;

                int id = Convert.ToInt16(lines[1]);
                #endregion

                #region insert into audit procedure

                result2 = AuditData.AduitFile.FillAudit(pageName, Uobject.UserName, authvalue);

                #endregion

                if (authvalue!= 0){
                    
                    getId = lines[1];
                    customerid =Convert.ToInt16 (lines[2]);
                    custType = lines[3];
                    getName = Uobject.UserName;
                    sessionrole = Selectrole(id);
                    return sessionrole;
                }

                else
                {
                    //List<RoleDesc> errorlog = new List<RoleDesc>();
                    RoleDesc rp = new RoleDesc();
                    rp.id = 001;
                    getName = result;
                    sessionrole =0;
                    //return errorlog;
                }
            }
            catch(Exception ex)
            {
                 ex.Message.ToString();
                return sessionrole;
            }
            finally
            {
                con.Close();
            }
            return sessionrole;
        }
        #endregion

        #region List of All role Id's
        //public List<RoleDesc> LoginDetail(Tbl_UserMaster Uobject)
        //{

        //    //List<RoleDesc> sessionrole = new List<RoleDesc>();

        //    int sessionrole = 0;
        //    con = Utility.Util.Connection("DBEntities");
        //    string result = "";
        //    string result2 = "";
        //    string pageName = "Login";

        //    try
        //    {
        //        #region CheckLogin
        //        SqlCommand cmd = new SqlCommand("SP_Login", con);
        //        con.Open();

        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@userName", Uobject.UserName);
        //        cmd.Parameters.AddWithValue("@Password", Uobject.Password);
        //        cmd.Parameters.AddWithValue("@Msg", "");
        //        result = cmd.ExecuteScalar().ToString();
        //        string[] lines = result.Split('-');
        //        int authvalue = lines[0].Contains("Login Successfull") ? 1 : 0;
        //        int id = Convert.ToInt16(lines[1]);
        //        #endregion

        //        #region insert into audit procedure

        //        result2 = AuditData.AduitFile.FillAudit(pageName, Uobject.UserName, authvalue);

        //        #endregion

        //        if (authvalue != 0)
        //        {

        //            getId = lines[1];
        //            getName = Uobject.UserName;
        //            sessionrole = Selectrole(id);

        //            return sessionrole;
        //        }

        //        else
        //        {
        //            //List<RoleDesc> errorlog = new List<RoleDesc>();
        //            RoleDesc rp = new RoleDesc();
        //            rp.id = 0;
        //            sessionrole.Add(rp);
        //            //return errorlog;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //        return sessionrole;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return sessionrole;
        //}
        #endregion


        public string CompleteProfile(Tbl_Customer_Master tcm,string Uid)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            String orgType = "";
            string pageName = "Profile";

            try
            {
                SqlCommand cmd = new SqlCommand("SP_CreateProfile", con);
                con.Open();
                if (tcm.OrganisationName == null)
                {
                    orgType = "I";
                }
                else
                {
                    orgType = "O";
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID",Uid);
                cmd.Parameters.AddWithValue("@First_Name", tcm.FirstName);
                cmd.Parameters.AddWithValue("@Last_Name", tcm.LastName);
                cmd.Parameters.AddWithValue("@Gender", tcm.Gender);
                cmd.Parameters.AddWithValue("@DOB",tcm.DOB);
                cmd.Parameters.AddWithValue("@PAN",tcm.PAN);
                cmd.Parameters.AddWithValue("@DrivingLicenseNo", tcm.DrivingLicenseNo);
                cmd.Parameters.AddWithValue("@IdProofeNumber", tcm.IDProofeNumber);

                cmd.Parameters.AddWithValue("@Address",tcm.Address);
                cmd.Parameters.AddWithValue("@Password", tcm.Password);
                cmd.Parameters.AddWithValue("@Dist",tcm.Dist);
                cmd.Parameters.AddWithValue("@State", tcm.State);
                cmd.Parameters.AddWithValue("@Country", tcm.Country);
                cmd.Parameters.AddWithValue("@Land_lineNo", tcm.LandlineNo);
                cmd.Parameters.AddWithValue("@OtherGovID", tcm.OtherGovID);

                cmd.Parameters.AddWithValue("@CINNo", tcm.CINNo);
                cmd.Parameters.AddWithValue("@GISTNo",tcm.GISTNo);
                cmd.Parameters.AddWithValue("@IsActive",tcm.IsActive);
                cmd.Parameters.AddWithValue("@IDProofe", tcm.IDProofe);
                cmd.Parameters.AddWithValue("@Nationality",tcm.Nationality);

                cmd.Parameters.AddWithValue("@OrganisationName", tcm.OrganisationName);
                cmd.Parameters.AddWithValue("@OrganisationType", orgType);
                cmd.Parameters.AddWithValue("@AutherizedPerson", tcm.AutherizedPerson);
                cmd.Parameters.AddWithValue("@AutherizPrsDesig", tcm.AutherizPrsDesig);
                cmd.Parameters.AddWithValue("@AutherizPrsEmail_Id", tcm.AutherizPrsEmail_Id);
                //cmd.Parameters.AddWithValue("@AutherizPrsMobile_No", "");
                cmd.Parameters.AddWithValue("@city", tcm.City);
                cmd.Parameters.AddWithValue("@Pincode", tcm.Pincode);

                result = cmd.ExecuteNonQuery().ToString();
                //int authvalue = result == "Register Successfully" ? 1 : 0;


                //#region insert into audit procedure

                //result2 = AuditData.AduitFile.FillAudit(pageName, Urobject.UserName, authvalue);

                //#endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return result;
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public string BirthDetails(BirthProcess bRobject)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string result2 = "";
            //String orgType = "";
            string pageName = "Birth Registration";
            try
            {
               

                #region Birthregistration
                SqlCommand cmd = new SqlCommand("Tbl_BirthRegistration_SP", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name_of_Informant", bRobject.initiatedBy);
                cmd.Parameters.AddWithValue("@Name_of_Child", bRobject.Name_of_Child);
                cmd.Parameters.AddWithValue("@Gender_Of_Child", bRobject.Gender_Of_Child);
                cmd.Parameters.AddWithValue("@Name_of_Father", bRobject.Name_of_Father);
                cmd.Parameters.AddWithValue("@Name_of_Mother", bRobject.Name_of_Mother);

                cmd.Parameters.AddWithValue("@Date_of_Birth", bRobject.Date_of_Birth);
                cmd.Parameters.AddWithValue("@Place_of_Birth", bRobject.Place_of_Birth);
                cmd.Parameters.AddWithValue("@Address", bRobject.Address);
                cmd.Parameters.AddWithValue("@Gram_Panchayat_Unit", bRobject.Gram_Panchayat_Unit);
                cmd.Parameters.AddWithValue("@Nationality", bRobject.Nationality);

                cmd.Parameters.AddWithValue("@City_name", "");
                cmd.Parameters.AddWithValue("@DocumentType", bRobject.DocumentType);
                cmd.Parameters.AddWithValue("@FileName", bRobject.FileName);
                cmd.Parameters.AddWithValue("@DocumentName", bRobject.DocumentName);
                cmd.Parameters.AddWithValue("@PassportNo", "");

                cmd.Parameters.AddWithValue("@FileContent", "");
              
                cmd.Parameters.AddWithValue("@UserID", "");
                cmd.Parameters.AddWithValue("@ModifyDate", "");
                cmd.Parameters.AddWithValue("@Status", "Process");
                cmd.Parameters.AddWithValue("@Level1", "0");
                cmd.Parameters.AddWithValue("@Level2", "0");
                cmd.Parameters.AddWithValue("@Level3", "0");
                cmd.Parameters.AddWithValue("@IsActive", 0);
                cmd.Parameters.AddWithValue("@Msg", "");
               
                result = cmd.ExecuteScalar().ToString();
                int authvalue = result == "Register Successfully" ? 1 : 0;
                #endregion

                #region AuditTable
                result2 = AuditData.AduitFile.FillAudit(pageName, bRobject.Name_of_Informant, authvalue);
               
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

        public List<Tbl_BirthRegistration> GetAllBirthRegistration()
        {
            List<Tbl_BirthRegistration> BirthList = new List<Tbl_BirthRegistration>();
            //con = Utility.Util.Connection("DBEntities");
            //List<Tbl_BirthRegistration> BirthList = new List<Tbl_BirthRegistration>();
            //SqlCommand cmd = new SqlCommand("Tbl_GetBirthRegistration_SP", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //con.Open();
            //da.Fill(dt);
            //con.Close();
            ////CultureInfo culture = new CultureInfo("en-US");
            ////Bind EmpModel generic list using LINQ 
            //BirthList = (from DataRow dr in dt.Rows

            //             select new Tbl_BirthRegistration()
            //             {

            //                 ID = Convert.ToInt32(dr["ID"]),
            //                 Name_of_Informant = Convert.ToString(dr["Name_of_Informant"]),
            //                 Name_of_Child = Convert.ToString(dr["Name_of_Child"]),
            //                 Name_of_Father = Convert.ToString(dr["Name_of_Father"]),
            //                 Name_of_Mother = Convert.ToString(dr["Name_of_Mother"]),
            //                 Address = Convert.ToString(dr["Address"]),
            //                 Gender_Of_Child = Convert.ToString(dr["Gender_Of_Child"]),
            //               //  Date_of_Birth = Convert.ToString(dr["Date_of_Birth"]),
            //                 //Date_of_Birth  = DateTime.ParseExact(dr["Date_of_Birth",  CultureInfo.InvariantCulture].ToString()),
            //                 Place_of_Birth = Convert.ToString(dr["Place_of_Birth"]),
            //                 Gram_Panchayat_Unit = Convert.ToString(dr["Gram_Panchayat_Unit"]),
            //                 DocumentType = Convert.ToString(dr["DocumentType"]),
            //                 DocumentName = Convert.ToString(dr["DocumentName"]),
            //               //  Nationality = Convert.ToString(dr["Nationality"]),
            //             }).ToList();


            return BirthList;
        }

        public string Pro(string Uid)
        {

            con = Utility.Util.Connection("DBEntities");
            string result = "";
            

            #region CheckLogin
            SqlCommand cmd = new SqlCommand("SP_CreateProfile1", con);
            con.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", 4);
           
            result = cmd.ExecuteNonQuery().ToString();

            return result;
            #endregion


        }

        private int Selectrole(int  userid)
        {
            RoleList = new List<RoleDesc>();
            string result = null;

            SqlCommand cmd = new SqlCommand("SP_ChkUserRole", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@u_id", userid);
            cmd.Parameters.AddWithValue("@Msg", "");
            result = cmd.ExecuteScalar().ToString();

            string[] lines = result.Split('|');
            int authvalue =Convert.ToInt16(lines[1]);


            #region Comment when use Role id is in List
            //SqlDataAdapter da = new SqlDataAdapter();
            //da.SelectCommand = cmd;
            //DataSet ds = new DataSet();

            //da.Fill(ds);

            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    RoleDesc rp = new RoleDesc();
            //    rp.id = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"].ToString());

            //    RoleList.Add(rp);
            //}
            #endregion
            return authvalue;
        }
       

        #region Register
        public String RegistrationDetail(Tbl_UserMaster Urobject)
        {
            con = Utility.Util.Connection("DBEntities");
            string result = "";
            string result2 = "";
            string pageName = "Register";

            try
            {
                SqlCommand cmd = new SqlCommand("Registration_SP", con);
                con.Open();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userName", Urobject.UserName);
                cmd.Parameters.AddWithValue("@MailId", Urobject.EmailId);
                cmd.Parameters.AddWithValue("@MobileNumber", Urobject.MobileNumber);
                cmd.Parameters.AddWithValue("@Password", Urobject.Password);
                cmd.Parameters.AddWithValue("@Created_by", "");
                cmd.Parameters.AddWithValue("@Modified_by", "");
                cmd.Parameters.AddWithValue("@Msg", "");
                //cmd.Parameters.AddWithValue("@Flag", 0);
                result = cmd.ExecuteScalar().ToString();
                int authvalue = result == "Register Successfully" ? 1 : 0;


                #region insert into audit procedure

                result2 = AuditData.AduitFile.FillAudit(pageName, Urobject.UserName, authvalue);

                #endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return result;
            }
            finally
            {
                con.Close();
            }

            return result;
        }
        #endregion


    }
}