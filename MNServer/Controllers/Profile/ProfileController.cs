using MNServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MNServer.Controllers.Login;
using System.Web.Routing;
using System.Web.Security;

namespace MNServer.Controllers.Profile
{
    public class ProfileController : Controller
    {

        public ActionResult IndexProfile(int id)
        {
            if(id==0)
            {
                TempData["Viewstr"] = "O";
                return View("Profile"); }
            else
            {
                TempData["Viewstr"] = "I";
                return View("Profile");
            }
          
        }

        // GET: Profile
        public ActionResult RegistrationType()
        {
            return View();
        }

        public ActionResult GetprofileView(Tbl_Customer_Master TcM)
        {
            string Uid = Session["U_ID"].ToString();
            DataAccessLayer objDB = new DataAccessLayer();
            string result = objDB.CompleteProfile(TcM,Uid);
           //rp.Showall = objDB.LoginDetail(Uobject);

            return View("test");
        }

      
        public ActionResult Abc()
        {
            return View("test");
        }

    }
}