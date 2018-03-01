using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LetStartSomethingNew.Models.GeneralMaster;

namespace LetStartSomethingNew.Controllers
{

    #region Forgerytoken
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ValidateAntiForgeryTokenWrapperAttribute : FilterAttribute, IAuthorizationFilter
{  
  private readonly ValidateAntiForgeryTokenAttribute _validator;
  private readonly AcceptVerbsAttribute _verbs;
 
  public ValidateAntiForgeryTokenWrapperAttribute(HttpVerbs verbs)  
  { 
    this._verbs = new AcceptVerbsAttribute(verbs);  
    this._validator = new ValidateAntiForgeryTokenAttribute();  
  }

  public void OnAuthorization(AuthorizationContext filterContext) 
  {   
      string httpMethodOverride = filterContext.HttpContext.Request.GetHttpMethodOverride();  
      if (this._verbs.Verbs.Contains(httpMethodOverride, StringComparer.OrdinalIgnoreCase))  
      {    
        this._validator.OnAuthorization(filterContext);   
     }    
  }
}
    #endregion

    public class GeneralMasterController : Controller
    {
        #region CommentedCode

        //[HttpPost]
        //public ActionResult SearchDiscount(FormCollection form)
        //{
        //    //Passing Using FormCOleection
        //    string pid = form["pid"];
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult SearchDiscount(DiscountTye m)
        //{
        //    //Passing Using model
        //    return View();
        //}

        // GET: GeneralMaster--http://travcostaging2016.iolcloud.com/backoffice/homepage.asp
        public ActionResult Index()
        {
            return View();
        }
        //[HttpDelete]
        //public ActionResult DeleteDiscountType()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult SearchDiscount()
        //{
        //    //Pass using Ajax

        //    if (ModelState.IsValid)
        //    {
        //        if (Convert.ToInt32(Request["Pid"]) != 0)
        //        {
        //            //GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
        //            TestClass t = new TestClass();
        //            //objDiscountType.listDiscountType = t.SearchDiscountTypeByPid(Convert.ToInt32(Request["Pid"]));
        //            return View("DisplayDiscountType", t.SearchDiscountTypeByPid(Convert.ToInt32(Request["Pid"])));
        //        }
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public ActionResult DisplayDiscountType()
        //{
        //    GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
        //    TestClass t = new TestClass();
        //    objDiscountType.listDiscountType = t.DisplayDiscountType();
        //    return View(objDiscountType);
        //}


        //[HttpGet]
        //public ActionResult DisplayDiscountType()
        //{
        //   // GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
        //    TestClass t = new TestClass();
        //    //GeneralMaster.DiscountType objDiscountType = t.DisplayDiscountType();
        //    return View(t.DisplayDiscountType());
        //}


        //[HttpPost]
        //public ActionResult DisplayDiscountType(int? index)
        //{
        //    // GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
        //    TestClass t = new TestClass();
        //    //GeneralMaster.DiscountType objDiscountType = t.DisplayDiscountType();
        //    return View(t.DisplayDiscountType());
        //}

        //[HttpGet]
        //public ActionResult DisplayDiscountType(int? a)
        // {
        //     return View(this.DiscountType(1,0));
        // }

        #endregion

