using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterClasses
{
   public class Employee
    {
        public int Pid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentXid { get; set; }
        public int DesignationXid { get; set; }
        public int CountryXid { get; set; }
        public int CountyXid { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Other { get; set; }
        public string GenerateBooking { get; set; }
        public string UserInitials { get; set; }
        public string Status { get; set; }
        public decimal MaxPurchaseAmt { get; set; }
        public int NotesXid { get; set; }
        public DateTime LastEdit { get; set; }
        public int LastEditByXid { get; set; }
        public int CompanyXid { get; set; }
        public int TempId { get; set; }
        public int AwaHandler { get; set; }
        public int ConsultantXid { get; set; }
    }
}
