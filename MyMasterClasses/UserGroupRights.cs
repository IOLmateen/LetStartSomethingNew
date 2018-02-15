using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMasterClasses
{
  public  class UserGroupRights
    {
        //        Pid ID
        //UserGroupXid ID
        //ModuleXid ID
        //PageName varchar
        //LinkName varchar
        //RightHeader varchar
        //AllowYN char
        //NotesXid    ID
        //LastEdit    LastEdit
        //LastEditByXid   LastEditByXid
        //CompanyXid  ID

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
}
