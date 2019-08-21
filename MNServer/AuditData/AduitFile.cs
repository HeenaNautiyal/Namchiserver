using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MNServer.AuditData
{
    public class AduitFile
    {
      
        public static string getDateFormat()
        {
            var dateTime = DateTime.Now.ToString(" dd-MMMM-yyyy hh:mm:ss:tt");
            return dateTime;
        }

        public static  string GetIpAddress()
        {
            string IpAddress = null;
            var request = HttpContext.Current.Request;

            System.Web.HttpContext context = System.Web.HttpContext.Current;
            IpAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IpAddress))
            {
                IpAddress = request.ServerVariables["REMOTE_ADDR"];
            }
            string[] IpAddress_arr = new string[4];
            IpAddress_arr = IpAddress.Split(':');
            if (IpAddress_arr.Length == 3)
            {
                IpAddress_arr[0] = IpAddress_arr[0] == null || IpAddress_arr[0].Trim().Length == 0 ? "127" : IpAddress_arr[0];
                IpAddress_arr[1] = IpAddress_arr[1] == null || IpAddress_arr[1].Trim().Length == 0 ? "0" : IpAddress_arr[1];
                string IP_Add = IpAddress_arr[0] + "." + IpAddress_arr[1] + ".0" + "." + IpAddress_arr[2];
                IpAddress = IP_Add;
            }
            else if (IpAddress_arr.Length == 4)
            {
                IpAddress_arr[0] = IpAddress_arr[0] == null || IpAddress_arr[0].Trim().Length == 0 ? "127" : IpAddress_arr[0];
                IpAddress_arr[1] = IpAddress_arr[1] == null || IpAddress_arr[1].Trim().Length == 0 ? "0" : IpAddress_arr[1];
                IpAddress_arr[2] = IpAddress_arr[2] == null || IpAddress_arr[2].Trim().Length == 0 ? "0" : IpAddress_arr[2];
                IpAddress_arr[3] = IpAddress_arr[3] == null || IpAddress_arr[3].Trim().Length == 0 ? "X" : IpAddress_arr[3];
                IpAddress = IpAddress_arr[0] + "." + IpAddress[1] + "." + IpAddress[2] + "." + IpAddress_arr[3];
            }

            return IpAddress;
        }

        public static string FillAudit(string pageName, string userName, int authvalue)
        {
            SqlConnection con = null;
            con = Utility.Util.Connection("DBEntities");
            string ip = GetIpAddress();
            string date = getDateFormat();
            string result2 = "";

            SqlCommand cmd2 = new SqlCommand("SP_SaveAuditData", con);
            cmd2.CommandType = CommandType.StoredProcedure;

            cmd2.Parameters.AddWithValue("@page_name", pageName);
            cmd2.Parameters.AddWithValue("@userName", userName);
            cmd2.Parameters.AddWithValue("@IpAddress", ip);
            cmd2.Parameters.AddWithValue("@TimeStamp", date);
            cmd2.Parameters.AddWithValue("@Autherisation", authvalue);
            cmd2.Parameters.AddWithValue("@ResultValue", "");

            result2 = cmd2.ExecuteScalar().ToString();

            return result2;
        }
    }
}