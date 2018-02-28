using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LetStartSomethingNew.Models.GeneralMaster
{
    public class GeneralMaster
    {
        public class UserGroupRights
        {
            public int Pid { get; set; }
            public int UserGroupXid { get; set; }
            public int ModuleXid { get; set; }
            public string PageName { get; set; }
            public string LinkName { get; set; }
            public string RightHeader { get; set; }
            public string AllowYN { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }


            public List<UserGroupRights> ListUserGroupRights { get; set; }

            public UserGroupRights()
            { }

        }

        public class Paging
        {
            private int maxrows=4;

            public int MaxRows
            {
                get { return maxrows; } 
                set { maxrows = value; }
            }
            public int CurrentPageIndex { get; set; }
            public int PageCount { get; set; }
        }


        public class Notes
        {
            public int Pid { get; set; }
            public string NotesName { get; set; }
            public string EntityType { get; set; }
            public int EntityXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string HTMLYN { get; set; }
            public string PrivateYN { get; set; }
            
        }


        public class DiscountType
        {
            public int Pid { get; set; }
            [Required(ErrorMessage = "DiscountTypeName is required")]
            public string DiscountTypeName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int Companyxid { get; set; }
            [Range(0, Int64.MaxValue, ErrorMessage = "SequenceNumber Should be Numeric")]
            public int Sequence { get; set; }
            public string Code { get; set; }

            public DiscountType()
            {
            }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public DiscountType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }
            public List<DiscountType> listDiscountType { get; set; }
        }

        public class RoomType
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string RoomTypeName { get; set; }
            public Nullable<int> MaxNoPpl { get; set; }
            public Nullable<int> MaxNoChild { get; set; }
            public Nullable<int> MaxNoChildFull { get; set; }
            public char AnyYesNo { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> QuickbedRoomTypeXid { get; set; }

            public List<RoomType> listRoomType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public RoomType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Activity
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string ActivityName { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }

            public List<Activity> listActivity { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Activity(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }
        }

        public class AddressType
        {
            public int Pid { get; set; }
            public string AddressTypeName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<AddressType> listAddressType { get; set; }
            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public AddressType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }





        public class Bank
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string BankName { get; set; }
            public string BankBranch { get; set; }
            public string GuichetCode { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Bank> listBank { get; set; }
            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Bank(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class BookingNote
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string NoteFor { get; set; }
            public string Note { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<BookingNote> listBookingNote { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public BookingNote(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }


        public class CardType
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string CardTypeName { get; set; }
            public Nullable<int> Length { get; set; }
            public string CCChargesYN { get; set; }
            public Nullable<decimal> CCCharges { get; set; }
            public string CCChargeApplyTo { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<CardType> listCardType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public CardType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class ClientChain  //ClientChain
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string ClientChainName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ClientChain> listClientChain { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ClientChain(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Airport
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string AirportName { get; set; }
            public Nullable<int> CityXid { get; set; }
            public Nullable<int> DescriptionXid { get; set; }
            public Nullable<int> AirlineXid { get; set; }
            public Nullable<int> CountryXid { get; set; }
            public string Psuedo { get; set; }
            public Nullable<decimal> GmtTimeDifference { get; set; }
            public Nullable<int> LatitudeDegree { get; set; }
            public Nullable<int> LatitudeMin { get; set; }
            public string LatitudeDirection { get; set; }
            public Nullable<int> LongitudeDegree { get; set; }
            public Nullable<int> LongitudeMin { get; set; }
            public string LongitudeDirection { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Airport> listAirport { get; set; }


        }

        public class VM_Airport
        {
            //Added Extra
            public string CountryName { get; set; }
            public string CityName { get; set; }

            public Airport Airport_Values { get; set; }
            public VM_Airport()
            {
                Airport_Values = new Airport();
            }

            public VM_Airport(int pid, string code, string airport, string country, string city, DateTime lastedit)
            {
                Airport_Values = new Airport();
                this.Airport_Values.Pid = pid;
                this.Airport_Values.Code = code;
                this.Airport_Values.AirportName = airport;
                this.Airport_Values.LastEdit = lastedit;

                this.CountryName = country;
                this.CityName = city;
            }
            public List<VM_Airport> listVMAirport { get; set; }
        }


        public class Country
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string CountryName { get; set; }
            public string DialingCode { get; set; }
            public string Taxable { get; set; }
            public string VatCode { get; set; }
            public string DateFormat { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> CurrencyXid { get; set; }

            public List<Country> listCountry { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Country(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }


        public class City
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string CityName { get; set; }
            public string GridReference { get; set; }
            public string Airports { get; set; }
            public int CountryXid { get; set; }
            public Nullable<int> CountyXid { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string RelatedAirportCode { get; set; }
            public string MappedYN { get; set; }

            public List<City> listCity { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public City(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Currency
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string CurrencyName { get; set; }
            public string CurrencySymbol { get; set; }
            public string DefaultCurrency { get; set; }
            public string NominalCode { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Currency> listCurrency { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Currency(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }


        public class TradeFairsTypes
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string TradeFairsTypeName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<TradeFairsTypes> listTradeFairsType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public TradeFairsTypes(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Facility
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string FacilityName { get; set; }
            public string Belongsto { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Facility> listFacility { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Facility(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class FinancialYear
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string FinancialYearName { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string CostCenter { get; set; }

            public List<FinancialYear> listFinancialYear { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public FinancialYear(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class HolidayDuration
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string HolidayDurationName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<HolidayDuration> listHolidayDuration { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public HolidayDuration(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class HolidayType
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string HolidayTypeName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<HolidayType> listHolidayType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public HolidayType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class HotelStandard
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string StandardName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string FromTable { get; set; }
            public Nullable<int> Xid { get; set; }
            public string ImagePath { get; set; }
            public Nullable<int> IOLStarRating { get; set; }
            public string IllusionsStandard { get; set; }

            public List<HotelStandard> listHotelStandard { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public HotelStandard(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }


        public class HotelChain
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string HotelChainsName { get; set; }
            public Nullable<int> PriorityXid { get; set; }
            public Nullable<int> SourceXid { get; set; }
            public string Status { get; set; }
            public string CommunicationMethod { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> hotelgroupxid { get; set; }

            public List<HotelChain> listHotelChain { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public HotelChain(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }


        public class InspectionCriteria
        {

            public int Pid { get; set; }
            public string InspectionCriteriaName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<InspectionCriteria> listInspectionCriteria { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public InspectionCriteria(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Language
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string LanguageName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Language> listLanguage { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Language(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Market
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string MarketName { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Market> listMarket { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Market(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class MealPlan
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string MealPlanName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<MealPlan> listMealPlan { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public MealPlan(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Nationality
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string NationalityName { get; set; }
            public string AnyYN { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> MarketXid { get; set; }

            public List<Nationality> listNationality { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Nationality(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class PaymentSchedules
        {

            public int Pid { get; set; }
            public string PaymentSchedulesName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> MarketXid { get; set; }

            public List<PaymentSchedules> listPaymentSchedules { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public PaymentSchedules(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class PaymentType
        {

            public int Pid { get; set; }
            public string PaymentTypeName { get; set; }
            public string YesNo { get; set; }
            public string NominalCode { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public string BelongsTo { get; set; }
            public string M_PaymentType { get; set; }
            public string LocationCode { get; set; }

            public List<PaymentType> listPaymentType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public PaymentType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class LogisticPickupType
        {

            public int Pid { get; set; }
            public string PickupType { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public string ShowBookingEngine { get; set; }
            public string ArrivalPoint { get; set; }

            public List<LogisticPickupType> listLogisticPickupType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public LogisticPickupType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class CrmPriority
        {
            public int Pid { get; set; }
            public string Priority { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<CrmPriority> listCrmPriority { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public CrmPriority(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class PropertyType
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string PropertyTypeName { get; set; }
            public Nullable<int> SupplementXid { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<PropertyType> listPropertyType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public PropertyType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Reason
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string ReasonName { get; set; }
            public string BelongsTo { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Reason> listReason { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Reason(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class ReportingState
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string ReportingStateName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ReportingState> listReportingState { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ReportingState(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Season
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string SeasonName { get; set; }
            public Nullable<DateTime> FromDate { get; set; }
            public int FinancialYearXid { get; set; }
            public Nullable<DateTime> ToDate { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public Season()
            {
            }
            public VM_Season SeasonValues { get; set; }
            public Season(VM_Season PassSeasonValues)
            {
                this.SeasonValues = PassSeasonValues;
            }
            public List<Season> listSeason { get; set; }
        }

        public class VM_Season
            {
            public string FinancialYear { get; set; }
            public List<VM_Season> listVMSeason { get; set; }
            }



        public class Source
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string  SourceName { get; set; }
            public string EntityType { get; set; }
            public string BelongsTo { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public Nullable<int> ProductXid { get; set; }
            public string Status { get; set; }
            public string migyn { get; set; }

            public List<Source> listSource { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Source(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Status
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string StatusName { get; set; }
            public string ParentId { get; set; }
            public string Colour { get; set; }
            public string StatusEntity { get; set; }
            public string ReasonYN { get; set; }
            public string SendYN { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string Movercolour { get; set; }

            public List<Status> listStatus { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Status(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Supplement
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string SupplementName { get; set; }
            public Nullable<int> SupplementTypeXid { get; set; }
            public Nullable<int> StandardXid { get; set; }
            public string BelongsTo { get; set; }
            public Nullable<int> CurrencyXid { get; set; }
            public Nullable<decimal> Rate { get; set; }
            public string PerWhat { get; set; }
            public string PNPH { get; set; }
            public string Taxable { get; set; }
            public string VatCode { get; set; }
            public string CommissionableYN { get; set; }
            public Nullable<int> MealPlanXid { get; set; }
            public string ShowOnRateScreenYN { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Supplement> listSupplement { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Supplement(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class SupplementType
        {

            public int Pid { get; set; }
            public string SupplementTypeName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string OTASupplementTYpe { get; set; }

            public List<SupplementType> listSupplementType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public SupplementType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class Tax
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string TaxName { get; set; }
            public string ActiveYN { get; set; }
            public int ParentId { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Tax> listTax { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Tax(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Title
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string TitleName { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public Nullable<int> Sequence { get; set; }
            public string Gender { get; set; }

            public List<Title> listTitle { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Title(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Company
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string CompanyName { get; set; }
            public string CompanyAddress { get; set; }
            public string Email { get; set; }
            public string AccountsEmail { get; set; }
            public string Fax { get; set; }
            public string Tel { get; set; }
            public string WwwAddress { get; set; }
            public string NominalCode { get; set; }
            public string CostCentre { get; set; }

            public string PostCode { get; set; }
            public string AgentCode { get; set; }
            public string ConsultantName { get; set; }
            public string Channel { get; set; }
            public string SubChannel { get; set; }
            public string Version { get; set; }
            public string BookingNotificationEmailAddress { get; set; }
            public Nullable<decimal> creditLimit { get; set; }
            public string UsePGYN { get; set; }
            public string IOLHotelVersion { get; set; }
            public string IOLServiceVersion { get; set; }
            public string InSecureCreditYN { get; set; }
            public string BookingSummaryUsePGYN { get; set; }
            public string MarkupBasedOnGrossOrNetMargin { get; set; }

            public Nullable<int> EquivalentCurrencyXid { get; set; }
            public Nullable<int> CountryXid { get; set; }
            public Nullable<int> CityXid { get; set; }
            public Nullable<int> ODCompanyXid { get; set; }

            public Nullable<int> MultiplePGValue { get; set; }
            public Nullable<int> Reminder1 { get; set; }
            public Nullable<int> Reminder2 { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            
            public List<Company> listCompany { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Company(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Department
        {

                public int Pid { get; set; }
            public string Code { get; set; }
            public string DepartmentName { get; set; }
            public string BelongsTo { get; set; }
            public string ThirdPartyInterfaceCode { get; set; }
            public string PrinterName { get; set; }
            public string ThirdPartyInterfaceLogin { get; set; }

            public string ThirdPartyInterfacePassword { get; set; }
            public string OpsAdminMailID { get; set; }
            public string Document_Header { get; set; }

            public string Payment_Details { get; set; }
            public string FaxEmail { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Department> listDepartment { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Department(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class Designation
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string DesignationName { get; set; }
            public string BelongsTo { get; set; }
            public Nullable<int> TemId { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Designation> listDesignation { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Designation(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class DMCSystemConfiguration
        {


            public int Pid { get; set; }
            public string InboudOROutBound { get; set; }
            public string SMTPServer { get; set; }
            public string SMTPServerPort { get; set; }
            public string SendUsing { get; set; }
            public string SMTPConnectionTimeout { get; set; }
            public string DomainName { get; set; }
            public string SMTPUserName { get; set; }
            public string SMTPPassword { get; set; }
            public string InterfaceYN { get; set; }

            public string HotelWebServiceUrl { get; set; }
            public string ServiceWebServiceUrl { get; set; }
            public string ImageDomain { get; set; }
            public string SMTPSenderEmail { get; set; }
            public string IWTXYesOrNo { get; set; }

            public Nullable<int> CompanyXid { get; set; }

            public Nullable<int> SMTPEnableSSL { get; set; }
            public Nullable<decimal> BookingFee { get; set; }
            public Nullable<int> ChildAgeFrom { get; set; }
            public Nullable<int> ChildAgeTo { get; set; }

            public List<DMCSystemConfiguration> listDMCSystemConfiguration { get; set; }


            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public DMCSystemConfiguration(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class ImageLibrary
        {

            public int Pid { get; set; }
            public Nullable<int> ImageLibraryCategoryXid { get; set; }
            public Nullable<int> OriginalFormatXid { get; set; }
            public Nullable<int> SlideNumber { get; set; }
            public string ThumbnailPath { get; set; }
            public string ThumbnailZoomPath { get; set; }
            public string HiResPath { get; set; }
            public string HiResLayoutPath { get; set; }
            public string HiResCompressedPath { get; set; }
            public string WebPath { get; set; }
            public string Keywords { get; set; }

            public string Description { get; set; }
            public Nullable<DateTime> DateTaken { get; set; }
            public string TakenBy { get; set; }

            public Nullable<DateTime> ExpiryDate { get; set; }
            public string Status { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ImageLibrary> listImageLibrary { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ImageLibrary(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Depot
        {

                public int Pid { get; set; }
            public string Code { get; set; }
            public string DepotName { get; set; }
            public string Address { get; set; }
            public Nullable<int> CountryXid { get; set; }
            public Nullable<int> CityXid { get; set; }
            public Nullable<int> SupplierXid { get; set; }
            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Depot> listDepot { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Depot(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class ContractingGroup
        {
            public int Pid { get; set; }
            public string CompanyCode { get; set; }
            public string CompanyName { get; set; }
            public string Address { get; set; }

            public Nullable<int> CityXid { get; set; }
            public string PostCode { get; set; }
            public string TaxID { get; set; }
            public string OTAOutputSet { get; set; }
            public string Document { get; set; }

            public string ReportFooterText { get; set; }

            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ContractingGroup> listContractingGroup { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ContractingGroup(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class TblTariff
        {

            public int Pid { get; set; }

            public string AmountOrPercentage { get; set; }
            public string DefaultYN { get; set; }
            public string TariffName { get; set; }

            public string Operator { get; set; }
            public string ChargeOn { get; set; }

            public Nullable<decimal> Value { get; set; }
            public Nullable<int> TariffXid { get; set; }
            public Nullable<int> CurrencyXid { get; set; }

            public Nullable<int> FromPax { get; set; }
            public Nullable<int> ToPax { get; set; }
            public string TariffDependencyMatrix { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<TblTariff> listTblTariff { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public TblTariff(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }


        public class TblTariffMarkets
        { 
            public int Pid { get; set; }
            public Nullable<int> MarketXid { get; set; }
            public Nullable<int> TariffXid { get; set; }
            public Nullable<DateTime> FromDate { get; set; }
            public Nullable<DateTime> ToDate { get; set; }
            public string AmountOrPercentage { get; set; }

            public Nullable<decimal> Value { get; set; }
            public Nullable<int> CurrencyXid { get; set; }

            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<TblTariffMarkets> listTblTariffMarkets { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public TblTariffMarkets(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }




        public class Client
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string ClientName { get; set; }
            public Nullable<int> ClientChainXid { get; set; }
            public string LegalName { get; set; }
            public string ResistrationNo { get; set; }
            public Nullable<int> PriorityXid { get; set; }
            public Nullable<int> LanguageXid { get; set; }
            public Nullable<decimal> CreditLimit { get; set; }
            public Nullable<decimal> ForwardBookingsCreditLimit { get; set; }
            public Nullable<decimal> PastBookingsCreditLimit { get; set; }
            public Nullable<int> NationalityXid { get; set; }
            public Nullable<int> DefaultCurrencyXid { get; set; }
            public Nullable<int> InvoiceDays { get; set; }
            public Nullable<int> CreditDays { get; set; }
            public Nullable<int> MarketXid { get; set; }
            public Nullable<int> SourceXid { get; set; }
            public Nullable<int> SubStatusXid { get; set; }
            public Nullable<int> SubMarketXid { get; set; }
            public Nullable<int> DefaultTariffXid { get; set; }
            public Nullable<int> OnlineBookingCutOffDays { get; set; }
            public Nullable<int> DateFormat { get; set; }
            public Nullable<int> DefaultExpiryDays { get; set; }
            public Nullable<int> B2BExpiryDays { get; set; }
            public Nullable<int> reminderdays1 { get; set; }
            public Nullable<int> reminderdays2 { get; set; }
            public Nullable<int> TaxApplicableCountryXid { get; set; }
            public Nullable<int> IncreaseCancellationDays { get; set; }
            public Nullable<DateTime> ExtractDate { get; set; }
            public int ParentXid { get; set; }
            public string ReceiveOtherInformation { get; set; }
            public string SubscribeToThirdParty { get; set; }
            public string Status { get; set; }
            public string ClientType { get; set; }
            public string ShowCommOnInv { get; set; }
            public string TaxRecoverableYN { get; set; }
            public string VatNumber { get; set; }
            public string VatRegistrationNo { get; set; }
            public string NominalCode { get; set; }
            public string PrepayReqd { get; set; }
            public string wwwAddress { get; set; }
            public string CommunicationMethod { get; set; }
            public string HeadOfficeYN { get; set; }
            public string CommissionYN { get; set; }
            public string PrivateYN { get; set; }
            public string PastBookingsBasedOn { get; set; }
            public string UseClientLogoYN { get; set; }
            public string AutoInvoicingYN { get; set; }
            public string AutoInvoiceLogic { get; set; }
            public string AutoInvoicePBPC { get; set; }
            public string SameAsBuyCurrencyYN { get; set; }
            public string ShowMsbeYN { get; set; }
            public string ExportedFileName { get; set; }
            public string SlabOrTariff { get; set; }
            public string SupplierBatchSenderAddress { get; set; }
            public string SupplierBatchSenderName { get; set; }
            public string ShowClientNameOnSupplierDocYN { get; set; }
            public string ShowAllocationLogicYN { get; set; }
            public string ScheduleDay { get; set; }
            public string BookingNotificationEmailAddress { get; set; }
            public string ExtractYN { get; set; }
            public string ExportToAccounts { get; set; }
            public string AutoCancelYN { get; set; }
            public string AutoReminderYN { get; set; }
            public string ClientInSecureCreditYN { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<Client> listClient { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Client(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class Airline
        {
            public int Pid { get; set; }
            public string Code { get; set; }
            public string AirlineName { get; set; }
            public string TicketingCode { get; set; }

            public string Logo { get; set; }
            public string Thumbnail { get; set; }
            public string DomesticOrInternational { get; set; }
            public string ViaSameDay { get; set; }

            public Nullable<int> CountryXid { get; set; }
            public Nullable<int> ViaNumberOfHours { get; set; }
            public Nullable<int> TicketingFareBasisCodeXid { get; set; }
            public Nullable<int> TicketingTourCodeXid { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            
            public List<Airline> listAirline { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public Airline(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }


        }

        public class ResourceType
        {

            public int Pid { get; set; }
            public string Code { get; set; }
            public string ResourceName { get; set; }
            public string ResourceTypeName { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ResourceType> listResourceType { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ResourceType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class HumanResource
        {
            public int Pid { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Id { get; set; }
            public string Category { get; set; }
            public string Address { get; set; }
            public string TelNo { get; set; }
            public string MobileNo { get; set; }
            public string Email { get; set; }
            public string FaxNo { get; set; }
            public string PassportNo { get; set; }
            public string AvailableYN { get; set; }
            public string Status { get; set; }
            public string PassNo { get; set; }
            public string Rate { get; set; }

            public Nullable<DateTime> Doj { get; set; }
            public Nullable<DateTime> Dob { get; set; }
            public Nullable<DateTime> VisaExpiryDate { get; set; }
            public Nullable<DateTime> PassValidityPeriod { get; set; }

            public Nullable<int> ResourceTypeXid { get; set; }
            public Nullable<int> DesignationXid { get; set; }
            public Nullable<int> LanguageDtlXid { get; set; }
            public Nullable<int> SupplierXid { get; set; }

            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public HumanResource()
            {
            }

            public VM_HumanResource HumanResourceValues { get; set; }
            public HumanResource(VM_HumanResource PassHumanResourceValues)
            {
                this.HumanResourceValues = PassHumanResourceValues;
            }
            public List<HumanResource> listHumanResource { get; set; }
        }

        public class VM_HumanResource
        {
            public string ResourceTypeName { get; set; }
            public string DesignationName { get; set; }

            public List<VM_HumanResource> listVMHumanResource { get; set; }
        }



        public class ResourceVehicleDtls
        {
            public int Pid { get; set; }
            public int ResourceTypeXid { get; set; }
            public string RegistrationNo { get; set; }
            public string Capacity { get; set; }
            public string Milage { get; set; }
            public string Ingarage { get; set; }
            public string PlateNo { get; set; }
            public string VehicleMake { get; set; }
            public string FuelTankCapacity { get; set; }
            public Nullable<int> VehicleTypeXid { get; set; }
            public string status { get; set; }
            public Nullable<DateTime> ExpiaryDate { get; set; }
            public string ChasisNumber { get; set; }
            public Nullable<int> SupplierXid { get; set; }

            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<ResourceVehicleDtls> listResourceVehicleDtls { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public ResourceVehicleDtls(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class LogisticVehicleType

        {


            public int Pid { get; set; }
            public string Code { get; set; }
            public string VehicleType { get; set; }
            public Nullable<int> Capacity { get; set; }
            public Nullable<int> ParentvehicleTypeXid { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<LogisticVehicleType> listLogisticVehicleType { get; set; }


            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public LogisticVehicleType(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }



        }

        public class LogisticPickupArea
        {

            public int Pid { get; set; }
            public string PickupArea { get; set; }
            public Nullable<int> CityXid { get; set; }
            public Nullable<int> CountryXid { get; set; }
            public Nullable<int> ZoneXid { get; set; }
            public string ActiveYN { get; set; }

            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }
            public string Code { get; set; }

            public List<LogisticPickupArea> listLogisticPickupArea { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public LogisticPickupArea(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }

        public class LogisticJourneyTimes
        {
            public int Pid { get; set; }
            public Nullable<int> CityXid { get; set; }
            public Nullable<int> ZoneXid { get; set; }
            public string JourneyType { get; set; }
            public string FromEntityType { get; set; }
            public Nullable<int> FromEntityXid { get; set; }
            public string ToEntityType { get; set; }
            public Nullable<int> ToEntityXid { get; set; }
            public Nullable<DateTime> FromDate { get; set; }
            public Nullable<DateTime> ToDate { get; set; }
            public string ValidOn { get; set; }


            public Nullable<int> NotesXid { get; set; }
            public DateTime LastEdit { get; set; }
            public int LastEditByXid { get; set; }
            public int CompanyXid { get; set; }

            public List<LogisticJourneyTimes> listLogisticJourneyTimes { get; set; }

            public UserGroupRights Usergrouprights { get; set; }
            public Paging PagingValues { get; set; }

            public LogisticJourneyTimes(Paging passpagingvalues, UserGroupRights passusergrouprights)
            {
                this.PagingValues = passpagingvalues;
                this.Usergrouprights = passusergrouprights;
            }
            public string NotesDescription { get; set; }

        }
    }
}