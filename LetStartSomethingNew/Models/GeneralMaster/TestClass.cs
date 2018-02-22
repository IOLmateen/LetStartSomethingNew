using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DataLayer;
using System.Data.SqlClient;

namespace LetStartSomethingNew.Models.GeneralMaster
{
    public class TestClass
    {
        BaseDataLayer objBaseDataLayer = new BaseDataLayer();

        //public List<GeneralMaster.DiscountType> DisplayDiscountType( )
        //{
        //    DataTable dt = objBaseDataLayer.getDALDiscountType().Tables[0];            
        //    var listDiscounType = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.DiscountType
        //                           {
        //                          Pid = Convert.ToInt32(dr["Pid"]),
        //                          DiscountTypeName = dr["DiscountType"].ToString(),
        //                          Sequence = Convert.ToInt32(dr["Sequence"])
        //                      }
        //                       ).ToList();
        //       return listDiscounType;
        //}
        public void getUserSettings(GeneralMaster.UserGroupRights objUsergrouprights,string pagename)
        {
            DataTable dtUserSetting = objBaseDataLayer.getDALUserPageSettings(pagename).Tables[0];
            objUsergrouprights.ListUserGroupRights = (from DataRow dr in dtUserSetting.Rows
                                                      select new GeneralMaster.UserGroupRights
                                                      {
                                                          PageName = dr["PageName"].ToString(),
                                                          LinkName = dr["LinkName"].ToString(),
                                                          RightHeader = dr["RightHeader"].ToString()
                                                      }).ToList();

        }

        public string getUserGroupRights(string pgname, string lnkname, string rightheader, string usergroupid, string companyid)
        {
            string FlagYN;
            int count = objBaseDataLayer.getDALUserGroupRights(pgname, lnkname, rightheader, usergroupid, companyid);
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



        public GeneralMaster.DiscountType DisplayDiscountType(int CurrPage,int SearchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayDiscountType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("DiscountType", SearchPid).Tables[0];

            //objDiscountType.listDiscountType = (from DataRow dr in dt.Rows
            //                       select new GeneralMaster.DiscountType
            //                       {
            //                           Pid = Convert.ToInt32(dr["Pid"]),
            //                           DiscountTypeName = dr["DiscountType"].ToString(),
            //                           Sequence = Convert.ToInt32(dr["Sequence"])
            //                       }
            //                   ).ToList();
            objDiscountType.listDiscountType = (from DataRow dr in dt.Rows
                                                select new GeneralMaster.DiscountType
                                                {
                                                    Pid = Convert.ToInt32(dr["Pid"]),
                                                    DiscountTypeName = dr["DiscountType"].ToString(),
                                                    Sequence = Convert.ToInt32(dr["Sequence"]),
                                                    LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                                }
                               ).Skip((CurrPage - 1) * objDiscountType.PagingValues.MaxRows)
                                .Take(objDiscountType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objDiscountType.PagingValues.MaxRows));
            objDiscountType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objDiscountType.PagingValues.CurrentPageIndex = CurrPage;

            return objDiscountType;
        }


        public List<GeneralMaster.DiscountType> SearchDiscountType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALDiscountTypeByPrefix(prefix).Tables[0];
            var listDiscounType = dt.AsEnumerable().Where(x => x.Field<string>("DiscountType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.DiscountType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            DiscountTypeName = x.Field<string>("DiscountType")
                                        }).ToList();

            //var listDiscounType = (from DataRow dr in dt.Rows
            //                       where dr.Field<string>("DiscountType").Contains(prefix)
            //                       select new DiscountType
            //                       {
            //                           Pid = Convert.ToInt32(dr["Pid"]),
            //                           DiscountTypeName = dr["DiscountType"].ToString(),
            //                       }
            //                   ).ToList();
            return listDiscounType;
        }
        //public GeneralMaster.DiscountType SearchDiscountTypeByPid(int id)
        //{
        //    GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
        //    DataTable dt = objBaseDataLayer.getDALDiscountTypeById(id).Tables[0];
        //    objDiscountType.listDiscountType = (from DataRow dr in dt.Rows
        //                                        select new GeneralMaster.DiscountType
        //                                        {
        //                                            Pid = Convert.ToInt32(dr["Pid"]),
        //                                            DiscountTypeName = dr["DiscountType"].ToString(),
        //                                            Sequence = Convert.ToInt32(dr["Sequence"]),
        //                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])

        //                                        }
        //                       ).ToList();
        //    return objDiscountType;
        //}
        //GETADD DISCOUNT
        public GeneralMaster.DiscountType ADiscountType()
        {
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
            return objDiscountType;
        }

        //GETEDIT DISCOUNT
        public GeneralMaster.DiscountType EDiscountType(int id)
        {
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
            DataTable dt = objBaseDataLayer.getDALGetDiscountTypeById(id,"E");

            objDiscountType.DiscountTypeName = Convert.ToString(dt.Rows[0]["DiscountType"]);
            objDiscountType.Sequence = Convert.ToInt32(dt.Rows[0]["Sequence"]);
            objDiscountType.LastEditByXid = Convert.ToInt32(dt.Rows[0]["LastEditByXid"]);
            objDiscountType.Companyxid = Convert.ToInt32(dt.Rows[0]["Companyxid"]);
            objDiscountType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if(objDiscountType.NotesXid != 0)
            { 
                objDiscountType.NotesDescription = GetNotesById(objDiscountType.NotesXid.GetValueOrDefault(-1));
            }
            return objDiscountType;
        }
        private string GetNotesById(int NotesXid)
        {
            return objBaseDataLayer.getDALGetNotesById(NotesXid,"E");
        }

