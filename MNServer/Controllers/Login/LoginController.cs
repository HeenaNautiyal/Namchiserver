using MNServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace MNServer.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        #region LoginSession

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult Login(Tbl_UserMaster Uobject)
        //{
        //    if (string.IsNullOrEmpty(Uobject.UserName))
        //    {
        //        ModelState.AddModelError("UserName", "User Name Cannot be blank");
        //    }
        //    if (string.IsNullOrEmpty(Uobject.Password))
        //    {
        //        ModelState.AddModelError("Password", "Password Came cannot be blank");
        //    }

        //    if (ModelState.IsValid) //checking model is valid or not
        //    {
        //        int a = 0;
        //        RoleDesc rp = new RoleDesc();
        //        DataAccessLayer objDB = new DataAccessLayer();
        //        int roleids = objDB.LoginDetail(Uobject);

        //        string Uid = objDB.getId;
        //        Session["U_ID"] = Uid;
        //        Session["Cust_ID"] = objDB.customerid;
        //        Session["CustType"] = objDB.custType;
        //        SortedList<int> rlist = new SortedList<int>();

        //        //rp.Showall
        //        if (rp.Showall != null && rp.Showall.Count > 0)
        //        {
        //            for (int i = 0; i < rp.Showall.Count; i++)
        //            {
        //                if (rp.Showall[i].id != 0)
        //                {
        //                    a = rp.Showall[i].id;
        //                    rlist.Add(a);
        //                }
        //                else
        //                {
        //                    return RedirectToAction("AppliedPage");
        //                }
        //            }
        //        }
        //        string strTmp = "";
        //        foreach (int Roleid in rlist)
        //        {

        //            if (Roleid == 2)
        //            {
        //                strTmp = "Max";
        //                TempData["ErrorMessage"] = "Applied";
        //                TempData["Roleid"] = Roleid.ToString();
        //            }
        //            else if (Roleid == 1)
        //            {
        //                strTmp = "view";
        //                TempData["ErrorMessage"] = "Applied";
        //                TempData["Roleid"] = Roleid.ToString();
        //            }

        //        }


        //        TempData["Viewstr"] = strTmp;
        //        return RedirectToAction("Index");
        //    }
        //    return View(Uobject);
        //}

        [HttpPost]
        public ActionResult Login(Tbl_UserMaster Uobject)
        {
            if (string.IsNullOrEmpty(Uobject.UserName))
            {
                ModelState.AddModelError("UserName", "User Name Cannot be blank");
            }
            if (string.IsNullOrEmpty(Uobject.Password))
            {
                ModelState.AddModelError("Password", "Password Came cannot be blank");
            }

            if (ModelState.IsValid) //checking model is valid or not
            {

                RoleDesc rp = new RoleDesc();
                DataAccessLayer objDB = new DataAccessLayer();
                //  rp.Showall = objDB.LoginDetail(Uobject);
                int roleids = objDB.LoginDetail(Uobject);
                if (roleids == 0)
                {
                    return View("AppliedPage");
                }
                string Uid = objDB.getId;
                string Uname = objDB.getName;
                Session["U_ID"] = Uid;
                Session["U_Name"] = Uname;
                Session["Cust_ID"] =objDB.customerid;
                Session["CustType"] = objDB.custType;
                #region List version
                //rp.Showall

                //if (rp.Showall != null && rp.Showall.Count > 0)
                //{
                //    for (int i = 0; i < rp.Showall.Count; i++)
                //    {
                //        if (rp.Showall[i].id != 0)
                //        {
                //            a = rp.Showall[i].id;
                //            UserRoleList.Add(a);

                //        }
                //        else
                //        {
                //            return RedirectToAction("AppliedPage");
                //        }
                //    }
                //}
                #endregion

                string strTmp = "";

                //Session["SortedList1"] = UserRoleList;
                Session["SortedList1"] = roleids;

                if (roleids == 2 || roleids == 4 || roleids == 8)
                {
                    strTmp = "view";
                    TempData["ErrorMessage"] = "Applied";
                    TempData["Roleid"] = roleids.ToString();
                }
                else if (roleids == 1)
                {
                    strTmp = "Max";
                    TempData["ErrorMessage"] = "Applied";
                    TempData["Roleid"] = roleids.ToString();
                }
                else if (roleids == 3 || roleids == 6)
                {
                    strTmp = "Admin";
                    TempData["ErrorMessage"] = "Applied";
                    TempData["Roleid"] = roleids.ToString();
                }


                TempData["Viewstr"] = strTmp;
                Session["ViewData"] = strTmp;
                return RedirectToAction("Index");
            }
            return View(Uobject);
        }

        #endregion

        #region Registration

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Tbl_UserMaster URobject)
        {

            if (string.IsNullOrEmpty(URobject.UserName))
            {
                ModelState.AddModelError("UserName", "Please Enter User Name");
            }

            if (!string.IsNullOrEmpty(URobject.EmailId))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);

                if (!re.IsMatch(URobject.EmailId))
                {
                    ModelState.AddModelError("MailId", "Invalid Email");
                }
            }
            else
            {
                ModelState.AddModelError("MailId", "Please Enter Email ID");
            }

            if (URobject.MobileNumber == 0)
            {
                ModelState.AddModelError("MobileNumber", "Please Enter Mobile Number");
            }
            if (string.IsNullOrEmpty(URobject.Password))
            {
                ModelState.AddModelError("Password", "Please Enter Password");
            }


            if (ModelState.IsValid) //checking model is valid or not
            {
                DataAccessLayer objDB = new DataAccessLayer();

                string result = objDB.RegistrationDetail(URobject);
                ViewBag.MyString = result;
                //     ModelState.Clear(); //clearing model




                ViewBag.MyString = result;
                ModelState.Clear();
                return View();
            }
            else
            {
                ModelState.AddModelError("", "");
                return View();
            }
        }
        #endregion



    }


}