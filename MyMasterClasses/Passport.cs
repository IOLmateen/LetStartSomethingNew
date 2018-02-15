using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyMasterClasses
{
    public class Passport
    {
        //public int Pid { get; set; }
        //public string EntityType { get; set; }
        //public int EntityXid { get; set; }
        //public string Login { get; set; }
        //public string Password { get; set; }
        //public string NickName { get; set; }
        //public DateTime LastLoggedIn { get; set; }
        //public string IpAddress { get; set; }
        //public int UserGroupXid { get; set; }
        //public string PasswordHint { get; set; }
        //public int NotesXid { get; set; }
        //public DateTime LastEdit { get; set; }
        //public int LastEditByXid { get; set; }
        //public int CompanyXid { get; set; }

        [Display(Name = "Your Login Name")]
        [Required]
        public string LoginName { get; set; }

        [Display(Name = "Your Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public Passport() {
        }
    }
}
