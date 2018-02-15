using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterClasses
{
    public class Department
    {
        public int Pid { get; set; }
        public string Code { get; set; }
        public string DepartmentName { get; set; }
        public string BelongsTo { get; set; }
        public int NotesXid { get; set; }
        public string ThirdPartInterfaceCode { get; set; }
        public DateTime LastEdit { get; set; }
        public int LastEditByXid { get; set; }
        public int CompanyXid { get; set; }
        public int PrinterName { get; set; }
        public string ThirdPartyInterfaceLogin { get; set; }
        public string ThirdPartyInterfacePassword { get; set; }
        public string OpsAdminMailID { get; set; }
        public string Document_Header { get; set; }
        public string Payment_Details { get; set; }
        public string FaxEmail { get; set; }

        
    }
}
