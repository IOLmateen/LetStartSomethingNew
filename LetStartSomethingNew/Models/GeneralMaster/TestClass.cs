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
            //DataTable dt = objBaseDataLayer.getDALDiscountTypeByPrefix(prefix).Tables[0];
            DataTable dt = objBaseDataLayer.getDALGetDiscountTypeById(-1,"P",prefix);
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
            DataTable dt = objBaseDataLayer.getDALGetDiscountTypeById(id,"E","");

            objDiscountType.DiscountTypeName = Convert.ToString(dt.Rows[0]["DiscountType"]);
            objDiscountType.Sequence = Convert.ToInt32(dt.Rows[0]["Sequence"]);
            objDiscountType.LastEditByXid = Convert.ToInt32(dt.Rows[0]["LastEditByXid"]);
            objDiscountType.Companyxid = Convert.ToInt32(dt.Rows[0]["Companyxid"]);
            objDiscountType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if(objDiscountType.NotesXid != -1 )
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
            objBaseDataLayer.getDALGetDiscountTypeById(id,"D","");
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

            if(model.NotesXid != -1 )
            { 
                if (model.NotesDescription != null)
                {
                    objDiscountType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objDiscountType.NotesXid = NotesChanges(objNotes.Pid, objDiscountType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid,"E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objDiscountType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objDiscountType.NotesXid = NotesChanges(objNotes.Pid, objDiscountType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
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

        //****************************************

        //public GeneralMaster.RoomType DisplayRoomType()
        //{
        //    GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("RoomType").Tables[0];
        //    objRoomType.listRoomType = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.RoomType
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               RoomTypeName = dr["RoomType"].ToString(),
        //                               MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
        //                               LastEdit = Convert.ToDateTime(dr["lastEdit"])
        //                           }
        //                       ).ToList();
        //    return objRoomType;
        //}
        internal GeneralMaster.RoomType DisplayRoomType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                                select new GeneralMaster.RoomType
                                                {
                                                    Pid = Convert.ToInt32(dr["Pid"]),
                                                    RoomTypeName = dr["RoomType"].ToString(),
                                                    MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                                    LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                                }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;
        }

        internal object SearchRoomType(string prefix)
        {
            
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.RoomType ARoomType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;

        }

        internal GeneralMaster.RoomType ARoomType(GeneralMaster.RoomType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "A");

            return objRoomType;

        }

        internal GeneralMaster.RoomType ERoomType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");

            
            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.RoomType ERoomType(GeneralMaster.RoomType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.RoomType DRoomType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        //******************************************
        #region Activity
        //public GeneralMaster.Activity DisplayActivity()
        //{
        //    GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Activity").Tables[0];
        //    objActivity.listActivity = (from DataRow dr in dt.Rows
        //                        select new GeneralMaster.Activity
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            Code = dr["Code"].ToString(),
        //                            ActivityName = Convert.ToString(dr["Activity"]),
        //                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
        //                        }
        //                       ).ToList();
        //    return objActivity;
        //}


        internal GeneralMaster.Activity DisplayActivity(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayActivity");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Activity", searchPid).Tables[0];

            objActivity.listActivity = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Activity
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = dr["Code"].ToString(),
                                            ActivityName = Convert.ToString(dr["Activity"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objActivity.PagingValues.MaxRows)
                                .Take(objActivity.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objActivity.PagingValues.MaxRows));
            objActivity.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objActivity.PagingValues.CurrentPageIndex = currPage;

            return objActivity;

        }

        internal object SearchActivity(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetActivityById(-1, "P", prefix);
            var listActivity = dt.AsEnumerable().Where(x => x.Field<string>("Activity").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Activity
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            ActivityName = x.Field<string>("Activity")
                                        }).ToList();

            return listActivity;
        }

        internal GeneralMaster.Activity AActivity()
        {
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            return objActivity;
        }

        internal GeneralMaster.Activity AActivity(GeneralMaster.Activity model)
        {
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objActivity.Pid = objNotes.Pid = -1;
            objActivity.Code = model.Code;
            objActivity.ActivityName = model.ActivityName;

            objActivity.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objActivity.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objActivity.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objActivity.NotesXid = NotesChanges(objNotes.Pid, objActivity.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyActivity(objActivity.Pid, objActivity.Code, objActivity.ActivityName, objActivity.NotesXid.GetValueOrDefault(-1),
                                                      objActivity.LastEditByXid, objActivity.CompanyXid, "A");

            return objActivity;

        }

        internal GeneralMaster.Activity EActivity(int pid)
        {
            GeneralMaster.Activity objActivity  = new GeneralMaster.Activity();
            DataTable dt = objBaseDataLayer.getDALGetActivityById(pid, "E", "");


            objActivity.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objActivity.Code = dt.Rows[0]["Code"].ToString();
            objActivity.ActivityName = dt.Rows[0]["ActivityName"].ToString();
            objActivity.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objActivity.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objActivity.NotesXid != -1)
            {
                objActivity.NotesDescription = GetNotesById(objActivity.NotesXid.GetValueOrDefault(-1));
            }
            return objActivity;

        }

        internal GeneralMaster.Activity EActivity(GeneralMaster.Activity model)
        {
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objActivity.Pid = model.Pid;
            objActivity.Code = model.Code;
            objActivity.ActivityName = model.ActivityName;

            objActivity.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objActivity.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objActivity.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objActivity.NotesXid = NotesChanges(objNotes.Pid, objActivity.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objActivity.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objActivity.NotesXid = NotesChanges(objNotes.Pid, objActivity.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyActivity(objActivity.Pid, objActivity.Code, objActivity.ActivityName, objActivity.NotesXid.GetValueOrDefault(-1),
                                                      objActivity.LastEditByXid, objActivity.CompanyXid, "E");

            return objActivity;

        }

        internal GeneralMaster.Activity DActivity(int pid)
        {
            GeneralMaster.Activity objActivity = new GeneralMaster.Activity();
            objBaseDataLayer.getDALGetActivityById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objActivity;
        }



        #endregion
        #region AddressType
        //public GeneralMaster.AddressType DisplayAddressType()
        //{
        //    GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("AddressType").Tables[0];
        //    objAddressType.listAddressType = (from DataRow dr in dt.Rows
        //                        select new GeneralMaster.AddressType
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            AddressTypeName = dr["addressType"].ToString(),
        //                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
        //                        }
        //                       ).ToList();
        //    return objAddressType;
        //}


        internal GeneralMaster.AddressType DisplayAddressType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayAddressType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("AddressType", searchPid).Tables[0];

            objAddressType.listAddressType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.AddressType
                                        {
                                            //Pid = Convert.ToInt32(dr["Pid"]),
                                            //RoomTypeName = dr["RoomType"].ToString(),
                                            //MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            //LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objAddressType.PagingValues.MaxRows)
                                .Take(objAddressType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objAddressType.PagingValues.MaxRows));
            objAddressType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objAddressType.PagingValues.CurrentPageIndex = currPage;

            return objAddressType;

        }

        internal object SearchAddressType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetAddressTypeById(-1, "P", prefix);
            var listobjAddressType = dt.AsEnumerable().Where(x => x.Field<string>("AddressType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.AddressType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            AddressTypeName = x.Field<string>("AddressType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.AddressType AAddressType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.AddressType AAddressType(GeneralMaster.AddressType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.AddressType EAddressType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.AddressType EAddressType(GeneralMaster.AddressType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.AddressType DAddressType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region Bank
        //public GeneralMaster.Bank DisplayBank()
        //{
        //    GeneralMaster.Bank objBank = new GeneralMaster.Bank();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Bank").Tables[0];
        //    objBank.listBank = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.Bank
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               Code = Convert.ToString(dr["Code"]),
        //                               BankName= Convert.ToString(dr["addressType"]),
        //                               BankBranch = Convert.ToString(dr["BankBranch"]),
        //                               GuichetCode = Convert.ToString(dr["GuichetCode"]),
        //                               LastEdit = Convert.ToDateTime(dr["lastEdit"])
        //                           }
        //                       ).ToList();
        //    return objBank;
        //}

        internal GeneralMaster.Bank DisplayBank(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchBank(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Bank ABank()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Bank ABank(GeneralMaster.Bank model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Bank EBank(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Bank EBank(GeneralMaster.Bank model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Bank DBank(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region BookingNote
        //public GeneralMaster.BookingNote DisplayBookingNote()
        //{
        //    GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("BookingNote").Tables[0];
        //    objBookingNote.listBookingNote = (from DataRow dr in dt.Rows
        //                    select new GeneralMaster.BookingNote
        //                    {
        //                        Pid = Convert.ToInt32(dr["Pid"]),
        //                        Code = Convert.ToString(dr["Code"]),
        //                        NoteFor = Convert.ToString(dr["NoteFor"]),
        //                        Note = Convert.ToString(dr["Note"])
        //                    }
        //                       ).ToList();
        //    return objBookingNote;
        //}

        internal GeneralMaster.BookingNote DisplayBookingNote(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchBookingNote(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.BookingNote ABookingNote()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.BookingNote ABookingNote(GeneralMaster.BookingNote model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.BookingNote EBookingNote(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.BookingNote EBookingNote(GeneralMaster.BookingNote model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.BookingNote DBookingNote(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }





        #endregion
        #region CardType
        //public GeneralMaster.CardType DisplayCardType()
        //{
        //    GeneralMaster.CardType objCardType = new GeneralMaster.CardType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("CardType").Tables[0];
        //    objCardType.listCardType = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.CardType
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               CardTypeName = Convert.ToString(dr["CardType"]),
        //                               Length = Convert.ToInt32(dr["Length"]),
        //                               CCChargesYN = Convert.ToString(dr["CCChargesYN"]),
        //                               CCCharges = Convert.ToDecimal(dr["CCCharges"]),
        //                               CCChargeApplyTo = Convert.ToString(dr["CCChargeApplyTo"]),
        //                               LastEdit = Convert.ToDateTime(dr["lastEdit"])
        //                           }
        //                       ).ToList();
        //    return objCardType;
        //}
        internal GeneralMaster.CardType DCardType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        internal GeneralMaster.CardType ECardType(GeneralMaster.CardType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.CardType ECardType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.CardType ACardType(GeneralMaster.CardType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.CardType ACardType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal object SearchCardType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.CardType DisplayCardType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }


        #endregion
        #region ClientChain
        //public GeneralMaster.ClientChain DisplayClientChain()
        //{
        //    GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ClientChain").Tables[0];
        //    objClientChain.listClientChain = (from DataRow dr in dt.Rows
        //                        select new GeneralMaster.ClientChain
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            Code = Convert.ToString(dr["Code"]),
        //                            ClientChainName = Convert.ToString(dr["ClientChain"])
        //                        }
        //                       ).ToList();
        //    return objClientChain;
        //}
        internal GeneralMaster.ClientChain DisplayClientChain(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchClientChain(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ClientChain AClientChain()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.ClientChain AClientChain(GeneralMaster.ClientChain model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ClientChain EClientChain(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.ClientChain EClientChain(GeneralMaster.ClientChain model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ClientChain DClientChain(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }






        #endregion
        #region Currency
        //public GeneralMaster.Currency DisplayCurrency()
        //{
        //    GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Currency").Tables[0];
        //    objCurrency.listCurrency = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.Currency
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               Code = Convert.ToString(dr["Code"]),
        //                               CurrencyName = Convert.ToString(dr["Currency"]),
        //                               CurrencySymbol = Convert.ToString(dr["CurrencySymbol"]),
        //                               DefaultCurrency = Convert.ToString(dr["DefaultCurrency"]),
        //                               NominalCode = Convert.ToString(dr["NominalCode"]),
        //                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                           }
        //                       ).ToList();
        //    return objCurrency;
        //}

        internal GeneralMaster.Currency DisplayCurrency(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchCurrency(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Currency ACurrency()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Currency ACurrency(GeneralMaster.Currency model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Currency ECurrency(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Currency ECurrency(GeneralMaster.Currency model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Currency DCurrency(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region Tradefairtypes
        //public GeneralMaster.TradeFairsTypes DisplayTradeFairsTypes()
        //{
        //    GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("TradeFairsTypes").Tables[0];
        //    objTradeFairsTypes.listTradeFairsType = (from DataRow dr in dt.Rows
        //                        select new GeneralMaster.TradeFairsTypes
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            Code = Convert.ToString(dr["Code"]),
        //                            TradeFairsTypeName = Convert.ToString(dr["TradeFairsType"]),
        //                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                        }
        //                       ).ToList();
        //    return objTradeFairsTypes;
        //}

        internal GeneralMaster.TradeFairsTypes DisplayTradeFairsTypes(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchTradeFairsTypes(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.TradeFairsTypes ATradeFairsTypes()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.TradeFairsTypes ATradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.TradeFairsTypes ETradeFairsTypes(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.TradeFairsTypes ETradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.TradeFairsTypes DTradeFairsTypes(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region Facility
        //public GeneralMaster.Facility DisplayFacility()
        //{
        //    GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Facility").Tables[0];
        //    objFacility.listFacility = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.Facility
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                   FacilityName = Convert.ToString(dr["Facility"]),
        //                                   Belongsto = Convert.ToString(dr["BelongsTo"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objFacility;
        //}

        internal GeneralMaster.Facility DisplayFacility(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchFacility(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Facility AFacility()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Facility AFacility(GeneralMaster.Facility model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Facility EFacility(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Facility EFacility(GeneralMaster.Facility model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Facility DFacility(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region Financialyear
        //public GeneralMaster.FinancialYear DisplayFinancialYear()
        //{
        //    GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("FinancialYear").Tables[0];
        //    objFinancialYear.listFinancialYear = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.FinancialYear
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                   FinancialYearName = Convert.ToString(dr["FinancialYear"]),
        //                                   FromDate = Convert.ToDateTime(dr["FromDate"]),
        //                                   ToDate = Convert.ToDateTime(dr["ToDate"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objFinancialYear;
        //}
        internal GeneralMaster.FinancialYear DisplayFinancialYear(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchFinancialYear(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.FinancialYear AFinancialYear()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.FinancialYear AFinancialYear(GeneralMaster.FinancialYear model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.FinancialYear EFinancialYear(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.FinancialYear EFinancialYear(GeneralMaster.FinancialYear model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.FinancialYear DFinancialYear(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region HolidayDuration
        //public GeneralMaster.HolidayDuration DisplayHolidayDuration()
        //{
        //    GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("HolidayDuration").Tables[0];
        //    objHolidayDuration.listHolidayDuration = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.HolidayDuration
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                   HolidayDurationName = Convert.ToString(dr["HolidayDuration"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objHolidayDuration;
        //}


        internal GeneralMaster.HolidayDuration DisplayHolidayDuration(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchHolidayDuration(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.HolidayDuration AHolidayDuration()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.HolidayDuration AHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.HolidayDuration EHolidayDuration(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.HolidayDuration EHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.HolidayDuration DHolidayDuration(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region HolidayType
        //public GeneralMaster.HolidayType DisplayHolidayType()
        //{
        //    GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("HolidayType").Tables[0];
        //    objHolidayType.listHolidayType = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.HolidayType
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                   HolidayTypeName = Convert.ToString(dr["HolidayType"])  
        //                               }
        //                       ).ToList();
        //    return objHolidayType;
        //}

        internal GeneralMaster.HolidayType DisplayHolidayType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchHolidayType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.HolidayType AHolidayType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.HolidayType AHolidayType(GeneralMaster.HolidayType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.HolidayType EHolidayType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.HolidayType EHolidayType(GeneralMaster.HolidayType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.HolidayType DHolidayType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region HotelStandard
        //public GeneralMaster.HotelStandard DisplayHotelStandard()
        //{
        //    GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("HotelStandard").Tables[0];
        //    objHotelStandard.listHotelStandard = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.HotelStandard
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   StandardName = Convert.ToString(dr["Standard"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objHotelStandard;
        //}
        internal GeneralMaster.HotelStandard DisplayHotelStandard(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchHotelStandard(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.HotelStandard AHotelStandard()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.HotelStandard AHotelStandard(GeneralMaster.HotelStandard model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.HotelStandard EHotelStandard(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.HotelStandard ERHotelStandard(GeneralMaster.HotelStandard model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.HotelStandard DHotelStandard(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region HotelChain
        //public GeneralMaster.HotelChain DisplayHotelChain()
        //{
        //    GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("HotelChain").Tables[0];
        //    objHotelChain.listHotelChain = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.HotelChain
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                    HotelChainsName =  Convert.ToString(dr["HotelChain"])

        //                               }
        //                       ).ToList();
        //    return objHotelChain;
        //}

        internal GeneralMaster.HotelChain DisplayHotelChain(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchHotelChain(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.HotelChain AHotelChain()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.HotelChain AHotelChain(GeneralMaster.HotelChain model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.HotelChain EHotelChain(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.HotelChain EHotelChain(GeneralMaster.HotelChain model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.HotelChain DHotelChain(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region InspectionCriteria
        //public GeneralMaster.InspectionCriteria DisplayInspectionCriteria()
        //{
        //    GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("InspectionCriteria").Tables[0];
        //    objInspectionCriteria.listInspectionCriteria = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.InspectionCriteria
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   InspectionCriteriaName = Convert.ToString(dr["InspectionCriteria"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objInspectionCriteria;
        //}

        internal GeneralMaster.InspectionCriteria DisplayInspectionCriteria(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchInspectionCriteria(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.InspectionCriteria AInspectionCriteria()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.InspectionCriteria AInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.InspectionCriteria EInspectionCriteria(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.InspectionCriteria EInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.InspectionCriteria DInspectionCriteria(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Language
        //public GeneralMaster.Language DisplayLanguage()
        //{
        //    GeneralMaster.Language objLanguage = new GeneralMaster.Language();

        //       DataTable dt = objBaseDataLayer.getDALGeneralMaster("Language").Tables[0];
        //       objLanguage.listLanguage = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.Language
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   Code = Convert.ToString(dr["Code"]),
        //                                   LanguageName = Convert.ToString(dr["language"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objLanguage;
        //}

        internal GeneralMaster.Language DisplayLanguage(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchLanguage(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Language ALanguage()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Language ALanguage(GeneralMaster.Language model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Language ELanguage(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Language ELanguage(GeneralMaster.Language model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Language DLanguage(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Market
        //public GeneralMaster.Market DisplayMarket()
        //{
        //    GeneralMaster.Market objMarket = new GeneralMaster.Market();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Market").Tables[0];
        //    objMarket.listMarket = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.Market
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   MarketName = Convert.ToString(dr["market"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objMarket;
        //}


        internal GeneralMaster.Market DisplayMarket(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchMarket(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Market AMarket()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Market AMarket(GeneralMaster.Market model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Market EMarket(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Market EMarket(GeneralMaster.Market model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Market DMarket(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region MealPlan

        //public GeneralMaster.MealPlan DisplayMealPlan()
        //{
        //    GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("MealPlan").Tables[0];
        //    objMealPlan.listMealPlan = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.MealPlan
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   MealPlanName = Convert.ToString(dr["Mealplan"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objMealPlan;
        //}


        internal GeneralMaster.MealPlan DisplayMealPlan(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchMealPlan(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.MealPlan AMealPlan()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.MealPlan AMealPlan(GeneralMaster.MealPlan model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.MealPlan EMealPlan(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.MealPlan EMealPlan(GeneralMaster.MealPlan model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.MealPlan DMealPlan(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Nationality
        //public GeneralMaster.Nationality DisplayNationality()
        //{
        //    GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Nationality").Tables[0];
        //    objNationality.listNationality = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.Nationality
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   NationalityName = Convert.ToString(dr["nationality"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objNationality;
        //}

        internal GeneralMaster.Nationality DisplayNationality(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchNationality(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Nationality ANationality()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Nationality ANationality(GeneralMaster.Nationality model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Nationality ENationality(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Nationality ENationality(GeneralMaster.Nationality model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Nationality DNationality(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }




        #endregion
        #region PaymentSchedule
        //public GeneralMaster.PaymentSchedules DisplayPaymentSchedules()
        //{
        //    GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("PaymentSchedules").Tables[0];
        //    objPaymentSchedules.listPaymentSchedules = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.PaymentSchedules
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   PaymentSchedulesName = Convert.ToString(dr["PaymentSchedule"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"]),
        //                               }
        //                       ).ToList();
        //    return objPaymentSchedules;
        //}
        internal GeneralMaster.PaymentSchedules DisplayPaymentSchedules(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchPaymentSchedules(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.PaymentSchedules APaymentSchedules()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.PaymentSchedules APaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.PaymentSchedules EPaymentSchedules(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.PaymentSchedules EPaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.PaymentSchedules DPaymentSchedules(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region paymentType
        //public GeneralMaster.PaymentType DisplayPaymentType()
        //{
        //    GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("PaymentType").Tables[0];
        //    objPaymentType.listPaymentType = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.PaymentType
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   PaymentTypeName =Convert.ToString(dr["PaymentType"]),
        //                                   NominalCode = Convert.ToString(dr["nominalCode"]),
        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objPaymentType;
        //}
        internal GeneralMaster.PaymentType DisplayPaymentType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchPaymentType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.PaymentType APaymentType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.PaymentType APaymentType(GeneralMaster.PaymentType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.PaymentType EPaymentType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.PaymentType EPaymentType(GeneralMaster.PaymentType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.PaymentType DPaymentType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Logisticpickuptype
        //public GeneralMaster.LogisticPickupType DisplayLogisticPickupType()
        //{
        //    GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticPickupType").Tables[0];
        //    objLogisticPickupType.listLogisticPickupType = (from DataRow dr in dt.Rows
        //                               select new GeneralMaster.LogisticPickupType
        //                               {
        //                                   Pid = Convert.ToInt32(dr["Pid"]),
        //                                   PickupType = Convert.ToString(dr["Pickuptype"]),
        //                                   ShowBookingEngine =Convert.ToString(dr["showbookingengine"]),
        //                                   ArrivalPoint = Convert.ToString(dr["ArrivalPOint"]),

        //                                   LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                               }
        //                       ).ToList();
        //    return objLogisticPickupType;
        //}

        internal GeneralMaster.LogisticPickupType DisplayLogisticPickuptype(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchLogisticPickupType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.LogisticPickupType ALogisticPickupType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.LogisticPickupType ALogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.LogisticPickupType ELogisticPickupType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.LogisticPickupType ELogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.LogisticPickupType DLogisticPickupType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Crmpriority
        ////Pending
        //public GeneralMaster.CrmPriority DisplayCrmPriority()
        //{
        //    GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("CrmPriority").Tables[0];
        //    objCrmPriority.listCrmPriority = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.CrmPriority
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               Priority = Convert.ToString(dr["priority"]),
        //                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                           }
        //                       ).ToList();
        //    return objCrmPriority;
        //}
        internal GeneralMaster.CrmPriority DisplayCrmPriority(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchCrmPriority(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.CrmPriority ACrmPriority()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.CrmPriority ACrmPriority(GeneralMaster.CrmPriority model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.CrmPriority ECrmPriority(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.CrmPriority ECrmPriority(GeneralMaster.CrmPriority model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.CrmPriority DCrmPriority(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region PropertyType
        //public GeneralMaster.PropertyType DisplayPropertyType()
        //{
        //    GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("PropertyType").Tables[0];
        //    objPropertyType.listPropertyType = (from DataRow dr in dt.Rows
        //                            select new GeneralMaster.PropertyType
        //                            {
        //                                Pid = Convert.ToInt32(dr["Pid"]),
        //                                PropertyTypeName = Convert.ToString(dr["PropertyType"]),
        //                                LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                            }
        //                       ).ToList();
        //    return objPropertyType;
        //}
        internal GeneralMaster.PropertyType DisplayPropertyType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchPropertyType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }
        internal GeneralMaster.PropertyType APropertyType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }
        internal GeneralMaster.PropertyType APropertyType(GeneralMaster.PropertyType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.PropertyType EPropertyType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.PropertyType EPropertyType(GeneralMaster.PropertyType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.PropertyType DPropertyType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Reason
        //public GeneralMaster.Reason DisplayReason()
        //{
        //    GeneralMaster.Reason objReason = new GeneralMaster.Reason();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Reason").Tables[0];
        //    objReason.listReason = (from DataRow dr in dt.Rows
        //                      select new GeneralMaster.Reason
        //                      {
        //                          Pid = Convert.ToInt32(dr["Pid"]),
        //                          ReasonName = Convert.ToString(dr["Reason"]),
        //                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                      }
        //                       ).ToList();
        //    return objReason;
        //}
        internal GeneralMaster.Reason DisplayReason(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchReason(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Reason AReason()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Reason AReason(GeneralMaster.Reason model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Reason EReason(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Reason EReason(GeneralMaster.Reason model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Reason DReason(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region ReportingState
        //public GeneralMaster.ReportingState DisplayReportingState()
        //{
        //    GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ReportingState").Tables[0];
        //    objReportingState.listReportingState = (from DataRow dr in dt.Rows
        //                              select new GeneralMaster.ReportingState
        //                              {
        //                                  Pid = Convert.ToInt32(dr["Pid"]),
        //                                  Code = Convert.ToString(dr["Code"]),
        //                                  ReportingStateName = Convert.ToString("ReportingState"),
        //                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                              }
        //                       ).ToList();
        //    return objReportingState;
        //}

        internal GeneralMaster.ReportingState DisplayReportingState(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchReportingState(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }
        internal GeneralMaster.ReportingState AReportingState()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }



        internal GeneralMaster.ReportingState AReportingState(GeneralMaster.ReportingState model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ReportingState EReportingState(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.ReportingState EReportingState(GeneralMaster.ReportingState model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ReportingState DReportingState(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Season
        //public GeneralMaster.Season DisplaySeason()  //MATZVM
        //{
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Season").Tables[0];
        //    //objSeason.listSeason = (from DataRow dr in dt.Rows
        //    //                  select new GeneralMaster.Season
        //    //                  {
        //    //                      Pid = Convert.ToInt32(dr["Pid"]),
        //    //                      Code = Convert.ToString(dr["code"]),
        //    //                      SeasonName = Convert.ToString(dr["season"]),
        //    //                      FromDate = Convert.ToDateTime(dr["FromDate"]),
        //    //                      ToDate = Convert.ToDateTime(dr["ToDate"]),
        //    //                      //    FinancialYearXid
        //    //                      LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //    //                  }
        //    //                   ).ToList();

        //    GeneralMaster.VM_Season vmseason = new GeneralMaster.VM_Season();
        //    GeneralMaster.Season objSeason = new GeneralMaster.Season(vmseason);

        //    objSeason.listSeason = (from DataRow dr in dt.Rows
        //                            select new GeneralMaster.Season
        //                            {
        //                                Pid = Convert.ToInt32(dr["Pid"]),
        //                                Code = Convert.ToString(dr["code"]),
        //                                SeasonName = Convert.ToString(dr["season"]),
        //                                FromDate = Convert.ToDateTime(dr["FromDate"]),
        //                                ToDate = Convert.ToDateTime(dr["ToDate"]),
        //                                //    FinancialYearXid
        //                                LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                            }
        //                       ).ToList();
        //    objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //                                                           select new GeneralMaster.VM_Season
        //                                                           {
        //                                                              FinancialYear  = Convert.ToString(dr["FinancialYear"]),
        //                                                           }
        //                    ).ToList();
        //    return objSeason;
        //}
        internal GeneralMaster.Season DisplaySeason(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchSeason(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Season ASeason()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Season ASeason(GeneralMaster.Season model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Season ESeason(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Season ESeason(GeneralMaster.Season model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Season DSeason(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Source
        //public GeneralMaster.Source DisplaySource()
        //{
        //    GeneralMaster.Source objSource = new GeneralMaster.Source();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Source").Tables[0];
        //    objSource.listSource = (from DataRow dr in dt.Rows
        //                      select new GeneralMaster.Source
        //                      {
        //                          Pid = Convert.ToInt32(dr["Pid"]),
        //                          SourceName = Convert.ToString(dr["Source"]),
        //                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                      }
        //                       ).ToList();
        //    return objSource;
        //}
        internal GeneralMaster.Source DisplaySource(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchSource(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Source ASource()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Source ASource(GeneralMaster.Source model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Source ESource(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Source ESource(GeneralMaster.Source model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Source DSource(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Status
        //public GeneralMaster.Status DisplayStatus()
        //{
        //    GeneralMaster.Status objStatus = new GeneralMaster.Status();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Status").Tables[0];
        //    objStatus.listStatus = (from DataRow dr in dt.Rows
        //                      select new GeneralMaster.Status
        //                      {
        //                          Pid = Convert.ToInt32(dr["Pid"]),
        //                          Code = Convert.ToString(dr["Code"]),
        //                          StatusName = Convert.ToString(dr["Status"]),
        //                          Colour = Convert.ToString(dr["Color"]),
        //                          StatusEntity = Convert.ToString(dr["statusEntity"]),
        //                          ReasonYN = Convert.ToString(dr["ReasonYN"]),
        //                          SendYN = Convert.ToString(dr["SendYN"]),
        //                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                      }
        //                       ).ToList();
        //    return objStatus;
        //}
        internal GeneralMaster.Status DisplayStatus(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;
        }

        internal object SearchStatus(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Status AStatus()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Status AStatus(GeneralMaster.Status model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Status EStatus(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Status EStatus(GeneralMaster.Status model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Status DStatus(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Supplement
        //public GeneralMaster.Supplement DisplaySupplement() //MATZVM
        //{
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Supplement").Tables[0];
        //    GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
        //    objSupplement.listSupplement = (from DataRow dr in dt.Rows
        //                          select new GeneralMaster.Supplement
        //                          {
        //                              Pid = Convert.ToInt32(dr["Pid"]),
        //                              Code = Convert.ToString(dr["Code"]),
        //                              SupplementName = Convert.ToString(dr["SupplementName"]),
        //                              //SupplementTypeXid
        //                              BelongsTo = Convert.ToString(dr["belongsTo"]),
        //                              //CurrencyXid
        //                              PerWhat = Convert.ToString(dr["perWhat"]),
        //                              PNPH = Convert.ToString(dr["PNPH"]),
        //                              Taxable = Convert.ToString(dr["taxable"]),
        //                              //MealPlanXid
        //                              VatCode = Convert.ToString(dr["vatcode"]),
        //                              ShowOnRateScreenYN = Convert.ToString(dr["ShowOnRateScreenYN"]),
        //                              LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                          }
        //                       ).ToList();

        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();

        //    return objSupplement;
        //}

        internal GeneralMaster.Supplement DisplaySupplement(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchSupplement(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Supplement ASupplement()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Supplement ASupplement(GeneralMaster.Supplement model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Supplement ESupplement(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Supplement ESupplement(GeneralMaster.Supplement model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Supplement DSupplement(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region SupplementType
        //public GeneralMaster.SupplementType DisplaySupplementType()
        //{
        //    GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();

        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("SupplementType").Tables[0];
        //    objSupplementType.listSupplementType = (from DataRow dr in dt.Rows
        //                              select new GeneralMaster.SupplementType
        //                              {
        //                                  Pid = Convert.ToInt32(dr["Pid"]),
        //                                  SupplementTypeName = Convert.ToString(dr["SupplementType"]),
        //                                  LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                              }
        //                       ).ToList();
        //    return objSupplementType;
        //}
        internal GeneralMaster.SupplementType DisplaySupplementType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchSupplementType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.SupplementType ASupplementType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Supplement ASupplementType(GeneralMaster.SupplementType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.SupplementType ESupplementType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.SupplementType ESupplementType(GeneralMaster.SupplementType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.SupplementType DSupplementType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Tax
        //public GeneralMaster.Tax DisplayTax()
        //{
        //    GeneralMaster.Tax objTax = new GeneralMaster.Tax();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Tax").Tables[0];
        //    objTax.listTax = (from DataRow dr in dt.Rows
        //                   select new GeneralMaster.Tax
        //                   {
        //                       Pid = Convert.ToInt32(dr["Pid"]),
        //                       Code = Convert.ToString(dr["Code"]),
        //                       TaxName = Convert.ToString(dr["Tax"]),
        //                       ActiveYN = Convert.ToString(dr["ActiveYN"]),
        //                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                   }
        //                       ).ToList();
        //    return objTax;
        //}

        internal GeneralMaster.Tax DisplayTax(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchTax(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Tax ATax()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Tax ATax(GeneralMaster.Tax model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Tax ETax(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.Tax ETax(GeneralMaster.Tax model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Tax DTax(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Title
        //public GeneralMaster.Title DisplayTitle()
        //{
        //    GeneralMaster.Title objTitle = new GeneralMaster.Title();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Title").Tables[0];
        //    objTitle.listTitle = (from DataRow dr in dt.Rows
        //                     select new GeneralMaster.Title
        //                     {
        //                         Pid = Convert.ToInt32(dr["Pid"]),
        //                         TitleName = Convert.ToString(dr["Title"]),
        //                         Sequence = Convert.ToInt32(dr["Sequence"]),
        //                         LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                     }
        //                       ).ToList();
        //    return objTitle;
        //}
        internal GeneralMaster.Title DisplayTitle(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchTitle(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Title ATitle()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Title ATitle(GeneralMaster.Title model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Title ETitle(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Title ETitle(GeneralMaster.Title model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Title DTitle(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Company
        //public GeneralMaster.Company DisplayCompany() //MATZVM
        //{
        //    GeneralMaster.Company objCompany = new GeneralMaster.Company();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Company").Tables[0];
        //    objCompany.listCompany = (from DataRow dr in dt.Rows
        //                       select new GeneralMaster.Company
        //                       {
        //                           Pid = Convert.ToInt32(dr["Pid"]),
        //                           Code = Convert.ToString(dr["Code"]),
        //                           CompanyName = Convert.ToString(dr["Company"]),
        //                           CompanyAddress = Convert.ToString(dr["CompanyAddress"]),
        //                           Email = Convert.ToString(dr["Email"]),
        //                           AccountsEmail = Convert.ToString(dr["AccountsEmail"]),
        //                           Fax = Convert.ToString(dr["Fax"]),
        //                           Tel = Convert.ToString(dr["Tel"]),
        //                           //CityXid
        //                           BookingNotificationEmailAddress = Convert.ToString(dr["BookingNotificationEmailAddress"]),
        //                           UsePGYN = Convert.ToString(dr["UsePGYN"]),
        //                           LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                       }
        //                       ).ToList();
        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objCompany;
        //}
        internal GeneralMaster.Company DisplayCompany(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchCompany(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Company ACompany()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Company ACompany(GeneralMaster.Company model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Company ECompany(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Company ECompany(GeneralMaster.Company model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Company DCompany(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Department
        //public GeneralMaster.Department DisplayDepartment()
        //{
        //    GeneralMaster.Department objDepartment = new GeneralMaster.Department();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Department").Tables[0];
        //    objDepartment.listDepartment = (from DataRow dr in dt.Rows
        //                          select new GeneralMaster.Department
        //                          {
        //                              Pid = Convert.ToInt32(dr["Pid"]),
        //                              DepartmentName = Convert.ToString(dr["Department"]),
        //                              BelongsTo = Convert.ToString(dr["belongsto"]),
        //                              LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                          }
        //                       ).ToList();
        //    return objDepartment;
        //}

        internal GeneralMaster.Department DisplayDepartment(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchDepartment(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Department ADepartment()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Department ADepartment(GeneralMaster.Department model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Department EDepartment(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Department EDepartment(GeneralMaster.Department model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Department DDepartment(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Designation
        //public GeneralMaster.Designation DisplayDesignation()
        //{
        //    GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Designation").Tables[0];
        //    objDesignation.listDesignation = (from DataRow dr in dt.Rows
        //                           select new GeneralMaster.Designation
        //                           {
        //                               Pid = Convert.ToInt32(dr["Pid"]),
        //                               DesignationName = Convert.ToString(dr["Designation"]),
        //                               BelongsTo = Convert.ToString(dr["BelongsTo"]),
        //                               LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                           }
        //                       ).ToList();
        //    return objDesignation;
        //}

        internal GeneralMaster.Designation DisplayDesignation(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchDesignation(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Designation ADesignation()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Designation ADesignation(GeneralMaster.Designation model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Designation EDesignation(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Designation EDesignation(GeneralMaster.Designation model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Designation DDesignation(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region DMCConfiguration
        //public GeneralMaster.DMCSystemConfiguration DisplayDMCSystemConfiguration()
        //{
        //    GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("DMCSystemConfiguration").Tables[0];
        //    objDMCSystemConfiguration.listDMCSystemConfiguration = (from DataRow dr in dt.Rows
        //                                      select new GeneralMaster.DMCSystemConfiguration
        //                                      {
        //                                          Pid = Convert.ToInt32(dr["Pid"]),
        //                                          SMTPServer = Convert.ToString(dr["SMTPServer"]),
        //                                          SendUsing = Convert.ToString(dr["SendUsing"]),
        //                                          SMTPServerPort = Convert.ToString(dr["SMTPServerPort"]),
        //                                          SMTPConnectionTimeout = Convert.ToString(dr["SMTPConnectionTimeout"]),
        //                                          SMTPUserName = Convert.ToString(dr["SMTPUserName"]),
        //                                          SMTPPassword = Convert.ToString(dr["SMTPPassword"]),
        //                                          ImageDomain = Convert.ToString(dr["ImageDomain"]),
        //                                          SMTPEnableSSL = Convert.ToInt32(dr["SMTPEnableSSL"]),
        //                                          SMTPSenderEmail = Convert.ToString(dr["SMTPSenderEmail"]),
        //                                          IWTXYesOrNo = Convert.ToString(dr["IWTXYesOrNo"]),
        //                                      }
        //                       ).ToList();
        //    return objDMCSystemConfiguration;
        //}
        internal GeneralMaster.DMCSystemConfiguration DisplayDMCSystemConfiguration(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchDMCSystemConfiguration(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.DMCSystemConfiguration DDMCSystemConfiguration(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region ImageLibrary
        //public GeneralMaster.ImageLibrary DisplayImageLibrary() //MATZVM
        //{
        //    GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ImageLibrary").Tables[0];
        //    objImageLibrary.listImageLibrary = (from DataRow dr in dt.Rows
        //                            select new GeneralMaster.ImageLibrary
        //                            {
        //                                Pid = Convert.ToInt32(dr["Pid"]),
        //                                // ImageLibraryCategoryXid  
        //                                Description = Convert.ToString(dr["Description"]),
        //                                SlideNumber = Convert.ToInt32(dr["SlideNumber"]),
        //                                ThumbnailPath = Convert.ToString(dr["thumbnailpath"]),
        //                                LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                            }
        //                       ).ToList();
        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();

        //    return objImageLibrary;
        //}

        internal GeneralMaster.ImageLibrary DisplayImageLibrary(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchImageLibrary(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ImageLibrary AImageLibrary()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.ImageLibrary EImageLibrary(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.ImageLibrary EImageLibrary(GeneralMaster.ImageLibrary model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ImageLibrary AImageLibrary(GeneralMaster.ImageLibrary model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ImageLibrary DImageLibrary(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Depot
        //public GeneralMaster.Depot DisplayDepot() //MATZVM
        //{
        //    GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Depot").Tables[0];
        //    objDepot.listDepot = (from DataRow dr in dt.Rows
        //                     select new GeneralMaster.Depot
        //                     {
        //                         Pid = Convert.ToInt32(dr["Pid"]),
        //                         Code = Convert.ToString(dr["Code"]),
        //                         DepotName = Convert.ToString(dr["Depot"]),
        //                         Address = Convert.ToString(dr["Address"]),
        //                         //CountryXid
        //                         //CityXid
        //                         //SupplierXid

        //                         LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                     }
        //                       ).ToList();
        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objDepot;
        //}

        internal GeneralMaster.Depot DisplayDepot(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchDepot(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Depot ADepot()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Depot ADepot(GeneralMaster.Depot model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Depot EDepot(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Depot EDepot(GeneralMaster.Depot model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Depot DDepot(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region ContractingGroup
        //public GeneralMaster.ContractingGroup DisplayContractingGroup()
        //{
        //    GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ContractingGroup").Tables[0];
        //    objContractingGroup.listContractingGroup = (from DataRow dr in dt.Rows
        //                                select new GeneralMaster.ContractingGroup
        //                                {
        //                                    Pid = Convert.ToInt32(dr["Pid"]),
        //                                    CompanyCode = Convert.ToString(dr["CompanyCode"]),
        //                                    CompanyName = Convert.ToString(dr["CompanyName"]),
        //                                    TaxID = Convert.ToString(dr["TaxId"]),
        //                                    OTAOutputSet = Convert.ToString(dr["OTAOutputSet"]),
        //                                    LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                                }
        //                       ).ToList();
        //    return objContractingGroup;
        //}

        internal GeneralMaster.ContractingGroup DisplayContractingGroup(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchContractingGroup(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ContractingGroup AContractingGroup()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.ContractingGroup AContractingGroup(GeneralMaster.ContractingGroup model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ContractingGroup EContractingGroup(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.ContractingGroup EContractingGroup(GeneralMaster.ContractingGroup model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ContractingGroup DContractingGroup(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region TblTariff
        //public GeneralMaster.TblTariff DisplayTblTariff()
        //{
        //    GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("TblTariff").Tables[0];
        //    objTblTariff.listTblTariff = (from DataRow dr in dt.Rows
        //                         select new GeneralMaster.TblTariff
        //                         {
        //                             Pid = Convert.ToInt32(dr["Pid"]),
        //                             TariffName = Convert.ToString(dr["Tariff"]),
        //                             DefaultYN = Convert.ToString(dr["Defaultyn"]),
        //                             LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                         }
        //                       ).ToList();
        //    return objTblTariff;
        //}

        internal GeneralMaster.TblTariff DisplayTblTariff(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchTblTariff(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.TblTariff ATblTariff()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.TblTariff ATblTariff(GeneralMaster.TblTariff model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.TblTariff ETblTariff(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.TblTariff ETblTariff(GeneralMaster.TblTariff model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.TblTariff DTblTariff(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region TblTariffMarkets
        //public GeneralMaster.TblTariffMarkets DisplayTblTariffMarkets() //MATZVM
        //{
        //    GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("TblTariffMarkets").Tables[0];
        //    objTblTariffMarkets.listTblTariffMarkets = (from DataRow dr in dt.Rows
        //                                select new GeneralMaster.TblTariffMarkets
        //                                {
        //                                    Pid = Convert.ToInt32(dr["Pid"]),
        //                                    //MarketXid
        //                                    FromDate = Convert.ToDateTime(dr["FromDate"]),
        //                                    ToDate = Convert.ToDateTime(dr["ToDate"]),
        //                                    AmountOrPercentage = Convert.ToString(dr["AmountOrPercentage"]),
        //                                    Value = Convert.ToInt32(dr["value"]),
        //                                    //CurrencyXid
        //                                }
        //                       ).ToList();
        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objTblTariffMarkets;
        //}
        internal GeneralMaster.TblTariffMarkets DisplayTblTariffMarkets(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchTblTariffMarkets(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.TblTariffMarkets ATblTariffMarkets()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.TblTariffMarkets ATblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.TblTariffMarkets ETblTariffMarkets(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.TblTariffMarkets ETblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.TblTariffMarkets DTblTariffMarkets(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region Client
        //public GeneralMaster.Client DisplayClient() //MATZVM
        //{
        //    GeneralMaster.Client objCLient = new GeneralMaster.Client();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Client").Tables[0];
        //    objCLient.listClient = (from DataRow dr in dt.Rows
        //                      select new GeneralMaster.Client
        //                      {
        //                          Pid = Convert.ToInt32(dr["Pid"]),
        //                          Code = Convert.ToString(dr["Code"]),
        //                          ClientType = Convert.ToString(dr["ClientType"]),
        //                          //ClientChainXid
        //                          CommunicationMethod = Convert.ToString(dr["CommunicationMethod"]),
        //                          //DefaultCurrencyXid
        //                          //Passport
        //                          //bank
        //                          //product
        //                          //BookingSummary
        //                          LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                      }
        //                       ).ToList();

        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objCLient;
        //}
        internal GeneralMaster.Client DisplayClient(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchClient(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Client AClient()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Client AClient(GeneralMaster.Client model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Client EClient(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Client EClient(GeneralMaster.Client model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Client DClient(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }



        #endregion
        #region Airline
        //public GeneralMaster.Airline DisplayAirline()
        //{
        //    GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Airline").Tables[0];
        //    objAirline.listAirline = (from DataRow dr in dt.Rows
        //                       select new GeneralMaster.Airline
        //                       {
        //                           Pid = Convert.ToInt32(dr["Pid"]),
        //                           Code = Convert.ToString(dr["Code"]),
        //                           AirlineName = Convert.ToString(dr["Airline"])
        //                       }
        //                       ).ToList();
        //    return objAirline;
        //}
        internal GeneralMaster.Airline DisplayAirline(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchAirline(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.Airline AAirline()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.Airline AAirline(GeneralMaster.Airline model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Airline EAirline(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.Airline EAirline(GeneralMaster.Airline model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.Airline DAirline(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region ResourceType
        //public GeneralMaster.ResourceType DisplayResourceType()
        //{
        //    GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ResourceType").Tables[0];
        //    objResourceType.listResourceType = (from DataRow dr in dt.Rows
        //                            select new GeneralMaster.ResourceType
        //                            {
        //                                Pid = Convert.ToInt32(dr["Pid"]),
        //                                Code = Convert.ToString(dr["Code"]),
        //                                ResourceName = Convert.ToString(dr["Resource"]),
        //                                ResourceTypeName = Convert.ToString(dr["ResourceType"])
        //                            }
        //                       ).ToList();
        //    return objResourceType;
        //}

        internal GeneralMaster.ResourceType DisplayResourceType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchResourceType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ResourceType AResourceType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.ResourceType AResourceType(GeneralMaster.ResourceType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ResourceType EResourceType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.ResourceType EResourceType(GeneralMaster.ResourceType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ResourceType DResourceType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }


        #endregion
        #region HumanResource
        //public GeneralMaster.HumanResource DisplayHumanResource() //MATZVM
        //{
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("HumanResource").Tables[0];

        //    GeneralMaster.VM_HumanResource vmobjhumanrsr = new GeneralMaster.VM_HumanResource();
        //    GeneralMaster.HumanResource objHumanRsr = new GeneralMaster.HumanResource(vmobjhumanrsr);
        // //   GeneralMaster.HumanResource objHumanRsr = new GeneralMaster.HumanResource(new GeneralMaster.VM_HumanResource());

        //    objHumanRsr.listHumanResource = (from DataRow dr in dt.Rows
        //                                     select new GeneralMaster.HumanResource
        //                                     {
        //                                         Pid = Convert.ToInt32(dr["Pid"]),
        //                                         Id = Convert.ToString(dr["Id"]),
        //                                         FirstName = Convert.ToString(dr["FirstName"]),
        //                                         LastName = Convert.ToString(dr["Lastname"]),
        //                                         Category = Convert.ToString(dr["Category"]),
        //                                         Doj = Convert.ToDateTime(dr["Doj"]),
        //                                         MobileNo = Convert.ToString(dr["mobileno"]),
        //                                         Email = Convert.ToString(dr["email"]),
        //                                     }
        //                       ).ToList();

        //    objHumanRsr.HumanResourceValues.listVMHumanResource = (from DataRow dr in dt.Rows
        //                                                           select new GeneralMaster.VM_HumanResource
        //                                                           {
        //                                                               DesignationName = Convert.ToString(dr["Designation"]),
        //                                                               ResourceTypeName = Convert.ToString(dr["ResourceType"])
        //                                                           }
        //                    ).ToList();

        //    return objHumanRsr;
        //}

        internal GeneralMaster.HumanResource DisplayHumanResource(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchHumanResource(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.HumanResource AHumanResource()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.HumanResource AHumanResource(GeneralMaster.HumanResource model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.HumanResource EHumanResource(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.HumanResource EHumanResource(GeneralMaster.HumanResource model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.HumanResource DHumanResource(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }
        #endregion
        #region ResourcevehicleType
        //public GeneralMaster.ResourceVehicleDtls DisplayResourceVehicleDtls() //MATZVM
        //{
        //    GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("ResourceVehicleDtls").Tables[0];
        //    objResourceVehicleDtls.listResourceVehicleDtls = (from DataRow dr in dt.Rows
        //                                   select new GeneralMaster.ResourceVehicleDtls
        //                                   {
        //                                       Pid = Convert.ToInt32(dr["Pid"]),
        //                                       RegistrationNo = Convert.ToString(dr["RegistrationNo"]),
        //                                       //VehicleTypeXid
        //                                       Capacity = Convert.ToString(dr["Capacity"]),
        //                                       Milage = Convert.ToString(dr["Milage"]),
        //                                       Ingarage = Convert.ToString(dr["Ingarage"]),
        //                                       PlateNo = Convert.ToString(dr["plateno"]),
        //                                       VehicleMake = Convert.ToString(dr["vehiclemake"]),
        //                                       FuelTankCapacity = Convert.ToString(dr["FuelTankCapacity"]),
        //                                       //SupplierXid
        //                                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                                   }
        //                       ).ToList();

        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objResourceVehicleDtls;
        //}

        internal GeneralMaster.ResourceVehicleDtls DisplayResourceVehicleDtls(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchResourceVehicleDtls(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.ResourceVehicleDtls DResourceVehicleDtls(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }




        #endregion
        #region LogisticvehicleType
        //public GeneralMaster.LogisticVehicleType DisplayLogisticVehicleType() //MATZVM
        //{
        //    GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticVehicleType").Tables[0];
        //    objLogisticVehicleType.listLogisticVehicleType = (from DataRow dr in dt.Rows
        //                                   select new GeneralMaster.LogisticVehicleType
        //                                   {
        //                                       Pid = Convert.ToInt32(dr["Pid"]),
        //                                       Code = Convert.ToString(dr["Code"]),
        //                                       VehicleType = Convert.ToString(dr["VehicleType"]),
        //                                       Capacity = Convert.ToInt32(dr["Capacity"]),
        //                                       //ParentvehicleTypeXid
        //                                       LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                                   }
        //                       ).ToList();

        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objLogisticVehicleType;
        //}
        internal GeneralMaster.LogisticVehicleType DisplayLogisticVehicleType(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchLogisticVehicleType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.LogisticVehicleType ALogisticVehicleType()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.LogisticVehicleType ALogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.LogisticVehicleType ELogisticVehicleType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;
        }

        internal GeneralMaster.LogisticVehicleType ELogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.LogisticVehicleType DLogisticVehicleType(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }




        #endregion
        #region Logisticpickuparea
        //public GeneralMaster.LogisticPickupArea DisplayLogisticPickupArea() //MATZVM
        //{
        //    GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticPickupArea").Tables[0];
        //    objLogisticPickupArea.listLogisticPickupArea = (from DataRow dr in dt.Rows
        //                                  select new GeneralMaster.LogisticPickupArea
        //                                  {
        //                                      Pid = Convert.ToInt32(dr["Pid"]),
        //                                      PickupArea = Convert.ToString(dr["PickupArea"]),
        //                                      // CityXid
        //                                      // CountryXid
        //                                      ActiveYN = Convert.ToString(dr["ActiveYN"])
        //                                  }
        //                       ).ToList();      
        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objLogisticPickupArea;
        //}

        internal GeneralMaster.LogisticPickupArea DisplayLogisticPickupArea(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchLogisticPickupArea(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.LogisticPickupArea ALogisticPickupArea()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.LogisticPickupArea ALogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.LogisticPickupArea ELogisticPickupArea(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.LogisticPickupArea ELogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.LogisticPickupArea DLogisticPickupArea(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }

        #endregion
        #region LogisticJourneytimes
        //public GeneralMaster.LogisticJourneyTimes DisplayLogisticJourneyTimes() //MATZVM
        //{
        //    GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("LogisticJourneyTimes").Tables[0];
        //    objLogisticJourneyTimes.listLogisticJourneyTimes = (from DataRow dr in dt.Rows
        //                                    select new GeneralMaster.LogisticJourneyTimes
        //                                    {
        //                                        Pid = Convert.ToInt32(dr["Pid"]),
        //                                        //Country
        //                                        //CityXid
        //                                    }
        //                       ).ToList();

        //    //objSeason.SeasonValues.listVMSeason = (from DataRow dr in dt.Rows
        //    //                                       select new GeneralMaster.VM_Season
        //    //                                       {
        //    //                                           FinancialYear = Convert.ToString(dr["FinancialYear"]),
        //    //                                       }
        //    //                ).ToList();
        //    return objLogisticJourneyTimes;
        //}
        internal GeneralMaster.LogisticJourneyTimes DisplayLogisticJourneyTimes(int currPage, int searchPid)
        {
            GeneralMaster.UserGroupRights objUsergrouprights = new GeneralMaster.UserGroupRights();
            getUserSettings(objUsergrouprights, "DisplayRoomType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objRoomType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objRoomType.PagingValues.MaxRows));
            objRoomType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objRoomType.PagingValues.CurrentPageIndex = currPage;

            return objRoomType;

        }

        internal object SearchLogisticJourneyTimes(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("RoomType")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes()
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            return objRoomType;
        }

        internal GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.DiscountTypeName, objRoomType.Sequence, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objRoomType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objRoomType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objRoomType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objRoomType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objRoomType.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objRoomType;

        }

        internal GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = model.Pid;
            objRoomType.RoomTypeName = model.RoomTypeName;
            objRoomType.MaxNoPpl = model.MaxNoPpl;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objRoomType.Pid, objRoomType.RoomTypeName, objRoomType.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objRoomType;

        }

        internal GeneralMaster.LogisticJourneyTimes DLogisticJourneyTimes(int pid)
        {
            GeneralMaster.RoomType objRoomType = new GeneralMaster.RoomType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objRoomType;
        }
        #endregion
        //public List<GeneralMaster.VM_Airport> DisplayAirport()
        //{
        //    DataTable dt = objBaseDataLayer.getDALGeneralMaster("Airport").Tables[0];
        //    GeneralMaster.VM_Airport obj1 = new GeneralMaster.VM_Airport();
        //    List<GeneralMaster.VM_Airport> listobj1 = new List<GeneralMaster.VM_Airport>();

        //    if (dt.Rows.Count > 0)
        //    {
        //       foreach (DataRow dr in dt.Rows)
        //        {
        //            obj1.Airport_Values.Pid = Convert.ToInt32(dr["Pid"]);
        //            obj1.Airport_Values.Code = Convert.ToString(dr["Code"]);
        //            obj1.Airport_Values.AirportName = Convert.ToString(dr["Airport"]);
        //            obj1.CountryName = Convert.ToString(dr["Country"]);
        //            obj1.CityName = Convert.ToString(dr["City"]);
        //            obj1.Airport_Values.LastEdit = Convert.ToDateTime(dr["LastEdit"]);
        //            listobj1.Add( new GeneralMaster.VM_Airport(obj1.Airport_Values.Pid, obj1.Airport_Values.Code,obj1.Airport_Values.AirportName,obj1.CountryName,obj1.CityName,obj1.Airport_Values.LastEdit));
        //        }
        //    }
        //    return listobj1;
        //}


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