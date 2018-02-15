using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterClasses
{
   public class Country
    {
        public int Pid { get; set; }
        public string Code { get; set; }
        public string CountryName { get; set; }
        public string DailingCode { get; set; }
        public string Taxable { get; set; }
        public string VatCode { get; set; }
        public string DateFormat { get; set; }
        public int NotesXid { get; set; }
        public DateTime LastEdit { get; set; }
        public int LastEditByXid { get; set; }
        public int CompanyXid { get; set; }
        public int CurrencyXid { get; set; }

    }
}
