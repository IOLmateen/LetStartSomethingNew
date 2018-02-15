using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace LetStartSomethingNew.Models.GeneralMaster
{
    public class Employee
    {
//        Pid int no	4	10   	0    	no
//FirstName   varchar no	100	     	     	no
//LastName    varchar no	100	     	     	yes
//DepartmentXid   int no	4	10   	0    	no
//DesignationXid  int no	4	10   	0    	no
//Address varchar no	255	     	     	yes
//CountryXid  int no	4	10   	0    	yes
//CountyXid   int no	4	10   	0    	yes
//CityXid int no	4	10   	0    	yes
//PostCode    char no	10	     	     	yes
//Email   varchar no	100	     	     	yes
//Tel char no	20	     	     	yes
//Fax char no	20	     	     	yes
//Mobile  char no	20	     	     	yes
//Other   char no	20	     	     	yes
//GenerateBooking char no	1	     	     	yes
//UserInitials    varchar no	50	     	     	yes
//Status  char no	1	     	     	yes
//MaxPurchaseAmt  money no	8	19   	4    	yes
//NotesXid    int no	4	10   	0    	yes
//Signature   text no	16	     	     	yes
//LastEdit    datetime no	8	     	     	no
//LastEditByXid   int no	4	10   	0    	no
//CompanyXid  int no	4	10   	0    	no
//tempid  int no	4	10   	0    	yes
//AWAHandler  tinyint no	1	3    	0    	yes
//ConsultantXid   int no	4	10   	0    	yes

        public int Pid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentXid { get; set; }
        public int DesignationXid { get; set; }
        public string Address { get; set; }
        public Nullable<int> CountryXid { get; set; }
        public Nullable<int> CountyXid { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string Other { get; set; }
        public char GenerateBooking { get; set; }
        public string UserInitials { get; set; }
        public char Status { get; set; }
        public decimal MaxPurchaseAmt { get; set; }
        public Nullable<int> NotesXid { get; set; }
        public string Signature { get; set; }
        public DateTime LastEdit { get; set; }
        public int LastEditByXid { get; set; }
        public int CompanyXid { get; set; }
        public int tempid { get; set; }
        public Nullable<int> AWAHandler { get; set; }
        public Nullable<int> ConsultantXid { get; set; }

    }
}