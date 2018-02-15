using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;

namespace LetStartSomethingNew.Models.GeneralMaster
{
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
        public int LastEdit { get; set; }
        public DateTime LastEditByXid { get; set; }
        public int CompanyXid { get; set; }
        public Nullable<int> QuickbedRoomTypeXid { get; set; }


        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string Action { get; set; }

        public List<RoomType> listRoomType { get; set; }


    }
}