        #region CheckUserGroupRights
        [HttpPost]
        public JsonResult CheckUserGroupRights(string pagename, string linkname, string rightheader)
        {
            TestClass t = new TestClass();
            string FlagYN = t.getUserGroupRights(pagename, linkname, rightheader, Session["UserGroupId"].ToString(), Session["CompanyId"].ToString());
            return Json(FlagYN, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DiscountType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayDiscountType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                { 
                 CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                 SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage","Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.DiscountType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.DiscountType(1,0));
            }
            return View("Error");

        }
        private GeneralMaster.DiscountType DiscountType(int currPage,int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayDiscountType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchDiscountTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchDiscountType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult ActionDiscountType()
        {
            return View(this.ADiscountType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.DiscountType ADiscountType()
        {
            TestClass t = new TestClass();
            return t.ADiscountType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult ActionDiscountType(GeneralMaster.DiscountType model)
        {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("DiscountName", "Please check DiscountName");
                    ModelState.AddModelError("Sequence", "Please check Sequence");
                    ModelState.AddModelError("Description", "Please check Description");
                }
                else
                {
                this.ADiscountType(model);
                return RedirectToAction("DisplayDiscountType");
                }
            return View("Error");
        }
      //POST ADDDISCOUNT
        private GeneralMaster.DiscountType ADiscountType(GeneralMaster.DiscountType model)
        {
            TestClass t = new TestClass();
            return t.ADiscountType(model);
        }

        
        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditDiscountType(int pid)
        {
            return View(this.EDiscountType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.DiscountType EDiscountType(int pid)
        {
            TestClass t = new TestClass();
            return t.EDiscountType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditDiscountType(GeneralMaster.DiscountType model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DiscountName", "Please check DiscountName");
                ModelState.AddModelError("Sequence", "Please check Sequence");
                ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EDiscountType(model);
                return RedirectToAction("DisplayDiscountType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.DiscountType EDiscountType(GeneralMaster.DiscountType model)
        {
            TestClass t = new TestClass();
            return t.EDiscountType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteDiscountType(int pid)
        {
            this.DDiscountType(pid);
            return RedirectToAction("DisplayDiscountType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.DiscountType DDiscountType(int pid)
        {
            TestClass t = new TestClass();
            return t.DDiscountType(pid);
        }
        #endregion




        #region RoomType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayRoomType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.RoomType(CurrPage, SearchPid));
            }
            else
            if (Request.HttpMethod == "GET")
            {
                return View(this.RoomType(1, 0));
            }
            return View("Error");

            ////GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            //TestClass t = new TestClass();
            ////objRoomType.listRoomType = t.DisplayRoomType();
            //return View(t.DisplayRoomType());
        }
        private GeneralMaster.RoomType RoomType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayRoomType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchRoomTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchRoomType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddRoomType()
        {
            return View(this.ARoomType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.RoomType ARoomType()
        {
            TestClass t = new TestClass();
            return t.ARoomType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddRoomType(GeneralMaster.RoomType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ARoomType(model);
                return RedirectToAction("DisplayRoomType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.RoomType ARoomType(GeneralMaster.RoomType model)
        {
            TestClass t = new TestClass();
            return t.ARoomType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditRoomType(int pid)
        {
            return View(this.ERoomType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.RoomType ERoomType(int pid)
        {
            TestClass t = new TestClass();
            return t.ERoomType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditRoomType(GeneralMaster.RoomType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ERoomType(model);
                return RedirectToAction("DisplayRoomType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.RoomType ERoomType(GeneralMaster.RoomType model)
        {
            TestClass t = new TestClass();
            return t.ERoomType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteRoomType(int pid)
        {
            this.DRoomType(pid);
            return RedirectToAction("DisplayRoomType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.RoomType DRoomType(int pid)
        {
            TestClass t = new TestClass();
            return t.DRoomType(pid);
        }
        #endregion

        #region Activity

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayActivity()
        {

            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Activity(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Activity(1, 0));
            }
            return View("Error");
            ////GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            //TestClass t = new TestClass();
            ////objActivity.listActivity = t.DisplayActivity();
            //return View(t.DisplayActivity());
        }
        private GeneralMaster.Activity Activity(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayActivity(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchActivityByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchActivity(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddActivity()
        {
            return View(this.AActivity());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Activity AActivity()
        {
            TestClass t = new TestClass();
            return t.AActivity();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddActivity(GeneralMaster.Activity model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AActivity(model);
                return RedirectToAction("DisplayActivity");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Activity AActivity(GeneralMaster.Activity model)
        {
            TestClass t = new TestClass();
            return t.AActivity(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditActivity(int pid)
        {
            return View(this.EActivity(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Activity EActivity(int pid)
        {
            TestClass t = new TestClass();
            return t.EActivity(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditActivity(GeneralMaster.Activity model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EActivity(model);
                return RedirectToAction("DisplayActivity");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Activity EActivity(GeneralMaster.Activity model)
        {
            TestClass t = new TestClass();
            return t.EActivity(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteActivity(int pid)
        {
            this.DActivity(pid);
            return RedirectToAction("DisplayActivity");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Activity DActivity(int pid)
        {
            TestClass t = new TestClass();
            return t.DActivity(pid);
        }
        #endregion
        #region Addresstype

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayAddressType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.AddressType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.AddressType(1, 0));
            }
            return View("Error");
            //// GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            //TestClass t = new TestClass();
            ////objAddressType.listAddressType = t.DisplayAddressType();
            //return View(t.DisplayAddressType());
        }
        private GeneralMaster.AddressType AddressType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayAddressType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchAddressTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchAddressType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddAddressType()
        {
            return View(this.AAddressType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.AddressType AAddressType()
        {
            TestClass t = new TestClass();
            return t.AAddressType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddAddressType(GeneralMaster.AddressType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AAddressType(model);
                return RedirectToAction("DisplayAddressType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.AddressType AAddressType(GeneralMaster.AddressType model)
        {
            TestClass t = new TestClass();
            return t.AAddressType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditAddressType(int pid)
        {
            return View(this.EAddressType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.AddressType EAddressType(int pid)
        {
            TestClass t = new TestClass();
            return t.EAddressType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditAddressType(GeneralMaster.AddressType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EAddressType(model);
                return RedirectToAction("DisplayAddressType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.AddressType EAddressType(GeneralMaster.AddressType model)
        {
            TestClass t = new TestClass();
            return t.EAddressType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteAddressType(int pid)
        {
            this.DAddressType(pid);
            return RedirectToAction("DisplayAddressType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.AddressType DAddressType(int pid)
        {
            TestClass t = new TestClass();
            return t.DAddressType(pid);
        }
        #endregion

        //No Changes in Airport
        #region Airport
        //        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        //        public ActionResult DisplayAirport()
        //        {
        //            //if (Request.HttpMethod == "POST")
        //            //{
        //            //    int CurrPage = 0;
        //            //    int SearchPid = 0;
        //            //    if (ModelState.IsValid)
        //            //    {
        //            //        CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
        //            //        SearchPid = Convert.ToInt32(Request["Pid"]);
        //            //    }
        //            //    else
        //            //    {
        //            //        ModelState.AddModelError("Currpage", "Please check Currpage");
        //            //        ModelState.AddModelError("SearchPid", "Please check SearchPid");
        //            //    }
        //            //    return View(this.DiscountType(CurrPage, SearchPid));
        //            //}
        //            //else if (Request.HttpMethod == "GET")
        //            //{
        //            //    return View(this.DiscountType(1, 0));
        //            //}
        //            return View("Error");
        //            //GeneralMaster.VM_Airport objAirport = new GeneralMaster.VM_Airport();
        //            //TestClass t = new TestClass();
        //            //objAirport.listVMAirport = t.DisplayAirport();
        //            //return View(objAirport);
        //        }
        //        private GeneralMaster.RoomType RoomType(int currPage, int SearchPid)
        //        {
        //            TestClass t = new TestClass();
        //            return t.DisplayRoomType(currPage, SearchPid);
        //        }

        //        [HttpPost]
        //        public JsonResult SearchRoomTypeByString(string prefix)
        //        {
        //            TestClass t = new TestClass();
        //            return Json(t.SearchRoomType(prefix), JsonRequestBehavior.AllowGet);
        //        }

        //        //GET ADDDISCOUNT
        //        [HttpGet]
        //        public ActionResult AddRoomType()
        //        {
        //            return View(this.ARoomType());
        //        }
        //        //GET ADDDISCOUNT
        //        private GeneralMaster.RoomType ARoomType()
        //        {
        //            TestClass t = new TestClass();
        //            return t.ADiscountType();
        //        }

        //        //POST ADDDISCOUNT
        //        [HttpPost]
        //        public ActionResult AddRoomType(GeneralMaster.RoomType model)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
        //                //ModelState.AddModelError("Sequence", "Please check Sequence");
        //                //ModelState.AddModelError("Description", "Please check Description");
        //            }
        //            else
        //            {
        //                this.ARoomType(model);
        //                return RedirectToAction("DisplayRoomType");
        //            }
        //            return View("Error");
        //        }
        //        //POST ADDDISCOUNT
        //        private GeneralMaster.RoomType ARoomType(GeneralMaster.RoomType model)
        //        {
        //            TestClass t = new TestClass();
        //            return t.ARoomType(model);
        //        }


        //        //GET EDITDISCOUNT
        //        [HttpGet]
        //        public ActionResult EditRoomType(int pid)
        //        {
        //            return View(this.ERoomType(pid));
        //        }
        //        //GET EDITDISCOUNT
        //        private GeneralMaster.RoomType ERoomType(int pid)
        //        {
        //            TestClass t = new TestClass();
        //            return t.ERoomType(pid);
        //        }


        //        ////POST EDITDISCOUNT
        //        [HttpPost]
        //        public ActionResult EditRoomType(GeneralMaster.RoomType model)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
        //                //ModelState.AddModelError("Sequence", "Please check Sequence");
        //                //ModelState.AddModelError("Description", "Please check Description");
        //            }
        //            else
        //            {
        //                this.ERoomType(model);
        //                return RedirectToAction("DisplayRoomType");
        //            }
        //            return View("Error");
        //        }
        //        //POst EDITDISCOUNT
        //        private GeneralMaster.RoomType ERoomType(GeneralMaster.RoomType model)
        //        {
        //            TestClass t = new TestClass();
        //            return t.ERoomType(model);
        //        }

        //        //GET DeeleteDISCOUNT
        //        [HttpGet]
        //        public ActionResult DeleteRoomType(int pid)
        //        {
        //            this.DRoomType(pid);
        //            return RedirectToAction("DisplayRoomType");
        //        }
        //        //GET DeeleteDISCOUNT
        //        private GeneralMaster.RoomType DRoomType(int pid)
        //        {
        //            TestClass t = new TestClass();
        //            return t.DRoomType(pid);
        //        }
        #endregion

        #region Bank
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayBank()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Bank(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Bank(1, 0));
            }
            return View("Error");
            ////GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            //TestClass t = new TestClass();
            ////objBank.listBank = t.DisplayBank();
            //return View(t.DisplayBank());
        }
        private GeneralMaster.Bank Bank(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayBank(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchBankByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchBank(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddBank()
        {
            return View(this.ABank());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Bank ABank()
        {
            TestClass t = new TestClass();
            return t.ABank();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddBank(GeneralMaster.Bank model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ABank(model);
                return RedirectToAction("DisplayBank");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Bank ABank(GeneralMaster.Bank model)
        {
            TestClass t = new TestClass();
            return t.ABank(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditBank(int pid)
        {
            return View(this.EBank(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Bank EBank(int pid)
        {
            TestClass t = new TestClass();
            return t.EBank(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditBank(GeneralMaster.Bank model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EBank(model);
                return RedirectToAction("DisplayBank");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Bank EBank(GeneralMaster.Bank model)
        {
            TestClass t = new TestClass();
            return t.EBank(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteBank(int pid)
        {
            this.DBank(pid);
            return RedirectToAction("DisplayBank");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Bank DBank(int pid)
        {
            TestClass t = new TestClass();
            return t.DBank(pid);
        }

        #endregion
        #region BookingNote
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayBookingNote()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.BookingNote(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.BookingNote(1, 0));
            }
            return View("Error");
            ////     GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            //     TestClass t = new TestClass();
            ////     objBookingNote.listBookingNote = t.DisplayBookingNote();
            //     return View(t.DisplayBookingNote());
        }
        private GeneralMaster.BookingNote BookingNote(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayBookingNote(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchBookingNoteByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchBookingNote(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddBookingNote()
        {
            return View(this.ABookingNote());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.BookingNote ABookingNote()
        {
            TestClass t = new TestClass();
            return t.ABookingNote();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddBookingNote(GeneralMaster.BookingNote model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ABookingNote(model);
                return RedirectToAction("DisplayBookingNote");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.BookingNote ABookingNote(GeneralMaster.BookingNote model)
        {
            TestClass t = new TestClass();
            return t.ABookingNote(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditBookingNote(int pid)
        {
            return View(this.EBookingNote(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.BookingNote EBookingNote(int pid)
        {
            TestClass t = new TestClass();
            return t.EBookingNote(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditBookingNote(GeneralMaster.BookingNote model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EBookingNote(model);
                return RedirectToAction("DisplayBookingNote");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.BookingNote EBookingNote(GeneralMaster.BookingNote model)
        {
            TestClass t = new TestClass();
            return t.EBookingNote(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteBookingNote(int pid)
        {
            this.DBookingNote(pid);
            return RedirectToAction("DisplayBookingNote");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.BookingNote DBookingNote(int pid)
        {
            TestClass t = new TestClass();
            return t.DBookingNote(pid);
        }
        #endregion

        #region CardType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayCardType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.CardType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.CardType(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            //   TestClass t = new TestClass();
            ////   objClientChain.listClientChain = t.DisplayClientChain();
            //   return View(t.DisplayClientChain());
        }
        private GeneralMaster.CardType CardType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayCardType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchCardTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchCardType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddCardType()
        {
            return View(this.ACardType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.CardType ACardType()
        {
            TestClass t = new TestClass();
            return t.ACardType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddCardType(GeneralMaster.CardType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ACardType(model);
                return RedirectToAction("DisplayCardType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.CardType ACardType(GeneralMaster.CardType model)
        {
            TestClass t = new TestClass();
            return t.ACardType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditCardType(int pid)
        {
            return View(this.ECardType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.CardType ECardType(int pid)
        {
            TestClass t = new TestClass();
            return t.ECardType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditCardType(GeneralMaster.CardType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ECardType(model);
                return RedirectToAction("DisplayCardType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.CardType ECardType(GeneralMaster.CardType model)
        {
            TestClass t = new TestClass();
            return t.ECardType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteCardType(int pid)
        {
            this.DCardType(pid);
            return RedirectToAction("DisplayCardType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.CardType DCardType(int pid)
        {
            TestClass t = new TestClass();
            return t.DCardType(pid);
        }

        #endregion


        #region ClientChain
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayClientChain()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ClientChain(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ClientChain(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            //   TestClass t = new TestClass();
            ////   objClientChain.listClientChain = t.DisplayClientChain();
            //   return View(t.DisplayClientChain());
        }
        private GeneralMaster.ClientChain ClientChain(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayClientChain(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchClientChainByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchClientChain(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddClientChain()
        {
            return View(this.AClientChain());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ClientChain AClientChain()
        {
            TestClass t = new TestClass();
            return t.AClientChain();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddClientChain(GeneralMaster.ClientChain model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AClientChain(model);
                return RedirectToAction("DisplayClientChain");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ClientChain AClientChain(GeneralMaster.ClientChain model)
        {
            TestClass t = new TestClass();
            return t.AClientChain(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditClientChain(int pid)
        {
            return View(this.EClientChain(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ClientChain EClientChain(int pid)
        {
            TestClass t = new TestClass();
            return t.EClientChain(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditClientChain(GeneralMaster.ClientChain model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EClientChain(model);
                return RedirectToAction("DisplayClientChain");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ClientChain EClientChain(GeneralMaster.ClientChain model)
        {
            TestClass t = new TestClass();
            return t.EClientChain(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteClientChain(int pid)
        {
            this.DClientChain(pid);
            return RedirectToAction("DisplayClientChain");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ClientChain DClientChain(int pid)
        {
            TestClass t = new TestClass();
            return t.DClientChain(pid);
        }

        #endregion
        #region Currency
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayCurrency()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Currency(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Currency(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            //   TestClass t = new TestClass();
            ////   objCurrency.listCurrency = t.DisplayCurrency();
            //   return View(t.DisplayCurrency());
        }
        private GeneralMaster.Currency Currency(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayCurrency(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchCurrencyByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchCurrency(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddCurrency()
        {
            return View(this.ACurrency());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Currency ACurrency()
        {
            TestClass t = new TestClass();
            return t.ACurrency();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddCurrency(GeneralMaster.Currency model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ACurrency(model);
                return RedirectToAction("DisplayCurrency");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Currency ACurrency(GeneralMaster.Currency model)
        {
            TestClass t = new TestClass();
            return t.ACurrency(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditCurrency(int pid)
        {
            return View(this.ECurrency(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Currency ECurrency(int pid)
        {
            TestClass t = new TestClass();
            return t.ECurrency(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditCurrency(GeneralMaster.Currency model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ECurrency(model);
                return RedirectToAction("DisplayCurrency");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Currency ECurrency(GeneralMaster.Currency model)
        {
            TestClass t = new TestClass();
            return t.ECurrency(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteCurrency(int pid)
        {
            this.DCurrency(pid);
            return RedirectToAction("DisplayCurrency");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Currency DCurrency(int pid)
        {
            TestClass t = new TestClass();
            return t.DCurrency(pid);
        }
        #endregion
        #region Tradefairtypes

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayTradeFairsTypes()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.TradeFairsTypes(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.TradeFairsTypes(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            //    TestClass t = new TestClass();
            ////    objTradeFairsTypes.listTradeFairsType = t.DisplayTradeFairsTypes();
            //    return View(t.DisplayTradeFairsTypes());
        }
        private GeneralMaster.TradeFairsTypes TradeFairsTypes(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayTradeFairsTypes(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchTradeFairsTypesByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchTradeFairsTypes(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddTradeFairsTypes()
        {
            return View(this.ATradeFairsTypes());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.TradeFairsTypes ATradeFairsTypes()
        {
            TestClass t = new TestClass();
            return t.ATradeFairsTypes();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddTradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ATradeFairsTypes(model);
                return RedirectToAction("DisplayTradeFairsTypes");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.TradeFairsTypes ATradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            TestClass t = new TestClass();
            return t.ATradeFairsTypes(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditTradeFairsTypes(int pid)
        {
            return View(this.ETradeFairsTypes(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.TradeFairsTypes ETradeFairsTypes(int pid)
        {
            TestClass t = new TestClass();
            return t.ETradeFairsTypes(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditTradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ETradeFairsTypes(model);
                return RedirectToAction("DisplayTradeFairsTypes");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.TradeFairsTypes ETradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            TestClass t = new TestClass();
            return t.ETradeFairsTypes(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteTradeFairsTypes(int pid)
        {
            this.DTradeFairsTypes(pid);
            return RedirectToAction("DisplayTradeFairsTypes");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.TradeFairsTypes DTradeFairsTypes(int pid)
        {
            TestClass t = new TestClass();
            return t.DTradeFairsTypes(pid);
        }

        #endregion
        #region Facility
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayFacility()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Facility(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Facility(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            //    TestClass t = new TestClass();
            ////    objFacility.listFacility = t.DisplayFacility();
            //    return View(t.DisplayFacility());
        }
        private GeneralMaster.Facility Facility(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayFacility(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchFacilityByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchFacility(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddFacility()
        {
            return View(this.AFacility());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Facility AFacility()
        {
            TestClass t = new TestClass();
            return t.AFacility();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddFacility(GeneralMaster.Facility model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AFacility(model);
                return RedirectToAction("DisplayFacility");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Facility AFacility(GeneralMaster.Facility model)
        {
            TestClass t = new TestClass();
            return t.AFacility(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditFacility(int pid)
        {
            return View(this.EFacility(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Facility EFacility(int pid)
        {
            TestClass t = new TestClass();
            return t.EFacility(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditFacility(GeneralMaster.Facility model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EFacility(model);
                return RedirectToAction("DisplayFacility");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Facility EFacility(GeneralMaster.Facility model)
        {
            TestClass t = new TestClass();
            return t.EFacility(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteFacility(int pid)
        {
            this.DFacility(pid);
            return RedirectToAction("DisplayFacility");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Facility DFacility(int pid)
        {
            TestClass t = new TestClass();
            return t.DFacility(pid);
        }
        #endregion
        #region FinancialYear
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayFinancialYear()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.FinancialYear(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.FinancialYear(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            //   TestClass t = new TestClass();
            ////   objFinancialYear.listFinancialYear = t.DisplayFinancialYear();
            //   return View(t.DisplayFinancialYear());
        }
        private GeneralMaster.FinancialYear FinancialYear(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayFinancialYear(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchFinancialYearByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchFinancialYear(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddFinancialYear()
        {
            return View(this.AFinancialYear());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.FinancialYear AFinancialYear()
        {
            TestClass t = new TestClass();
            return t.AFinancialYear();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddFinancialYear(GeneralMaster.FinancialYear model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AFinancialYear(model);
                return RedirectToAction("DisplayFinancialYear");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.FinancialYear AFinancialYear(GeneralMaster.FinancialYear model)
        {
            TestClass t = new TestClass();
            return t.AFinancialYear(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditFinancialYear(int pid)
        {
            return View(this.EFinancialYear(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.FinancialYear EFinancialYear(int pid)
        {
            TestClass t = new TestClass();
            return t.EFinancialYear(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditFinancialYear(GeneralMaster.FinancialYear model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EFinancialYear(model);
                return RedirectToAction("DisplayFinancialYear");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.FinancialYear EFinancialYear(GeneralMaster.FinancialYear model)
        {
            TestClass t = new TestClass();
            return t.EFinancialYear(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteFinancialYear(int pid)
        {
            this.DFinancialYear(pid);
            return RedirectToAction("DisplayFinancialYear");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.FinancialYear DFinancialYear(int pid)
        {
            TestClass t = new TestClass();
            return t.DFinancialYear(pid);
        }
        #endregion
        #region HolidayDuration

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayHolidayDuration()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.HolidayDuration(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.HolidayDuration(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            //   TestClass t = new TestClass();
            ////   objHolidayDuration.listHolidayDuration = t.DisplayHolidayDuration();
            //   return View(t.DisplayHolidayDuration());
        }

        private GeneralMaster.HolidayDuration HolidayDuration(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayHolidayDuration(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchHolidayDurationByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchHolidayDuration(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddHolidayDuration()
        {
            return View(this.AHolidayDuration());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.HolidayDuration AHolidayDuration()
        {
            TestClass t = new TestClass();
            return t.AHolidayDuration();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AHolidayDuration(model);
                return RedirectToAction("DisplayHolidayDuration");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.HolidayDuration AHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            TestClass t = new TestClass();
            return t.AHolidayDuration(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditHolidayDuration(int pid)
        {
            return View(this.EHolidayDuration(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.HolidayDuration EHolidayDuration(int pid)
        {
            TestClass t = new TestClass();
            return t.EHolidayDuration(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EHolidayDuration(model);
                return RedirectToAction("DisplayHolidayDuration");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.HolidayDuration EHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            TestClass t = new TestClass();
            return t.EHolidayDuration(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteHolidayDuration(int pid)
        {
            this.DHolidayDuration(pid);
            return RedirectToAction("DisplayHolidayDuration");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.HolidayDuration DHolidayDuration(int pid)
        {
            TestClass t = new TestClass();
            return t.DHolidayDuration(pid);
        }

        #endregion
        #region HolidayType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayHolidayType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.HolidayType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.HolidayType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            //    TestClass t = new TestClass();
            ////    objHolidayType.listHolidayType = t.DisplayHolidayType();
            //    return View(t.DisplayHolidayType());
        }
        private GeneralMaster.HolidayType HolidayType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayHolidayType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchHolidayTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchHolidayType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddHolidayType()
        {
            return View(this.AHolidayType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.HolidayType AHolidayType()
        {
            TestClass t = new TestClass();
            return t.AHolidayType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddHolidayType(GeneralMaster.HolidayType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AHolidayType(model);
                return RedirectToAction("DisplayHolidayType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.HolidayType AHolidayType(GeneralMaster.HolidayType model)
        {
            TestClass t = new TestClass();
            return t.AHolidayType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditHolidayType(int pid)
        {
            return View(this.EHolidayType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.HolidayType EHolidayType(int pid)
        {
            TestClass t = new TestClass();
            return t.EHolidayType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditHolidayType(GeneralMaster.HolidayType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EHolidayType(model);
                return RedirectToAction("DisplayHolidayType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.HolidayType EHolidayType(GeneralMaster.HolidayType model)
        {
            TestClass t = new TestClass();
            return t.EHolidayType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteHolidayType(int pid)
        {
            this.DHolidayType(pid);
            return RedirectToAction("DisplayHolidayType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.HolidayType DHolidayType(int pid)
        {
            TestClass t = new TestClass();
            return t.DHolidayType(pid);
        }
        #endregion
        #region HotelStandard

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayHotelStandard()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.HotelStandard(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.HotelStandard(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            //   TestClass t = new TestClass();
            ////   objHotelStandard.listHotelStandard = t.DisplayHotelStandard();
            //   return View(t.DisplayHotelStandard());
        }

        private GeneralMaster.HotelStandard HotelStandard(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayHotelStandard(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchHotelStandardByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchHotelStandard(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddHotelStandard()
        {
            return View(this.AHotelStandard());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.HotelStandard AHotelStandard()
        {
            TestClass t = new TestClass();
            return t.AHotelStandard();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddHotelStandard(GeneralMaster.HotelStandard model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AHotelStandard(model);
                return RedirectToAction("DisplayHotelStandard");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.HotelStandard AHotelStandard(GeneralMaster.HotelStandard model)
        {
            TestClass t = new TestClass();
            return t.AHotelStandard(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditHotelStandard(int pid)
        {
            return View(this.EHotelStandard(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.HotelStandard EHotelStandard(int pid)
        {
            TestClass t = new TestClass();
            return t.EHotelStandard(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditHotelStandard(GeneralMaster.HotelStandard model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EHotelStandard(model);
                return RedirectToAction("DisplayHotelStandard");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.HotelStandard EHotelStandard(GeneralMaster.HotelStandard model)
        {
            TestClass t = new TestClass();
            return t.ERHotelStandard(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteHotelStandard(int pid)
        {
            this.DHotelStandard(pid);
            return RedirectToAction("DisplayHotelStandard");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.HotelStandard DHotelStandard(int pid)
        {
            TestClass t = new TestClass();
            return t.DHotelStandard(pid);
        }
        #endregion
        #region HotelChain
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayHotelChain()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.HotelChain(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.HotelChain(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            //    TestClass t = new TestClass();
            ////    objHotelChain.listHotelChain = t.DisplayHotelChain();
            //    return View(t.DisplayHotelChain());
        }

        private GeneralMaster.HotelChain HotelChain(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayHotelChain(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchHotelChainByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchHotelChain(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddHotelChain()
        {
            return View(this.AHotelChain());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.HotelChain AHotelChain()
        {
            TestClass t = new TestClass();
            return t.AHotelChain();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddHotelChain(GeneralMaster.HotelChain model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AHotelChain(model);
                return RedirectToAction("DisplayHotelChain");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.HotelChain AHotelChain(GeneralMaster.HotelChain model)
        {
            TestClass t = new TestClass();
            return t.AHotelChain(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditHotelChain(int pid)
        {
            return View(this.EHotelChain(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.HotelChain EHotelChain(int pid)
        {
            TestClass t = new TestClass();
            return t.EHotelChain(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditHotelChain(GeneralMaster.HotelChain model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EHotelChain(model);
                return RedirectToAction("DisplayHotelChain");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.HotelChain EHotelChain(GeneralMaster.HotelChain model)
        {
            TestClass t = new TestClass();
            return t.EHotelChain(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteHotelChain(int pid)
        {
            this.DHotelChain(pid);
            return RedirectToAction("DisplayHotelChain");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.HotelChain DHotelChain(int pid)
        {
            TestClass t = new TestClass();
            return t.DHotelChain(pid);
        }

        #endregion
        #region InspectionCriteria
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayInspectionCriteria()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.InspectionCriteria(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.InspectionCriteria(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            //    TestClass t = new TestClass();
            ////    objInspectionCriteria.listInspectionCriteria = t.DisplayInspectionCriteria();
            //    return View(t.DisplayInspectionCriteria());
        }

        private GeneralMaster.InspectionCriteria InspectionCriteria(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayInspectionCriteria(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchInspectionCriteriaByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchInspectionCriteria(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddInspectionCriteria()
        {
            return View(this.AInspectionCriteria());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.InspectionCriteria AInspectionCriteria()
        {
            TestClass t = new TestClass();
            return t.AInspectionCriteria();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AInspectionCriteria(model);
                return RedirectToAction("DisplayInspectionCriteria");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.InspectionCriteria AInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            TestClass t = new TestClass();
            return t.AInspectionCriteria(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditInspectionCriteria(int pid)
        {
            return View(this.EInspectionCriteria(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.InspectionCriteria EInspectionCriteria(int pid)
        {
            TestClass t = new TestClass();
            return t.EInspectionCriteria(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EInspectionCriteria(model);
                return RedirectToAction("DisplayInspectionCriteria");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.InspectionCriteria EInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            TestClass t = new TestClass();
            return t.EInspectionCriteria(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteInspectionCriteria(int pid)
        {
            this.DInspectionCriteria(pid);
            return RedirectToAction("DisplayInspectionCriteria");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.InspectionCriteria DInspectionCriteria(int pid)
        {
            TestClass t = new TestClass();
            return t.DInspectionCriteria(pid);
        }
        #endregion
        #region Language
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayLanguage()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Language(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Language(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            //    TestClass t = new TestClass();
            ////    objLanguage.listLanguage = t.DisplayLanguage();
            //    return View(t.DisplayLanguage());
        }
        private GeneralMaster.Language Language(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayLanguage(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchLanguageByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchLanguage(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddLanguage()
        {
            return View(this.ALanguage());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Language ALanguage()
        {
            TestClass t = new TestClass();
            return t.ALanguage();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddLanguage(GeneralMaster.Language model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ALanguage(model);
                return RedirectToAction("DisplayLanguage");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Language ALanguage(GeneralMaster.Language model)
        {
            TestClass t = new TestClass();
            return t.ALanguage(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditLanguage(int pid)
        {
            return View(this.ELanguage(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Language ELanguage(int pid)
        {
            TestClass t = new TestClass();
            return t.ELanguage(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditLanguage(GeneralMaster.Language model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ELanguage(model);
                return RedirectToAction("DisplayLanguage");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Language ELanguage(GeneralMaster.Language model)
        {
            TestClass t = new TestClass();
            return t.ELanguage(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteLanguage(int pid)
        {
            this.DLanguage(pid);
            return RedirectToAction("DisplayLanguage");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Language DLanguage(int pid)
        {
            TestClass t = new TestClass();
            return t.DLanguage(pid);
        }

        #endregion
        #region Market
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]

        public ActionResult DisplayMarket()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Market(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Market(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Market objMarket = new GeneralMaster.Market();
            //    TestClass t = new TestClass();
            ////    objMarket.listMarket = t.DisplayMarket();
            //    return View(t.DisplayMarket());
        }
        private GeneralMaster.Market Market(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayMarket(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchMarketByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchMarket(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddMarket()
        {
            return View(this.AMarket());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Market AMarket()
        {
            TestClass t = new TestClass();
            return t.AMarket();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddMarket(GeneralMaster.Market model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AMarket(model);
                return RedirectToAction("DisplayMarket");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Market AMarket(GeneralMaster.Market model)
        {
            TestClass t = new TestClass();
            return t.AMarket(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditMarket(int pid)
        {
            return View(this.EMarket(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Market EMarket(int pid)
        {
            TestClass t = new TestClass();
            return t.EMarket(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditMarket(GeneralMaster.Market model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EMarket(model);
                return RedirectToAction("DisplayMarket");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Market EMarket(GeneralMaster.Market model)
        {
            TestClass t = new TestClass();
            return t.EMarket(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteMarket(int pid)
        {
            this.DMarket(pid);
            return RedirectToAction("DisplayMarket");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Market DMarket(int pid)
        {
            TestClass t = new TestClass();
            return t.DMarket(pid);
        }

        #endregion
        #region MealPlan
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayMealPlan()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.MealPlan(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.MealPlan(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            //    TestClass t = new TestClass();
            // //   objMealPlan.listMealPlan = t.DisplayMealPlan();
            //    return View(t.DisplayMealPlan());
        }

        private GeneralMaster.MealPlan MealPlan(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayMealPlan(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchMealPlanByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchMealPlan(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddMealPlan()
        {
            return View(this.AMealPlan());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.MealPlan AMealPlan()
        {
            TestClass t = new TestClass();
            return t.AMealPlan();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddMealPlan(GeneralMaster.MealPlan model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AMealPlan(model);
                return RedirectToAction("DisplayMealPlan");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.MealPlan AMealPlan(GeneralMaster.MealPlan model)
        {
            TestClass t = new TestClass();
            return t.AMealPlan(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditMealPlan(int pid)
        {
            return View(this.EMealPlan(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.MealPlan EMealPlan(int pid)
        {
            TestClass t = new TestClass();
            return t.EMealPlan(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditMealPlan(GeneralMaster.MealPlan model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EMealPlan(model);
                return RedirectToAction("DisplayMealPlan");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.MealPlan EMealPlan(GeneralMaster.MealPlan model)
        {
            TestClass t = new TestClass();
            return t.EMealPlan(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteMealPlan(int pid)
        {
            this.DMealPlan(pid);
            return RedirectToAction("DisplayMealPlan");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.MealPlan DMealPlan(int pid)
        {
            TestClass t = new TestClass();
            return t.DMealPlan(pid);
        }
        #endregion
        #region Nationality

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayNationality()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Nationality(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Nationality(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            //   TestClass t = new TestClass();
            ////   objNationality.listNationality = t.DisplayNationality();
            //   return View(t.DisplayNationality());
        }
        private GeneralMaster.Nationality Nationality(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayNationality(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchNationalityByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchNationality(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddNationality()
        {
            return View(this.ANationality());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Nationality ANationality()
        {
            TestClass t = new TestClass();
            return t.ANationality();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddNationality(GeneralMaster.Nationality model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ANationality(model);
                return RedirectToAction("DisplayNationality");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Nationality ANationality(GeneralMaster.Nationality model)
        {
            TestClass t = new TestClass();
            return t.ANationality(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditNationality(int pid)
        {
            return View(this.ENationality(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Nationality ENationality(int pid)
        {
            TestClass t = new TestClass();
            return t.ENationality(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditNationality(GeneralMaster.Nationality model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ENationality(model);
                return RedirectToAction("DisplayNationality");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Nationality ENationality(GeneralMaster.Nationality model)
        {
            TestClass t = new TestClass();
            return t.ENationality(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteNationality(int pid)
        {
            this.DNationality(pid);
            return RedirectToAction("DisplayNationality");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Nationality DNationality(int pid)
        {
            TestClass t = new TestClass();
            return t.DNationality(pid);
        }
        #endregion
        #region PaymentSchedules

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayPaymentSchedules()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.PaymentSchedules(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.PaymentSchedules(1, 0));
            }
            return View("Error");
            //// GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            // TestClass t = new TestClass();
            // //objPaymentSchedules.listPaymentSchedules = t.DisplayPaymentSchedules();
            // return View(t.DisplayPaymentSchedules());
        }
        private GeneralMaster.PaymentSchedules PaymentSchedules(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayPaymentSchedules(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchPaymentSchedulesByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchPaymentSchedules(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddPaymentSchedules()
        {
            return View(this.APaymentSchedules());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.PaymentSchedules APaymentSchedules()
        {
            TestClass t = new TestClass();
            return t.APaymentSchedules();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddPaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.APaymentSchedules(model);
                return RedirectToAction("DisplayPaymentSchedules");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.PaymentSchedules APaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            TestClass t = new TestClass();
            return t.APaymentSchedules(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditPaymentSchedules(int pid)
        {
            return View(this.EPaymentSchedules(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.PaymentSchedules EPaymentSchedules(int pid)
        {
            TestClass t = new TestClass();
            return t.EPaymentSchedules(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditPaymentSchedulese(GeneralMaster.PaymentSchedules model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EPaymentSchedules(model);
                return RedirectToAction("DisplayPaymentSchedules");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.PaymentSchedules EPaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            TestClass t = new TestClass();
            return t.EPaymentSchedules(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeletePaymentSchedules(int pid)
        {
            this.DPaymentSchedules(pid);
            return RedirectToAction("DisplayPaymentSchedules");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.PaymentSchedules DPaymentSchedules(int pid)
        {
            TestClass t = new TestClass();
            return t.DPaymentSchedules(pid);
        }
        #endregion
        #region PaymentType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayPaymentType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.PaymentType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.PaymentType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            //    TestClass t = new TestClass();
            // //   objPaymentType.listPaymentType = t.DisplayPaymentType();
            //    return View(t.DisplayPaymentType());
        }

        private GeneralMaster.PaymentType PaymentType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayPaymentType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchPaymentTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchPaymentType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddPaymentType()
        {
            return View(this.APaymentType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.PaymentType APaymentType()
        {
            TestClass t = new TestClass();
            return t.APaymentType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddPaymentType(GeneralMaster.PaymentType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.APaymentType(model);
                return RedirectToAction("DisplayPaymentType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.PaymentType APaymentType(GeneralMaster.PaymentType model)
        {
            TestClass t = new TestClass();
            return t.APaymentType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditPaymentType(int pid)
        {
            return View(this.EPaymentType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.PaymentType EPaymentType(int pid)
        {
            TestClass t = new TestClass();
            return t.EPaymentType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditPaymentType(GeneralMaster.PaymentType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EPaymentType(model);
                return RedirectToAction("DisplayPaymentType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.PaymentType EPaymentType(GeneralMaster.PaymentType model)
        {
            TestClass t = new TestClass();
            return t.EPaymentType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeletePaymentType(int pid)
        {
            this.DPaymentType(pid);
            return RedirectToAction("DisplayPaymentType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.PaymentType DPaymentType(int pid)
        {
            TestClass t = new TestClass();
            return t.DPaymentType(pid);
        }
        #endregion
        #region LogisticPickuptype
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayLogisticPickupType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.LogisticPickupType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.LogisticPickupType(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            //   TestClass t = new TestClass();
            ////   objLogisticPickupType.listLogisticPickupType = t.DisplayLogisticPickupType();
            //   return View(t.DisplayLogisticPickupType());
        }
        private GeneralMaster.LogisticPickupType LogisticPickupType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayLogisticPickuptype(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchLogisticPickupTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchLogisticPickupType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddLogisticPickupType()
        {
            return View(this.ALogisticPickupType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.LogisticPickupType ALogisticPickupType()
        {
            TestClass t = new TestClass();
            return t.ALogisticPickupType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddLogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ALogisticPickupType(model);
                return RedirectToAction("DisplayLogisticPickupType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.LogisticPickupType ALogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            TestClass t = new TestClass();
            return t.ALogisticPickupType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditLogisticPickupType(int pid)
        {
            return View(this.ELogisticPickupType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.LogisticPickupType ELogisticPickupType(int pid)
        {
            TestClass t = new TestClass();
            return t.ELogisticPickupType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditLogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ELogisticPickupType(model);
                return RedirectToAction("DisplayLogisticPickupType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.LogisticPickupType ELogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            TestClass t = new TestClass();
            return t.ELogisticPickupType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteLogisticPickupType(int pid)
        {
            this.DLogisticPickupType(pid);
            return RedirectToAction("DisplayLogisticPickupType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.LogisticPickupType DLogisticPickupType(int pid)
        {
            TestClass t = new TestClass();
            return t.DLogisticPickupType(pid);
        }
        #endregion
        #region CrmPriority
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayCrmPriority()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.CrmPriority(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.CrmPriority(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            //TestClass t = new TestClass();
            ////    objCrmPriority.listCrmPriority = t.DisplayCrmPriority();
            //return View(t.DisplayCrmPriority());
        }
        private GeneralMaster.CrmPriority CrmPriority(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayCrmPriority(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchCrmPriorityByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchCrmPriority(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddCrmPriority()
        {
            return View(this.ACrmPriority());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.CrmPriority ACrmPriority()
        {
            TestClass t = new TestClass();
            return t.ACrmPriority();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddCrmPriority(GeneralMaster.CrmPriority model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ACrmPriority(model);
                return RedirectToAction("DisplayCrmPriority");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.CrmPriority ACrmPriority(GeneralMaster.CrmPriority model)
        {
            TestClass t = new TestClass();
            return t.ACrmPriority(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditCrmPriority(int pid)
        {
            return View(this.ECrmPriority(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.CrmPriority ECrmPriority(int pid)
        {
            TestClass t = new TestClass();
            return t.ECrmPriority(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditCrmPriority(GeneralMaster.CrmPriority model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ECrmPriority(model);
                return RedirectToAction("DisplayCrmPriority");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.CrmPriority ECrmPriority(GeneralMaster.CrmPriority model)
        {
            TestClass t = new TestClass();
            return t.ECrmPriority(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteCrmPriority(int pid)
        {
            this.DCrmPriority(pid);
            return RedirectToAction("DisplayCrmPriority");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.CrmPriority DCrmPriority(int pid)
        {
            TestClass t = new TestClass();
            return t.DCrmPriority(pid);
        }
        #endregion
        #region PropertyType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayPropertyType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.PropertyType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.PropertyType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            //TestClass t = new TestClass();
            ////    objPropertyType.listPropertyType = t.DisplayPropertyType();
            //return View(t.DisplayPropertyType());
        }
        private GeneralMaster.PropertyType PropertyType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayPropertyType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchPropertyTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchPropertyType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddPropertyType()
        {
            return View(this.APropertyType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.PropertyType APropertyType()
        {
            TestClass t = new TestClass();
            return t.APropertyType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddPropertyType(GeneralMaster.PropertyType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.APropertyType(model);
                return RedirectToAction("DisplayPropertyType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.PropertyType APropertyType(GeneralMaster.PropertyType model)
        {
            TestClass t = new TestClass();
            return t.APropertyType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditPropertyType(int pid)
        {
            return View(this.EPropertyType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.PropertyType EPropertyType(int pid)
        {
            TestClass t = new TestClass();
            return t.EPropertyType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditPropertyType(GeneralMaster.PropertyType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EPropertyType(model);
                return RedirectToAction("DisplayPropertyType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.PropertyType EPropertyType(GeneralMaster.PropertyType model)
        {
            TestClass t = new TestClass();
            return t.EPropertyType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeletePropertyType(int pid)
        {
            this.DPropertyType(pid);
            return RedirectToAction("DisplayPropertyType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.PropertyType DPropertyType(int pid)
        {
            TestClass t = new TestClass();
            return t.DPropertyType(pid);
        }

        #endregion
        #region Reason
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayReason()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Reason(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Reason(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            //TestClass t = new TestClass();
            ////    objReason.listReason = t.DisplayReason();
            //return View(t.DisplayReason());
        }
        private GeneralMaster.Reason Reason(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayReason(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchReasonByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchReason(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddReason()
        {
            return View(this.AReason());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Reason AReason()
        {
            TestClass t = new TestClass();
            return t.AReason();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddReason(GeneralMaster.Reason model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AReason(model);
                return RedirectToAction("DisplayReason");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Reason AReason(GeneralMaster.Reason model)
        {
            TestClass t = new TestClass();
            return t.AReason(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditReason(int pid)
        {
            return View(this.EReason(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Reason EReason(int pid)
        {
            TestClass t = new TestClass();
            return t.EReason(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditReason(GeneralMaster.Reason model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EReason(model);
                return RedirectToAction("DisplayReason");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Reason EReason(GeneralMaster.Reason model)
        {
            TestClass t = new TestClass();
            return t.EReason(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteReason(int pid)
        {
            this.DReason(pid);
            return RedirectToAction("DisplayReason");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Reason DReason(int pid)
        {
            TestClass t = new TestClass();
            return t.DReason(pid);
        }

        #endregion
        #region ReportingState
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayReportingState()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ReportingState(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ReportingState(1, 0));
            }
            return View("Error");
            //    GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            TestClass t = new TestClass();
            //    objReportingState.listReportingState = t.DisplayReportingState();
            //return View(t.DisplayReportingState());
        }
        private GeneralMaster.ReportingState ReportingState(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayReportingState(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchReportingStateByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchReportingState(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddReportingState()
        {
            return View(this.AReportingState());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ReportingState AReportingState()
        {
            TestClass t = new TestClass();
            return t.AReportingState();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddReportingState(GeneralMaster.ReportingState model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AReportingState(model);
                return RedirectToAction("DisplayReportingState");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ReportingState AReportingState(GeneralMaster.ReportingState model)
        {
            TestClass t = new TestClass();
            return t.AReportingState(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditReportingState(int pid)
        {
            return View(this.EReportingState(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ReportingState EReportingState(int pid)
        {
            TestClass t = new TestClass();
            return t.EReportingState(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditReportingState(GeneralMaster.ReportingState model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EReportingState(model);
                return RedirectToAction("DisplayReportingState");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ReportingState EReportingState(GeneralMaster.ReportingState model)
        {
            TestClass t = new TestClass();
            return t.EReportingState(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteReportingState(int pid)
        {
            this.DReportingState(pid);
            return RedirectToAction("DisplayReportingState");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ReportingState DReportingState(int pid)
        {
            TestClass t = new TestClass();
            return t.DReportingState(pid);
        }

        #endregion
        #region Season
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplaySeason()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Season(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Season(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Season objSeason = new GeneralMaster.Season();
            //TestClass t = new TestClass();
            ////    objSeason.listSeason = t.DisplaySeason();
            //return View(t.DisplaySeason());
        }
        private GeneralMaster.Season Season(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplaySeason(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchSeasonByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchSeason(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddSeason()
        {
            return View(this.ASeason());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Season ASeason()
        {
            TestClass t = new TestClass();
            return t.ASeason();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddSeason(GeneralMaster.Season model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ASeason(model);
                return RedirectToAction("DisplaySeason");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Season ASeason(GeneralMaster.Season model)
        {
            TestClass t = new TestClass();
            return t.ASeason(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditSeason(int pid)
        {
            return View(this.ESeason(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Season ESeason(int pid)
        {
            TestClass t = new TestClass();
            return t.ESeason(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditSeason(GeneralMaster.Season model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ESeason(model);
                return RedirectToAction("DisplaySeason");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Season ESeason(GeneralMaster.Season model)
        {
            TestClass t = new TestClass();
            return t.ESeason(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteSeason(int pid)
        {
            this.DSeason(pid);
            return RedirectToAction("DisplaySeason");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Season DSeason(int pid)
        {
            TestClass t = new TestClass();
            return t.DSeason(pid);
        }

        #endregion
        #region Source
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplaySource()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Source(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Source(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Source objSource = new GeneralMaster.Source();
            //TestClass t = new TestClass();
            ////    objSource.listSource = t.DisplaySource();
            //return View(t.DisplaySource());
        }
        private GeneralMaster.Source Source(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplaySource(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchSourceByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchSource(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddSource()
        {
            return View(this.ASource());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Source ASource()
        {
            TestClass t = new TestClass();
            return t.ASource();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddSource(GeneralMaster.Source model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ASource(model);
                return RedirectToAction("DisplaySource");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Source ASource(GeneralMaster.Source model)
        {
            TestClass t = new TestClass();
            return t.ASource(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditSource(int pid)
        {
            return View(this.ESource(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Source ESource(int pid)
        {
            TestClass t = new TestClass();
            return t.ESource(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditSource(GeneralMaster.Source model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ESource(model);
                return RedirectToAction("DisplaySource");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Source ESource(GeneralMaster.Source model)
        {
            TestClass t = new TestClass();
            return t.ESource(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteSource(int pid)
        {
            this.DSource(pid);
            return RedirectToAction("DisplaySource");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Source DSource(int pid)
        {
            TestClass t = new TestClass();
            return t.DSource(pid);
        }

        #endregion
        #region Status
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayStatus()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Status(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Status(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Status objStatus = new GeneralMaster.Status();
            //TestClass t = new TestClass();
            ////    objStatus.listStatus = t.DisplayStatus();
            //return View(t.DisplayStatus());
        }
        private GeneralMaster.Status Status(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayStatus(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchStatusByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchStatus(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddStatus()
        {
            return View(this.AStatus());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Status AStatus()
        {
            TestClass t = new TestClass();
            return t.AStatus();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddStatus(GeneralMaster.Status model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AStatus(model);
                return RedirectToAction("DisplayStatus");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Status AStatus(GeneralMaster.Status model)
        {
            TestClass t = new TestClass();
            return t.AStatus(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditStatus(int pid)
        {
            return View(this.EStatus(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Status EStatus(int pid)
        {
            TestClass t = new TestClass();
            return t.EStatus(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditStatus(GeneralMaster.Status model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EStatus(model);
                return RedirectToAction("DisplayStatus");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Status EStatus(GeneralMaster.Status model)
        {
            TestClass t = new TestClass();
            return t.EStatus(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteStatus(int pid)
        {
            this.DStatus(pid);
            return RedirectToAction("DisplayStatus");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Status DStatus(int pid)
        {
            TestClass t = new TestClass();
            return t.DStatus(pid);
        }
        #endregion
        #region Supplement
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplaySupplement()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Supplement(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Supplement(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            //TestClass t = new TestClass();
            ////    objSupplement.listSupplement = t.DisplaySupplement();
            //return View(t.DisplaySupplement());
        }
        private GeneralMaster.Supplement Supplement(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplaySupplement(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchSupplementByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchSupplement(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddSupplement()
        {
            return View(this.ASupplement());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Supplement ASupplement()
        {
            TestClass t = new TestClass();
            return t.ASupplement();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddSupplement(GeneralMaster.Supplement model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ASupplement(model);
                return RedirectToAction("DisplaySupplement");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Supplement ASupplement(GeneralMaster.Supplement model)
        {
            TestClass t = new TestClass();
            return t.ASupplement(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditSupplement(int pid)
        {
            return View(this.ESupplement(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Supplement ESupplement(int pid)
        {
            TestClass t = new TestClass();
            return t.ESupplement(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditSupplement(GeneralMaster.Supplement model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ESupplement(model);
                return RedirectToAction("DisplaySupplement");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Supplement ESupplement(GeneralMaster.Supplement model)
        {
            TestClass t = new TestClass();
            return t.ESupplement(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteSupplement(int pid)
        {
            this.DSupplement(pid);
            return RedirectToAction("DisplaySupplement");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Supplement DSupplement(int pid)
        {
            TestClass t = new TestClass();
            return t.DSupplement(pid);
        }

        #endregion
        #region SupplementType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplaySupplementType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.SupplementType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.SupplementType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            //TestClass t = new TestClass();
            ////    objSupplementType.listSupplementType = t.DisplaySupplementType();
            //return View(t.DisplaySupplementType());
        }
        private GeneralMaster.SupplementType SupplementType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplaySupplementType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchSupplementTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchSupplementType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddSupplementType()
        {
            return View(this.ASupplementType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.SupplementType ASupplementType()
        {
            TestClass t = new TestClass();
            return t.ASupplementType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddSupplementType(GeneralMaster.SupplementType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ASupplementType(model);
                return RedirectToAction("DisplaySupplementType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Supplement ASupplementType(GeneralMaster.SupplementType model)
        {
            TestClass t = new TestClass();
            return t.ASupplementType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditSupplementType(int pid)
        {
            return View(this.ESupplementType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.SupplementType ESupplementType(int pid)
        {
            TestClass t = new TestClass();
            return t.ESupplementType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditSupplementType(GeneralMaster.SupplementType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ESupplementType(model);
                return RedirectToAction("DisplaySupplementType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.SupplementType ESupplementType(GeneralMaster.SupplementType model)
        {
            TestClass t = new TestClass();
            return t.ESupplementType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteSupplementType(int pid)
        {
            this.DSupplementType(pid);
            return RedirectToAction("DisplaySupplementType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.SupplementType DSupplementType(int pid)
        {
            TestClass t = new TestClass();
            return t.DSupplementType(pid);
        }

        #endregion
        #region Tax
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayTax()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Tax(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Tax(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            //TestClass t = new TestClass();
            ////    objTax.listTax = t.DisplayTax();
            //return View(t.DisplayTax());
        }
        private GeneralMaster.Tax Tax(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayTax(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchTaxByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchTax(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddTax()
        {
            return View(this.ATax());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Tax ATax()
        {
            TestClass t = new TestClass();
            return t.ATax();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddTax(GeneralMaster.Tax model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ATax(model);
                return RedirectToAction("DisplayTax");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Tax ATax(GeneralMaster.Tax model)
        {
            TestClass t = new TestClass();
            return t.ATax(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditTax(int pid)
        {
            return View(this.ETax(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Tax ETax(int pid)
        {
            TestClass t = new TestClass();
            return t.ETax(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditTax(GeneralMaster.Tax model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ETax(model);
                return RedirectToAction("DisplayTax");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Tax ETax(GeneralMaster.Tax model)
        {
            TestClass t = new TestClass();
            return t.ETax(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteTax(int pid)
        {
            this.DTax(pid);
            return RedirectToAction("DisplayTax");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Tax DTax(int pid)
        {
            TestClass t = new TestClass();
            return t.DTax(pid);
        }

        #endregion
        #region Title
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayTitle()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Title(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Title(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Title objTitle = new GeneralMaster.Title();
            //TestClass t = new TestClass();
            ////    objTitle.listTitle = t.DisplayTitle();
            //return View(t.DisplayTitle());
        }
        private GeneralMaster.Title Title(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayTitle(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchTitleByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchTitle(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddTitle()
        {
            return View(this.ATitle());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Title ATitle()
        {
            TestClass t = new TestClass();
            return t.ATitle();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddTitle(GeneralMaster.Title model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ATitle(model);
                return RedirectToAction("DisplayTitle");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Title ATitle(GeneralMaster.Title model)
        {
            TestClass t = new TestClass();
            return t.ATitle(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditTitle(int pid)
        {
            return View(this.ETitle(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Title ETitle(int pid)
        {
            TestClass t = new TestClass();
            return t.ETitle(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditTitle(GeneralMaster.Title model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ETitle(model);
                return RedirectToAction("DisplayTitle");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Title ETitle(GeneralMaster.Title model)
        {
            TestClass t = new TestClass();
            return t.ETitle(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteTitle(int pid)
        {
            this.DTitle(pid);
            return RedirectToAction("DisplayTitle");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Title DTitle(int pid)
        {
            TestClass t = new TestClass();
            return t.DTitle(pid);
        }

        #endregion
        #region Company
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayCompany()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Company(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Company(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Company objCompany = new GeneralMaster.Company();
            //TestClass t = new TestClass();
            ////    objCompany.listCompany = t.DisplayCompany();
            //return View(t.DisplayCompany());
        }
        private GeneralMaster.Company Company(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayCompany(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchCompanyyString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchCompany(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddCompany()
        {
            return View(this.ACompany());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Company ACompany()
        {
            TestClass t = new TestClass();
            return t.ACompany();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddCompany(GeneralMaster.Company model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ACompany(model);
                return RedirectToAction("DisplayCompany");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Company ACompany(GeneralMaster.Company model)
        {
            TestClass t = new TestClass();
            return t.ACompany(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditCompany(int pid)
        {
            return View(this.ECompany(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Company ECompany(int pid)
        {
            TestClass t = new TestClass();
            return t.ECompany(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditCompany(GeneralMaster.Company model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ECompany(model);
                return RedirectToAction("DisplayCompany");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Company ECompany(GeneralMaster.Company model)
        {
            TestClass t = new TestClass();
            return t.ECompany(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteCompany(int pid)
        {
            this.DCompany(pid);
            return RedirectToAction("DisplayCompany");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Company DCompany(int pid)
        {
            TestClass t = new TestClass();
            return t.DCompany(pid);
        }
        #endregion
        #region Department
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayDepartment()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Department(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Department(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            //TestClass t = new TestClass();
            ////    objDepartment.listDepartment = t.DisplayDepartment();
            //return View(t.DisplayDepartment());
        }

        private GeneralMaster.Department Department(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayDepartment(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchDepartmentByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchDepartment(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddDepartment()
        {
            return View(this.ADepartment());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Department ADepartment()
        {
            TestClass t = new TestClass();
            return t.ADepartment();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddDepartment(GeneralMaster.Department model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ADepartment(model);
                return RedirectToAction("DisplayDepartment");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Department ADepartment(GeneralMaster.Department model)
        {
            TestClass t = new TestClass();
            return t.ADepartment(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditDepartment(int pid)
        {
            return View(this.EDepartment(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Department EDepartment(int pid)
        {
            TestClass t = new TestClass();
            return t.EDepartment(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditDepartment(GeneralMaster.Department model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EDepartment(model);
                return RedirectToAction("DisplayDepartment");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Department EDepartment(GeneralMaster.Department model)
        {
            TestClass t = new TestClass();
            return t.EDepartment(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteDepartment(int pid)
        {
            this.DDepartment(pid);
            return RedirectToAction("DisplayDepartment");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Department DDepartment(int pid)
        {
            TestClass t = new TestClass();
            return t.DDepartment(pid);
        }

        #endregion
        #region Designation
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayDesignation()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Designation(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Designation(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            //TestClass t = new TestClass();
            ////    objDesignation.listDesignation = t.DisplayDesignation();
            //return View(t.DisplayDesignation());
        }

        private GeneralMaster.Designation Designation(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayDesignation(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchDesignationByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchDesignation(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddDesignation()
        {
            return View(this.ADesignation());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Designation ADesignation()
        {
            TestClass t = new TestClass();
            return t.ADesignation();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddDesignation(GeneralMaster.Designation model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ADesignation(model);
                return RedirectToAction("DisplayDesignation");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Designation ADesignation(GeneralMaster.Designation model)
        {
            TestClass t = new TestClass();
            return t.ADesignation(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditDesignation(int pid)
        {
            return View(this.EDesignation(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Designation EDesignation(int pid)
        {
            TestClass t = new TestClass();
            return t.EDesignation(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditDesignation(GeneralMaster.Designation model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EDesignation(model);
                return RedirectToAction("DisplayDesignation");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Designation EDesignation(GeneralMaster.Designation model)
        {
            TestClass t = new TestClass();
            return t.EDesignation(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteDesignation(int pid)
        {
            this.DDesignation(pid);
            return RedirectToAction("DisplayDesignation");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Designation DDesignation(int pid)
        {
            TestClass t = new TestClass();
            return t.DDesignation(pid);
        }

        #endregion
        #region DMCSystemConfiguration
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayDMCSystemConfiguration()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.DMCSystemConfiguration(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.DMCSystemConfiguration(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            //TestClass t = new TestClass();
            ////   objDMCSystemConfiguration.listDMCSystemConfiguration = t.DisplayDMCSystemConfiguration();
            //return View(t.DisplayDMCSystemConfiguration());
        }
        private GeneralMaster.DMCSystemConfiguration DMCSystemConfiguration(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayDMCSystemConfiguration(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchDMCSystemConfigurationByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchDMCSystemConfiguration(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddDMCSystemConfiguration()
        {
            return View(this.ADMCSystemConfiguration());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration()
        {
            TestClass t = new TestClass();
            return t.ADMCSystemConfiguration();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddDMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ADMCSystemConfiguration(model);
                return RedirectToAction("DisplayDMCSystemConfiguration");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            TestClass t = new TestClass();
            return t.ADMCSystemConfiguration(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditDMCSystemConfiguration(int pid)
        {
            return View(this.EDMCSystemConfiguration(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(int pid)
        {
            TestClass t = new TestClass();
            return t.EDMCSystemConfiguration(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditDMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EDMCSystemConfiguration(model);
                return RedirectToAction("DisplayDMCSystemConfiguration");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            TestClass t = new TestClass();
            return t.EDMCSystemConfiguration(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteDMCSystemConfiguration(int pid)
        {
            this.DDMCSystemConfiguration(pid);
            return RedirectToAction("DisplayDMCSystemConfiguration");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.DMCSystemConfiguration DDMCSystemConfiguration(int pid)
        {
            TestClass t = new TestClass();
            return t.DDMCSystemConfiguration(pid);
        }



        #endregion
        #region ImageLibrary
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayImageLibrary()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ImageLibrary(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ImageLibrary(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            //TestClass t = new TestClass();
            ////    objImageLibrary.listImageLibrary = t.DisplayImageLibrary();
            //return View(t.DisplayImageLibrary());
        }

        private GeneralMaster.ImageLibrary ImageLibrary(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayImageLibrary(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchImageLibraryByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchImageLibrary(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddImageLibrary()
        {
            return View(this.AImageLibrary());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ImageLibrary AImageLibrary()
        {
            TestClass t = new TestClass();
            return t.AImageLibrary();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddImageLibrary(GeneralMaster.ImageLibrary model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AImageLibrary(model);
                return RedirectToAction("DisplayImageLibrary");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ImageLibrary AImageLibrary(GeneralMaster.ImageLibrary model)
        {
            TestClass t = new TestClass();
            return t.AImageLibrary(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditImageLibrary(int pid)
        {
            return View(this.EImageLibrary(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ImageLibrary EImageLibrary(int pid)
        {
            TestClass t = new TestClass();
            return t.EImageLibrary(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditImageLibrary(GeneralMaster.ImageLibrary model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EImageLibrary(model);
                return RedirectToAction("DisplayImageLibrary");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ImageLibrary EImageLibrary(GeneralMaster.ImageLibrary model)
        {
            TestClass t = new TestClass();
            return t.EImageLibrary(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteImageLibrary(int pid)
        {
            this.DImageLibrary(pid);
            return RedirectToAction("DisplayImageLibrary");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ImageLibrary DImageLibrary(int pid)
        {
            TestClass t = new TestClass();
            return t.DImageLibrary(pid);
        }

        #endregion
        #region Depot
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayDepot()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Depot(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Depot(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            //TestClass t = new TestClass();
            ////    objDepot.listDepot = t.DisplayDepot();
            //return View(t.DisplayDepot());
        }
        private GeneralMaster.Depot Depot(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayDepot(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchDepotByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchRDepot(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddDepot()
        {
            return View(this.ADepot());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Depot ADepot()
        {
            TestClass t = new TestClass();
            return t.ADepot();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddDepot(GeneralMaster.Depot model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ADepot(model);
                return RedirectToAction("DisplayDepot");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Depot ADepot(GeneralMaster.Depot model)
        {
            TestClass t = new TestClass();
            return t.ADepot(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditDepot(int pid)
        {
            return View(this.EDepot(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Depot EDepot(int pid)
        {
            TestClass t = new TestClass();
            return t.EDepot(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditDepot(GeneralMaster.Depot model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EDepot(model);
                return RedirectToAction("DisplayDepot");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Depot EDepot(GeneralMaster.Depot model)
        {
            TestClass t = new TestClass();
            return t.EDepot(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteDepot(int pid)
        {
            this.DDepot(pid);
            return RedirectToAction("DisplayDepot");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Depot DDepot(int pid)
        {
            TestClass t = new TestClass();
            return t.DDepot(pid);
        }

        #endregion
        #region ContractingGroup

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayContractingGroup()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ContractingGroup(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ContractingGroup(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            //TestClass t = new TestClass();
            ////    objContractingGroup.listContractingGroup = t.DisplayContractingGroup();
            //return View(t.DisplayContractingGroup());
        }
        private GeneralMaster.ContractingGroup ContractingGroup(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayContractingGroup(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchContractingGroupByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchContractingGroup(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddContractingGroup()
        {
            return View(this.AContractingGroup());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ContractingGroup AContractingGroup()
        {
            TestClass t = new TestClass();
            return t.AContractingGroup();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddContractingGroup(GeneralMaster.ContractingGroup model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AContractingGroup(model);
                return RedirectToAction("DisplayContractingGroup");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ContractingGroup AContractingGroup(GeneralMaster.ContractingGroup model)
        {
            TestClass t = new TestClass();
            return t.AContractingGroup(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditContractingGroup(int pid)
        {
            return View(this.EContractingGroup(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ContractingGroup EContractingGroup(int pid)
        {
            TestClass t = new TestClass();
            return t.ContractingGroup(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditContractingGroup(GeneralMaster.ContractingGroup model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EContractingGroup(model);
                return RedirectToAction("DisplayContractingGroup");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ContractingGroup EContractingGroup(GeneralMaster.ContractingGroup model)
        {
            TestClass t = new TestClass();
            return t.EContractingGroup(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteContractingGroup(int pid)
        {
            this.DContractingGroup(pid);
            return RedirectToAction("DisplayContractingGroup");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ContractingGroup DContractingGroup(int pid)
        {
            TestClass t = new TestClass();
            return t.DContractingGroup(pid);
        }
        #endregion
        #region Tbltariff

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayTblTariff()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.TblTariff(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.TblTariff(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            //TestClass t = new TestClass();
            ////    objTblTariff.listTblTariff = t.DisplayTblTariff();
            //return View(t.DisplayTblTariff());
        }
        private GeneralMaster.TblTariff TblTariff(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayTblTariff(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchTblTariffByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchTblTariff(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddTblTariff()
        {
            return View(this.ATblTariff());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.TblTariff ATblTariff()
        {
            TestClass t = new TestClass();
            return t.ATblTariff();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddTblTariff(GeneralMaster.TblTariff model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ATblTariff(model);
                return RedirectToAction("DisplayTblTariff");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.TblTariff ATblTariff(GeneralMaster.TblTariff model)
        {
            TestClass t = new TestClass();
            return t.ATblTariff(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditTblTariff(int pid)
        {
            return View(this.ETblTariff(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.TblTariff ETblTariff(int pid)
        {
            TestClass t = new TestClass();
            return t.ETblTariff(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditTblTariff(GeneralMaster.TblTariff model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ETblTariff(model);
                return RedirectToAction("DisplayTblTariff");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.TblTariff ETblTariff(GeneralMaster.TblTariff model)
        {
            TestClass t = new TestClass();
            return t.ETblTariff(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteTblTariff(int pid)
        {
            this.DTblTariff(pid);
            return RedirectToAction("DisplayTblTariff");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.TblTariff DTblTariff(int pid)
        {
            TestClass t = new TestClass();
            return t.DTblTariff(pid);
        }
        #endregion


        #region TblTariffMarkets

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayTblTariffMarkets()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.TblTariffMarkets(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.TblTariffMarkets(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            //TestClass t = new TestClass();
            ////    objTblTariff.listTblTariff = t.DisplayTblTariff();
            //return View(t.DisplayTblTariff());
        }
        private GeneralMaster.TblTariffMarkets TblTariffMarkets(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayTblTariffMarkets(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchTblTariffMarketsByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchTblTariffMarkets(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddTblTariffMarkets()
        {
            return View(this.ATblTariffMarkets());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.TblTariffMarkets ATblTariffMarkets()
        {
            TestClass t = new TestClass();
            return t.ATblTariffMarkets();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddTblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ATblTariffMarkets(model);
                return RedirectToAction("DisplayTblTariffMarkets");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.TblTariffMarkets ATblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            TestClass t = new TestClass();
            return t.ATblTariffMarkets(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditTblTariffMarkets(int pid)
        {
            return View(this.ETblTariffMarkets(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.TblTariffMarkets ETblTariffMarkets(int pid)
        {
            TestClass t = new TestClass();
            return t.ETblTariffMarkets(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditTblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ETblTariffMarkets(model);
                return RedirectToAction("DisplayTblTariffMarkets");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.TblTariffMarkets ETblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            TestClass t = new TestClass();
            return t.ETblTariffMarkets(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteTblTariffMarkets(int pid)
        {
            this.DTblTariffMarkets(pid);
            return RedirectToAction("DisplayTblTariffMarkets");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.TblTariffMarkets DTblTariffMarkets(int pid)
        {
            TestClass t = new TestClass();
            return t.DTblTariffMarkets(pid);
        }
        #endregion


        #region Client
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayClient()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Client(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Client(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Client objClient = new GeneralMaster.Client();
            //TestClass t = new TestClass();
            ////    objClient.listClient = t.DisplayClient();
            //return View(t.DisplayClient());
        }
        private GeneralMaster.Client Client(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayClient(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchClientByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchClient(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddClient()
        {
            return View(this.AClient());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Client AClient()
        {
            TestClass t = new TestClass();
            return t.AClient();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddClient(GeneralMaster.Client model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AClient(model);
                return RedirectToAction("DisplayClient");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Client AClient(GeneralMaster.Client model)
        {
            TestClass t = new TestClass();
            return t.AClient(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditClient(int pid)
        {
            return View(this.EClient(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Client EClient(int pid)
        {
            TestClass t = new TestClass();
            return t.EClient(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditClient(GeneralMaster.Client model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EClient(model);
                return RedirectToAction("DisplayClient");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Client EClient(GeneralMaster.Client model)
        {
            TestClass t = new TestClass();
            return t.EClient(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteClient(int pid)
        {
            this.DClient(pid);
            return RedirectToAction("DisplayClient");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Client DClient(int pid)
        {
            TestClass t = new TestClass();
            return t.DClient(pid);
        }

        #endregion
        #region Airline
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayAirline()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.Airline(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.Airline(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            //TestClass t = new TestClass();
            ////    objAirline.listAirline = t.DisplayAirline();
            //return View(t.DisplayAirline());
        }
        private GeneralMaster.Airline Airline(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayAirline(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchAirlineByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchAirline(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddAirline()
        {
            return View(this.AAirline());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.Airline AAirline()
        {
            TestClass t = new TestClass();
            return t.AAirline();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddAirline(GeneralMaster.Airline model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AAirline(model);
                return RedirectToAction("DisplayAirline");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.Airline AAirline(GeneralMaster.Airline model)
        {
            TestClass t = new TestClass();
            return t.AAirline(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditAirline(int pid)
        {
            return View(this.EAirline(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.Airline EAirline(int pid)
        {
            TestClass t = new TestClass();
            return t.EAirline(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditAirline(GeneralMaster.Airline model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EAirline(model);
                return RedirectToAction("DisplayAirline");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.Airline EAirline(GeneralMaster.Airline model)
        {
            TestClass t = new TestClass();
            return t.EAirline(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteAirline(int pid)
        {
            this.DAirline(pid);
            return RedirectToAction("DisplayAirline");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.Airline DAirline(int pid)
        {
            TestClass t = new TestClass();
            return t.DAirline(pid);
        }

        #endregion
        #region ResourceType
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayResourceType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ResourceType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ResourceType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            //TestClass t = new TestClass();
            ////    objResourceType.listResourceType = t.DisplayResourceType();
            //return View(t.DisplayResourceType());
        }
        private GeneralMaster.ResourceType ResourceType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayResourceType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchResourceTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchResourceType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddResourceType()
        {
            return View(this.AResourceType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ResourceType AResourceType()
        {
            TestClass t = new TestClass();
            return t.AResourceType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddResourceType(GeneralMaster.ResourceType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AResourceType(model);
                return RedirectToAction("DisplayResourceType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ResourceType AResourceType(GeneralMaster.ResourceType model)
        {
            TestClass t = new TestClass();
            return t.AResourceType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditResourceType(int pid)
        {
            return View(this.EResourceType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ResourceType EResourceType(int pid)
        {
            TestClass t = new TestClass();
            return t.EResourceType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditResourceType(GeneralMaster.ResourceType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EResourceType(model);
                return RedirectToAction("DisplayResourceType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ResourceType EResourceType(GeneralMaster.ResourceType model)
        {
            TestClass t = new TestClass();
            return t.EResourceType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteResourceType(int pid)
        {
            this.DResourceType(pid);
            return RedirectToAction("DisplayResourceType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ResourceType DResourceType(int pid)
        {
            TestClass t = new TestClass();
            return t.DResourceType(pid);
        }
        #endregion
        #region HumanResource
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayHumanResource()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.HumanResource(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.HumanResource(1, 0));
            }
            return View("Error");
            //// GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource();
            // TestClass t = new TestClass();
            // //objHumanResource.listHumanResource = t.DisplayHumanResource();
            // //GeneralMaster.HumanResource objHumanresource = t.DisplayHumanResource();

            // return View(t.DisplayHumanResource());
        }
        private GeneralMaster.HumanResource HumanResource(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayHumanResource(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchHumanResourceByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchHumanResource(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddHumanResource()
        {
            return View(this.AHumanResource());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.HumanResource AHumanResource()
        {
            TestClass t = new TestClass();
            return t.AHumanResource();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddHumanResource(GeneralMaster.HumanResource model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AHumanResource(model);
                return RedirectToAction("DisplayHumanResource");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.HumanResource AHumanResource(GeneralMaster.HumanResource model)
        {
            TestClass t = new TestClass();
            return t.AHumanResource(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditHumanResource(int pid)
        {
            return View(this.EHumanResource(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.HumanResource EHumanResource(int pid)
        {
            TestClass t = new TestClass();
            return t.EHumanResource(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditHumanResource(GeneralMaster.HumanResource model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EHumanResource(model);
                return RedirectToAction("DisplayHumanResource");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.HumanResource EHumanResource(GeneralMaster.HumanResource model)
        {
            TestClass t = new TestClass();
            return t.EHumanResource(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteHumanResource(int pid)
        {
            this.DHumanResource(pid);
            return RedirectToAction("DisplayHumanResource");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.HumanResource DHumanResource(int pid)
        {
            TestClass t = new TestClass();
            return t.DHumanResource(pid);
        }

        #endregion
        #region ResourceVehicleDtls

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayResourceVehicleDtls()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.ResourceVehicleDtls(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.ResourceVehicleDtls(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            //TestClass t = new TestClass();
            ////    objResourceVehicleDtls.listResourceVehicleDtls = t.DisplayResourceVehicleDtls();
            //return View(t.DisplayResourceVehicleDtls());
        }
        private GeneralMaster.ResourceVehicleDtls ResourceVehicleDtls(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayResourceVehicleDtls(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchResourceVehicleDtlsByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchResourceVehicleDtls(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddResourceVehicleDtls()
        {
            return View(this.AResourceVehicleDtls());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls()
        {
            TestClass t = new TestClass();
            return t.AResourceVehicleDtls();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.AResourceVehicleDtls(model);
                return RedirectToAction("DisplayResourceVehicleDtls");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            TestClass t = new TestClass();
            return t.AResourceVehicleDtls(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditResourceVehicleDtls(int pid)
        {
            return View(this.EResourceVehicleDtls(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(int pid)
        {
            TestClass t = new TestClass();
            return t.EResourceVehicleDtls(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.EResourceVehicleDtls(model);
                return RedirectToAction("DisplayResourceVehicleDtls");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            TestClass t = new TestClass();
            return t.EResourceVehicleDtls(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteResourceVehicleDtls(int pid)
        {
            this.DResourceVehicleDtls(pid);
            return RedirectToAction("DisplayResourceVehicleDtls");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.ResourceVehicleDtls DResourceVehicleDtls(int pid)
        {
            TestClass t = new TestClass();
            return t.DResourceVehicleDtls(pid);
        }
        #endregion
        #region LogisticVehicleType


        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayLogisticVehicleType()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.LogisticVehicleType(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.LogisticVehicleType(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            //TestClass t = new TestClass();
            ////    objLogisticVehicleType.listLogisticVehicleType = t.DisplayLogisticVehicleType();
            //return View(t.DisplayLogisticVehicleType());
        }
        private GeneralMaster.LogisticVehicleType LogisticVehicleType(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayLogisticVehicleType(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchLogisticVehicleTypeByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchLogisticVehicleType(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddLogisticVehicleType()
        {
            return View(this.ALogisticVehicleType());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.LogisticVehicleType ALogisticVehicleType()
        {
            TestClass t = new TestClass();
            return t.ALogisticVehicleType();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddLogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ALogisticVehicleType(model);
                return RedirectToAction("DisplayLogisticVehicleType");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.LogisticVehicleType ALogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            TestClass t = new TestClass();
            return t.ALogisticVehicleType(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditLogisticVehicleType(int pid)
        {
            return View(this.ELogisticVehicleType(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.LogisticVehicleType ELogisticVehicleType(int pid)
        {
            TestClass t = new TestClass();
            return t.ELogisticVehicleType(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditLogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ELogisticVehicleType(model);
                return RedirectToAction("DisplayLogisticVehicleType");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.LogisticVehicleType ELogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            TestClass t = new TestClass();
            return t.ELogisticVehicleType(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteLogisticVehicleType(int pid)
        {
            this.DLogisticVehicleType(pid);
            return RedirectToAction("DisplayLogisticVehicleType");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.LogisticVehicleType DLogisticVehicleType(int pid)
        {
            TestClass t = new TestClass();
            return t.DLogisticVehicleType(pid);
        }
        #endregion
        #region LogisticPickupArea

        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayLogisticPickupArea()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.LogisticPickupArea(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.LogisticPickupArea(1, 0));
            }
            return View("Error");
            ////   GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            //TestClass t = new TestClass();
            ////    objLogisticPickupArea.listLogisticPickupArea = t.DisplayLogisticPickupArea();
            //return View(t.DisplayLogisticPickupArea());
        }
        private GeneralMaster.LogisticPickupArea LogisticPickupArea(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayLogisticPickupArea(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchLogisticPickupAreaByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchLogisticPickupArea(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddLogisticPickupArea()
        {
            return View(this.ALogisticPickupArea());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.LogisticPickupArea ALogisticPickupArea()
        {
            TestClass t = new TestClass();
            return t.ALogisticPickupArea();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddLogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ALogisticPickupArea(model);
                return RedirectToAction("DisplayLogisticPickupArea");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.LogisticPickupArea ALogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            TestClass t = new TestClass();
            return t.ALogisticPickupArea(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditLogisticPickupArea(int pid)
        {
            return View(this.ELogisticPickupArea(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.LogisticPickupArea ELogisticPickupArea(int pid)
        {
            TestClass t = new TestClass();
            return t.ELogisticPickupArea(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditLogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ELogisticPickupArea(model);
                return RedirectToAction("DisplayLogisticPickupArea");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.LogisticPickupArea ELogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            TestClass t = new TestClass();
            return t.ELogisticPickupArea(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteLogisticPickupArea(int pid)
        {
            this.DLogisticPickupArea(pid);
            return RedirectToAction("DisplayLogisticPickupArea");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.LogisticPickupArea DLogisticPickupArea(int pid)
        {
            TestClass t = new TestClass();
            return t.DLogisticPickupArea(pid);
        }

        #endregion
        #region LogisticJourneyTimes
        [ValidateAntiForgeryTokenWrapper(HttpVerbs.Post)]
        public ActionResult DisplayLogisticJourneyTimes()
        {
            if (Request.HttpMethod == "POST")
            {
                int CurrPage = 0;
                int SearchPid = 0;
                if (ModelState.IsValid)
                {
                    CurrPage = Convert.ToInt32(Request["currentPageIndex"]);
                    SearchPid = Convert.ToInt32(Request["Pid"]);
                }
                else
                {
                    ModelState.AddModelError("Currpage", "Please check Currpage");
                    ModelState.AddModelError("SearchPid", "Please check SearchPid");
                }
                return View(this.LogisticJourneyTimes(CurrPage, SearchPid));
            }
            else if (Request.HttpMethod == "GET")
            {
                return View(this.LogisticJourneyTimes(1, 0));
            }
            return View("Error");
            ////    GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            //TestClass t = new TestClass();
            ////    objLogisticJourneyTimes.listLogisticJourneyTimes = t.DisplayLogisticJourneyTimes();
            //return View(t.DisplayLogisticJourneyTimes());
        }
        private GeneralMaster.LogisticJourneyTimes LogisticJourneyTimes(int currPage, int SearchPid)
        {
            TestClass t = new TestClass();
            return t.DisplayLogisticJourneyTimes(currPage, SearchPid);
        }

        [HttpPost]
        public JsonResult SearchLogisticJourneyTimesByString(string prefix)
        {
            TestClass t = new TestClass();
            return Json(t.SearchLogisticJourneyTimes(prefix), JsonRequestBehavior.AllowGet);
        }

        //GET ADDDISCOUNT
        [HttpGet]
        public ActionResult AddLogisticJourneyTimes()
        {
            return View(this.ALogisticJourneyTimes());
        }
        //GET ADDDISCOUNT
        private GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes()
        {
            TestClass t = new TestClass();
            return t.ALogisticJourneyTimes();
        }

        //POST ADDDISCOUNT
        [HttpPost]
        public ActionResult AddLogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ALogisticJourneyTimes(model);
                return RedirectToAction("DisplayLogisticJourneyTimes");
            }
            return View("Error");
        }
        //POST ADDDISCOUNT
        private GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            TestClass t = new TestClass();
            return t.ALogisticJourneyTimes(model);
        }


        //GET EDITDISCOUNT
        [HttpGet]
        public ActionResult EditLogisticJourneyTimes(int pid)
        {
            return View(this.ELogisticJourneyTimes(pid));
        }
        //GET EDITDISCOUNT
        private GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(int pid)
        {
            TestClass t = new TestClass();
            return t.ELogisticJourneyTimes(pid);
        }


        ////POST EDITDISCOUNT
        [HttpPost]
        public ActionResult EditLogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            if (!ModelState.IsValid)
            {
                //ModelState.AddModelError("DiscountName", "Please check DiscountName");
                //ModelState.AddModelError("Sequence", "Please check Sequence");
                //ModelState.AddModelError("Description", "Please check Description");
            }
            else
            {
                this.ELogisticJourneyTimes(model);
                return RedirectToAction("DisplayLogisticJourneyTimes");
            }
            return View("Error");
        }
        //POst EDITDISCOUNT
        private GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            TestClass t = new TestClass();
            return t.ELogisticJourneyTimes(model);
        }

        //GET DeeleteDISCOUNT
        [HttpGet]
        public ActionResult DeleteLogisticJourneyTimes(int pid)
        {
            this.DLogisticJourneyTimes(pid);
            return RedirectToAction("DisplayLogisticJourneyTimes");
        }
        //GET DeeleteDISCOUNT
        private GeneralMaster.LogisticJourneyTimes DLogisticJourneyTimes(int pid)
        {
            TestClass t = new TestClass();
            return t.DLogisticJourneyTimes(pid);
        }
        #endregion
    }
}