        //GETDelete Discount
        public GeneralMaster.DiscountType DDiscountType(int id)
        {
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
            objBaseDataLayer.getDALGetDiscountTypeById(id,"D");
            //string s =DeleteNotesById(notesxid);

            return objDiscountType;
        }
        //private string DeleteNotesById(int NotesXid)
        //{
        //    return objBaseDataLayer.getDALGetNotesById(NotesXid,"D");
        //}

        //POSTEDIT Discount
        public GeneralMaster.DiscountType EDiscountType(GeneralMaster.DiscountType model)
        {
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDiscountType.Pid = model.Pid;
            objDiscountType.DiscountTypeName = model.DiscountTypeName;
            objDiscountType.Sequence = model.Sequence;

            objDiscountType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDiscountType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDiscountType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                objDiscountType.NotesXid = NotesChanges(objNotes.Pid, objDiscountType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid,"E");
            }

            objBaseDataLayer.getDALInsertModifyDiscountType(objDiscountType.Pid,objDiscountType.DiscountTypeName, objDiscountType.Sequence, objDiscountType.NotesXid.GetValueOrDefault(-1),
                                                      objDiscountType.LastEditByXid, objDiscountType.Companyxid,"E");

            return objDiscountType;

        }
        //POSTADD DISCOUNT
        public GeneralMaster.DiscountType ADiscountType(GeneralMaster.DiscountType model)
        {
            GeneralMaster.DiscountType objDiscountType = new GeneralMaster.DiscountType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDiscountType.Pid = objNotes.Pid = - 1;
            objDiscountType.DiscountTypeName = model.DiscountTypeName;
            objDiscountType.Sequence = model.Sequence;

            objDiscountType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDiscountType.Companyxid = objNotes.CompanyXid =  Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDiscountType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objDiscountType.NotesXid = NotesChanges(objNotes.Pid ,objDiscountType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid,"A");
            }

            objBaseDataLayer.getDALInsertModifyDiscountType(objDiscountType.Pid,objDiscountType.DiscountTypeName, objDiscountType.Sequence, objDiscountType.NotesXid.GetValueOrDefault(-1),
                                                      objDiscountType.LastEditByXid, objDiscountType.Companyxid,"A");

            return objDiscountType;
        }
        //POSTADD DISCOUNT for NOTES
        private int NotesChanges(int Pid,string NotesName,int LastEditByXid, int CompanyXid,string Action)
        {
            return objBaseDataLayer.getDALNotesChanges(Pid,NotesName, LastEditByXid, CompanyXid,Action);
        }








        public GeneralMaster.RoomType DisplayRoomType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("RoomType").Tables[0];
            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.RoomType
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       RoomTypeName = dr["RoomType"].ToString(),
                                       MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                       LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                   }
                               ).ToList();
            return objRoomType;
        }

        public GeneralMaster.Activity DisplayActivity()
        {
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Activity").Tables[0];
            objActivity.listActivity = (from DataRow dr in dt.Rows
                                select new GeneralMaster.Activity
                                {
                                    Pid = Convert.ToInt32(dr["Pid"]),
                                    Code = dr["Code"].ToString(),
                                    ActivityName = Convert.ToString(dr["Activity"]),
                                    LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                }
                               ).ToList();
            return objActivity;
        }

        public GeneralMaster.AddressType DisplayAddressType()
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("AddressType").Tables[0];
            objAddressType.listAddressType = (from DataRow dr in dt.Rows
                                select new GeneralMaster.AddressType
                                {
                                    Pid = Convert.ToInt32(dr["Pid"]),
                                    AddressTypeName = dr["addressType"].ToString(),
                                    LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                }
                               ).ToList();
            return objAddressType;
        }

        public GeneralMaster.Bank DisplayBank()
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Bank").Tables[0];
            objBank.listBank = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.Bank
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       Code = Convert.ToString(dr["Code"]),
                                       BankName= Convert.ToString(dr["addressType"]),
                                       BankBranch = Convert.ToString(dr["BankBranch"]),
                                       GuichetCode = Convert.ToString(dr["GuichetCode"]),
                                       LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                   }
                               ).ToList();
            return objBank;
        }
        public GeneralMaster.BookingNote DisplayBookingNote()
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("BookingNote").Tables[0];
            objBookingNote.listBookingNote = (from DataRow dr in dt.Rows
                            select new GeneralMaster.BookingNote
                            {
                                Pid = Convert.ToInt32(dr["Pid"]),
                                Code = Convert.ToString(dr["Code"]),
                                NoteFor = Convert.ToString(dr["NoteFor"]),
                                Note = Convert.ToString(dr["Note"])
                            }
                               ).ToList();
            return objBookingNote;
        }


        public GeneralMaster.CardType DisplayCardType()
        {
            GeneralMaster.CardType objCardType = new GeneralMaster.CardType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("CardType").Tables[0];
            objCardType.listCardType = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.CardType
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       CardTypeName = Convert.ToString(dr["CardType"]),
                                       Length = Convert.ToInt32(dr["Length"]),
                                       CCChargesYN = Convert.ToString(dr["CCChargesYN"]),
                                       CCCharges = Convert.ToDecimal(dr["CCCharges"]),
                                       CCChargeApplyTo = Convert.ToString(dr["CCChargeApplyTo"]),
                                       LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                   }
                               ).ToList();
            return objCardType;
        }


        public GeneralMaster.ClientChain DisplayClientChain()
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ClientChain").Tables[0];
            objClientChain.listClientChain = (from DataRow dr in dt.Rows
                                select new GeneralMaster.ClientChain
                                {
                                    Pid = Convert.ToInt32(dr["Pid"]),
                                    Code = Convert.ToString(dr["Code"]),
                                    ClientChainName = Convert.ToString(dr["ClientChain"])
                                }
                               ).ToList();
            return objClientChain;
        }


        public GeneralMaster.Currency DisplayCurrency()
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Currency").Tables[0];
            objCurrency.listCurrency = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.Currency
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       Code = Convert.ToString(dr["Code"]),
                                       CurrencyName = Convert.ToString(dr["Currency"]),
                                       CurrencySymbol = Convert.ToString(dr["CurrencySymbol"]),
                                       DefaultCurrency = Convert.ToString(dr["DefaultCurrency"]),
                                       NominalCode = Convert.ToString(dr["NominalCode"]),
                                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                   }
                               ).ToList();
            return objCurrency;
        }



        public GeneralMaster.TradeFairsTypes DisplayTradeFairsTypes()
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("TradeFairsTypes").Tables[0];
            objTradeFairsTypes.listTradeFairsType = (from DataRow dr in dt.Rows
                                select new GeneralMaster.TradeFairsTypes
                                {
                                    Pid = Convert.ToInt32(dr["Pid"]),
                                    Code = Convert.ToString(dr["Code"]),
                                    TradeFairsTypeName = Convert.ToString(dr["TradeFairsType"]),
                                    LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                }
                               ).ToList();
            return objTradeFairsTypes;
        }


        public GeneralMaster.Facility DisplayFacility()
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Facility").Tables[0];
            objFacility.listFacility = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.Facility
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                           FacilityName = Convert.ToString(dr["Facility"]),
                                           Belongsto = Convert.ToString(dr["BelongsTo"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objFacility;
        }

        public GeneralMaster.FinancialYear DisplayFinancialYear()
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("FinancialYear").Tables[0];
            objFinancialYear.listFinancialYear = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.FinancialYear
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                           FinancialYearName = Convert.ToString(dr["FinancialYear"]),
                                           FromDate = Convert.ToDateTime(dr["FromDate"]),
                                           ToDate = Convert.ToDateTime(dr["ToDate"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objFinancialYear;
        }
        public GeneralMaster.HolidayDuration DisplayHolidayDuration()
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("HolidayDuration").Tables[0];
            objHolidayDuration.listHolidayDuration = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.HolidayDuration
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                           HolidayDurationName = Convert.ToString(dr["HolidayDuration"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objHolidayDuration;
        }
        public GeneralMaster.HolidayType DisplayHolidayType()
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("HolidayType").Tables[0];
            objHolidayType.listHolidayType = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.HolidayType
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                           HolidayTypeName = Convert.ToString(dr["HolidayType"])  
                                       }
                               ).ToList();
            return objHolidayType;
        }
        public GeneralMaster.HotelStandard DisplayHotelStandard()
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("HotelStandard").Tables[0];
            objHotelStandard.listHotelStandard = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.HotelStandard
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           StandardName = Convert.ToString(dr["Standard"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objHotelStandard;
        }
        public GeneralMaster.HotelChain DisplayHotelChain()
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("HotelChain").Tables[0];
            objHotelChain.listHotelChain = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.HotelChain
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                            HotelChainsName =  Convert.ToString(dr["HotelChain"])
                                  
                                       }
                               ).ToList();
            return objHotelChain;
        }
        
        public GeneralMaster.InspectionCriteria DisplayInspectionCriteria()
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("InspectionCriteria").Tables[0];
            objInspectionCriteria.listInspectionCriteria = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.InspectionCriteria
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           InspectionCriteriaName = Convert.ToString(dr["InspectionCriteria"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objInspectionCriteria;
        }




        public GeneralMaster.Language DisplayLanguage()
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();

               DataTable dt = objBaseDataLayer.getDALGeneralMaster("Language").Tables[0];
               objLanguage.listLanguage = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.Language
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           Code = Convert.ToString(dr["Code"]),
                                           LanguageName = Convert.ToString(dr["language"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objLanguage;
        }
        public GeneralMaster.Market DisplayMarket()
        {
            GeneralMaster.Market objMarket = new GeneralMaster.Market();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Market").Tables[0];
            objMarket.listMarket = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.Market
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           MarketName = Convert.ToString(dr["market"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objMarket;
        }
        public GeneralMaster.MealPlan DisplayMealPlan()
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("MealPlan").Tables[0];
            objMealPlan.listMealPlan = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.MealPlan
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           MealPlanName = Convert.ToString(dr["Mealplan"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objMealPlan;
        }
        public GeneralMaster.Nationality DisplayNationality()
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Nationality").Tables[0];
            objNationality.listNationality = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.Nationality
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           NationalityName = Convert.ToString(dr["nationality"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objNationality;
        }
        public GeneralMaster.PaymentSchedules DisplayPaymentSchedules()
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("PaymentSchedules").Tables[0];
            objPaymentSchedules.listPaymentSchedules = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.PaymentSchedules
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           PaymentSchedulesName = Convert.ToString(dr["PaymentSchedule"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"]),
                                       }
                               ).ToList();
            return objPaymentSchedules;
        }
        public GeneralMaster.PaymentType DisplayPaymentType()
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("PaymentType").Tables[0];
            objPaymentType.listPaymentType = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.PaymentType
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           PaymentTypeName =Convert.ToString(dr["PaymentType"]),
                                           NominalCode = Convert.ToString(dr["nominalCode"]),
                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objPaymentType;
        }
        public GeneralMaster.LogisticPickupType DisplayLogisticPickupType()
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticPickupType").Tables[0];
            objLogisticPickupType.listLogisticPickupType = (from DataRow dr in dt.Rows
                                       select new GeneralMaster.LogisticPickupType
                                       {
                                           Pid = Convert.ToInt32(dr["Pid"]),
                                           PickupType = Convert.ToString(dr["Pickuptype"]),
                                           ShowBookingEngine =Convert.ToString(dr["showbookingengine"]),
                                           ArrivalPoint = Convert.ToString(dr["ArrivalPOint"]),

                                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                       }
                               ).ToList();
            return objLogisticPickupType;
        }
        //Pending
        public GeneralMaster.CrmPriority DisplayCrmPriority()
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("CrmPriority").Tables[0];
            objCrmPriority.listCrmPriority = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.CrmPriority
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       Priority = Convert.ToString(dr["priority"]),
                                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                   }
                               ).ToList();
            return objCrmPriority;
        }

        public GeneralMaster.PropertyType DisplayPropertyType()
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("PropertyType").Tables[0];
            objPropertyType.listPropertyType = (from DataRow dr in dt.Rows
                                    select new GeneralMaster.PropertyType
                                    {
                                        Pid = Convert.ToInt32(dr["Pid"]),
                                        PropertyTypeName = Convert.ToString(dr["PropertyType"]),
                                        LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                    }
                               ).ToList();
            return objPropertyType;
        }

        public GeneralMaster.Reason DisplayReason()
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Reason").Tables[0];
            objReason.listReason = (from DataRow dr in dt.Rows
                              select new GeneralMaster.Reason
                              {
                                  Pid = Convert.ToInt32(dr["Pid"]),
                                  ReasonName = Convert.ToString(dr["Reason"]),
                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
                              }
                               ).ToList();
            return objReason;
        }

        public GeneralMaster.ReportingState DisplayReportingState()
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ReportingState").Tables[0];
            objReportingState.listReportingState = (from DataRow dr in dt.Rows
                                      select new GeneralMaster.ReportingState
                                      {
                                          Pid = Convert.ToInt32(dr["Pid"]),
                                          Code = Convert.ToString(dr["Code"]),
                                          ReportingStateName = Convert.ToString("ReportingState"),
                                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                      }
                               ).ToList();
            return objReportingState;
        }

        public GeneralMaster.Season DisplaySeason()  //MATZVM
        {
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Season").Tables[0];
            //objSeason.listSeason = (from DataRow dr in dt.Rows
            //                  select new GeneralMaster.Season
            //                  {
            //                      Pid = Convert.ToInt32(dr["Pid"]),
            //                      Code = Convert.ToString(dr["code"]),
            //                      SeasonName = Convert.ToString(dr["season"]),
            //                      FromDate = Convert.ToDateTime(dr["FromDate"]),
            //                      ToDate = Convert.ToDateTime(dr["ToDate"]),
            //                      //    FinancialYearXid
            //                      LastEdit = Convert.ToDateTime(dr["LastEdit"])
            //                  }
            //                   ).ToList();

            GeneralMaster.VM_Season vmseason = new GeneralMaster.VM_Season();
            GeneralMaster.Season objSeason = new GeneralMaster.Season(vmseason);

            objSeason.listSeason = (from DataRow dr in dt.Rows
                                    select new GeneralMaster.Season
                                    {
                                        Pid = Convert.ToInt32(dr["Pid"]),
                                        Code = Convert.ToString(dr["code"]),
                                        SeasonName = Convert.ToString(dr["season"]),
                                        FromDate = Convert.ToDateTime(dr["FromDate"]),
                                        ToDate = Convert.ToDateTime(dr["ToDate"]),
                                        //    FinancialYearXid
                                        LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                    }
                               ).ToList();
            objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
                                                                   select new GeneralMaster.VM_Season
                                                                   {
                                                                      FinancialYear  = Convert.ToString(dr["FinancialYear"]),
                                                                   }
                            ).ToList();
            return objSeason;
        }

        public GeneralMaster.Source DisplaySource()
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Source").Tables[0];
            objSource.listSource = (from DataRow dr in dt.Rows
                              select new GeneralMaster.Source
                              {
                                  Pid = Convert.ToInt32(dr["Pid"]),
                                  SourceName = Convert.ToString(dr["Source"]),
                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
                              }
                               ).ToList();
            return objSource;
        }

        public GeneralMaster.Status DisplayStatus()
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Status").Tables[0];
            objStatus.listStatus = (from DataRow dr in dt.Rows
                              select new GeneralMaster.Status
                              {
                                  Pid = Convert.ToInt32(dr["Pid"]),
                                  Code = Convert.ToString(dr["Code"]),
                                  StatusName = Convert.ToString(dr["Status"]),
                                  Colour = Convert.ToString(dr["Color"]),
                                  StatusEntity = Convert.ToString(dr["statusEntity"]),
                                  ReasonYN = Convert.ToString(dr["ReasonYN"]),
                                  SendYN = Convert.ToString(dr["SendYN"]),
                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
                              }
                               ).ToList();
            return objStatus;
        }

        public GeneralMaster.Supplement DisplaySupplement() //MATZVM
        {
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Supplement").Tables[0];
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            objSupplement.listSupplement = (from DataRow dr in dt.Rows
                                  select new GeneralMaster.Supplement
                                  {
                                      Pid = Convert.ToInt32(dr["Pid"]),
                                      Code = Convert.ToString(dr["Code"]),
                                      SupplementName = Convert.ToString(dr["SupplementName"]),
                                      //SupplementTypeXid
                                      BelongsTo = Convert.ToString(dr["belongsTo"]),
                                      //CurrencyXid
                                      PerWhat = Convert.ToString(dr["perWhat"]),
                                      PNPH = Convert.ToString(dr["PNPH"]),
                                      Taxable = Convert.ToString(dr["taxable"]),
                                      //MealPlanXid
                                      VatCode = Convert.ToString(dr["vatcode"]),
                                      ShowOnRateScreenYN = Convert.ToString(dr["ShowOnRateScreenYN"]),
                                      LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                  }
                               ).ToList();

            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();

            return objSupplement;
        }

        public GeneralMaster.SupplementType DisplaySupplementType()
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();

            DataTable dt = objBaseDataLayer.getDALGeneralMaster("SupplementType").Tables[0];
            objSupplementType.listSupplementType = (from DataRow dr in dt.Rows
                                      select new GeneralMaster.SupplementType
                                      {
                                          Pid = Convert.ToInt32(dr["Pid"]),
                                          SupplementTypeName = Convert.ToString(dr["SupplementType"]),
                                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                      }
                               ).ToList();
            return objSupplementType;
        }


        public GeneralMaster.Tax DisplayTax()
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Tax").Tables[0];
            objTax.listTax = (from DataRow dr in dt.Rows
                           select new GeneralMaster.Tax
                           {
                               Pid = Convert.ToInt32(dr["Pid"]),
                               Code = Convert.ToString(dr["Code"]),
                               TaxName = Convert.ToString(dr["Tax"]),
                               ActiveYN = Convert.ToString(dr["ActiveYN"]),
                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
                           }
                               ).ToList();
            return objTax;
        }
        public GeneralMaster.Title DisplayTitle()
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Title").Tables[0];
            objTitle.listTitle = (from DataRow dr in dt.Rows
                             select new GeneralMaster.Title
                             {
                                 Pid = Convert.ToInt32(dr["Pid"]),
                                 TitleName = Convert.ToString(dr["Title"]),
                                 Sequence = Convert.ToInt32(dr["Sequence"]),
                                 LastEdit = Convert.ToDateTime(dr["LastEdit"])
                             }
                               ).ToList();
            return objTitle;
        }
        public GeneralMaster.Company DisplayCompany() //MATZVM
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Company").Tables[0];
            objCompany.listCompany = (from DataRow dr in dt.Rows
                               select new GeneralMaster.Company
                               {
                                   Pid = Convert.ToInt32(dr["Pid"]),
                                   Code = Convert.ToString(dr["Code"]),
                                   CompanyName = Convert.ToString(dr["Company"]),
                                   CompanyAddress = Convert.ToString(dr["CompanyAddress"]),
                                   Email = Convert.ToString(dr["Email"]),
                                   AccountsEmail = Convert.ToString(dr["AccountsEmail"]),
                                   Fax = Convert.ToString(dr["Fax"]),
                                   Tel = Convert.ToString(dr["Tel"]),
                                   //CityXid
                                   BookingNotificationEmailAddress = Convert.ToString(dr["BookingNotificationEmailAddress"]),
                                   UsePGYN = Convert.ToString(dr["UsePGYN"]),
                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
                               }
                               ).ToList();
            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objCompany;
        }
        public GeneralMaster.Department DisplayDepartment()
        {
            GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Department").Tables[0];
            objDepartment.listDepartment = (from DataRow dr in dt.Rows
                                  select new GeneralMaster.Department
                                  {
                                      Pid = Convert.ToInt32(dr["Pid"]),
                                      DepartmentName = Convert.ToString(dr["Department"]),
                                      BelongsTo = Convert.ToString(dr["belongsto"]),
                                      LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                  }
                               ).ToList();
            return objDepartment;
        }
        public GeneralMaster.Designation DisplayDesignation()
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Designation").Tables[0];
            objDesignation.listDesignation = (from DataRow dr in dt.Rows
                                   select new GeneralMaster.Designation
                                   {
                                       Pid = Convert.ToInt32(dr["Pid"]),
                                       DesignationName = Convert.ToString(dr["Designation"]),
                                       BelongsTo = Convert.ToString(dr["BelongsTo"]),
                                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                   }
                               ).ToList();
            return objDesignation;
        }
        public GeneralMaster.DMCSystemConfiguration DisplayDMCSystemConfiguration()
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("DMCSystemConfiguration").Tables[0];
            objDMCSystemConfiguration.listDMCSystemConfiguration = (from DataRow dr in dt.Rows
                                              select new GeneralMaster.DMCSystemConfiguration
                                              {
                                                  Pid = Convert.ToInt32(dr["Pid"]),
                                                  SMTPServer = Convert.ToString(dr["SMTPServer"]),
                                                  SendUsing = Convert.ToString(dr["SendUsing"]),
                                                  SMTPServerPort = Convert.ToString(dr["SMTPServerPort"]),
                                                  SMTPConnectionTimeout = Convert.ToString(dr["SMTPConnectionTimeout"]),
                                                  SMTPUserName = Convert.ToString(dr["SMTPUserName"]),
                                                  SMTPPassword = Convert.ToString(dr["SMTPPassword"]),
                                                  ImageDomain = Convert.ToString(dr["ImageDomain"]),
                                                  SMTPEnableSSL = Convert.ToInt32(dr["SMTPEnableSSL"]),
                                                  SMTPSenderEmail = Convert.ToString(dr["SMTPSenderEmail"]),
                                                  IWTXYesOrNo = Convert.ToString(dr["IWTXYesOrNo"]),
                                              }
                               ).ToList();
            return objDMCSystemConfiguration;
        }
        public GeneralMaster.ImageLibrary DisplayImageLibrary() //MATZVM
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ImageLibrary").Tables[0];
            objImageLibrary.listImageLibrary = (from DataRow dr in dt.Rows
                                    select new GeneralMaster.ImageLibrary
                                    {
                                        Pid = Convert.ToInt32(dr["Pid"]),
                                        // ImageLibraryCategoryXid  
                                        Description = Convert.ToString(dr["Description"]),
                                        SlideNumber = Convert.ToInt32(dr["SlideNumber"]),
                                        ThumbnailPath = Convert.ToString(dr["thumbnailpath"]),
                                        LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                    }
                               ).ToList();
            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();

            return objImageLibrary;
        }
        public GeneralMaster.Depot DisplayDepot() //MATZVM
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Depot").Tables[0];
            objDepot.listDepot = (from DataRow dr in dt.Rows
                             select new GeneralMaster.Depot
                             {
                                 Pid = Convert.ToInt32(dr["Pid"]),
                                 Code = Convert.ToString(dr["Code"]),
                                 DepotName = Convert.ToString(dr["Depot"]),
                                 Address = Convert.ToString(dr["Address"]),
                                 //CountryXid
                                 //CityXid
                                 //SupplierXid

                                 LastEdit = Convert.ToDateTime(dr["LastEdit"])
                             }
                               ).ToList();
            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objDepot;
        }
        public GeneralMaster.ContractingGroup DisplayContractingGroup()
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ContractingGroup").Tables[0];
            objContractingGroup.listContractingGroup = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ContractingGroup
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            CompanyCode = Convert.ToString(dr["CompanyCode"]),
                                            CompanyName = Convert.ToString(dr["CompanyName"]),
                                            TaxID = Convert.ToString(dr["TaxId"]),
                                            OTAOutputSet = Convert.ToString(dr["OTAOutputSet"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).ToList();
            return objContractingGroup;
        }
        public GeneralMaster.TblTariff DisplayTblTariff()
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("TblTariff").Tables[0];
            objTblTariff.listTblTariff = (from DataRow dr in dt.Rows
                                 select new GeneralMaster.TblTariff
                                 {
                                     Pid = Convert.ToInt32(dr["Pid"]),
                                     TariffName = Convert.ToString(dr["Tariff"]),
                                     DefaultYN = Convert.ToString(dr["Defaultyn"]),
                                     LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                 }
                               ).ToList();
            return objTblTariff;
        }
        public GeneralMaster.TblTariffMarkets DisplayTblTariffMarkets() //MATZVM
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("TblTariffMarkets").Tables[0];
            objTblTariffMarkets.listTblTariffMarkets = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.TblTariffMarkets
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            //MarketXid
                                            FromDate = Convert.ToDateTime(dr["FromDate"]),
                                            ToDate = Convert.ToDateTime(dr["ToDate"]),
                                            AmountOrPercentage = Convert.ToString(dr["AmountOrPercentage"]),
                                            Value = Convert.ToInt32(dr["value"]),
                                            //CurrencyXid
                                        }
                               ).ToList();
            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objTblTariffMarkets;
        }
        public GeneralMaster.Client DisplayClient() //MATZVM
        {
            GeneralMaster.Client objCLient = new GeneralMaster.Client();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Client").Tables[0];
            objCLient.listClient = (from DataRow dr in dt.Rows
                              select new GeneralMaster.Client
                              {
                                  Pid = Convert.ToInt32(dr["Pid"]),
                                  Code = Convert.ToString(dr["Code"]),
                                  ClientType = Convert.ToString(dr["ClientType"]),
                                  //ClientChainXid
                                  CommunicationMethod = Convert.ToString(dr["CommunicationMethod"]),
                                  //DefaultCurrencyXid
                                  //Passport
                                  //bank
                                  //product
                                  //BookingSummary
                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
                              }
                               ).ToList();

            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objCLient;
        }
        public GeneralMaster.Airline DisplayAirline()
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Airline").Tables[0];
            objAirline.listAirline = (from DataRow dr in dt.Rows
                               select new GeneralMaster.Airline
                               {
                                   Pid = Convert.ToInt32(dr["Pid"]),
                                   Code = Convert.ToString(dr["Code"]),
                                   AirlineName = Convert.ToString(dr["Airline"])
                               }
                               ).ToList();
            return objAirline;
        }
        public GeneralMaster.ResourceType DisplayResourceType()
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ResourceType").Tables[0];
            objResourceType.listResourceType = (from DataRow dr in dt.Rows
                                    select new GeneralMaster.ResourceType
                                    {
                                        Pid = Convert.ToInt32(dr["Pid"]),
                                        Code = Convert.ToString(dr["Code"]),
                                        ResourceName = Convert.ToString(dr["Resource"]),
                                        ResourceTypeName = Convert.ToString(dr["ResourceType"])
                                    }
                               ).ToList();
            return objResourceType;
        }


        public GeneralMaster.HumanResource DisplayHumanResource() //MATZVM
        {
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("HumanResource").Tables[0];

            GeneralMaster.VM_HumanResource vmobjhumanrsr = new GeneralMaster.VM_HumanResource();
            GeneralMaster.HumanResource objHumanRsr = new GeneralMaster.HumanResource(vmobjhumanrsr);
         //   GeneralMaster.HumanResource objHumanRsr = new GeneralMaster.HumanResource(new GeneralMaster.VM_HumanResource());

            objHumanRsr.listHumanResource = (from DataRow dr in dt.Rows
                                             select new GeneralMaster.HumanResource
                                             {
                                                 Pid = Convert.ToInt32(dr["Pid"]),
                                                 Id = Convert.ToString(dr["Id"]),
                                                 FirstName = Convert.ToString(dr["FirstName"]),
                                                 LastName = Convert.ToString(dr["Lastname"]),
                                                 Category = Convert.ToString(dr["Category"]),
                                                 Doj = Convert.ToDateTime(dr["Doj"]),
                                                 MobileNo = Convert.ToString(dr["mobileno"]),
                                                 Email = Convert.ToString(dr["email"]),
                                             }
                               ).ToList();

            objHumanRsr.HumanResourceValues.listVMHumanResource = (from DataRow dr in dt.Rows
                                                                   select new GeneralMaster.VM_HumanResource
                                                                   {
                                                                       DesignationName = Convert.ToString(dr["Designation"]),
                                                                       ResourceTypeName = Convert.ToString(dr["ResourceType"])
                                                                   }
                            ).ToList();

            return objHumanRsr;
        }

        public GeneralMaster.ResourceVehicleDtls DisplayResourceVehicleDtls() //MATZVM
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("ResourceVehicleDtls").Tables[0];
            objResourceVehicleDtls.listResourceVehicleDtls = (from DataRow dr in dt.Rows
                                           select new GeneralMaster.ResourceVehicleDtls
                                           {
                                               Pid = Convert.ToInt32(dr["Pid"]),
                                               RegistrationNo = Convert.ToString(dr["RegistrationNo"]),
                                               //VehicleTypeXid
                                               Capacity = Convert.ToString(dr["Capacity"]),
                                               Milage = Convert.ToString(dr["Milage"]),
                                               Ingarage = Convert.ToString(dr["Ingarage"]),
                                               PlateNo = Convert.ToString(dr["plateno"]),
                                               VehicleMake = Convert.ToString(dr["vehiclemake"]),
                                               FuelTankCapacity = Convert.ToString(dr["FuelTankCapacity"]),
                                               //SupplierXid
                                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                           }
                               ).ToList();

            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objResourceVehicleDtls;
        }
        public GeneralMaster.LogisticVehicleType DisplayLogisticVehicleType() //MATZVM
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticVehicleType").Tables[0];
            objLogisticVehicleType.listLogisticVehicleType = (from DataRow dr in dt.Rows
                                           select new GeneralMaster.LogisticVehicleType
                                           {
                                               Pid = Convert.ToInt32(dr["Pid"]),
                                               Code = Convert.ToString(dr["Code"]),
                                               VehicleType = Convert.ToString(dr["VehicleType"]),
                                               Capacity = Convert.ToInt32(dr["Capacity"]),
                                               //ParentvehicleTypeXid
                                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                           }
                               ).ToList();

            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objLogisticVehicleType;
        }
        public GeneralMaster.LogisticPickupArea DisplayLogisticPickupArea() //MATZVM
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticPickupArea").Tables[0];
            objLogisticPickupArea.listLogisticPickupArea = (from DataRow dr in dt.Rows
                                          select new GeneralMaster.LogisticPickupArea
                                          {
                                              Pid = Convert.ToInt32(dr["Pid"]),
                                              PickupArea = Convert.ToString(dr["PickupArea"]),
                                              // CityXid
                                              // CountryXid
                                              ActiveYN = Convert.ToString(dr["ActiveYN"])
                                          }
                               ).ToList();      
            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objLogisticPickupArea;
        }
        public GeneralMaster.LogisticJourneyTimes DisplayLogisticJourneyTimes() //MATZVM
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticJourneyTimes").Tables[0];
            objLogisticJourneyTimes.listLogisticJourneyTimes = (from DataRow dr in dt.Rows
                                            select new GeneralMaster.LogisticJourneyTimes
                                            {
                                                Pid = Convert.ToInt32(dr["Pid"]),
                                                //Country
                                                //CityXid
                                            }
                               ).ToList();

            //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
            //                                       select new GeneralMaster.VM_Season
            //                                       {
            //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
            //                                       }
            //                ).ToList();
            return objLogisticJourneyTimes;
        }



        public List<GeneralMaster.VM_Airport> DisplayAirport()
        {
            DataTable dt = objBaseDataLayer.getDALGeneralMaster("Airport").Tables[0];
            GeneralMaster.VM_Airport obj1 = new GeneralMaster.VM_Airport();
            List<GeneralMaster.VM_Airport> listobj1 = new List<GeneralMaster.VM_Airport>();

            if (dt.Rows.Count > 0)
            {
               foreach (DataRow dr in dt.Rows)
                {
                    obj1.Airport_Values.Pid = Convert.ToInt32(dr["Pid"]);
                    obj1.Airport_Values.Code = Convert.ToString(dr["Code"]);
                    obj1.Airport_Values.AirportName = Convert.ToString(dr["Airport"]);
                    obj1.CountryName = Convert.ToString(dr["Country"]);
                    obj1.CityName = Convert.ToString(dr["City"]);
                    obj1.Airport_Values.LastEdit = Convert.ToDateTime(dr["LastEdit"]);
                    listobj1.Add( new GeneralMaster.VM_Airport(obj1.Airport_Values.Pid, obj1.Airport_Values.Code,obj1.Airport_Values.AirportName,obj1.CountryName,obj1.CityName,obj1.Airport_Values.LastEdit));
                }
            }
            return listobj1;
        }
       

        //private SqlConnection con;
        ////To Handle connection related activities    
        //private void connection()
        //{
        //    string constr = "Server=10.10.1.19;Database=travcostaging2016_MM;uid=illusions;password=illusions";
        //    con = new SqlConnection(constr);

        //}
        //public List<Employee> GetAllEmployees()
        //{
        //    connection();
        //    List<Employee> EmpList = new List<Employee>();


        //    SqlCommand com = new SqlCommand("Select Pid,FirstName from M_Employee", con);
        //    com.CommandType = CommandType.Text;
        //    SqlDataAdapter da = new SqlDataAdapter(com);
        //    DataTable dt = new DataTable();

        //    con.Open();
        //    da.Fill(dt);
        //    con.Close();
        //    //Bind EmpModel generic list using dataRow     
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        EmpList.Add(
        //            new Employee
        //            {
        //                Pid = Convert.ToInt32(dr["PId"]),
        //                FirstName = Convert.ToString(dr["FirstName"]),

        //            }
        //            );
        //    }

        //    return EmpList;
        //}

        //public bool AddEmployee(Employee obj)
        //{
        //    connection();
        //    SqlCommand com = new SqlCommand("Insert", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Address", obj.Address);

        //    con.Open();
        //    int i = com.ExecuteNonQuery();
        //    con.Close();
        //    if (i >= 1)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool UpdateEmployee(Employee obj)
        //{
        //    connection();
        //    SqlCommand com = new SqlCommand("UpdateEmpDetails", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@Address", obj.Address);
        //    con.Open();
        //    int i = com.ExecuteNonQuery();
        //    con.Close();
        //    if (i >= 1)
        //    {

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteEmployee(int Id)
        //{
        //    connection();
        //    SqlCommand com = new SqlCommand("DeleteEmpById", con);
        //    com.CommandType = CommandType.StoredProcedure;
        //    com.Parameters.AddWithValue("@EmpId", Id);

        //    con.Open();
        //    int i = com.ExecuteNonQuery();
        //    con.Close();
        //    if (i >= 1)
        //    {
        //            return true;
        //    }
        //    else
        //    {
        //         return false;
        //    }
        //}
    }
}