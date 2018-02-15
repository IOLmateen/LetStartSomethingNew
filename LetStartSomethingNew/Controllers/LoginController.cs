using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Mvc;
using LetStartSomethingNew.Models;
using MyMasterClasses;
using System.Web.Services;
using System.Web.SessionState;

namespace LetStartSomethingNew.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {

              Passport objPassLogin = new Passport();  
              return View("LoginPage", objPassLogin);
        }

        public ActionResult Logout()
        {
            return View("LoginPage");
        }

        [HttpGet]
        public ActionResult Home()
        {
            if (Session["Login"].ToString() != null || Session["Password"].ToString() != null)
            { 
                return View("HomePage", CommonCode());
            }
            return View("LoginErrorPage");
        }

        [HttpPost]
        public ActionResult Home(Passport objPassLogin)
        {
                 if (ModelState.IsValid)
                { 
                    Session["Login"] = objPassLogin.LoginName.ToString();
                    Session["Password"] = objPassLogin.Password.ToString();
                    //Login objLogin = new Models.Login();
                    //objLogin.objPassport.LoginName = Session["Login"].ToString();
                    //objLogin.objPassport.Password = Session["Password"].ToString();
                    //objLogin.getLogin(objLogin.objPassport.LoginName.ToString(), objLogin.objPassport.Password.ToString() + "#");
                    //if(Session["LoginId"] != null)
                    //{
                    //    objLogin.getUserSettings();
                    //    return View("HomePage", objLogin);
                    //}
                    return View("HomePage", CommonCode());

                }
            
                    return View("LoginErrorPage");
     
        }


        private object CommonCode()
        {
            Login objLogin = new Models.Login();
            objLogin.objPassport.LoginName = Session["Login"].ToString();
            objLogin.objPassport.Password = Session["Password"].ToString();
            objLogin.getLogin(objLogin.objPassport.LoginName.ToString(), objLogin.objPassport.Password.ToString() + "#");
            objLogin.getUserSettings();
            return objLogin;
        }
        
        [HttpPost]
        public JsonResult CheckUserGroupRights(string pagename,string linkname,string rightheader)
        {
            Login objLogin = new Login("UserRights");
            string FlagYN = objLogin.getUserGroupRights(pagename, linkname,rightheader,Session["UserGroupId"].ToString(),Session["CompanyId"].ToString()); 
            return Json(FlagYN, JsonRequestBehavior.AllowGet);            
        }
    }
}