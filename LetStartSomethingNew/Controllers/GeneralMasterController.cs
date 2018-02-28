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
        //GET EDITDISCOUNT
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

        



        [HttpGet]
        public ActionResult DisplayRoomType()
        {
            //GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            TestClass t = new TestClass();
            //objRoomType.listRoomType = t.DisplayRoomType();
            return View(t.DisplayRoomType());
        }


        [HttpGet]
        public ActionResult DisplayActivity()
        {
            //GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            TestClass t = new TestClass();
            //objActivity.listActivity = t.DisplayActivity();
            return View(t.DisplayActivity());
        }


        [HttpGet]
        public ActionResult DisplayAddressType()
        {
           // GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            TestClass t = new TestClass();
            //objAddressType.listAddressType = t.DisplayAddressType();
            return View(t.DisplayAddressType());
        }

        [HttpGet]
        public ActionResult DisplayAirport()
        {
            GeneralMaster.VM_Airport objAirport = new GeneralMaster.VM_Airport();
            TestClass t = new TestClass();
            objAirport.listVMAirport = t.DisplayAirport();
            return View(objAirport);
        }


        [HttpGet]
        public ActionResult DisplayBank()
        {
            //GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            TestClass t = new TestClass();
            //objBank.listBank = t.DisplayBank();
            return View(t.DisplayBank());
        }

        [HttpGet]
        public ActionResult DisplayBookingNote()
        {
       //     GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            TestClass t = new TestClass();
       //     objBookingNote.listBookingNote = t.DisplayBookingNote();
            return View(t.DisplayBookingNote());
        }
        [HttpGet]
        public ActionResult DisplayClientChain()
        {
         //   GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            TestClass t = new TestClass();
         //   objClientChain.listClientChain = t.DisplayClientChain();
            return View(t.DisplayClientChain());
        }

        [HttpGet]
        public ActionResult DisplayCurrency()
        {
         //   GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            TestClass t = new TestClass();
         //   objCurrency.listCurrency = t.DisplayCurrency();
            return View(t.DisplayCurrency());
        }

        [HttpGet]
        public ActionResult DisplayTradeFairsTypes()
        {
        //    GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            TestClass t = new TestClass();
        //    objTradeFairsTypes.listTradeFairsType = t.DisplayTradeFairsTypes();
            return View(t.DisplayTradeFairsTypes());
        }
        [HttpGet]
        public ActionResult DisplayFacility()
        {
        //    GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            TestClass t = new TestClass();
        //    objFacility.listFacility = t.DisplayFacility();
            return View(t.DisplayFacility());
        }


        [HttpGet]
        public ActionResult DisplayFinancialYear()
        {
         //   GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            TestClass t = new TestClass();
         //   objFinancialYear.listFinancialYear = t.DisplayFinancialYear();
            return View(t.DisplayFinancialYear());
        }

        [HttpGet]
        public ActionResult DisplayHolidayDuration()
        {
         //   GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            TestClass t = new TestClass();
         //   objHolidayDuration.listHolidayDuration = t.DisplayHolidayDuration();
            return View(t.DisplayHolidayDuration());
        }


        [HttpGet]
        public ActionResult DisplayHolidayType()
        {
        //    GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            TestClass t = new TestClass();
        //    objHolidayType.listHolidayType = t.DisplayHolidayType();
            return View(t.DisplayHolidayType());
        }

        [HttpGet]
        public ActionResult DisplayHotelStandard()
        {
         //   GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            TestClass t = new TestClass();
         //   objHotelStandard.listHotelStandard = t.DisplayHotelStandard();
            return View(t.DisplayHotelStandard());
        }


        [HttpGet]
        public ActionResult DisplayHotelChain()
        {
        //    GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            TestClass t = new TestClass();
        //    objHotelChain.listHotelChain = t.DisplayHotelChain();
            return View(t.DisplayHotelChain());
        }

       
        [HttpGet]
        public ActionResult DisplayInspectionCriteria()
        {
        //    GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            TestClass t = new TestClass();
        //    objInspectionCriteria.listInspectionCriteria = t.DisplayInspectionCriteria();
            return View(t.DisplayInspectionCriteria());
        }


        [HttpGet]
        public ActionResult DisplayLanguage()
        {
        //    GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            TestClass t = new TestClass();
        //    objLanguage.listLanguage = t.DisplayLanguage();
            return View(t.DisplayLanguage());
        }

        [HttpGet]
        public ActionResult DisplayMarket()
        {
        //    GeneralMaster.Market objMarket = new GeneralMaster.Market();
            TestClass t = new TestClass();
        //    objMarket.listMarket = t.DisplayMarket();
            return View(t.DisplayMarket());
        }

        [HttpGet]
        public ActionResult DisplayMealPlan()
        {
        //    GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            TestClass t = new TestClass();
         //   objMealPlan.listMealPlan = t.DisplayMealPlan();
            return View(t.DisplayMealPlan());
        }



        [HttpGet]
        public ActionResult DisplayNationality()
        {
         //   GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            TestClass t = new TestClass();
         //   objNationality.listNationality = t.DisplayNationality();
            return View(t.DisplayNationality());
        }


        [HttpGet]
        public ActionResult DisplayPaymentSchedules()
        {
           // GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            TestClass t = new TestClass();
            //objPaymentSchedules.listPaymentSchedules = t.DisplayPaymentSchedules();
            return View(t.DisplayPaymentSchedules());
        }


        [HttpGet]
        public ActionResult DisplayPaymentType()
        {
        //    GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            TestClass t = new TestClass();
         //   objPaymentType.listPaymentType = t.DisplayPaymentType();
            return View(t.DisplayPaymentType());
        }


        [HttpGet]
        public ActionResult DisplayLogisticPickupType()
        {
         //   GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            TestClass t = new TestClass();
         //   objLogisticPickupType.listLogisticPickupType = t.DisplayLogisticPickupType();
            return View(t.DisplayLogisticPickupType());
        }

        [HttpGet]
        public ActionResult DisplayCrmPriority()
        {
            //    GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            TestClass t = new TestClass();
            //    objCrmPriority.listCrmPriority = t.DisplayCrmPriority();
            return View(t.DisplayCrmPriority());
        }

        [HttpGet]
        public ActionResult DisplayPropertyType()
        {
            //    GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            TestClass t = new TestClass();
            //    objPropertyType.listPropertyType = t.DisplayPropertyType();
            return View(t.DisplayPropertyType());
        }

        [HttpGet]
        public ActionResult DisplayReason()
        {
            //    GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            TestClass t = new TestClass();
            //    objReason.listReason = t.DisplayReason();
            return View(t.DisplayReason());
        }

        [HttpGet]
        public ActionResult DisplayReportingState()
        {
            //    GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            TestClass t = new TestClass();
            //    objReportingState.listReportingState = t.DisplayReportingState();
            return View(t.DisplayReportingState());
        }

        [HttpGet]
        public ActionResult DisplaySeason()
        {
            //    GeneralMaster.Season objSeason = new GeneralMaster.Season();
            TestClass t = new TestClass();
            //    objSeason.listSeason = t.DisplaySeason();
            return View(t.DisplaySeason());
        }



        [HttpGet]
        public ActionResult DisplaySource()
        {
            //    GeneralMaster.Source objSource = new GeneralMaster.Source();
            TestClass t = new TestClass();
            //    objSource.listSource = t.DisplaySource();
            return View(t.DisplaySource());
        }

        [HttpGet]
        public ActionResult DisplayStatus()
        {
            //    GeneralMaster.Status objStatus = new GeneralMaster.Status();
            TestClass t = new TestClass();
            //    objStatus.listStatus = t.DisplayStatus();
            return View(t.DisplayStatus());
        }


        [HttpGet]
        public ActionResult DisplaySupplement()
        {
            //    GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            TestClass t = new TestClass();
            //    objSupplement.listSupplement = t.DisplaySupplement();
            return View(t.DisplaySupplement());
        }

        [HttpGet]
        public ActionResult DisplaySupplementType()
        {
            //    GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            TestClass t = new TestClass();
            //    objSupplementType.listSupplementType = t.DisplaySupplementType();
            return View(t.DisplaySupplementType());
        }


        [HttpGet]
        public ActionResult DisplayTax()
        {
            //    GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            TestClass t = new TestClass();
            //    objTax.listTax = t.DisplayTax();
            return View(t.DisplayTax());
        }

        [HttpGet]
        public ActionResult DisplayTitle()
        {
            //    GeneralMaster.Title objTitle = new GeneralMaster.Title();
            TestClass t = new TestClass();
            //    objTitle.listTitle = t.DisplayTitle();
            return View(t.DisplayTitle());
        }

        [HttpGet]
        public ActionResult DisplayCompany()
        {
            //    GeneralMaster.Company objCompany = new GeneralMaster.Company();
            TestClass t = new TestClass();
            //    objCompany.listCompany = t.DisplayCompany();
            return View(t.DisplayCompany());
        }

        [HttpGet]
        public ActionResult DisplayDepartment()
        {
            //    GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            TestClass t = new TestClass();
            //    objDepartment.listDepartment = t.DisplayDepartment();
            return View(t.DisplayDepartment());
        }


        [HttpGet]
        public ActionResult DisplayDesignation()
        {
            //    GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            TestClass t = new TestClass();
            //    objDesignation.listDesignation = t.DisplayDesignation();
            return View(t.DisplayDesignation());
        }


        [HttpGet]
        public ActionResult DisplayDMCSystemConfiguration()
        {
            //   GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            TestClass t = new TestClass();
            //   objDMCSystemConfiguration.listDMCSystemConfiguration = t.DisplayDMCSystemConfiguration();
            return View(t.DisplayDMCSystemConfiguration());
        }



        [HttpGet]
        public ActionResult DisplayImageLibrary()
        {
            //    GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            TestClass t = new TestClass();
            //    objImageLibrary.listImageLibrary = t.DisplayImageLibrary();
            return View(t.DisplayImageLibrary());
        }


        [HttpGet]
        public ActionResult DisplayDepot()
        {
            //    GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            TestClass t = new TestClass();
            //    objDepot.listDepot = t.DisplayDepot();
            return View(t.DisplayDepot());
        }

        [HttpGet]
        public ActionResult DisplayContractingGroup()
        {
            //    GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            TestClass t = new TestClass();
            //    objContractingGroup.listContractingGroup = t.DisplayContractingGroup();
            return View(t.DisplayContractingGroup());
        }


        [HttpGet]
        public ActionResult DisplayTblTariff()
        {
            //    GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            TestClass t = new TestClass();
            //    objTblTariff.listTblTariff = t.DisplayTblTariff();
            return View(t.DisplayTblTariff());
        }

        [HttpGet]
        public ActionResult DisplayClient()
        {
            //    GeneralMaster.Client objClient = new GeneralMaster.Client();
            TestClass t = new TestClass();
            //    objClient.listClient = t.DisplayClient();
            return View(t.DisplayClient());
        }

        [HttpGet]
        public ActionResult DisplayAirline()
        {
            //    GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            TestClass t = new TestClass();
            //    objAirline.listAirline = t.DisplayAirline();
            return View(t.DisplayAirline());
        }

        [HttpGet]
        public ActionResult DisplayResourceType()
        {
            //    GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            TestClass t = new TestClass();
            //    objResourceType.listResourceType = t.DisplayResourceType();
            return View(t.DisplayResourceType());
        }

        [HttpGet]
        public ActionResult DisplayHumanResource()
        {
           // GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource();
            TestClass t = new TestClass();
            //objHumanResource.listHumanResource = t.DisplayHumanResource();
            //GeneralMaster.HumanResource objHumanresource = t.DisplayHumanResource();

            return View(t.DisplayHumanResource());
        }

        [HttpGet]
        public ActionResult DisplayResourceVehicleDtls()
        {
            //    GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            TestClass t = new TestClass();
            //    objResourceVehicleDtls.listResourceVehicleDtls = t.DisplayResourceVehicleDtls();
            return View(t.DisplayResourceVehicleDtls());
        }

        [HttpGet]
        public ActionResult DisplayLogisticVehicleType()
        {
            //    GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            TestClass t = new TestClass();
            //    objLogisticVehicleType.listLogisticVehicleType = t.DisplayLogisticVehicleType();
            return View(t.DisplayLogisticVehicleType());
        }

        [HttpGet]
        public ActionResult DisplayLogisticPickupArea()
        {
            //   GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            TestClass t = new TestClass();
            //    objLogisticPickupArea.listLogisticPickupArea = t.DisplayLogisticPickupArea();
            return View(t.DisplayLogisticPickupArea());
        }

        [HttpGet]
        public ActionResult DisplayLogisticJourneyTimes()
        {
            //    GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            TestClass t = new TestClass();
            //    objLogisticJourneyTimes.listLogisticJourneyTimes = t.DisplayLogisticJourneyTimes();
            return View(t.DisplayLogisticJourneyTimes());
        }






            
       
    }
}