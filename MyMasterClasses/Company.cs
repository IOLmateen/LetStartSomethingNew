using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterClasses
{
    public class Company
    {
        public int Pid { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Email { get; set; }
        public string AccountEmail { get; set; }
        public string Fax { get; set; }
        public string Tel { get; set; }
        public string WwwAddress { get; set; }
        public int EquivalentCurrecyXid { get; set; }
        public int CountryXid { get; set; }
        public int CityXid { get; set; }
        public int NotesXid { get; set; }
        public DateTime LastEdit { get; set; }
        public int LastEditByXid { get; set; }
        public string NominalCode { get; set; }
        public string CostCentre { get; set; }
        public string PostCode { get; set; }
        public string AgentCode { get; set; }
        public string ConsultantName { get; set; }
        public string Channel { get; set; }
        public string SubChannel { get; set; }
        public int ODCompanyXid { get; set; }
        public string Version { get; set; }
        public string BookingNotificationEmailAddress { get; set; }
        public decimal Money { get; set; }
        public string UsePGYN { get; set; }
        public int MultiplePGValue { get; set; }
        public int Reminder1 { get; set; }
        public int Reminder2 { get; set; }
        public string IOLHotelVersion { get; set; }
        public string IOLServiceVersion { get; set; }
        public string InSecureCreditYN { get; set; }
        public string BookingSummaryUsePGYN { get; set; }
        public string MarkupBasedOnGrossOrNetMargin { get; set; }

    }
}
