using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MyMasterClasses;
using DataLayer;
using System.Web.SessionState;



namespace LetStartSomethingNew.Models
{
    public class Login
    {
        
        public BaseDataLayer objDAL { get; set; }
        public Passport objPassport { get; set; }
        //public Employee objEmployee { get; set; }
        //public Department objDepartment { get; set; }
        //public Company objCompany { get; set; }
        //public Country objCountry { get; set; }
        public UserGroupRights objUserGroupRights { get; set; }
        
        public Login()
        {
            CreateObject();
        }

        public Login(string UserRights)
        {
            objDAL = new BaseDataLayer();
            objUserGroupRights = new UserGroupRights();
        }

        public void getLogin(string login, string password)
        {
            DataTable dtLogin = objDAL.getDALLogin(login, password).Tables[0];

            if (dtLogin.Rows.Count == 1)
            {
                foreach (DataRow drLogin in dtLogin.Rows)
                {
                    HttpContext.Current.Session["LoginId"] = Convert.ToInt32(drLogin["Pid"]);
                    HttpContext.Current.Session["UserId"] = Convert.ToInt32(drLogin["EntityXid"]);
                    HttpContext.Current.Session["UserGroupId"] = Convert.ToInt32(drLogin["UserGroupXid"]);
                    HttpContext.Current.Session["CompanyId"] = Convert.ToInt32(drLogin["CompanyXid"]);

                    if (drLogin["EntityType"].ToString() == "E")
                    {
                        DataTable dtRsCmpny = objDAL.getDALCompany(Convert.ToInt32(drLogin["CompanyXid"])).Tables[0];
                        if (dtRsCmpny.Rows.Count > 0)
                        {
                            foreach (DataRow drRsCmpny in dtRsCmpny.Rows)
                            {
                                HttpContext.Current.Session["CompanyName"] = drRsCmpny["Company"].ToString();
                                HttpContext.Current.Session["CountryId"] = Convert.ToInt32(drRsCmpny["CountryXid"]);
                                HttpContext.Current.Session["ODCompanyXId"] = Convert.ToInt32(drRsCmpny["ODCompanyXId"]);
                                HttpContext.Current.Session["DateFormat"] = Convert.ToInt32(drRsCmpny["DateFormat"]);
                            }
                        }

                        DataTable dtRsEmp = objDAL.getDALEmployee(Convert.ToInt32(drLogin["EntityXid"]), Convert.ToInt32(drLogin["CompanyXid"])).Tables[0];
                        if (dtRsEmp.Rows.Count > 0)
                        {
                            foreach (DataRow drRsEmp in dtRsEmp.Rows)
                            {
                                HttpContext.Current.Session["UserDept"] = Convert.ToInt32(drRsEmp["DepartmentXid"]);
                                HttpContext.Current.Session["UserDesig"] = Convert.ToInt32(drRsEmp["DesignationXid"]);
                                HttpContext.Current.Session["UserEmail"] = drRsEmp["Email"].ToString();
                                HttpContext.Current.Session["UserName"] = drRsEmp["EmployeeName"].ToString();
                                HttpContext.Current.Session["CompanyId"] = Convert.ToInt32(drRsEmp["CompanyXid"]);
                                HttpContext.Current.Session["UserDepartment"] = drRsEmp["Department"].ToString();
                            }
                        }
                        HttpContext.Current.Session["RecPerPage"] = 25;
                    }

                }
            }
        }   
        public void getUserSettings()
        {
            DataTable dtUserSetting = objDAL.getDALUserSettings().Tables[0];
            objUserGroupRights.ListUserGroupRights = (from DataRow dr in dtUserSetting.Rows
                                            select new UserGroupRights()
                                            { 
                                                PageName    = dr["PageName"].ToString(),
                                                LinkName    = dr["LinkName"].ToString(),
                                                RightHeader = dr["RightHeader"].ToString()
                                            }).ToList();

        }


        public string getUserGroupRights(string pgname,string lnkname,string rightheader,string usergroupid,string companyid)
        {
            string FlagYN;
           int count = objDAL.getDALUserGroupRights(pgname,lnkname,rightheader,usergroupid,companyid);
            //if(count > 0 )
            //{
            //    FlagYN = "YES";
            //}
            //else
            //{
            //    FlagYN = "NO";   
            //}

            FlagYN = count > 0 ? "YES" : "NO";

            return FlagYN;
        }

        private void CreateObject()
        {
            objDAL = new BaseDataLayer();
            objPassport = new Passport();
            //objEmployee = new Employee();
            //objDepartment = new Department();
            //objCompany = new Company();
            //objCountry = new Country();
            objUserGroupRights = new UserGroupRights();

        }
    }
}

    
