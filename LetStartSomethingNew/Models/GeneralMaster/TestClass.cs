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
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            AddressTypeName = dr["addressType"].ToString(),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
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
            var listAddressType = dt.AsEnumerable().Where(x => x.Field<string>("AddressType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.AddressType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            AddressTypeName = x.Field<string>("AddressType")
                                        }).ToList();

            return listAddressType;

        }

        internal GeneralMaster.AddressType AAddressType()
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            return objAddressType;
        }

        internal GeneralMaster.AddressType AAddressType(GeneralMaster.AddressType model)
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objAddressType.Pid = objNotes.Pid = -1;
            objAddressType.AddressTypeName = model.AddressTypeName;
            
            objAddressType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objAddressType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objAddressType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objAddressType.NotesXid = NotesChanges(objNotes.Pid, objAddressType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyAddressType(objAddressType.Pid, objAddressType.DiscountTypeName, objAddressType.Sequence, objAddressType.NotesXid.GetValueOrDefault(-1),
                                                      objAddressType.LastEditByXid, objAddressType.CompanyXid, "A");

            return objAddressType;

        }

        internal GeneralMaster.AddressType EAddressType(int pid)
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            DataTable dt = objBaseDataLayer.getDALGetAddressTypeById(pid, "E", "");


            objAddressType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objAddressType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objAddressType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objAddressType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objAddressType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objAddressType.NotesXid != -1)
            {
                objAddressType.NotesDescription = GetNotesById(objAddressType.NotesXid.GetValueOrDefault(-1));
            }
            return objAddressType;

        }

        internal GeneralMaster.AddressType EAddressType(GeneralMaster.AddressType model)
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objAddressType.Pid = model.Pid;
            objAddressType.RoomTypeName = model.RoomTypeName;
            objAddressType.MaxNoPpl = model.MaxNoPpl;

            objAddressType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objAddressType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objAddressType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objAddressType.NotesXid = NotesChanges(objNotes.Pid, objAddressType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objAddressType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objAddressType.NotesXid = NotesChanges(objNotes.Pid, objAddressType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyAddressType(objAddressType.Pid, objAddressType.RoomTypeName, objAddressType.MaxNoPpl, objAddressType.NotesXid.GetValueOrDefault(-1),
                                                      objAddressType.LastEditByXid, objAddressType.CompanyXid, "E");

            return objAddressType;

        }

        internal GeneralMaster.AddressType DAddressType(int pid)
        {
            GeneralMaster.AddressType objAddressType = new GeneralMaster.AddressType();
            objBaseDataLayer.getDALGetAddressTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objAddressType;
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
            getUserSettings(objUsergrouprights, "DisplayBank");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Bank objBank = new GeneralMaster.Bank(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Bank", searchPid).Tables[0];

            objBank.listBank = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Bank
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            BankName = Convert.ToString(dr["addressType"]),
                                            BankBranch = Convert.ToString(dr["BankBranch"]),
                                            GuichetCode = Convert.ToString(dr["GuichetCode"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objBank.PagingValues.MaxRows)
                                .Take(objBank.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objBank.PagingValues.MaxRows));
            objBank.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objBank.PagingValues.CurrentPageIndex = currPage;

            return objBank;

        }

        internal object SearchBank(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetBankById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("Bank").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Bank
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Bank")
                                        }).ToList();

            return listRoomType;

        }

        

        internal GeneralMaster.Bank ABank()
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            return objBank;
        }

        internal GeneralMaster.Bank ABank(GeneralMaster.Bank model)
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objBank.Pid = objNotes.Pid = -1;
            objBank.DiscountTypeName = model.DiscountTypeName;
            objBank.Sequence = model.Sequence;

            objBank.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objBank.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objBank.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objBank.NotesXid = NotesChanges(objNotes.Pid, objBank.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyBank(objBank.Pid, objBank.DiscountTypeName, objBank.Sequence, objBank.NotesXid.GetValueOrDefault(-1),
                                                      objBank.LastEditByXid, objBank.CompanyXid, "A");

            return objBank;

        }

        internal GeneralMaster.Bank EBank(int pid)
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            DataTable dt = objBaseDataLayer.getDALGetBankById(pid, "E", "");


            objBank.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objBank.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objBank.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objRoomType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objBank.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objBank.NotesXid != -1)
            {
                objBank.NotesDescription = GetNotesById(objBank.NotesXid.GetValueOrDefault(-1));
            }
            return objBank;

        }

        internal GeneralMaster.Bank EBank(GeneralMaster.Bank model)
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objBank.Pid = model.Pid;
            objBank.RoomTypeName = model.RoomTypeName;
            objBank.MaxNoPpl = model.MaxNoPpl;

            objBank.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objBank.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objBank.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objBank.NotesXid = NotesChanges(objNotes.Pid, objBank.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objBank.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objBank.NotesXid = NotesChanges(objNotes.Pid, objBank.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyBank(objBank.Pid, objBank.RoomTypeName, objBank.MaxNoPpl, objBank.NotesXid.GetValueOrDefault(-1),
                                                      objBank.LastEditByXid, objBank.CompanyXid, "E");

            return objBank;

        }

        internal GeneralMaster.Bank DBank(int pid)
        {
            GeneralMaster.Bank objBank = new GeneralMaster.Bank();
            objBaseDataLayer.getDALGetBankById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objBank;
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
            getUserSettings(objUsergrouprights, "DisplayBookingNote");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("BookingNote", searchPid).Tables[0];

            objBookingNote.listBookingNote = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.BookingNote
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            NoteFor = Convert.ToString(dr["NoteFor"]),
                                            Note = Convert.ToString(dr["Note"])

                                        }
                               ).Skip((currPage - 1) * objBookingNote.PagingValues.MaxRows)
                                .Take(objBookingNote.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objBookingNote.PagingValues.MaxRows));
            objBookingNote.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objBookingNote.PagingValues.CurrentPageIndex = currPage;

            return objBookingNote;

        }

        internal object SearchBookingNote(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetBookingNoteyId(-1, "P", prefix);
            var listBookingNote = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.BookingNote
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            BookingNoteName = x.Field<string>("RoomType")
                                        }).ToList();

            return listBookingNote;

        }

        internal GeneralMaster.BookingNote ABookingNote()
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            return objBookingNote;
        }

        internal GeneralMaster.BookingNote ABookingNote(GeneralMaster.BookingNote model)
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objBookingNote.Pid = objNotes.Pid = -1;
            objBookingNote.DiscountTypeName = model.DiscountTypeName;
            objBookingNote.Sequence = model.Sequence;

            objBookingNote.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objBookingNote.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objBookingNote.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objBookingNote.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyBookingNote(objBookingNote.Pid, objBookingNote.DiscountTypeName, objBookingNote.Sequence, objBookingNote.NotesXid.GetValueOrDefault(-1),
                                                      objBookingNote.LastEditByXid, objBookingNote.Companyxid, "A");

            return objBookingNote;

        }

        internal GeneralMaster.BookingNote EBookingNote(int pid)
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            DataTable dt = objBaseDataLayer.getDALGetBookingNoteById(pid, "E", "");


            objBookingNote.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objBookingNote.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objBookingNote.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objBookingNote.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objBookingNote.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objBookingNote.NotesXid != -1)
            {
                objBookingNote.NotesDescription = GetNotesById(objBookingNote.NotesXid.GetValueOrDefault(-1));
            }
            return objBookingNote;

        }

        internal GeneralMaster.BookingNote EBookingNote(GeneralMaster.BookingNote model)
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objBookingNote.Pid = model.Pid;
            objBookingNote.RoomTypeName = model.RoomTypeName;
            objBookingNote.MaxNoPpl = model.MaxNoPpl;

            objBookingNote.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objBookingNote.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objBookingNote.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objBookingNote.NotesXid = NotesChanges(objNotes.Pid, objBookingNote.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objBookingNote.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objBookingNote.NotesXid = NotesChanges(objNotes.Pid, objBookingNote.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyBookingNote(objBookingNote.Pid, objBookingNote.RoomTypeName, objBookingNote.MaxNoPpl, objBookingNote.NotesXid.GetValueOrDefault(-1),
                                                      objBookingNote.LastEditByXid, objBookingNote.CompanyXid, "E");

            return objBookingNote;

        }

        internal GeneralMaster.BookingNote DBookingNote(int pid)
        {
            GeneralMaster.BookingNote objBookingNote = new GeneralMaster.BookingNote();
            objBaseDataLayer.getDALGetBookingNoteById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objBookingNote;
        }





        #endregion
        #region CardType ???
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
            getUserSettings(objUsergrouprights, "DisplayClientChain");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ClientChain", searchPid).Tables[0];

            objClientChain.listClientChain = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ClientChain
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            ClientChainName = Convert.ToString(dr["ClientChain"])
                                        }
                               ).Skip((currPage - 1) * objClientChain.PagingValues.MaxRows)
                                .Take(objClientChain.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objClientChain.PagingValues.MaxRows));
            objClientChain.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objClientChain.PagingValues.CurrentPageIndex = currPage;

            return objClientChain;

        }

        internal object SearchClientChain(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetClientChainById(-1, "P", prefix);
            var listClientChain = dt.AsEnumerable().Where(x => x.Field<string>("ClientChain").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ClientChain
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            ClientChainName = x.Field<string>("ClientChain")
                                        }).ToList();

            return listClientChain;

        }

        internal GeneralMaster.ClientChain AClientChain()
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            return objClientChain;
        }

        internal GeneralMaster.ClientChain AClientChain(GeneralMaster.ClientChain model)
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objClientChain.Pid = objNotes.Pid = -1;
            objClientChain.DiscountTypeName = model.DiscountTypeName;
            objClientChain.Sequence = model.Sequence;

            objClientChain.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objClientChain.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objClientChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objClientChain.NotesXid = NotesChanges(objNotes.Pid, objClientChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyClientChain(objClientChain.Pid, objClientChain.DiscountTypeName, objClientChain.Sequence, objClientChain.NotesXid.GetValueOrDefault(-1),
                                                      objClientChain.LastEditByXid, objClientChain.Companyxid, "A");

            return objClientChain;

        }

        internal GeneralMaster.ClientChain EClientChain(int pid)
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            DataTable dt = objBaseDataLayer.getDALGetClientChainById(pid, "E", "");


            objClientChain.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objClientChain.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objClientChain.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objClientChain.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objClientChain.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objClientChain.NotesXid != -1)
            {
                objClientChain.NotesDescription = GetNotesById(objClientChain.NotesXid.GetValueOrDefault(-1));
            }
            return objClientChain;

        }

        internal GeneralMaster.ClientChain EClientChain(GeneralMaster.ClientChain model)
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objClientChain.Pid = model.Pid;
            objClientChain.RoomTypeName = model.RoomTypeName;
            objClientChain.MaxNoPpl = model.MaxNoPpl;

            objClientChain.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objClientChain.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objClientChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objClientChain.NotesXid = NotesChanges(objNotes.Pid, objClientChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objClientChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objClientChain.NotesXid = NotesChanges(objNotes.Pid, objClientChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyClientChain(objClientChain.Pid, objClientChain.RoomTypeName, objClientChain.MaxNoPpl, objClientChain.NotesXid.GetValueOrDefault(-1),
                                                      objClientChain.LastEditByXid, objClientChain.CompanyXid, "E");

            return objClientChain;

        }

        internal GeneralMaster.ClientChain DClientChain(int pid)
        {
            GeneralMaster.ClientChain objClientChain = new GeneralMaster.ClientChain();
            objBaseDataLayer.getDALGetClientChainById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objClientChain;
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
            getUserSettings(objUsergrouprights, "DisplayCurrency");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Currency", searchPid).Tables[0];

            objCurrency.listCurrency = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Currency
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            CurrencyName = Convert.ToString(dr["Currency"]),
                                            CurrencySymbol = Convert.ToString(dr["CurrencySymbol"]),
                                            DefaultCurrency = Convert.ToString(dr["DefaultCurrency"]),
                                            NominalCode = Convert.ToString(dr["NominalCode"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objCurrency.PagingValues.MaxRows)
                                .Take(objCurrency.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objCurrency.PagingValues.MaxRows));
            objCurrency.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objCurrency.PagingValues.CurrentPageIndex = currPage;

            return objCurrency;

        }

        internal object SearchCurrency(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetCurrencyById(-1, "P", prefix);
            var listCurrency = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Currency
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            CurrencyName = x.Field<string>("Currency")
                                        }).ToList();

            return listCurrency;

        }

        internal GeneralMaster.Currency ACurrency()
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            return objCurrency;
        }

        internal GeneralMaster.Currency ACurrency(GeneralMaster.Currency model)
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCurrency.Pid = objNotes.Pid = -1;
            objCurrency.DiscountTypeName = model.DiscountTypeName;
            objCurrency.Sequence = model.Sequence;

            objCurrency.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCurrency.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objCurrency.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objCurrency.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyCurrency(objCurrency.Pid, objCurrency.DiscountTypeName, objCurrency.Sequence, objCurrency.NotesXid.GetValueOrDefault(-1),
                                                      objCurrency.LastEditByXid, objCurrency.Companyxid, "A");

            return objCurrency;

        }

        internal GeneralMaster.Currency ECurrency(int pid)
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            DataTable dt = objBaseDataLayer.getDALGetCurrencyById(pid, "E", "");


            objCurrency.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objCurrency.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objCurrency.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objCurrency.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objCurrency.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objCurrency.NotesXid != -1)
            {
                objCurrency.NotesDescription = GetNotesById(objCurrency.NotesXid.GetValueOrDefault(-1));
            }
            return objCurrency;

        }

        internal GeneralMaster.Currency ECurrency(GeneralMaster.Currency model)
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCurrency.Pid = model.Pid;
            objCurrency.RoomTypeName = model.RoomTypeName;
            objCurrency.MaxNoPpl = model.MaxNoPpl;

            objCurrency.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCurrency.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objCurrency.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objCurrency.NotesXid = NotesChanges(objNotes.Pid, objCurrency.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objCurrency.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objCurrency.NotesXid = NotesChanges(objNotes.Pid, objCurrency.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyCurrency(objCurrency.Pid, objCurrency.RoomTypeName, objCurrency.MaxNoPpl, objCurrency.NotesXid.GetValueOrDefault(-1),
                                                      objCurrency.LastEditByXid, objCurrency.CompanyXid, "E");

            return objCurrency;
        }

        internal GeneralMaster.Currency DCurrency(int pid)
        {
            GeneralMaster.Currency objCurrency = new GeneralMaster.Currency();
            objBaseDataLayer.getDALGetCurrencyById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objCurrency;
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
            getUserSettings(objUsergrouprights, "DisplayTradeFairsTypes");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("TradeFairsTypes", searchPid).Tables[0];

            objTradeFairsTypes.listTradeFairsType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.TradeFairsTypes
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objTradeFairsTypes.PagingValues.MaxRows)
                                .Take(objTradeFairsTypes.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objTradeFairsTypes.PagingValues.MaxRows));
            objTradeFairsTypes.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objTradeFairsTypes.PagingValues.CurrentPageIndex = currPage;

            return objTradeFairsTypes;

        }

        internal object SearchTradeFairsTypes(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetTradeFairsTypesById(-1, "P", prefix);
            var listTradeFairsTypes = dt.AsEnumerable().Where(x => x.Field<string>("TradeFairsTypes").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.TradeFairsTypes
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("TradeFairsTypes")
                                        }).ToList();

            return listTradeFairsTypes;

        }

        internal GeneralMaster.TradeFairsTypes ATradeFairsTypes()
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            return objTradeFairsTypes;
        }

        internal GeneralMaster.TradeFairsTypes ATradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTradeFairsTypes.Pid = objNotes.Pid = -1;
            objTradeFairsTypes.DiscountTypeName = model.DiscountTypeName;
            objTradeFairsTypes.Sequence = model.Sequence;

            objTradeFairsTypes.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTradeFairsTypes.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objTradeFairsTypes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objTradeFairsTypes.NotesXid = NotesChanges(objNotes.Pid, objTradeFairsTypes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyTradeFairsTypes(objTradeFairsTypes.Pid, objTradeFairsTypes.DiscountTypeName, objTradeFairsTypes.Sequence, objTradeFairsTypes.NotesXid.GetValueOrDefault(-1),
                                                      objTradeFairsTypes.LastEditByXid, objTradeFairsTypes.Companyxid, "A");

            return objTradeFairsTypes;

        }

        internal GeneralMaster.TradeFairsTypes ETradeFairsTypes(int pid)
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            DataTable dt = objBaseDataLayer.getDALGetTradeFairsTypesById(pid, "E", "");


            objTradeFairsTypes.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objTradeFairsTypes.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objTradeFairsTypes.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objTradeFairsTypes.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objTradeFairsTypes.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objTradeFairsTypes.NotesXid != -1)
            {
                objTradeFairsTypes.NotesDescription = GetNotesById(objTradeFairsTypes.NotesXid.GetValueOrDefault(-1));
            }
            return objTradeFairsTypes;

        }

        internal GeneralMaster.TradeFairsTypes ETradeFairsTypes(GeneralMaster.TradeFairsTypes model)
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTradeFairsTypes.Pid = model.Pid;
            objTradeFairsTypes.RoomTypeName = model.RoomTypeName;
            objTradeFairsTypes.MaxNoPpl = model.MaxNoPpl;

            objTradeFairsTypes.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTradeFairsTypes.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objTradeFairsTypes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objTradeFairsTypes.NotesXid = NotesChanges(objNotes.Pid, objTradeFairsTypes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objTradeFairsTypes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objTradeFairsTypes.NotesXid = NotesChanges(objNotes.Pid, objTradeFairsTypes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyTradeFairsTypes(objTradeFairsTypes.Pid, objTradeFairsTypes.RoomTypeName, objTradeFairsTypes.MaxNoPpl, objTradeFairsTypes.NotesXid.GetValueOrDefault(-1),
                                                      objTradeFairsTypes.LastEditByXid, objTradeFairsTypes.CompanyXid, "E");

            return objTradeFairsTypes;

        }

        internal GeneralMaster.TradeFairsTypes DTradeFairsTypes(int pid)
        {
            GeneralMaster.TradeFairsTypes objTradeFairsTypes = new GeneralMaster.TradeFairsTypes();
            objBaseDataLayer.getDALGetTradeFairsTypesById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objTradeFairsTypes;
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
            getUserSettings(objUsergrouprights, "DisplayFacility");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objFacility.listFacility = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Facility
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objFacility.PagingValues.MaxRows)
                                .Take(objFacility.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objFacility.PagingValues.MaxRows));
            objFacility.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objFacility.PagingValues.CurrentPageIndex = currPage;

            return objFacility;

        }

        internal object SearchFacility(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetFacilityTypesById(-1, "P", prefix);
            var listFacility = dt.AsEnumerable().Where(x => x.Field<string>("Facility").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.TradeFairsTypes
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            FacilityName = x.Field<string>("Facility")
                                        }).ToList();

            return listFacility;

        }

        internal GeneralMaster.Facility AFacility()
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            return objFacility;
        }

        internal GeneralMaster.Facility AFacility(GeneralMaster.Facility model)
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objFacility.Pid = objNotes.Pid = -1;
            objFacility.DiscountTypeName = model.DiscountTypeName;
            objFacility.Sequence = model.Sequence;

            objFacility.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objFacility.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objFacility.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objFacility.NotesXid = NotesChanges(objNotes.Pid, objFacility.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyFacility(objFacility.Pid, objFacility.DiscountTypeName, objFacility.Sequence, objFacility.NotesXid.GetValueOrDefault(-1),
                                                      objFacility.LastEditByXid, objFacility.Companyxid, "A");

            return objFacility;

        }

        internal GeneralMaster.Facility EFacility(int pid)
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            DataTable dt = objBaseDataLayer.getDALGetFacilityById(pid, "E", "");


            objFacility.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objFacility.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objFacility.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objFacility.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objFacility.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objFacility.NotesXid != -1)
            {
                objFacility.NotesDescription = GetNotesById(objFacility.NotesXid.GetValueOrDefault(-1));
            }
            return objFacility;

        }

        internal GeneralMaster.Facility EFacility(GeneralMaster.Facility model)
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objFacility.Pid = model.Pid;
            objFacility.RoomTypeName = model.RoomTypeName;
            objFacility.MaxNoPpl = model.MaxNoPpl;

            objFacility.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objFacility.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objFacility.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objFacility.NotesXid = NotesChanges(objNotes.Pid, objFacility.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objFacility.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objFacility.NotesXid = NotesChanges(objNotes.Pid, objFacility.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyFacility(objFacility.Pid, objFacility.RoomTypeName, objRoomType.MaxNoPpl, objFacility.NotesXid.GetValueOrDefault(-1),
                                                      objFacility.LastEditByXid, objFacility.CompanyXid, "E");

            return objFacility;

        }

        internal GeneralMaster.Facility DFacility(int pid)
        {
            GeneralMaster.Facility objFacility = new GeneralMaster.Facility();
            objBaseDataLayer.getDALGetFacilityById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objFacility;
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
            getUserSettings(objUsergrouprights, "DisplayFinancialYear");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("FinancialYear", searchPid).Tables[0];

            objFinancialYear.listFinancialYear = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.FinancialYear
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objFinancialYear.PagingValues.MaxRows)
                                .Take(objFinancialYear.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objFinancialYear.PagingValues.MaxRows));
            objFinancialYear.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objFinancialYear.PagingValues.CurrentPageIndex = currPage;

            return objFinancialYear;

        }

        internal object SearchFinancialYear(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetFinancialyearById(-1, "P", prefix);
            var listFinancialYear = dt.AsEnumerable().Where(x => x.Field<string>("FinancialYear").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("FinancialYear")
                                        }).ToList();

            return listFinancialYear;

        }

        internal GeneralMaster.FinancialYear AFinancialYear()
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            return objFinancialYear;
        }

        internal GeneralMaster.FinancialYear AFinancialYear(GeneralMaster.FinancialYear model)
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objFinancialYear.Pid = objNotes.Pid = -1;
            objFinancialYear.DiscountTypeName = model.DiscountTypeName;
            objFinancialYear.Sequence = model.Sequence;

            objFinancialYear.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objFinancialYear.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objFinancialYear.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objFinancialYear.NotesXid = NotesChanges(objNotes.Pid, objFinancialYear.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyFinanicialYear(objFinancialYear.Pid, objFinancialYear.DiscountTypeName, objFinancialYear.Sequence, objFinancialYear.NotesXid.GetValueOrDefault(-1),
                                                      objFinancialYear.LastEditByXid, objFinancialYear.Companyxid, "A");

            return objFinancialYear;

        }

        internal GeneralMaster.FinancialYear EFinancialYear(int pid)
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objFinancialYear.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objFinancialYear.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objFinancialYear.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objFinancialYear.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objFinancialYear.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objFinancialYear.NotesXid != -1)
            {
                objFinancialYear.NotesDescription = GetNotesById(objFinancialYear.NotesXid.GetValueOrDefault(-1));
            }
            return objFinancialYear;

        }

        internal GeneralMaster.FinancialYear EFinancialYear(GeneralMaster.FinancialYear model)
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objFinancialYear.Pid = model.Pid;
            objFinancialYear.RoomTypeName = model.RoomTypeName;
            objFinancialYear.MaxNoPpl = model.MaxNoPpl;

            objFinancialYear.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objFinancialYear.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objFinancialYear.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objFinancialYear.NotesXid = NotesChanges(objNotes.Pid, objFinancialYear.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objFinancialYear.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objFinancialYear.NotesXid = NotesChanges(objNotes.Pid, objFinancialYear.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyFinancialYear(objFinancialYear.Pid, objFinancialYear.RoomTypeName, objFinancialYear.MaxNoPpl, objFinancialYear.NotesXid.GetValueOrDefault(-1),
                                                      objFinancialYear.LastEditByXid, objFinancialYear.CompanyXid, "E");

            return objFinancialYear;

        }

        internal GeneralMaster.FinancialYear DFinancialYear(int pid)
        {
            GeneralMaster.FinancialYear objFinancialYear = new GeneralMaster.FinancialYear();
            objBaseDataLayer.getDALGetFinancialYearById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objFinancialYear;
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
            getUserSettings(objUsergrouprights, "DisplayHolidayDuration");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("HolidayDuration", searchPid).Tables[0];

            objHolidayDuration.listHolidayDuration = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.HolidayDuration
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            HolidayDurationName = Convert.ToString(dr["HolidayDuration"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objHolidayDuration.PagingValues.MaxRows)
                                .Take(objHolidayDuration.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objHolidayDuration.PagingValues.MaxRows));
            objHolidayDuration.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objHolidayDuration.PagingValues.CurrentPageIndex = currPage;

            return objHolidayDuration;

        }

        internal object SearchHolidayDuration(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetHolidayDurationById(-1, "P", prefix);
            var listHolidayDuration = dt.AsEnumerable().Where(x => x.Field<string>("HolidayDuration").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.HolidayDuration
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("HolidayDuration")
                                        }).ToList();

            return listHolidayDuration;

        }

        internal GeneralMaster.HolidayDuration AHolidayDuration()
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            return objHolidayDuration;
        }

        internal GeneralMaster.HolidayDuration AHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHolidayDuration.Pid = objNotes.Pid = -1;
            objHolidayDuration.DiscountTypeName = model.DiscountTypeName;
            objHolidayDuration.Sequence = model.Sequence;

            objHolidayDuration.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHolidayDuration.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objHolidayDuration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objHolidayDuration.NotesXid = NotesChanges(objNotes.Pid, objHolidayDuration.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyHolidayDuration(objHolidayDuration.Pid, objHolidayDuration.DiscountTypeName, objHolidayDuration.Sequence, objHolidayDuration.NotesXid.GetValueOrDefault(-1),
                                                      objHolidayDuration.LastEditByXid, objHolidayDuration.Companyxid, "A");

            return objHolidayDuration;

        }

        internal GeneralMaster.HolidayDuration EHolidayDuration(int pid)
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            DataTable dt = objBaseDataLayer.getDALGetHolidayDurationById(pid, "E", "");


            objHolidayDuration.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objHolidayDuration.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objHolidayDuration.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objHolidayDuration.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objHolidayDuration.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objHolidayDuration.NotesXid != -1)
            {
                objHolidayDuration.NotesDescription = GetNotesById(objHolidayDuration.NotesXid.GetValueOrDefault(-1));
            }
            return objHolidayDuration;

        }

        internal GeneralMaster.HolidayDuration EHolidayDuration(GeneralMaster.HolidayDuration model)
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHolidayDuration.Pid = model.Pid;
            objHolidayDuration.RoomTypeName = model.RoomTypeName;
            objHolidayDuration.MaxNoPpl = model.MaxNoPpl;

            objHolidayDuration.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHolidayDuration.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objHolidayDuration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objHolidayDuration.NotesXid = NotesChanges(objNotes.Pid, objHolidayDuration.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objHolidayDuration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objHolidayDuration.NotesXid = NotesChanges(objNotes.Pid, objHolidayDuration.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyHolidayDuration(objHolidayDuration.Pid, objHolidayDuration.RoomTypeName, objHolidayDuration.MaxNoPpl, objHolidayDuration.NotesXid.GetValueOrDefault(-1),
                                                      objHolidayDuration.LastEditByXid, objHolidayDuration.CompanyXid, "E");

            return objHolidayDuration;

        }

        internal GeneralMaster.HolidayDuration DHolidayDuration(int pid)
        {
            GeneralMaster.HolidayDuration objHolidayDuration = new GeneralMaster.HolidayDuration();
            objBaseDataLayer.getDALGetHolidayDurationById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objHolidayDuration;
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
            getUserSettings(objUsergrouprights, "DisplayHolidayType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("HolidayType", searchPid).Tables[0];

            objHolidayType.listHolidayType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.HolidayType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            HolidayTypeName = Convert.ToString(dr["HolidayType"])
                                        }
                               ).Skip((currPage - 1) * objHolidayType.PagingValues.MaxRows)
                                .Take(objHolidayType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objHolidayType.PagingValues.MaxRows));
            objHolidayType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objHolidayType.PagingValues.CurrentPageIndex = currPage;

            return objHolidayType;

        }

        internal object SearchHolidayType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetHolidayTypeById(-1, "P", prefix);
            var listHolidayType = dt.AsEnumerable().Where(x => x.Field<string>("HolidayType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.HolidayType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("HolidayType")
                                        }).ToList();

            return listHolidayType;

        }

        internal GeneralMaster.HolidayType AHolidayType()
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            return objHolidayType;
        }

        internal GeneralMaster.HolidayType AHolidayType(GeneralMaster.HolidayType model)
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHolidayType.Pid = objNotes.Pid = -1;
            objHolidayType.DiscountTypeName = model.DiscountTypeName;
            objHolidayType.Sequence = model.Sequence;

            objHolidayType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHolidayType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objHolidayType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objHolidayType.NotesXid = NotesChanges(objNotes.Pid, objHolidayType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyHolidayType(objHolidayType.Pid, objHolidayType.DiscountTypeName, objHolidayType.Sequence, objHolidayType.NotesXid.GetValueOrDefault(-1),
                                                      objHolidayType.LastEditByXid, objHolidayType.Companyxid, "A");

            return objHolidayType;

        }

        internal GeneralMaster.HolidayType EHolidayType(int pid)
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            DataTable dt = objBaseDataLayer.getDALGetHolidayTypeById(pid, "E", "");


            objHolidayType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objHolidayType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objHolidayType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objHolidayType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objHolidayType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objHolidayType.NotesXid != -1)
            {
                objHolidayType.NotesDescription = GetNotesById(objHolidayType.NotesXid.GetValueOrDefault(-1));
            }
            return objHolidayType;

        }

        internal GeneralMaster.HolidayType EHolidayType(GeneralMaster.HolidayType model)
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHolidayType.Pid = model.Pid;
            objHolidayType.RoomTypeName = model.RoomTypeName;
            objHolidayType.MaxNoPpl = model.MaxNoPpl;

            objHolidayType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHolidayType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objHolidayType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objHolidayType.NotesXid = NotesChanges(objNotes.Pid, objHolidayType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objHolidayType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objHolidayType.NotesXid = NotesChanges(objNotes.Pid, objHolidayType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyHolidayType(objHolidayType.Pid, objHolidayType.RoomTypeName, objHolidayType.MaxNoPpl, objHolidayType.NotesXid.GetValueOrDefault(-1),
                                                      objHolidayType.LastEditByXid, objHolidayType.CompanyXid, "E");

            return objHolidayType;

        }

        internal GeneralMaster.HolidayType DHolidayType(int pid)
        {
            GeneralMaster.HolidayType objHolidayType = new GeneralMaster.HolidayType();
            objBaseDataLayer.getDALGetHolidayTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objHolidayType;
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
            getUserSettings(objUsergrouprights, "DisplayHotelStandard");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("HotelStandard", searchPid).Tables[0];

            objHotelStandard.listHotelStandard = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.HotelStandard
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            StandardName = Convert.ToString(dr["Standard"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objHotelStandard.PagingValues.MaxRows)
                                .Take(objHotelStandard.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objHotelStandard.PagingValues.MaxRows));
            objHotelStandard.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objHotelStandard.PagingValues.CurrentPageIndex = currPage;

            return objHotelStandard;

        }

        internal object SearchHotelStandard(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetStandardById(-1, "P", prefix);
            var listHotelStandard = dt.AsEnumerable().Where(x => x.Field<string>("HotelStandard").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.HotelStandard
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("HotelStandard")
                                        }).ToList();

            return listHotelStandard;

        }

        internal GeneralMaster.HotelStandard AHotelStandard()
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            return objHotelStandard;
        }

        internal GeneralMaster.HotelStandard AHotelStandard(GeneralMaster.HotelStandard model)
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHotelStandard.Pid = objNotes.Pid = -1;
            objHotelStandard.DiscountTypeName = model.DiscountTypeName;
            objHotelStandard.Sequence = model.Sequence;

            objHotelStandard.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHotelStandard.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objHotelStandard.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objHotelStandard.NotesXid = NotesChanges(objNotes.Pid, objHotelStandard.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyHotelStandard(objHotelStandard.Pid, objHotelStandard.DiscountTypeName, objHotelStandard.Sequence, objHotelStandard.NotesXid.GetValueOrDefault(-1),
                                                      objHotelStandard.LastEditByXid, objHotelStandard.Companyxid, "A");

            return objHotelStandard;

        }

        internal GeneralMaster.HotelStandard EHotelStandard(int pid)
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            DataTable dt = objBaseDataLayer.getDALGetHotelStandardById(pid, "E", "");


            objHotelStandard.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objHotelStandard.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objHotelStandard.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objHotelStandard.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objHotelStandard.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objHotelStandard.NotesXid != -1)
            {
                objHotelStandard.NotesDescription = GetNotesById(objHotelStandard.NotesXid.GetValueOrDefault(-1));
            }
            return objHotelStandard;

        }

        internal GeneralMaster.HotelStandard ERHotelStandard(GeneralMaster.HotelStandard model)
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHotelStandard.Pid = model.Pid;
            objHotelStandard.RoomTypeName = model.RoomTypeName;
            objHotelStandard.MaxNoPpl = model.MaxNoPpl;

            objHotelStandard.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHotelStandard.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objHotelStandard.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objHotelStandard.NotesXid = NotesChanges(objNotes.Pid, objHotelStandard.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objHotelStandard.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objHotelStandard.NotesXid = NotesChanges(objNotes.Pid, objHotelStandard.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyHotelStandard(objobjHotelStandard.Pid, objHotelStandard.RoomTypeName, objHotelStandard.MaxNoPpl, objHotelStandard.NotesXid.GetValueOrDefault(-1),
                                                      objHotelStandard.LastEditByXid, objHotelStandard.CompanyXid, "E");

            return objHotelStandard;

        }

        internal GeneralMaster.HotelStandard DHotelStandard(int pid)
        {
            GeneralMaster.HotelStandard objHotelStandard = new GeneralMaster.HotelStandard();
            objBaseDataLayer.getDALGetHotelStandardById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objHotelStandard;
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
            getUserSettings(objUsergrouprights, "DisplayHotelChain");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("HotelChain", searchPid).Tables[0];

            objHotelChain.listHotelChain = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.HotelChain
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            HotelChainsName = Convert.ToString(dr["HotelChain"])
                                        }
                               ).Skip((currPage - 1) * objHotelChain.PagingValues.MaxRows)
                                .Take(objHotelChain.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objHotelChain.PagingValues.MaxRows));
            objHotelChain.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objHotelChain.PagingValues.CurrentPageIndex = currPage;

            return objHotelChain;

        }

        internal object SearchHotelChain(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetHotelChainById(-1, "P", prefix);
            var listHotelChain = dt.AsEnumerable().Where(x => x.Field<string>("HotelChain").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.HotelChain
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("HotelChain")
                                        }).ToList();

            return listHotelChain;

        }

        internal GeneralMaster.HotelChain AHotelChain()
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            return objHotelChain;
        }

        internal GeneralMaster.HotelChain AHotelChain(GeneralMaster.HotelChain model)
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHotelChain.Pid = objNotes.Pid = -1;
            objHotelChain.DiscountTypeName = model.DiscountTypeName;
            objHotelChain.Sequence = model.Sequence;

            objHotelChain.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHotelChain.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objHotelChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objHotelChain.NotesXid = NotesChanges(objNotes.Pid, objHotelChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyHotelChain(objHotelChain.Pid, objHotelChain.DiscountTypeName, objHotelChain.Sequence, objHotelChain.NotesXid.GetValueOrDefault(-1),
                                                      objHotelChain.LastEditByXid, objHotelChain.Companyxid, "A");

            return objHotelChain;

        }

        internal GeneralMaster.HotelChain EHotelChain(int pid)
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            DataTable dt = objBaseDataLayer.getDALGetHotelChainById(pid, "E", "");


            objHotelChain.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objHotelChain.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objHotelChain.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objHotelChain.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objHotelChain.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objHotelChain.NotesXid != -1)
            {
                objHotelChain.NotesDescription = GetNotesById(objHotelChain.NotesXid.GetValueOrDefault(-1));
            }
            return objHotelChain;

        }

        internal GeneralMaster.HotelChain EHotelChain(GeneralMaster.HotelChain model)
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objHotelChain.Pid = model.Pid;
            objHotelChain.RoomTypeName = model.RoomTypeName;
            objHotelChain.MaxNoPpl = model.MaxNoPpl;

            objHotelChain.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objHotelChain.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objHotelChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objHotelChain.NotesXid = NotesChanges(objNotes.Pid, objHotelChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objHotelChain.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objHotelChain.NotesXid = NotesChanges(objNotes.Pid, objHotelChain.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyHotelChain(objHotelChain.Pid, objHotelChain.RoomTypeName, objHotelChain.MaxNoPpl, objHotelChain.NotesXid.GetValueOrDefault(-1),
                                                      objHotelChain.LastEditByXid, objHotelChain.CompanyXid, "E");

            return objHotelChain;

        }

        internal GeneralMaster.HotelChain DHotelChain(int pid)
        {
            GeneralMaster.HotelChain objHotelChain = new GeneralMaster.HotelChain();
            objBaseDataLayer.getDALGetHotelChainById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objHotelChain;
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
            getUserSettings(objUsergrouprights, "DisplayInspectionCriteria");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("InspectionCriteria", searchPid).Tables[0];

            objInspectionCriteria.listInspectionCriteria = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.InspectionCriteria
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            InspectionCriteriaName = Convert.ToString(dr["InspectionCriteria"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objInspectionCriteria.PagingValues.MaxRows)
                                .Take(objInspectionCriteria.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objInspectionCriteria.PagingValues.MaxRows));
            objInspectionCriteria.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objInspectionCriteria.PagingValues.CurrentPageIndex = currPage;

            return objInspectionCriteria;

        }

        internal object SearchInspectionCriteria(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetInspectionCriteriaById(-1, "P", prefix);
            var listInspectionCriteria = dt.AsEnumerable().Where(x => x.Field<string>("InspectionCriteria").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.InspectionCriteria
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("InspectionCriteria")
                                        }).ToList();

            return listInspectionCriteria;

        }

        internal GeneralMaster.InspectionCriteria AInspectionCriteria()
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            return objInspectionCriteria;
        }

        internal GeneralMaster.InspectionCriteria AInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objInspectionCriteria.Pid = objNotes.Pid = -1;
            objInspectionCriteria.DiscountTypeName = model.DiscountTypeName;
            objInspectionCriteria.Sequence = model.Sequence;

            objInspectionCriteria.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objInspectionCriteria.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objInspectionCriteria.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objInspectionCriteria.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyInspectionCriteria(objInspectionCriteria.Pid, objInspectionCriteria.DiscountTypeName, objInspectionCriteria.Sequence, objInspectionCriteria.NotesXid.GetValueOrDefault(-1),
                                                      objInspectionCriteria.LastEditByXid, objInspectionCriteria.Companyxid, "A");

            return objInspectionCriteria;

        }

        internal GeneralMaster.InspectionCriteria EInspectionCriteria(int pid)
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            DataTable dt = objBaseDataLayer.getDALGetInspectionCriteriaById(pid, "E", "");


            objInspectionCriteria.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objInspectionCriteria.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objInspectionCriteria.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objInspectionCriteria.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objInspectionCriteria.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objInspectionCriteria.NotesXid != -1)
            {
                objInspectionCriteria.NotesDescription = GetNotesById(objInspectionCriteria.NotesXid.GetValueOrDefault(-1));
            }
            return objInspectionCriteria;
        }

        internal GeneralMaster.InspectionCriteria EInspectionCriteria(GeneralMaster.InspectionCriteria model)
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objInspectionCriteria.Pid = model.Pid;
            objInspectionCriteria.RoomTypeName = model.RoomTypeName;
            objInspectionCriteria.MaxNoPpl = model.MaxNoPpl;

            objInspectionCriteria.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objInspectionCriteria.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objInspectionCriteria.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objInspectionCriteria.NotesXid = NotesChanges(objNotes.Pid, objInspectionCriteria.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objInspectionCriteria.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objInspectionCriteria.NotesXid = NotesChanges(objNotes.Pid, objInspectionCriteria.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyInspectionCriteria(objInspectionCriteria.Pid, objInspectionCriteria.RoomTypeName, objInspectionCriteria.MaxNoPpl, objInspectionCriteria.NotesXid.GetValueOrDefault(-1),
                                                      objInspectionCriteria.LastEditByXid, objInspectionCriteria.CompanyXid, "E");

            return objInspectionCriteria;

        }

        internal GeneralMaster.InspectionCriteria DInspectionCriteria(int pid)
        {
            GeneralMaster.InspectionCriteria objInspectionCriteria = new GeneralMaster.InspectionCriteria();
            objBaseDataLayer.getDALGetInspectionCriteriaById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objInspectionCriteria;
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
            getUserSettings(objUsergrouprights, "DisplayLanguage");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Language objLanguage = new GeneralMaster.Language(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Language", searchPid).Tables[0];

            objLanguage.listLanguage = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Language
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            LanguageName = Convert.ToString(dr["language"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objLanguage.PagingValues.MaxRows)
                                .Take(objLanguage.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objLanguage.PagingValues.MaxRows));
            objLanguage.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objLanguage.PagingValues.CurrentPageIndex = currPage;

            return objLanguage;

        }

        internal object SearchLanguage(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetLanguageById(-1, "P", prefix);
            var listLanguage = dt.AsEnumerable().Where(x => x.Field<string>("Language").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Language
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Language")
                                        }).ToList();

            return listLanguage;

        }

        internal GeneralMaster.Language ALanguage()
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            return objLanguage;
        }

        internal GeneralMaster.Language ALanguage(GeneralMaster.Language model)
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLanguage.Pid = objNotes.Pid = -1;
            objLanguage.DiscountTypeName = model.DiscountTypeName;
            objLanguage.Sequence = model.Sequence;

            objLanguage.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLanguage.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objLanguage.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objLanguage.NotesXid = NotesChanges(objNotes.Pid, objLanguage.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyLanguage(objLanguage.Pid, objLanguage.DiscountTypeName, objLanguage.Sequence, objLanguage.NotesXid.GetValueOrDefault(-1),
                                                      objLanguage.LastEditByXid, objLanguage.Companyxid, "A");

            return objLanguage;

        }

        internal GeneralMaster.Language ELanguage(int pid)
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            DataTable dt = objBaseDataLayer.getDALGetLanguageById(pid, "E", "");


            objLanguage.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objLanguage.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objLanguage.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objLanguage.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objLanguage.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objLanguage.NotesXid != -1)
            {
                objLanguage.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objLanguage;
        }

        internal GeneralMaster.Language ELanguage(GeneralMaster.Language model)
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLanguage.Pid = model.Pid;
            objLanguage.RoomTypeName = model.RoomTypeName;
            objLanguage.MaxNoPpl = model.MaxNoPpl;

            objLanguage.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLanguage.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objLanguage.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objLanguage.NotesXid = NotesChanges(objNotes.Pid, objLanguage.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objLanguage.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objLanguage.NotesXid = NotesChanges(objNotes.Pid, objLanguage.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyLanguage(objLanguage.Pid, objLanguage.RoomTypeName, objLanguage.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objRoomType.LastEditByXid, objRoomType.CompanyXid, "E");

            return objLanguage;

        }

        internal GeneralMaster.Language DLanguage(int pid)
        {
            GeneralMaster.Language objLanguage = new GeneralMaster.Language();
            objBaseDataLayer.getDALGetLanguageById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objLanguage;
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
            getUserSettings(objUsergrouprights, "DisplayMarket");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Market objMarket = new GeneralMaster.Market(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Market", searchPid).Tables[0];

            objMarket.listMarket = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Market
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            MarketName = Convert.ToString(dr["market"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objMarket.PagingValues.MaxRows)
                                .Take(objMarket.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objMarket.PagingValues.MaxRows));
            objMarket.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objMarket.PagingValues.CurrentPageIndex = currPage;

            return objMarket;

        }

        internal object SearchMarket(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetMarketById(-1, "P", prefix);
            var listMarket = dt.AsEnumerable().Where(x => x.Field<string>("Market").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Market
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Market")
                                        }).ToList();

            return listMarket;

        }

        internal GeneralMaster.Market AMarket()
        {
            GeneralMaster.Market objRoomType = new GeneralMaster.Market();
            return objRoomType;
        }

        internal GeneralMaster.Market AMarket(GeneralMaster.Market model)
        {
            GeneralMaster.Market objMarket = new GeneralMaster.Market();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objMarket.Pid = objNotes.Pid = -1;
            objMarket.DiscountTypeName = model.DiscountTypeName;
            objMarket.Sequence = model.Sequence;

            objMarket.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objMarket.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objMarket.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objMarket.NotesXid = NotesChanges(objNotes.Pid, objMarket.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyMarket(objMarket.Pid, objMarket.DiscountTypeName, objMarket.Sequence, objMarket.NotesXid.GetValueOrDefault(-1),
                                                      objMarket.LastEditByXid, objMarket.Companyxid, "A");

            return objMarket;

        }

        internal GeneralMaster.Market EMarket(int pid)
        {
            GeneralMaster.Market objMarket = new GeneralMaster.Market();
            DataTable dt = objBaseDataLayer.getDALGetMarketById(pid, "E", "");


            objMarket.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objMarket.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objMarket.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objMarket.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objMarket.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objMarket.NotesXid != -1)
            {
                objMarket.NotesDescription = GetNotesById(objMarket.NotesXid.GetValueOrDefault(-1));
            }
            return objMarket;
        }

        internal GeneralMaster.Market EMarket(GeneralMaster.Market model)
        {
            GeneralMaster.Market objMarket = new GeneralMaster.Market();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objMarket.Pid = model.Pid;
            objMarket.RoomTypeName = model.RoomTypeName;
            objMarket.MaxNoPpl = model.MaxNoPpl;

            objMarket.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objMarket.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objMarket.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objMarket.NotesXid = NotesChanges(objNotes.Pid, objMarket.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objMarket.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objMarket.NotesXid = NotesChanges(objNotes.Pid, objMarket.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyMarket(objMarket.Pid, objMarket.RoomTypeName, objMarket.MaxNoPpl, objMarket.NotesXid.GetValueOrDefault(-1),
                                                      objMarket.LastEditByXid, objMarket.CompanyXid, "E");

            return objMarket;

        }

        internal GeneralMaster.Market DMarket(int pid)
        {
            GeneralMaster.Market objMarket = new GeneralMaster.Market();
            objBaseDataLayer.getDALGetMarketById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objMarket;
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
            getUserSettings(objUsergrouprights, "DisplayMealPlan");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("MealPlan", searchPid).Tables[0];

            objMealPlan.listMealPlan = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.MealPlan
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            MealPlanName = Convert.ToString(dr["Mealplan"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objMealPlan.PagingValues.MaxRows)
                                .Take(objMealPlan.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objMealPlan.PagingValues.MaxRows));
            objMealPlan.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objMealPlan.PagingValues.CurrentPageIndex = currPage;

            return objMealPlan;

        }

        internal object SearchMealPlan(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetMealPlanById(-1, "P", prefix);
            var listMealPlan = dt.AsEnumerable().Where(x => x.Field<string>("MealPlan").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.MealPlan
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("MealPlan")
                                        }).ToList();

            return listMealPlan;

        }

        internal GeneralMaster.MealPlan AMealPlan()
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            return objMealPlan;
        }

        internal GeneralMaster.MealPlan AMealPlan(GeneralMaster.MealPlan model)
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objMealPlan.Pid = objNotes.Pid = -1;
            objMealPlan.DiscountTypeName = model.DiscountTypeName;
            objMealPlan.Sequence = model.Sequence;

            objMealPlan.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objMealPlan.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objMealPlan.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objMealPlan.NotesXid = NotesChanges(objNotes.Pid, objMealPlan.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyMealPlan(objMealPlan.Pid, objMealPlan.DiscountTypeName, objMealPlan.Sequence, objMealPlan.NotesXid.GetValueOrDefault(-1),
                                                      objMealPlan.LastEditByXid, objMealPlan.Companyxid, "A");

            return objMealPlan;

        }

        internal GeneralMaster.MealPlan EMealPlan(int pid)
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            DataTable dt = objBaseDataLayer.getDALGetMealPlanById(pid, "E", "");


            objMealPlan.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objMealPlan.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objMealPlan.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objMealPlan.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objMealPlan.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objMealPlan.NotesXid != -1)
            {
                objMealPlan.NotesDescription = GetNotesById(objMealPlan.NotesXid.GetValueOrDefault(-1));
            }
            return objMealPlan;
        }

        internal GeneralMaster.MealPlan EMealPlan(GeneralMaster.MealPlan model)
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objMealPlan.Pid = model.Pid;
            objMealPlan.RoomTypeName = model.RoomTypeName;
            objMealPlan.MaxNoPpl = model.MaxNoPpl;

            objMealPlan.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objMealPlan.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objMealPlan.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objMealPlan.NotesXid = NotesChanges(objNotes.Pid, objMealPlan.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objMealPlan.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objMealPlan.NotesXid = NotesChanges(objNotes.Pid, objMealPlan.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyMealPlan(objMealPlan.Pid, objMealPlan.RoomTypeName, objMealPlan.MaxNoPpl, objMealPlan.NotesXid.GetValueOrDefault(-1),
                                                      objMealPlan.LastEditByXid, objMealPlan.CompanyXid, "E");

            return objMealPlan;

        }

        internal GeneralMaster.MealPlan DMealPlan(int pid)
        {
            GeneralMaster.MealPlan objMealPlan = new GeneralMaster.MealPlan();
            objBaseDataLayer.getDALGetMealPlanById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objMealPlan;
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
            getUserSettings(objUsergrouprights, "DisplayNationality");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Nationality", searchPid).Tables[0];

            objNationality.listNationality = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Nationality
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            NationalityName = Convert.ToString(dr["nationality"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objNationality.PagingValues.MaxRows)
                                .Take(objNationality.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objNationality.PagingValues.MaxRows));
            objNationality.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objNationality.PagingValues.CurrentPageIndex = currPage;

            return objNationality;

        }

        internal object SearchNationality(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetNationalityById(-1, "P", prefix);
            var listNationality = dt.AsEnumerable().Where(x => x.Field<string>("Nationality").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Nationality
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Nationality")
                                        }).ToList();

            return listNationality;

        }

        internal GeneralMaster.Nationality ANationality()
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            return objNationality;
        }

        internal GeneralMaster.Nationality ANationality(GeneralMaster.Nationality model)
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objNationality.Pid = objNotes.Pid = -1;
            objNationality.DiscountTypeName = model.DiscountTypeName;
            objNationality.Sequence = model.Sequence;

            objNationality.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objNationality.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objNationality.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objNationality.NotesXid = NotesChanges(objNotes.Pid, objNationality.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyNationality(objNationality.Pid, objNationality.DiscountTypeName, objNationality.Sequence, objNationality.NotesXid.GetValueOrDefault(-1),
                                                      objNationality.LastEditByXid, objNationality.Companyxid, "A");

            return objNationality;

        }

        internal GeneralMaster.Nationality ENationality(int pid)
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            DataTable dt = objBaseDataLayer.getDALGetNationalityById(pid, "E", "");


            objNationality.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objNationality.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objNationality.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objNationality.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objNationality.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objNationality.NotesXid != -1)
            {
                objNationality.NotesDescription = GetNotesById(objNationality.NotesXid.GetValueOrDefault(-1));
            }
            return objNationality;
        }

        internal GeneralMaster.Nationality ENationality(GeneralMaster.Nationality model)
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objNationality.Pid = model.Pid;
            objNationality.RoomTypeName = model.RoomTypeName;
            objNationality.MaxNoPpl = model.MaxNoPpl;

            objNationality.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objNationality.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objNationality.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objNationality.NotesXid = NotesChanges(objNotes.Pid, objNationality.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objNationality.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNationality.NotesXid = NotesChanges(objNotes.Pid, objNationality.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyNationality(objNationality.Pid, objNationality.RoomTypeName, objNationality.MaxNoPpl, objNationality.NotesXid.GetValueOrDefault(-1),
                                                      objNationality.LastEditByXid, objNationality.CompanyXid, "E");

            return objNationality;

        }

        internal GeneralMaster.Nationality DNationality(int pid)
        {
            GeneralMaster.Nationality objNationality = new GeneralMaster.Nationality();
            objBaseDataLayer.getDALGetNationalityById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objNationality;
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
            getUserSettings(objUsergrouprights, "DisplayPaymentSchedules");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("PaymentSchedules", searchPid).Tables[0];

            objPaymentSchedules.listPaymentSchedules = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.PaymentSchedules
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            PaymentSchedulesName = Convert.ToString(dr["PaymentSchedule"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"]),
                                        }
                               ).Skip((currPage - 1) * objPaymentSchedules.PagingValues.MaxRows)
                                .Take(objPaymentSchedules.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objPaymentSchedules.PagingValues.MaxRows));
            objPaymentSchedules.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objPaymentSchedules.PagingValues.CurrentPageIndex = currPage;

            return objPaymentSchedules;

        }

        internal object SearchPaymentSchedules(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetPaymentSchedulesById(-1, "P", prefix);
            var listPaymentSchedules = dt.AsEnumerable().Where(x => x.Field<string>("PaymentSchedules").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.PaymentSchedules
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("PaymentSchedules")
                                        }).ToList();

            return listPaymentSchedules;

        }

        internal GeneralMaster.PaymentSchedules APaymentSchedules()
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            return objPaymentSchedules;
        }

        internal GeneralMaster.PaymentSchedules APaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPaymentSchedules.Pid = objNotes.Pid = -1;
            objPaymentSchedules.DiscountTypeName = model.DiscountTypeName;
            objPaymentSchedules.Sequence = model.Sequence;

            objPaymentSchedules.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPaymentSchedules.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objPaymentSchedules.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objPaymentSchedules.NotesXid = NotesChanges(objNotes.Pid, objPaymentSchedules.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyPaymentSchedules(objPaymentSchedules.Pid, objPaymentSchedules.DiscountTypeName, objPaymentSchedules.Sequence, objPaymentSchedules.NotesXid.GetValueOrDefault(-1),
                                                      objPaymentSchedules.LastEditByXid, objPaymentSchedules.Companyxid, "A");

            return objPaymentSchedules;

        }

        internal GeneralMaster.PaymentSchedules EPaymentSchedules(int pid)
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            DataTable dt = objBaseDataLayer.getDALGetPaymentSchedulesById(pid, "E", "");


            objPaymentSchedules.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objPaymentSchedules.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objPaymentSchedules.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objPaymentSchedules.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objPaymentSchedules.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objPaymentSchedules.NotesXid != -1)
            {
                objPaymentSchedules.NotesDescription = GetNotesById(objPaymentSchedules.NotesXid.GetValueOrDefault(-1));
            }
            return objPaymentSchedules;
        }

        internal GeneralMaster.PaymentSchedules EPaymentSchedules(GeneralMaster.PaymentSchedules model)
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPaymentSchedules.Pid = model.Pid;
            objPaymentSchedules.RoomTypeName = model.RoomTypeName;
            objPaymentSchedules.MaxNoPpl = model.MaxNoPpl;

            objPaymentSchedules.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPaymentSchedules.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objPaymentSchedules.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objPaymentSchedules.NotesXid = NotesChanges(objNotes.Pid, objPaymentSchedules.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objPaymentSchedules.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objPaymentSchedules.NotesXid = NotesChanges(objNotes.Pid, objPaymentSchedules.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyPaymentSchedules(objPaymentSchedules.Pid, objPaymentSchedules.RoomTypeName, objPaymentSchedules.MaxNoPpl, objPaymentSchedules.NotesXid.GetValueOrDefault(-1),
                                                      objPaymentSchedules.LastEditByXid, objPaymentSchedules.CompanyXid, "E");

            return objPaymentSchedules;

        }

        internal GeneralMaster.PaymentSchedules DPaymentSchedules(int pid)
        {
            GeneralMaster.PaymentSchedules objPaymentSchedules = new GeneralMaster.PaymentSchedules();
            objBaseDataLayer.getDALGetPaymentSchedulesById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objPaymentSchedules;
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
            getUserSettings(objUsergrouprights, "DisplayPaymentType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("PaymentType", searchPid).Tables[0];

            objPaymentType.listPaymentType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.PaymentType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            PaymentTypeName = Convert.ToString(dr["PaymentType"]),
                                            NominalCode = Convert.ToString(dr["nominalCode"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objPaymentType.PagingValues.MaxRows)
                                .Take(objPaymentType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objPaymentType.PagingValues.MaxRows));
            objPaymentType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objPaymentType.PagingValues.CurrentPageIndex = currPage;

            return objPaymentType;

        }

        internal object SearchPaymentType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetPaymentTypeById(-1, "P", prefix);
            var listPaymentType = dt.AsEnumerable().Where(x => x.Field<string>("PaymentType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.PaymentType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("PaymentType")
                                        }).ToList();

            return listPaymentType;

        }

        internal GeneralMaster.PaymentType APaymentType()
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            return objPaymentType;
        }

        internal GeneralMaster.PaymentType APaymentType(GeneralMaster.PaymentType model)
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPaymentType.Pid = objNotes.Pid = -1;
            objPaymentType.DiscountTypeName = model.DiscountTypeName;
            objPaymentType.Sequence = model.Sequence;

            objPaymentType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPaymentType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objPaymentType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objPaymentType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyPaymentType(objPaymentType.Pid, objPaymentType.DiscountTypeName, objPaymentType.Sequence, objPaymentType.NotesXid.GetValueOrDefault(-1),
                                                      objPaymentType.LastEditByXid, objPaymentType.Companyxid, "A");

            return objPaymentType;

        }

        internal GeneralMaster.PaymentType EPaymentType(int pid)
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            DataTable dt = objBaseDataLayer.getDALGetPaymentTypeById(pid, "E", "");


            objPaymentType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objPaymentType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objPaymentType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objPaymentType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objPaymentType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objPaymentType.NotesXid != -1)
            {
                objPaymentType.NotesDescription = GetNotesById(objPaymentType.NotesXid.GetValueOrDefault(-1));
            }
            return objPaymentType;
        }

        internal GeneralMaster.PaymentType EPaymentType(GeneralMaster.PaymentType model)
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPaymentType.Pid = model.Pid;
            objPaymentType.RoomTypeName = model.RoomTypeName;
            objPaymentType.MaxNoPpl = model.MaxNoPpl;

            objPaymentType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPaymentType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objPaymentType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objPaymentType.NotesXid = NotesChanges(objNotes.Pid, objPaymentType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objPaymentType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objPaymentType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyPaymentType(objPaymentType.Pid, objPaymentType.RoomTypeName, objPaymentType.MaxNoPpl, objPaymentType.NotesXid.GetValueOrDefault(-1),
                                                      objPaymentType.LastEditByXid, objPaymentType.CompanyXid, "E");

            return objPaymentType;

        }

        internal GeneralMaster.PaymentType DPaymentType(int pid)
        {
            GeneralMaster.PaymentType objPaymentType = new GeneralMaster.PaymentType();
            objBaseDataLayer.getDALGetPaymentTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objPaymentType;
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
            getUserSettings(objUsergrouprights, "DisplayLogisticPickupType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("LogisticPickupType", searchPid).Tables[0];

            objLogisticPickupType.listLogisticPickupType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.LogisticPickupType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            PickupType = Convert.ToString(dr["Pickuptype"]),
                                            ShowBookingEngine = Convert.ToString(dr["showbookingengine"]),
                                            ArrivalPoint = Convert.ToString(dr["ArrivalPOint"]),

                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objLogisticPickupType.PagingValues.MaxRows)
                                .Take(objLogisticPickupType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objLogisticPickupType.PagingValues.MaxRows));
            objLogisticPickupType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objLogisticPickupType.PagingValues.CurrentPageIndex = currPage;

            return objLogisticPickupType;

        }

        internal object SearchLogisticPickupType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetLogisticPickupTypeById(-1, "P", prefix);
            var listLogisticPickupType = dt.AsEnumerable().Where(x => x.Field<string>("LogisticPickupType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.LogisticPickupType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("LogisticPickupType")
                                        }).ToList();

            return listLogisticPickupType;

        }

        internal GeneralMaster.LogisticPickupType ALogisticPickupType()
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            return objLogisticPickupType;
        }

        internal GeneralMaster.LogisticPickupType ALogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticPickupType.Pid = objNotes.Pid = -1;
            objLogisticPickupType.DiscountTypeName = model.DiscountTypeName;
            objLogisticPickupType.Sequence = model.Sequence;

            objLogisticPickupType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticPickupType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objLogisticPickupType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objLogisticPickupType.NotesXid = NotesChanges(objNotes.Pid, objLogisticPickupType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyLogisticPickupType(objLogisticPickupType.Pid, objLogisticPickupType.DiscountTypeName, objLogisticPickupType.Sequence, objLogisticPickupType.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticPickupType.LastEditByXid, objLogisticPickupType.Companyxid, "A");

            return objLogisticPickupType;

        }

        internal GeneralMaster.LogisticPickupType ELogisticPickupType(int pid)
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            DataTable dt = objBaseDataLayer.getDALGetLogisticPickupTypeById(pid, "E", "");


            objLogisticPickupType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objLogisticPickupType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objLogisticPickupType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objLogisticPickupType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objLogisticPickupType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objLogisticPickupType.NotesXid != -1)
            {
                objLogisticPickupType.NotesDescription = GetNotesById(objLogisticPickupType.NotesXid.GetValueOrDefault(-1));
            }
            return objLogisticPickupType;
        }

        internal GeneralMaster.LogisticPickupType ELogisticPickupType(GeneralMaster.LogisticPickupType model)
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticPickupType.Pid = model.Pid;
            objLogisticPickupType.RoomTypeName = model.RoomTypeName;
            objLogisticPickupType.MaxNoPpl = model.MaxNoPpl;

            objLogisticPickupType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticPickupType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objLogisticPickupType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objLogisticPickupType.NotesXid = NotesChanges(objNotes.Pid, objLogisticPickupType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objLogisticPickupType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objLogisticPickupType.NotesXid = NotesChanges(objNotes.Pid, objLogisticPickupType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyLogisticPickupType(objLogisticPickupType.Pid, objLogisticPickupType.RoomTypeName, objLogisticPickupType.MaxNoPpl, objLogisticPickupType.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticPickupType.LastEditByXid, objLogisticPickupType.CompanyXid, "E");

            return objLogisticPickupType;

        }

        internal GeneralMaster.LogisticPickupType DLogisticPickupType(int pid)
        {
            GeneralMaster.LogisticPickupType objLogisticPickupType = new GeneralMaster.LogisticPickupType();
            objBaseDataLayer.getDALGetLogisticPickupTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objLogisticPickupType;
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
            getUserSettings(objUsergrouprights, "DisplayCrmPriority");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("CrmPriority", searchPid).Tables[0];

            objCrmPriority.listCrmPriority = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.CrmPriority
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Priority = Convert.ToString(dr["priority"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objCrmPriority.PagingValues.MaxRows)
                                .Take(objCrmPriority.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objCrmPriority.PagingValues.MaxRows));
            objCrmPriority.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objCrmPriority.PagingValues.CurrentPageIndex = currPage;

            return objCrmPriority;

        }

        internal object SearchCrmPriority(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetCrmPriorityById(-1, "P", prefix);
            var listCrmPriority = dt.AsEnumerable().Where(x => x.Field<string>("CrmPriority").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.CrmPriority
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("CrmPriority")
                                        }).ToList();

            return listCrmPriority;

        }

        internal GeneralMaster.CrmPriority ACrmPriority()
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            return objCrmPriority;
        }

        internal GeneralMaster.CrmPriority ACrmPriority(GeneralMaster.CrmPriority model)
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCrmPriority.Pid = objNotes.Pid = -1;
            objCrmPriority.DiscountTypeName = model.DiscountTypeName;
            objCrmPriority.Sequence = model.Sequence;

            objCrmPriority.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCrmPriority.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objCrmPriority.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objCrmPriority.NotesXid = NotesChanges(objNotes.Pid, objCrmPriority.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyCrmPriority(objCrmPriority.Pid, objCrmPriority.DiscountTypeName, objCrmPriority.Sequence, objCrmPriority.NotesXid.GetValueOrDefault(-1),
                                                      objCrmPriority.LastEditByXid, objCrmPriority.Companyxid, "A");

            return objCrmPriority;

        }

        internal GeneralMaster.CrmPriority ECrmPriority(int pid)
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            DataTable dt = objBaseDataLayer.getDALGetCrmPriorityById(pid, "E", "");


            objCrmPriority.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objCrmPriority.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objCrmPriority.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objCrmPriority.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objCrmPriority.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objCrmPriority.NotesXid != -1)
            {
                objCrmPriority.NotesDescription = GetNotesById(objCrmPriority.NotesXid.GetValueOrDefault(-1));
            }
            return objCrmPriority;
        }

        internal GeneralMaster.CrmPriority ECrmPriority(GeneralMaster.CrmPriority model)
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCrmPriority.Pid = model.Pid;
            objCrmPriority.RoomTypeName = model.RoomTypeName;
            objCrmPriority.MaxNoPpl = model.MaxNoPpl;

            objCrmPriority.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCrmPriority.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objCrmPriority.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objCrmPriority.NotesXid = NotesChanges(objNotes.Pid, objCrmPriority.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objCrmPriority.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objCrmPriority.NotesXid = NotesChanges(objNotes.Pid, objCrmPriority.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyCrmPriority(objCrmPriority.Pid, objCrmPriority.RoomTypeName, objCrmPriority.MaxNoPpl, objCrmPriority.NotesXid.GetValueOrDefault(-1),
                                                      objCrmPriority.LastEditByXid, objCrmPriority.CompanyXid, "E");

            return objCrmPriority;

        }

        internal GeneralMaster.CrmPriority DCrmPriority(int pid)
        {
            GeneralMaster.CrmPriority objCrmPriority = new GeneralMaster.CrmPriority();
            objBaseDataLayer.getDALGetCrmPriorityById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objCrmPriority;
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
            getUserSettings(objUsergrouprights, "DisplayPropertyType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("PropertyType", searchPid).Tables[0];

            objPropertyType.listPropertyType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.PropertyType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            PropertyTypeName = Convert.ToString(dr["PropertyType"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objPropertyType.PagingValues.MaxRows)
                                .Take(objPropertyType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objPropertyType.PagingValues.MaxRows));
            objPropertyType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objPropertyType.PagingValues.CurrentPageIndex = currPage;

            return objPropertyType;

        }

        internal object SearchPropertyType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetPropertyTypeById(-1, "P", prefix);
            var listPropertyType = dt.AsEnumerable().Where(x => x.Field<string>("PropertyType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.PropertyType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("PropertyType")
                                        }).ToList();

            return listPropertyType;

        }
        internal GeneralMaster.PropertyType APropertyType()
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            return objPropertyType;
        }
        internal GeneralMaster.PropertyType APropertyType(GeneralMaster.PropertyType model)
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPropertyType.Pid = objNotes.Pid = -1;
            objPropertyType.DiscountTypeName = model.DiscountTypeName;
            objPropertyType.Sequence = model.Sequence;

            objPropertyType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPropertyType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objPropertyType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objPropertyType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyPropertyType(objPropertyType.Pid, objPropertyType.DiscountTypeName, objPropertyType.Sequence, objPropertyType.NotesXid.GetValueOrDefault(-1),
                                                      objPropertyType.LastEditByXid, objPropertyType.Companyxid, "A");

            return objPropertyType;

        }

        internal GeneralMaster.PropertyType EPropertyType(int pid)
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            DataTable dt = objBaseDataLayer.getDALGetPropertyTypeById(pid, "E", "");


            objPropertyType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objPropertyType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objPropertyType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objPropertyType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objPropertyType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objPropertyType.NotesXid != -1)
            {
                objPropertyType.NotesDescription = GetNotesById(objPropertyType.NotesXid.GetValueOrDefault(-1));
            }
            return objPropertyType;
        }

        internal GeneralMaster.PropertyType EPropertyType(GeneralMaster.PropertyType model)
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objPropertyType.Pid = model.Pid;
            objPropertyType.RoomTypeName = model.RoomTypeName;
            objPropertyType.MaxNoPpl = model.MaxNoPpl;

            objPropertyType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objPropertyType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objPropertyType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objPropertyType.NotesXid = NotesChanges(objNotes.Pid, objPropertyType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objPropertyType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objPropertyType.NotesXid = NotesChanges(objNotes.Pid, objPropertyType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyPropertyType(objPropertyType.Pid, objPropertyType.RoomTypeName, objPropertyType.MaxNoPpl, objPropertyType.NotesXid.GetValueOrDefault(-1),
                                                      objPropertyType.LastEditByXid, objPropertyType.CompanyXid, "E");

            return objPropertyType;

        }

        internal GeneralMaster.PropertyType DPropertyType(int pid)
        {
            GeneralMaster.PropertyType objPropertyType = new GeneralMaster.PropertyType();
            objBaseDataLayer.getDALGetPropertyTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objPropertyType;
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
            getUserSettings(objUsergrouprights, "DisplayReason");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Reason objReason = new GeneralMaster.Reason(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Reason", searchPid).Tables[0];

            objReason.listReason = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Reason
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            ReasonName = Convert.ToString(dr["Reason"]),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objReason.PagingValues.MaxRows)
                                .Take(objReason.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objReason.PagingValues.MaxRows));
            objReason.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objReason.PagingValues.CurrentPageIndex = currPage;

            return objReason;

        }

        internal object SearchReason(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetReasonById(-1, "P", prefix);
            var listReason= dt.AsEnumerable().Where(x => x.Field<string>("Reason").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Reason
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Reason")
                                        }).ToList();

            return listReason;

        }

        internal GeneralMaster.Reason AReason()
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            return objReason;
        }

        internal GeneralMaster.Reason AReason(GeneralMaster.Reason model)
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objReason.Pid = objNotes.Pid = -1;
            objReason.DiscountTypeName = model.DiscountTypeName;
            objReason.Sequence = model.Sequence;

            objReason.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objReason.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objReason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objReason.NotesXid = NotesChanges(objNotes.Pid, objReason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyReason(objReason.Pid, objReason.DiscountTypeName, objReason.Sequence, objReason.NotesXid.GetValueOrDefault(-1),
                                                      objReason.LastEditByXid, objReason.Companyxid, "A");

            return objReason;

        }

        internal GeneralMaster.Reason EReason(int pid)
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            DataTable dt = objBaseDataLayer.getDALGetReasonById(pid, "E", "");


            objReason.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objReason.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objReason.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objReason.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objReason.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objReason.NotesXid != -1)
            {
                objReason.NotesDescription = GetNotesById(objReason.NotesXid.GetValueOrDefault(-1));
            }
            return objReason;
        }

        internal GeneralMaster.Reason EReason(GeneralMaster.Reason model)
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objReason.Pid = model.Pid;
            objReason.RoomTypeName = model.RoomTypeName;
            objReason.MaxNoPpl = model.MaxNoPpl;

            objReason.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objReason.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objReason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objReason.NotesXid = NotesChanges(objNotes.Pid, objReason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objReason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objReason.NotesXid = NotesChanges(objNotes.Pid, objReason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyReason(objReason.Pid, objReason.RoomTypeName, objReason.MaxNoPpl, objReason.NotesXid.GetValueOrDefault(-1),
                                                      objReason.LastEditByXid, objReason.CompanyXid, "E");

            return objReason;

        }

        internal GeneralMaster.Reason DReason(int pid)
        {
            GeneralMaster.Reason objReason = new GeneralMaster.Reason();
            objBaseDataLayer.getDALGetReasonById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objReason;
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
            getUserSettings(objUsergrouprights, "DisplayReportingState");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ReportingState", searchPid).Tables[0];

            objReportingState.listReportingState = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ReportingState
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            Code = Convert.ToString(dr["Code"]),
                                            ReportingStateName = Convert.ToString("ReportingState"),
                                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
                                        }
                               ).Skip((currPage - 1) * objReportingState.PagingValues.MaxRows)
                                .Take(objReportingState.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objReportingState.PagingValues.MaxRows));
            objReportingState.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objReportingState.PagingValues.CurrentPageIndex = currPage;

            return objReportingState;

        }

        internal object SearchReportingState(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetReportingStateById(-1, "P", prefix);
            var listReportingState = dt.AsEnumerable().Where(x => x.Field<string>("ReportingState").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ReportingState
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("ReportingState")
                                        }).ToList();

            return listReportingState;

        }
        internal GeneralMaster.ReportingState AReportingState()
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            return objReportingState;
        }



        internal GeneralMaster.ReportingState AReportingState(GeneralMaster.ReportingState model)
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objReportingState.Pid = objNotes.Pid = -1;
            objReportingState.DiscountTypeName = model.DiscountTypeName;
            objReportingState.Sequence = model.Sequence;

            objReportingState.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objReportingState.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objReportingState.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objReportingState.NotesXid = NotesChanges(objNotes.Pid, objReportingState.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyReportingState(objReportingState.Pid, objReportingState.DiscountTypeName, objReportingState.Sequence, objReportingState.NotesXid.GetValueOrDefault(-1),
                                                      objReportingState.LastEditByXid, objReportingState.Companyxid, "A");

            return objReportingState;

        }

        internal GeneralMaster.ReportingState EReportingState(int pid)
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            DataTable dt = objBaseDataLayer.getDALGetReportingStateById(pid, "E", "");


            objReportingState.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objReportingState.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objReportingState.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objReportingState.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objReportingState.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objReportingState.NotesXid != -1)
            {
                objReportingState.NotesDescription = GetNotesById(objReportingState.NotesXid.GetValueOrDefault(-1));
            }
            return objReportingState;
        }

        internal GeneralMaster.ReportingState EReportingState(GeneralMaster.ReportingState model)
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objReportingState.Pid = model.Pid;
            objReportingState.RoomTypeName = model.RoomTypeName;
            objReportingState.MaxNoPpl = model.MaxNoPpl;

            objReportingState.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objReportingState.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objReportingState.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objReportingState.NotesXid = NotesChanges(objNotes.Pid, objReportingState.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objReportingState.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objReportingState.NotesXid = NotesChanges(objNotes.Pid, objReportingState.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyReportingState(objReportingState.Pid, objReportingState.RoomTypeName, objReportingState.MaxNoPpl, objReportingState.NotesXid.GetValueOrDefault(-1),
                                                      objReportingState.LastEditByXid, objReportingState.CompanyXid, "E");

            return objReportingState;

        }

        internal GeneralMaster.ReportingState DReportingState(int pid)
        {
            GeneralMaster.ReportingState objReportingState = new GeneralMaster.ReportingState();
            objBaseDataLayer.getDALGetReportingStateById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objReportingState;
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
            getUserSettings(objUsergrouprights, "DisplaySeason");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Season objSeason = new GeneralMaster.Season(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Season", searchPid).Tables[0];

            objSeason.listSeason = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objSeason.PagingValues.MaxRows)
                                .Take(objSeason.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objSeason.PagingValues.MaxRows));
            objSeason.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objSeason.PagingValues.CurrentPageIndex = currPage;

            return objSeason;

        }

        internal object SearchSeason(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listSeason = dt.AsEnumerable().Where(x => x.Field<string>("Season").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Season
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Season")
                                        }).ToList();

            return listSeason;

        }

        internal GeneralMaster.Season ASeason()
        {
            GeneralMaster.Season objSeason = new GeneralMaster.Season();
            return objSeason;
        }

        internal GeneralMaster.Season ASeason(GeneralMaster.Season model)
        {
            GeneralMaster.Season objSeason = new GeneralMaster.Season();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSeason.Pid = objNotes.Pid = -1;
            objSeason.DiscountTypeName = model.DiscountTypeName;
            objSeason.Sequence = model.Sequence;

            objSeason.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSeason.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objSeason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objSeason.NotesXid = NotesChanges(objNotes.Pid, objSeason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSeason.Pid, objSeason.DiscountTypeName, objSeason.Sequence, objSeason.NotesXid.GetValueOrDefault(-1),
                                                      objSeason.LastEditByXid, objSeason.Companyxid, "A");

            return objSeason;

        }

        internal GeneralMaster.Season ESeason(int pid)
        {
            GeneralMaster.Season objSeason = new GeneralMaster.Season();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objSeason.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objSeason.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objSeason.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objSeason.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objSeason.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objSeason.NotesXid != -1)
            {
                objSeason.NotesDescription = GetNotesById(objSeason.NotesXid.GetValueOrDefault(-1));
            }
            return objSeason;
        }

        internal GeneralMaster.Season ESeason(GeneralMaster.Season model)
        {
            GeneralMaster.Season objSeason = new GeneralMaster.Season();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSeason.Pid = model.Pid;
            objSeason.RoomTypeName = model.RoomTypeName;
            objSeason.MaxNoPpl = model.MaxNoPpl;

            objSeason.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSeason.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objSeason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objSeason.NotesXid = NotesChanges(objNotes.Pid, objSeason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objSeason.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objSeason.NotesXid = NotesChanges(objNotes.Pid, objSeason.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSeason.Pid, objSeason.RoomTypeName, objSeason.MaxNoPpl, objSeason.NotesXid.GetValueOrDefault(-1),
                                                      objSeason.LastEditByXid, objSeason.CompanyXid, "E");

            return objSeason;

        }

        internal GeneralMaster.Season DSeason(int pid)
        {
            GeneralMaster.Season objSeason = new GeneralMaster.Season();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objSeason;
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
            getUserSettings(objUsergrouprights, "DisplaySource");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Source objSource = new GeneralMaster.Source(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Source", searchPid).Tables[0];

            objSource.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objSource.PagingValues.MaxRows)
                                .Take(objSource.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objSource.PagingValues.MaxRows));
            objSource.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objSource.PagingValues.CurrentPageIndex = currPage;

            return objSource;

        }

        internal object SearchSource(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listSource = dt.AsEnumerable().Where(x => x.Field<string>("Source").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Source
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Source")
                                        }).ToList();

            return listSource;

        }

        internal GeneralMaster.Source ASource()
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            return objSource;
        }

        internal GeneralMaster.Source ASource(GeneralMaster.Source model)
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSource.Pid = objNotes.Pid = -1;
            objSource.DiscountTypeName = model.DiscountTypeName;
            objSource.Sequence = model.Sequence;

            objSource.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSource.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objSource.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objSource.NotesXid = NotesChanges(objNotes.Pid, objSource.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSource.Pid, objSource.DiscountTypeName, objSource.Sequence, objSource.NotesXid.GetValueOrDefault(-1),
                                                      objSource.LastEditByXid, objSource.Companyxid, "A");

            return objSource;

        }

        internal GeneralMaster.Source ESource(int pid)
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objSource.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objSource.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objSource.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objSource.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objSource.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objSource.NotesXid != -1)
            {
                objSource.NotesDescription = GetNotesById(objSource.NotesXid.GetValueOrDefault(-1));
            }
            return objSource;
        }

        internal GeneralMaster.Source ESource(GeneralMaster.Source model)
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSource.Pid = model.Pid;
            objSource.RoomTypeName = model.RoomTypeName;
            objSource.MaxNoPpl = model.MaxNoPpl;

            objSource.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSource.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objSource.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objSource.NotesXid = NotesChanges(objNotes.Pid, objSource.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objSource.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objSource.NotesXid = NotesChanges(objNotes.Pid, objSource.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSource.Pid, objSource.RoomTypeName, objSource.MaxNoPpl, objSource.NotesXid.GetValueOrDefault(-1),
                                                      objSource.LastEditByXid, objSource.CompanyXid, "E");

            return objSource;

        }

        internal GeneralMaster.Source DSource(int pid)
        {
            GeneralMaster.Source objSource = new GeneralMaster.Source();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objSource;
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
            getUserSettings(objUsergrouprights, "DisplayStatus");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Status objStatus = new GeneralMaster.Status(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Status", searchPid).Tables[0];

            objStatus.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Status
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objStatus.PagingValues.MaxRows)
                                .Take(objStatus.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objStatus.PagingValues.MaxRows));
            objStatus.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objStatus.PagingValues.CurrentPageIndex = currPage;

            return objStatus;
        }

        internal object SearchStatus(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listStatus = dt.AsEnumerable().Where(x => x.Field<string>("Status").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Status
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Status")
                                        }).ToList();

            return listStatus;

        }

        internal GeneralMaster.Status AStatus()
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            return objStatus;
        }

        internal GeneralMaster.Status AStatus(GeneralMaster.Status model)
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objStatus.Pid = objNotes.Pid = -1;
            objStatus.DiscountTypeName = model.DiscountTypeName;
            objStatus.Sequence = model.Sequence;

            objStatus.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objStatus.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objStatus.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objStatus.NotesXid = NotesChanges(objNotes.Pid, objStatus.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objStatus.Pid, objStatus.DiscountTypeName, objStatus.Sequence, objStatus.NotesXid.GetValueOrDefault(-1),
                                                      objStatus.LastEditByXid, objStatus.Companyxid, "A");

            return objStatus;

        }

        internal GeneralMaster.Status EStatus(int pid)
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objStatus.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objStatus.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objStatus.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objStatus.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objStatus.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objStatus.NotesXid != -1)
            {
                objStatus.NotesDescription = GetNotesById(objStatus.NotesXid.GetValueOrDefault(-1));
            }
            return objStatus;

        }

        internal GeneralMaster.Status EStatus(GeneralMaster.Status model)
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objStatus.Pid = model.Pid;
            objStatus.RoomTypeName = model.RoomTypeName;
            objStatus.MaxNoPpl = model.MaxNoPpl;

            objStatus.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objStatus.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objStatus.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objStatus.NotesXid = NotesChanges(objNotes.Pid, objStatus.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objStatus.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objStatus.NotesXid = NotesChanges(objNotes.Pid, objStatus.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objStatus.Pid, objStatus.RoomTypeName, objStatus.MaxNoPpl, objStatus.NotesXid.GetValueOrDefault(-1),
                                                      objStatus.LastEditByXid, objStatus.CompanyXid, "E");

            return objStatus;

        }

        internal GeneralMaster.Status DStatus(int pid)
        {
            GeneralMaster.Status objStatus = new GeneralMaster.Status();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objStatus;
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
            getUserSettings(objUsergrouprights, "DisplaySupplement");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Supplement", searchPid).Tables[0];

            objSupplement.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objSupplement.PagingValues.MaxRows)
                                .Take(objSupplement.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objSupplement.PagingValues.MaxRows));
            objSupplement.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objSupplement.PagingValues.CurrentPageIndex = currPage;

            return objSupplement;

        }

        internal object SearchSupplement(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listSupplement = dt.AsEnumerable().Where(x => x.Field<string>("Supplement").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Supplement
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Supplement")
                                        }).ToList();

            return listSupplement;

        }

        internal GeneralMaster.Supplement ASupplement()
        {
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            return objSupplement;
        }

        internal GeneralMaster.Supplement ASupplement(GeneralMaster.Supplement model)
        {
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSupplement.Pid = objNotes.Pid = -1;
            objSupplement.DiscountTypeName = model.DiscountTypeName;
            objSupplement.Sequence = model.Sequence;

            objSupplement.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSupplement.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objSupplement.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objSupplement.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSupplement.Pid, objSupplement.DiscountTypeName, objSupplement.Sequence, objSupplement.NotesXid.GetValueOrDefault(-1),
                                                      objSupplement.LastEditByXid, objSupplement.Companyxid, "A");

            return objSupplement;

        }

        internal GeneralMaster.Supplement ESupplement(int pid)
        {
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objSupplement.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objSupplement.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objSupplement.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objSupplement.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objSupplement.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objRoomType.NotesXid != -1)
            {
                objSupplement.NotesDescription = GetNotesById(objSupplement.NotesXid.GetValueOrDefault(-1));
            }
            return objSupplement;

        }

        internal GeneralMaster.Supplement ESupplement(GeneralMaster.Supplement model)
        {
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSupplement.Pid = model.Pid;
            objSupplement.RoomTypeName = model.RoomTypeName;
            objSupplement.MaxNoPpl = model.MaxNoPpl;

            objSupplement.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSupplement.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objSupplement.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objSupplement.NotesXid = NotesChanges(objNotes.Pid, objSupplement.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objSupplement.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objSupplement.NotesXid = NotesChanges(objNotes.Pid, objSupplement.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSupplement.Pid, objSupplement.RoomTypeName, objSupplement.MaxNoPpl, objSupplement.NotesXid.GetValueOrDefault(-1),
                                                      objSupplement.LastEditByXid, objSupplement.CompanyXid, "E");

            return objSupplement;

        }

        internal GeneralMaster.Supplement DSupplement(int pid)
        {
            GeneralMaster.Supplement objSupplement = new GeneralMaster.Supplement();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objSupplement;
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
            getUserSettings(objUsergrouprights, "DisplaySupplementType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("SupplementType", searchPid).Tables[0];

            objSupplementType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.SupplementType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objSupplementType.PagingValues.MaxRows)
                                .Take(objSupplementType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objSupplementType.PagingValues.MaxRows));
            objSupplementType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objSupplementType.PagingValues.CurrentPageIndex = currPage;

            return objSupplementType;

        }

        internal object SearchSupplementType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listSupplementType = dt.AsEnumerable().Where(x => x.Field<string>("SupplementType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.SupplementType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("SupplementType")
                                        }).ToList();

            return listSupplementType;

        }

        internal GeneralMaster.SupplementType ASupplementType()
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            return objSupplementType;
        }

        internal GeneralMaster.SupplementType ASupplementType(GeneralMaster.SupplementType model)
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSupplementType.Pid = objNotes.Pid = -1;
            objSupplementType.DiscountTypeName = model.DiscountTypeName;
            objSupplementType.Sequence = model.Sequence;

            objSupplementType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSupplementType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objSupplementType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objSupplementType.NotesXid = NotesChanges(objNotes.Pid, objSupplementType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSupplementType.Pid, objSupplementType.DiscountTypeName, objSupplementType.Sequence, objSupplementType.NotesXid.GetValueOrDefault(-1),
                                                      objSupplementType.LastEditByXid, objSupplementType.Companyxid, "A");

            return objSupplementType;

        }

        internal GeneralMaster.SupplementType ESupplementType(int pid)
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objSupplementType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objSupplementType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objSupplementType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objSupplementType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objSupplementType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objSupplementType.NotesXid != -1)
            {
                objSupplementType.NotesDescription = GetNotesById(objSupplementType.NotesXid.GetValueOrDefault(-1));
            }
            return objSupplementType;

        }

        internal GeneralMaster.SupplementType ESupplementType(GeneralMaster.SupplementType model)
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objSupplementType.Pid = model.Pid;
            objSupplementType.RoomTypeName = model.RoomTypeName;
            objSupplementType.MaxNoPpl = model.MaxNoPpl;

            objSupplementType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objSupplementType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objSupplementType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objSupplementType.NotesXid = NotesChanges(objNotes.Pid, objSupplementType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objSupplementType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objSupplementType.NotesXid = NotesChanges(objNotes.Pid, objSupplementType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objSupplementType.Pid, objSupplementType.RoomTypeName, objSupplementType.MaxNoPpl, objSupplementType.NotesXid.GetValueOrDefault(-1),
                                                      objSupplementType.LastEditByXid, objSupplementType.CompanyXid, "E");

            return objSupplementType;

        }

        internal GeneralMaster.SupplementType DSupplementType(int pid)
        {
            GeneralMaster.SupplementType objSupplementType = new GeneralMaster.SupplementType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objSupplementType;
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
            getUserSettings(objUsergrouprights, "DisplayTax");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Tax objTax = new GeneralMaster.Tax(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Tax", searchPid).Tables[0];

            objTax.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objTax.PagingValues.MaxRows)
                                .Take(objTax.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objTax.PagingValues.MaxRows));
            objTax.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objTax.PagingValues.CurrentPageIndex = currPage;

            return objTax;

        }

        internal object SearchTax(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listTax = dt.AsEnumerable().Where(x => x.Field<string>("Tax").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Tax
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Tax")
                                        }).ToList();

            return listTax;

        }

        internal GeneralMaster.Tax ATax()
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            return objTax;
        }

        internal GeneralMaster.Tax ATax(GeneralMaster.Tax model)
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTax.Pid = objNotes.Pid = -1;
            objTax.DiscountTypeName = model.DiscountTypeName;
            objTax.Sequence = model.Sequence;

            objTax.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTax.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objTax.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objTax.NotesXid = NotesChanges(objNotes.Pid, objTax.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTax.Pid, objTax.DiscountTypeName, objTax.Sequence, objTax.NotesXid.GetValueOrDefault(-1),
                                                      objTax.LastEditByXid, objTax.Companyxid, "A");

            return objTax;

        }

        internal GeneralMaster.Tax ETax(int pid)
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objTax.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objTax.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objTax.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objTax.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objTax.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objTax.NotesXid != -1)
            {
                objTax.NotesDescription = GetNotesById(objTax.NotesXid.GetValueOrDefault(-1));
            }
            return objTax;

        }

        internal GeneralMaster.Tax ETax(GeneralMaster.Tax model)
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTax.Pid = model.Pid;
            objTax.RoomTypeName = model.RoomTypeName;
            objTax.MaxNoPpl = model.MaxNoPpl;

            objTax.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTax.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objRoomType.NotesXid = NotesChanges(objNotes.Pid, objTax.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objTax.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objTax.NotesXid = NotesChanges(objNotes.Pid, objTax.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTax.Pid, objTax.RoomTypeName, objTax.MaxNoPpl, objTax.NotesXid.GetValueOrDefault(-1),
                                                      objTax.LastEditByXid, objTax.CompanyXid, "E");

            return objTax;

        }

        internal GeneralMaster.Tax DTax(int pid)
        {
            GeneralMaster.Tax objTax = new GeneralMaster.Tax();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objTax ;
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
            getUserSettings(objUsergrouprights, "DisplayTitle");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Title objTitle = new GeneralMaster.Title(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Title", searchPid).Tables[0];

            objTitle.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Title
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objTitle.PagingValues.MaxRows)
                                .Take(objTitle.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objTitle.PagingValues.MaxRows));
            objTitle.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objTitle.PagingValues.CurrentPageIndex = currPage;

            return objTitle;

        }

        internal object SearchTitle(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listTitle = dt.AsEnumerable().Where(x => x.Field<string>("Title").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Title
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Title")
                                        }).ToList();

            return listTitle;

        }

        internal GeneralMaster.Title ATitle()
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            return objTitle;
        }

        internal GeneralMaster.Title ATitle(GeneralMaster.Title model)
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTitle.Pid = objNotes.Pid = -1;
            objTitle.DiscountTypeName = model.DiscountTypeName;
            objTitle.Sequence = model.Sequence;

            objTitle.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTitle.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objTitle.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objTitle.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTitle.Pid, objTitle.DiscountTypeName, objTitle.Sequence, objTitle.NotesXid.GetValueOrDefault(-1),
                                                      objTitle.LastEditByXid, objTitle.Companyxid, "A");

            return objTitle;

        }

        internal GeneralMaster.Title ETitle(int pid)
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objTitle.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objTitle.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objTitle.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objTitle.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objTitle.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objTitle.NotesXid != -1)
            {
                objTitle.NotesDescription = GetNotesById(objTitle.NotesXid.GetValueOrDefault(-1));
            }
            return objTitle;
        }

        internal GeneralMaster.Title ETitle(GeneralMaster.Title model)
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTitle.Pid = model.Pid;
            objTitle.RoomTypeName = model.RoomTypeName;
            objTitle.MaxNoPpl = model.MaxNoPpl;

            objTitle.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTitle.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objTitle.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objTitle.NotesXid = NotesChanges(objNotes.Pid, objTitle.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objTitle.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objTitle.NotesXid = NotesChanges(objNotes.Pid, objTitle.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTitle.Pid, objTitle.RoomTypeName, objTitle.MaxNoPpl, objTitle.NotesXid.GetValueOrDefault(-1),
                                                      objTitle.LastEditByXid, objTitle.CompanyXid, "E");

            return objTitle;

        }

        internal GeneralMaster.Title DTitle(int pid)
        {
            GeneralMaster.Title objTitle = new GeneralMaster.Title();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objTitle;
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
            getUserSettings(objUsergrouprights, "DisplayCompany");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Company objCompany = new GeneralMaster.Company(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objCompany.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objCompany.PagingValues.MaxRows)
                                .Take(objCompany.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objCompany.PagingValues.MaxRows));
            objCompany.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objCompany.PagingValues.CurrentPageIndex = currPage;

            return objCompany;

        }

        internal object SearchCompany(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listCompany = dt.AsEnumerable().Where(x => x.Field<string>("Company").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Company
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Company")
                                        }).ToList();

            return listCompany;

        }

        internal GeneralMaster.Company ACompany()
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            return objCompany;
        }

        internal GeneralMaster.Company ACompany(GeneralMaster.Company model)
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCompany.Pid = objNotes.Pid = -1;
            objCompany.DiscountTypeName = model.DiscountTypeName;
            objCompany.Sequence = model.Sequence;

            objCompany.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCompany.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objCompany.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objCompany.NotesXid = NotesChanges(objNotes.Pid, objCompany.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objCompany.Pid, objCompany.DiscountTypeName, objCompany.Sequence, objCompany.NotesXid.GetValueOrDefault(-1),
                                                      objCompany.LastEditByXid, objCompany.Companyxid, "A");

            return objCompany;

        }

        internal GeneralMaster.Company ECompany(int pid)
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objCompany.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objCompany.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objCompany.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objCompany.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objCompany.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objCompany.NotesXid != -1)
            {
                objCompany.NotesDescription = GetNotesById(objCompany.NotesXid.GetValueOrDefault(-1));
            }
            return objCompany;
        }

        internal GeneralMaster.Company ECompany(GeneralMaster.Company model)
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objCompany.Pid = model.Pid;
            objCompany.RoomTypeName = model.RoomTypeName;
            objCompany.MaxNoPpl = model.MaxNoPpl;

            objCompany.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objCompany.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objCompany.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objCompany.NotesXid = NotesChanges(objNotes.Pid, objCompany.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objCompany.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objCompany.NotesXid = NotesChanges(objNotes.Pid, objCompany.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objCompany.Pid, objCompany.RoomTypeName, objCompany.MaxNoPpl, objCompany.NotesXid.GetValueOrDefault(-1),
                                                      objCompany.LastEditByXid, objCompany.CompanyXid, "E");

            return objCompany;

        }

        internal GeneralMaster.Company DCompany(int pid)
        {
            GeneralMaster.Company objCompany = new GeneralMaster.Company();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objCompany;
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
            getUserSettings(objUsergrouprights, "DisplayDepartment");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Department objDepartment = new GeneralMaster.Department(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Department", searchPid).Tables[0];

            objDepartment.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Department
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objRoomType.PagingValues.MaxRows)
                                .Take(objRoomType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objDepartment.PagingValues.MaxRows));
            objDepartment.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objDepartment.PagingValues.CurrentPageIndex = currPage;

            return objDepartment;

        }

        internal object SearchDepartment(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listDepartment = dt.AsEnumerable().Where(x => x.Field<string>("Department").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Department
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Department")
                                        }).ToList();

            return listDepartment;

        }

        internal GeneralMaster.Department ADepartment()
        {
            GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            return objDepartment;
        }

        internal GeneralMaster.Department ADepartment(GeneralMaster.Department model)
        {
            GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDepartment.Pid = objNotes.Pid = -1;
            objDepartment.DiscountTypeName = model.DiscountTypeName;
            objDepartment.Sequence = model.Sequence;

            objDepartment.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDepartment.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDepartment.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objDepartment.NotesXid = NotesChanges(objNotes.Pid, objDepartment.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDepartment.Pid, objDepartment.DiscountTypeName, objDepartment.Sequence, objDepartment.NotesXid.GetValueOrDefault(-1),
                                                      objDepartment.LastEditByXid, objDepartment.Companyxid, "A");

            return objDepartment;

        }

        internal GeneralMaster.Department EDepartment(int pid)
        {
            GeneralMaster.Department objDepartment  = new GeneralMaster.Department();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objDepartment.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objDepartment.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objDepartment.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objDepartment.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objDepartment.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objDepartment.NotesXid != -1)
            {
                objDepartment.NotesDescription = GetNotesById(objRoomType.NotesXid.GetValueOrDefault(-1));
            }
            return objDepartment;
        }

        internal GeneralMaster.Department EDepartment(GeneralMaster.Department model)
        {
            GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDepartment.Pid = model.Pid;
            objDepartment.RoomTypeName = model.RoomTypeName;
            objDepartment.MaxNoPpl = model.MaxNoPpl;

            objDepartment.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDepartment.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objDepartment.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objDepartment.NotesXid = NotesChanges(objNotes.Pid, objDepartment.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objDepartment.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objDepartment.NotesXid = NotesChanges(objNotes.Pid, objDepartment.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDepartment.Pid, objDepartment.RoomTypeName, objDepartment.MaxNoPpl, objDepartment.NotesXid.GetValueOrDefault(-1),
                                                      objDepartment.LastEditByXid, objDepartment.CompanyXid, "E");

            return objDepartment;

        }

        internal GeneralMaster.Department DDepartment(int pid)
        {
            GeneralMaster.Department objDepartment = new GeneralMaster.Department();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objDepartment;
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
            getUserSettings(objUsergrouprights, "DisplayDesignation");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Designation", searchPid).Tables[0];

            objDesignation.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objDesignation.PagingValues.MaxRows)
                                .Take(objDesignation.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objDesignation.PagingValues.MaxRows));
            objDesignation.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objDesignation.PagingValues.CurrentPageIndex = currPage;

            return objDesignation;

        }

        internal object SearchDesignation(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listDesignation = dt.AsEnumerable().Where(x => x.Field<string>("Designation").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Designation
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Designation")
                                        }).ToList();

            return listDesignation;

        }

        internal GeneralMaster.Designation ADesignation()
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            return objDesignation;
        }

        internal GeneralMaster.Designation ADesignation(GeneralMaster.Designation model)
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDesignation.Pid = objNotes.Pid = -1;
            objDesignation.DiscountTypeName = model.DiscountTypeName;
            objDesignation.Sequence = model.Sequence;

            objDesignation.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDesignation.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDesignation.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objDesignation.NotesXid = NotesChanges(objNotes.Pid, objDesignation.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDesignation.Pid, objDesignation.DiscountTypeName, objDesignation.Sequence, objDesignation.NotesXid.GetValueOrDefault(-1),
                                                      objDesignation.LastEditByXid, objDesignation.Companyxid, "A");

            return objDesignation;

        }

        internal GeneralMaster.Designation EDesignation(int pid)
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objDesignation.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objDesignation.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objDesignation.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objDesignation.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objDesignation.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objDesignation.NotesXid != -1)
            {
                objDesignation.NotesDescription = GetNotesById(objDesignation.NotesXid.GetValueOrDefault(-1));
            }
            return objDesignation;
        }

        internal GeneralMaster.Designation EDesignation(GeneralMaster.Designation model)
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDesignation.Pid = model.Pid;
            objDesignation.RoomTypeName = model.RoomTypeName;
            objDesignation.MaxNoPpl = model.MaxNoPpl;

            objDesignation.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDesignation.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objDesignation.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objDesignation.NotesXid = NotesChanges(objNotes.Pid, objDesignation.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objDesignation.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objDesignation.NotesXid = NotesChanges(objNotes.Pid, objDesignation.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDesignation.Pid, objDesignation.RoomTypeName, objDesignation.MaxNoPpl, objDesignation.NotesXid.GetValueOrDefault(-1),
                                                      objDesignation.LastEditByXid, objDesignation.CompanyXid, "E");

            return objDesignation;

        }

        internal GeneralMaster.Designation DDesignation(int pid)
        {
            GeneralMaster.Designation objDesignation = new GeneralMaster.Designation();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objDesignation;
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
            getUserSettings(objUsergrouprights, "DisplayDMCSystemConfiguration");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("DMCSystemConfiguration", searchPid).Tables[0];

            objDMCSystemConfiguration.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.DMCSystemConfiguration
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objDMCSystemConfiguration.PagingValues.MaxRows)
                                .Take(objDMCSystemConfiguration.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objDMCSystemConfiguration.PagingValues.MaxRows));
            objDMCSystemConfiguration.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objDMCSystemConfiguration.PagingValues.CurrentPageIndex = currPage;

            return objDMCSystemConfiguration;

        }

        internal object SearchDMCSystemConfiguration(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listDMCSystemConfiguration = dt.AsEnumerable().Where(x => x.Field<string>("DMCSystemConfiguration").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.DMCSystemConfiguration
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("DMCSystemConfiguration")
                                        }).ToList();

            return listDMCSystemConfiguration;

        }

        internal GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration()
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            return objDMCSystemConfiguration;
        }

        internal GeneralMaster.DMCSystemConfiguration ADMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDMCSystemConfiguration.Pid = objNotes.Pid = -1;
            objDMCSystemConfiguration.DiscountTypeName = model.DiscountTypeName;
            objDMCSystemConfiguration.Sequence = model.Sequence;

            objDMCSystemConfiguration.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDMCSystemConfiguration.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDMCSystemConfiguration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objDMCSystemConfiguration.NotesXid = NotesChanges(objNotes.Pid, objDMCSystemConfiguration.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDMCSystemConfiguration.Pid, objDMCSystemConfiguration.DiscountTypeName, objDMCSystemConfiguration.Sequence, objDMCSystemConfiguration.NotesXid.GetValueOrDefault(-1),
                                                      objDMCSystemConfiguration.LastEditByXid, objDMCSystemConfiguration.Companyxid, "A");

            return objDMCSystemConfiguration;

        }

        internal GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(int pid)
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objDMCSystemConfiguration.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objDMCSystemConfiguration.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objDMCSystemConfiguration.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objDMCSystemConfiguration.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objDMCSystemConfiguration.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objDMCSystemConfiguration.NotesXid != -1)
            {
                objDMCSystemConfiguration.NotesDescription = GetNotesById(objDMCSystemConfiguration.NotesXid.GetValueOrDefault(-1));
            }
            return objDMCSystemConfiguration;
        }

        internal GeneralMaster.DMCSystemConfiguration EDMCSystemConfiguration(GeneralMaster.DMCSystemConfiguration model)
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDMCSystemConfiguration.Pid = model.Pid;
            objDMCSystemConfiguration.RoomTypeName = model.RoomTypeName;
            objDMCSystemConfiguration.MaxNoPpl = model.MaxNoPpl;

            objDMCSystemConfiguration.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDMCSystemConfiguration.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objDMCSystemConfiguration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objDMCSystemConfiguration.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objDMCSystemConfiguration.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objDMCSystemConfiguration.NotesXid = NotesChanges(objNotes.Pid, objDMCSystemConfiguration.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDMCSystemConfiguration.Pid, objDMCSystemConfiguration.RoomTypeName, objDMCSystemConfiguration.MaxNoPpl, objDMCSystemConfiguration.NotesXid.GetValueOrDefault(-1),
                                                      objDMCSystemConfiguration.LastEditByXid, objDMCSystemConfiguration.CompanyXid, "E");

            return objDMCSystemConfiguration;

        }

        internal GeneralMaster.DMCSystemConfiguration DDMCSystemConfiguration(int pid)
        {
            GeneralMaster.DMCSystemConfiguration objDMCSystemConfiguration = new GeneralMaster.DMCSystemConfiguration();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objDMCSystemConfiguration;
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
            getUserSettings(objUsergrouprights, "DisplayImageLibrary");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ImageLibrary", searchPid).Tables[0];

            objImageLibrary.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ImageLibrary
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objImageLibrary.PagingValues.MaxRows)
                                .Take(objImageLibrary.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objImageLibrary.PagingValues.MaxRows));
            objImageLibrary.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objImageLibrary.PagingValues.CurrentPageIndex = currPage;

            return objImageLibrary;

        }

        internal object SearchImageLibrary(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listRoomType = dt.AsEnumerable().Where(x => x.Field<string>("ImageLibrary").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ImageLibrary
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("ImageLibrary")
                                        }).ToList();

            return listRoomType;

        }

        internal GeneralMaster.ImageLibrary AImageLibrary()
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            return objImageLibrary;
        }

        internal GeneralMaster.ImageLibrary EImageLibrary(int pid)
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objImageLibrary.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objImageLibrary.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objImageLibrary.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objImageLibrary.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objImageLibrary.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objImageLibrary.NotesXid != -1)
            {
                objImageLibrary.NotesDescription = GetNotesById(objImageLibrary.NotesXid.GetValueOrDefault(-1));
            }
            return objImageLibrary;
        }

        internal GeneralMaster.ImageLibrary EImageLibrary(GeneralMaster.ImageLibrary model)
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objImageLibrary.Pid = model.Pid;
            objImageLibrary.RoomTypeName = model.RoomTypeName;
            objImageLibrary.MaxNoPpl = model.MaxNoPpl;

            objImageLibrary.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objImageLibrary.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objImageLibrary.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objImageLibrary.NotesXid = NotesChanges(objNotes.Pid, objImageLibrary.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objImageLibrary.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objImageLibrary.NotesXid = NotesChanges(objNotes.Pid, objImageLibrary.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objImageLibrary.Pid, objImageLibrary.RoomTypeName, objImageLibrary.MaxNoPpl, objRoomType.NotesXid.GetValueOrDefault(-1),
                                                      objImageLibrary.LastEditByXid, objImageLibrary.CompanyXid, "E");

            return objImageLibrary;

        }

        internal GeneralMaster.ImageLibrary AImageLibrary(GeneralMaster.ImageLibrary model)
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objImageLibrary.Pid = objNotes.Pid = -1;
            objImageLibrary.DiscountTypeName = model.DiscountTypeName;
            objImageLibrary.Sequence = model.Sequence;

            objImageLibrary.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objImageLibrary.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objImageLibrary.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objImageLibrary.NotesXid = NotesChanges(objNotes.Pid, objImageLibrary.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objImageLibrary.Pid, objImageLibrary.DiscountTypeName, objImageLibrary.Sequence, objImageLibrary.NotesXid.GetValueOrDefault(-1),
                                                      objImageLibrary.LastEditByXid, objImageLibrary.Companyxid, "A");

            return objImageLibrary;

        }

        internal GeneralMaster.ImageLibrary DImageLibrary(int pid)
        {
            GeneralMaster.ImageLibrary objImageLibrary = new GeneralMaster.ImageLibrary();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objImageLibrary;
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
            getUserSettings(objUsergrouprights, "DisplayDepot");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Depot", searchPid).Tables[0];

            objDepot.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objDepot.PagingValues.MaxRows)
                                .Take(objDepot.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objDepot.PagingValues.MaxRows));
            objDepot.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objDepot.PagingValues.CurrentPageIndex = currPage;

            return objDepot;

        }

        internal object SearchDepot(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listDepot = dt.AsEnumerable().Where(x => x.Field<string>("Depot").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Depot
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Depot")
                                        }).ToList();

            return listDepot;

        }

        internal GeneralMaster.Depot ADepot()
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            return objDepot;
        }

        internal GeneralMaster.Depot ADepot(GeneralMaster.Depot model)
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDepot.Pid = objNotes.Pid = -1;
            objDepot.DiscountTypeName = model.DiscountTypeName;
            objDepot.Sequence = model.Sequence;

            objDepot.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDepot.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objDepot.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objDepot.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDepot.Pid, objDepot.DiscountTypeName, objDepot.Sequence, objDepot.NotesXid.GetValueOrDefault(-1),
                                                      objDepot.LastEditByXid, objDepot.Companyxid, "A");

            return objDepot;

        }

        internal GeneralMaster.Depot EDepot(int pid)
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objDepot.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objDepot.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objDepot.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objDepot.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objDepot.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objDepot.NotesXid != -1)
            {
                objDepot.NotesDescription = GetNotesById(objDepot.NotesXid.GetValueOrDefault(-1));
            }
            return objDepot;
        }

        internal GeneralMaster.Depot EDepot(GeneralMaster.Depot model)
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objDepot.Pid = model.Pid;
            objDepot.RoomTypeName = model.RoomTypeName;
            objDepot.MaxNoPpl = model.MaxNoPpl;

            objDepot.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objDepot.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objDepot.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objDepot.NotesXid = NotesChanges(objNotes.Pid, objDepot.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objDepot.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objDepot.NotesXid = NotesChanges(objNotes.Pid, objDepot.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objDepot.Pid, objDepot.RoomTypeName, objDepot.MaxNoPpl, objDepot.NotesXid.GetValueOrDefault(-1),
                                                      objDepot.LastEditByXid, objDepot.CompanyXid, "E");

            return objDepot;

        }

        internal GeneralMaster.Depot DDepot(int pid)
        {
            GeneralMaster.Depot objDepot = new GeneralMaster.Depot();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objDepot;
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
            getUserSettings(objUsergrouprights, "DisplayContractingGroup");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ContractingGroup", searchPid).Tables[0];

            objContractingGroup.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ContractingGroup
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objContractingGroup.PagingValues.MaxRows)
                                .Take(objContractingGroup.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objContractingGroup.PagingValues.MaxRows));
            objContractingGroup.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objContractingGroup.PagingValues.CurrentPageIndex = currPage;

            return objContractingGroup;

        }

        internal object SearchContractingGroup(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listContractingGroup = dt.AsEnumerable().Where(x => x.Field<string>("ContractingGroup").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ContractingGroup
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("ContractingGroup")
                                        }).ToList();

            return listContractingGroup;

        }

        internal GeneralMaster.ContractingGroup AContractingGroup()
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            return objContractingGroup;
        }

        internal GeneralMaster.ContractingGroup AContractingGroup(GeneralMaster.ContractingGroup model)
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objContractingGroup.Pid = objNotes.Pid = -1;
            objContractingGroup.DiscountTypeName = model.DiscountTypeName;
            objContractingGroup.Sequence = model.Sequence;

            objContractingGroup.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objContractingGroup.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objContractingGroup.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objContractingGroup.NotesXid = NotesChanges(objNotes.Pid, objContractingGroup.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objContractingGroup.Pid, objContractingGroup.DiscountTypeName, objContractingGroup.Sequence, objContractingGroup.NotesXid.GetValueOrDefault(-1),
                                                      objContractingGroup.LastEditByXid, objContractingGroup.Companyxid, "A");

            return objContractingGroup;

        }

        internal GeneralMaster.ContractingGroup EContractingGroup(int pid)
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objContractingGroup.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objContractingGroup.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objContractingGroup.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objContractingGroup.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objContractingGroup.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objContractingGroup.NotesXid != -1)
            {
                objContractingGroup.NotesDescription = GetNotesById(objContractingGroup.NotesXid.GetValueOrDefault(-1));
            }
            return objContractingGroup;

        }

        internal GeneralMaster.ContractingGroup EContractingGroup(GeneralMaster.ContractingGroup model)
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objContractingGroup.Pid = model.Pid;
            objContractingGroup.RoomTypeName = model.RoomTypeName;
            objContractingGroup.MaxNoPpl = model.MaxNoPpl;

            objContractingGroup.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objContractingGroup.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objContractingGroup.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objContractingGroup.NotesXid = NotesChanges(objNotes.Pid, objContractingGroup.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objContractingGroup.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objContractingGroup.NotesXid = NotesChanges(objNotes.Pid, objContractingGroup.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objContractingGroup.Pid, objContractingGroup.RoomTypeName, objContractingGroup.MaxNoPpl, objContractingGroup.NotesXid.GetValueOrDefault(-1),
                                                      objContractingGroup.LastEditByXid, objContractingGroup.CompanyXid, "E");

            return objContractingGroup;

        }

        internal GeneralMaster.ContractingGroup DContractingGroup(int pid)
        {
            GeneralMaster.ContractingGroup objContractingGroup = new GeneralMaster.ContractingGroup();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objContractingGroup;
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
            getUserSettings(objUsergrouprights, "DisplayTblTariff");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("TblTariff", searchPid).Tables[0];

            objTblTariff.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.TblTariff
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objTblTariff.PagingValues.MaxRows)
                                .Take(objTblTariff.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objTblTariff.PagingValues.MaxRows));
            objTblTariff.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objTblTariff.PagingValues.CurrentPageIndex = currPage;

            return objTblTariff;

        }

        internal object SearchTblTariff(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listTblTariff = dt.AsEnumerable().Where(x => x.Field<string>("TblTariff").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.TblTariff
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("TblTariff")
                                        }).ToList();

            return listTblTariff;

        }

        internal GeneralMaster.TblTariff ATblTariff()
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            return objTblTariff;
        }

        internal GeneralMaster.TblTariff ATblTariff(GeneralMaster.TblTariff model)
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTblTariff.Pid = objNotes.Pid = -1;
            objTblTariff.DiscountTypeName = model.DiscountTypeName;
            objTblTariff.Sequence = model.Sequence;

            objTblTariff.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTblTariff.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objTblTariff.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objTblTariff.NotesXid = NotesChanges(objNotes.Pid, objTblTariff.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTblTariff.Pid, objTblTariff.DiscountTypeName, objTblTariff.Sequence, objTblTariff.NotesXid.GetValueOrDefault(-1),
                                                      objTblTariff.LastEditByXid, objTblTariff.Companyxid, "A");

            return objTblTariff;

        }

        internal GeneralMaster.TblTariff ETblTariff(int pid)
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objTblTariff.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objTblTariff.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objTblTariff.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objTblTariff.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objTblTariff.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objTblTariff.NotesXid != -1)
            {
                objTblTariff.NotesDescription = GetNotesById(objTblTariff.NotesXid.GetValueOrDefault(-1));
            }
            return objTblTariff;

        }

        internal GeneralMaster.TblTariff ETblTariff(GeneralMaster.TblTariff model)
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTblTariff.Pid = model.Pid;
            objTblTariff.RoomTypeName = model.RoomTypeName;
            objTblTariff.MaxNoPpl = model.MaxNoPpl;

            objTblTariff.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTblTariff.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objTblTariff.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objTblTariff.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objTblTariff.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objTblTariff.NotesXid = NotesChanges(objNotes.Pid, objTblTariff.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTblTariff.Pid, objTblTariff.RoomTypeName, objTblTariff.MaxNoPpl, objTblTariff.NotesXid.GetValueOrDefault(-1),
                                                      objTblTariff.LastEditByXid, objTblTariff.CompanyXid, "E");

            return objTblTariff;

        }

        internal GeneralMaster.TblTariff DTblTariff(int pid)
        {
            GeneralMaster.TblTariff objTblTariff = new GeneralMaster.TblTariff();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objTblTariff;
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
            getUserSettings(objUsergrouprights, "DisplayTblTariffMarkets");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("TblTariffMarkets", searchPid).Tables[0];

            objTblTariffMarkets.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objTblTariffMarkets.PagingValues.MaxRows)
                                .Take(objTblTariffMarkets.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objTblTariffMarkets.PagingValues.MaxRows));
            objTblTariffMarkets.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objTblTariffMarkets.PagingValues.CurrentPageIndex = currPage;

            return objTblTariffMarkets;

        }

        internal object SearchTblTariffMarkets(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listTblTariffMarkets = dt.AsEnumerable().Where(x => x.Field<string>("TblTariffMarkets").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.TblTariffMarkets
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("TblTariffMarkets")
                                        }).ToList();

            return listTblTariffMarkets;

        }

        internal GeneralMaster.TblTariffMarkets ATblTariffMarkets()
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            return objTblTariffMarkets;
        }

        internal GeneralMaster.TblTariffMarkets ATblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTblTariffMarkets.Pid = objNotes.Pid = -1;
            objTblTariffMarkets.DiscountTypeName = model.DiscountTypeName;
            objTblTariffMarkets.Sequence = model.Sequence;

            objTblTariffMarkets.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTblTariffMarkets.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objTblTariffMarkets.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objTblTariffMarkets.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTblTariffMarkets.Pid, objTblTariffMarkets.DiscountTypeName, objTblTariffMarkets.Sequence, objTblTariffMarkets.NotesXid.GetValueOrDefault(-1),
                                                      objTblTariffMarkets.LastEditByXid, objTblTariffMarkets.Companyxid, "A");

            return objTblTariffMarkets;

        }

        internal GeneralMaster.TblTariffMarkets ETblTariffMarkets(int pid)
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objTblTariffMarkets.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objTblTariffMarkets.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objTblTariffMarkets.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objTblTariffMarkets.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objTblTariffMarkets.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objTblTariffMarkets.NotesXid != -1)
            {
                objTblTariffMarkets.NotesDescription = GetNotesById(objTblTariffMarkets.NotesXid.GetValueOrDefault(-1));
            }
            return objTblTariffMarkets;
        }

        internal GeneralMaster.TblTariffMarkets ETblTariffMarkets(GeneralMaster.TblTariffMarkets model)
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objTblTariffMarkets.Pid = model.Pid;
            objTblTariffMarkets.RoomTypeName = model.RoomTypeName;
            objTblTariffMarkets.MaxNoPpl = model.MaxNoPpl;

            objTblTariffMarkets.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objTblTariffMarkets.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objTblTariffMarkets.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objTblTariffMarkets.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objTblTariffMarkets.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objTblTariffMarkets.NotesXid = NotesChanges(objNotes.Pid, objTblTariffMarkets.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objTblTariffMarkets.Pid, objTblTariffMarkets.RoomTypeName, objTblTariffMarkets.MaxNoPpl, objTblTariffMarkets.NotesXid.GetValueOrDefault(-1),
                                                      objTblTariffMarkets.LastEditByXid, objTblTariffMarkets.CompanyXid, "E");

            return objTblTariffMarkets;

        }

        internal GeneralMaster.TblTariffMarkets DTblTariffMarkets(int pid)
        {
            GeneralMaster.TblTariffMarkets objTblTariffMarkets = new GeneralMaster.TblTariffMarkets();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objTblTariffMarkets;
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
            getUserSettings(objUsergrouprights, "DisplayClient");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Client objClient = new GeneralMaster.Client(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Client", searchPid).Tables[0];

            objClient.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.Client
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objClient.PagingValues.MaxRows)
                                .Take(objClient.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objClient.PagingValues.MaxRows));
            objClient.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objClient.PagingValues.CurrentPageIndex = currPage;

            return objClient;

        }

        internal object SearchClient(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listClient = dt.AsEnumerable().Where(x => x.Field<string>("Client").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.Client
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Client")
                                        }).ToList();

            return listClient;

        }

        internal GeneralMaster.Client AClient()
        {
            GeneralMaster.Client objRoomType = new GeneralMaster.Client();
            return objClient;
        }

        internal GeneralMaster.Client AClient(GeneralMaster.Client model)
        {
            GeneralMaster.Client objClient = new GeneralMaster.Client();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objClient.Pid = objNotes.Pid = -1;
            objClient.DiscountTypeName = model.DiscountTypeName;
            objClient.Sequence = model.Sequence;

            objClient.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objClient.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objClient.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objClient.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objClient.Pid, objClient.DiscountTypeName, objClient.Sequence, objClient.NotesXid.GetValueOrDefault(-1),
                                                      objClient.LastEditByXid, objClient.Companyxid, "A");

            return objClient;

        }

        internal GeneralMaster.Client EClient(int pid)
        {
            GeneralMaster.Client objClient = new GeneralMaster.Client();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objClient.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objClient.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objClient.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objClient.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objClient.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objClient.NotesXid != -1)
            {
                objClient.NotesDescription = GetNotesById(objClient.NotesXid.GetValueOrDefault(-1));
            }
            return objClient;
        }

        internal GeneralMaster.Client EClient(GeneralMaster.Client model)
        {
            GeneralMaster.Client objClient = new GeneralMaster.Client();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objClient.Pid = model.Pid;
            objClient.RoomTypeName = model.RoomTypeName;
            objClient.MaxNoPpl = model.MaxNoPpl;

            objClient.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objClient.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objClient.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objClient.NotesXid = NotesChanges(objNotes.Pid, objClient.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objClient.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objClient.NotesXid = NotesChanges(objNotes.Pid, objClient.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objClient.Pid, objClient.RoomTypeName, objClient.MaxNoPpl, objClient.NotesXid.GetValueOrDefault(-1),
                                                      objClient.LastEditByXid, objClient.CompanyXid, "E");

            return objClient;

        }

        internal GeneralMaster.Client DClient(int pid)
        {
            GeneralMaster.Client objClient = new GeneralMaster.Client();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objClient;
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
            getUserSettings(objUsergrouprights, "DisplayAirline");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("Airline", searchPid).Tables[0];

            objAirline.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objAirline.PagingValues.MaxRows)
                                .Take(objAirline.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objAirline.PagingValues.MaxRows));
            objAirline.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objAirline.PagingValues.CurrentPageIndex = currPage;

            return objAirline;

        }

        internal object SearchAirline(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listAirline = dt.AsEnumerable().Where(x => x.Field<string>("Airline").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.RoomType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("Airline")
                                        }).ToList();

            return listAirline;

        }

        internal GeneralMaster.Airline AAirline()
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            return objAirline;
        }

        internal GeneralMaster.Airline AAirline(GeneralMaster.Airline model)
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objAirline.Pid = objNotes.Pid = -1;
            objAirline.DiscountTypeName = model.DiscountTypeName;
            objAirline.Sequence = model.Sequence;

            objAirline.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objAirline.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objRoomType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objRoomType.NotesXid = NotesChanges(objNotes.Pid, objAirline.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objAirline.Pid, objAirline.DiscountTypeName, objAirline.Sequence, objAirline.NotesXid.GetValueOrDefault(-1),
                                                      objAirline.LastEditByXid, objAirline.Companyxid, "A");

            return objRoomType;

        }

        internal GeneralMaster.Airline EAirline(int pid)
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objAirline.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objAirline.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objAirline.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objAirline.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objAirline.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objAirline.NotesXid != -1)
            {
                objAirline.NotesDescription = GetNotesById(objAirline.NotesXid.GetValueOrDefault(-1));
            }
            return objAirline;
        }

        internal GeneralMaster.Airline EAirline(GeneralMaster.Airline model)
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objAirline.Pid = model.Pid;
            objAirline.RoomTypeName = model.RoomTypeName;
            objAirline.MaxNoPpl = model.MaxNoPpl;

            objAirline.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objAirline.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objAirline.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objAirline.NotesXid = NotesChanges(objNotes.Pid, objAirline.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objAirline.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objAirline.NotesXid = NotesChanges(objNotes.Pid, objAirline.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objAirline.Pid, objAirline.RoomTypeName, objAirline.MaxNoPpl, objAirline.NotesXid.GetValueOrDefault(-1),
                                                      objAirline.LastEditByXid, objAirline.CompanyXid, "E");

            return objAirline;

        }

        internal GeneralMaster.Airline DAirline(int pid)
        {
            GeneralMaster.Airline objAirline = new GeneralMaster.Airline();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objAirline;
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
            getUserSettings(objUsergrouprights, "DisplayResourceType");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ResourceType", searchPid).Tables[0];

            objResourceType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.ResourceType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objResourceType.PagingValues.MaxRows)
                                .Take(objResourceType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objResourceType.PagingValues.MaxRows));
            objResourceType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objResourceType.PagingValues.CurrentPageIndex = currPage;

            return objResourceType;

        }

        internal object SearchResourceType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listResourceType = dt.AsEnumerable().Where(x => x.Field<string>("ResourceType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ResourceType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("ResourceType")
                                        }).ToList();

            return listResourceType;

        }

        internal GeneralMaster.ResourceType AResourceType()
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            return objResourceType;
        }

        internal GeneralMaster.ResourceType AResourceType(GeneralMaster.ResourceType model)
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objResourceType.Pid = objNotes.Pid = -1;
            objResourceType.DiscountTypeName = model.DiscountTypeName;
            objResourceType.Sequence = model.Sequence;

            objResourceType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objResourceType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objResourceType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objResourceType.NotesXid = NotesChanges(objNotes.Pid, objResourceType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objResourceType.Pid, objResourceType.DiscountTypeName, objResourceType.Sequence, objResourceType.NotesXid.GetValueOrDefault(-1),
                                                      objResourceType.LastEditByXid, objResourceType.Companyxid, "A");

            return objResourceType;

        }

        internal GeneralMaster.ResourceType EResourceType(int pid)
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objResourceType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objResourceType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objResourceType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objResourceType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objResourceType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objResourceType.NotesXid != -1)
            {
                objResourceType.NotesDescription = GetNotesById(objResourceType.NotesXid.GetValueOrDefault(-1));
            }
            return objResourceType;
        }

        internal GeneralMaster.ResourceType EResourceType(GeneralMaster.ResourceType model)
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objResourceType.Pid = model.Pid;
            objResourceType.RoomTypeName = model.RoomTypeName;
            objResourceType.MaxNoPpl = model.MaxNoPpl;

            objResourceType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objResourceType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objResourceType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objResourceType.NotesXid = NotesChanges(objNotes.Pid, objResourceType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objResourceType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objResourceType.NotesXid = NotesChanges(objNotes.Pid, objResourceType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objResourceType.Pid, objResourceType.RoomTypeName, objResourceType.MaxNoPpl, objResourceType.NotesXid.GetValueOrDefault(-1),
                                                      objResourceType.LastEditByXid, objResourceType.CompanyXid, "E");

            return objResourceType;

        }

        internal GeneralMaster.ResourceType DResourceType(int pid)
        {
            GeneralMaster.ResourceType objResourceType = new GeneralMaster.ResourceType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objResourceType;
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
            getUserSettings(objUsergrouprights, "DisplayHumanResource");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("HumanResource", searchPid).Tables[0];

            objHumanResource.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.HumanResource
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objHumanResource.PagingValues.MaxRows)
                                .Take(objHumanResource.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objHumanResource.PagingValues.MaxRows));
            objHumanResource.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objHumanResource.PagingValues.CurrentPageIndex = currPage;

            return objHumanResource;

        }

        internal object SearchHumanResource(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listHumanResource = dt.AsEnumerable().Where(x => x.Field<string>("HumanResource").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.HumanResource
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("HumanResource")
                                        }).ToList();

            return listHumanResource;

        }

        internal GeneralMaster.HumanResource AHumanResource()
        {
            GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource();
            return objHumanResource;
        }

        internal GeneralMaster.HumanResource AHumanResource(GeneralMaster.HumanResource model)
        {
            GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objRoomType.Pid = objNotes.Pid = -1;
            objRoomType.DiscountTypeName = model.DiscountTypeName;
            objRoomType.Sequence = model.Sequence;

            objRoomType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objRoomType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objHumanResource.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objHumanResource.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objHumanResource.Pid, objHumanResource.DiscountTypeName, objHumanResource.Sequence, objHumanResource.NotesXid.GetValueOrDefault(-1),
                                                      objHumanResource.LastEditByXid, objHumanResource.Companyxid, "A");

            return objHumanResource;

        }

        internal GeneralMaster.HumanResource EHumanResource(int pid)
        {
            GeneralMaster.HumanResource objRoomType = new GeneralMaster.HumanResource();
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
            GeneralMaster.HumanResource objRoomType = new GeneralMaster.HumanResource();
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
            GeneralMaster.HumanResource objHumanResource = new GeneralMaster.HumanResource();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objHumanResource;
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
            getUserSettings(objUsergrouprights, "DisplayResourceVehicleDtls");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("ResourceVehicleDtls", searchPid).Tables[0];

            objResourceVehicleDtls.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.RoomType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objResourceVehicleDtls.PagingValues.MaxRows)
                                .Take(objResourceVehicleDtls.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objResourceVehicleDtls.PagingValues.MaxRows));
            objResourceVehicleDtls.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objResourceVehicleDtls.PagingValues.CurrentPageIndex = currPage;

            return objResourceVehicleDtls;

        }

        internal object SearchResourceVehicleDtls(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listResourceVehicleDtls = dt.AsEnumerable().Where(x => x.Field<string>("ResourceVehicleDtls").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.ResourceVehicleDtls
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("ResourceVehicleDtls")
                                        }).ToList();

            return listResourceVehicleDtls;

        }

        internal GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls()
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            return objResourceVehicleDtls;
        }

        internal GeneralMaster.ResourceVehicleDtls AResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objResourceVehicleDtls.Pid = objNotes.Pid = -1;
            objResourceVehicleDtls.DiscountTypeName = model.DiscountTypeName;
            objResourceVehicleDtls.Sequence = model.Sequence;

            objResourceVehicleDtls.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objResourceVehicleDtls.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objResourceVehicleDtls.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objResourceVehicleDtls.NotesXid = NotesChanges(objNotes.Pid, objResourceVehicleDtls.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objResourceVehicleDtls.Pid, objResourceVehicleDtls.DiscountTypeName, objResourceVehicleDtls.Sequence, objResourceVehicleDtls.NotesXid.GetValueOrDefault(-1),
                                                      objResourceVehicleDtls.LastEditByXid, objResourceVehicleDtls.Companyxid, "A");

            return objResourceVehicleDtls;

        }

        internal GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(int pid)
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objResourceVehicleDtls.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objResourceVehicleDtls.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objResourceVehicleDtls.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objResourceVehicleDtls.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objResourceVehicleDtls.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objResourceVehicleDtls.NotesXid != -1)
            {
                objResourceVehicleDtls.NotesDescription = GetNotesById(objResourceVehicleDtls.NotesXid.GetValueOrDefault(-1));
            }
            return objResourceVehicleDtls;
        }

        internal GeneralMaster.ResourceVehicleDtls EResourceVehicleDtls(GeneralMaster.ResourceVehicleDtls model)
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objResourceVehicleDtls.Pid = model.Pid;
            objResourceVehicleDtls.RoomTypeName = model.RoomTypeName;
            objResourceVehicleDtls.MaxNoPpl = model.MaxNoPpl;

            objResourceVehicleDtls.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objResourceVehicleDtls.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objResourceVehicleDtls.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objResourceVehicleDtls.NotesXid = NotesChanges(objNotes.Pid, objResourceVehicleDtls.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objResourceVehicleDtls.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objResourceVehicleDtls.NotesXid = NotesChanges(objNotes.Pid, objResourceVehicleDtls.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objResourceVehicleDtls.Pid, objResourceVehicleDtls.RoomTypeName, objResourceVehicleDtls.MaxNoPpl, objResourceVehicleDtls.NotesXid.GetValueOrDefault(-1),
                                                      objResourceVehicleDtls.LastEditByXid, objResourceVehicleDtls.CompanyXid, "E");

            return objResourceVehicleDtls;

        }

        internal GeneralMaster.ResourceVehicleDtls DResourceVehicleDtls(int pid)
        {
            GeneralMaster.ResourceVehicleDtls objResourceVehicleDtls = new GeneralMaster.ResourceVehicleDtls();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objResourceVehicleDtls;
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
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("RoomType", searchPid).Tables[0];

            objLogisticVehicleType.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.LogisticVehicleType
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objLogisticVehicleType.PagingValues.MaxRows)
                                .Take(objLogisticVehicleType.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objLogisticVehicleType.PagingValues.MaxRows));
            objLogisticVehicleType.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objLogisticVehicleType.PagingValues.CurrentPageIndex = currPage;

            return objLogisticVehicleType;

        }

        internal object SearchLogisticVehicleType(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listLogisticVehicleType = dt.AsEnumerable().Where(x => x.Field<string>("LogisticVehicleType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.LogisticVehicleType
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("LogisticVehicleType")
                                        }).ToList();

            return listLogisticVehicleType;

        }

        internal GeneralMaster.LogisticVehicleType ALogisticVehicleType()
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            return objLogisticVehicleType;
        }

        internal GeneralMaster.LogisticVehicleType ALogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticVehicleType.Pid = objNotes.Pid = -1;
            objLogisticVehicleType.DiscountTypeName = model.DiscountTypeName;
            objLogisticVehicleType.Sequence = model.Sequence;

            objLogisticVehicleType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticVehicleType.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objLogisticVehicleType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objLogisticVehicleType.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticVehicleType.Pid, objLogisticVehicleType.DiscountTypeName, objLogisticVehicleType.Sequence, objLogisticVehicleType.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticVehicleType.LastEditByXid, objLogisticVehicleType.Companyxid, "A");

            return objLogisticVehicleType;

        }

        internal GeneralMaster.LogisticVehicleType ELogisticVehicleType(int pid)
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objLogisticVehicleType.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objLogisticVehicleType.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objLogisticVehicleType.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objLogisticVehicleType.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objLogisticVehicleType.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objLogisticVehicleType.NotesXid != -1)
            {
                objLogisticVehicleType.NotesDescription = GetNotesById(objLogisticVehicleType.NotesXid.GetValueOrDefault(-1));
            }
            return objLogisticVehicleType;
        }

        internal GeneralMaster.LogisticVehicleType ELogisticVehicleType(GeneralMaster.LogisticVehicleType model)
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticVehicleType.Pid = model.Pid;
            objLogisticVehicleType.RoomTypeName = model.RoomTypeName;
            objLogisticVehicleType.MaxNoPpl = model.MaxNoPpl;

            objLogisticVehicleType.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticVehicleType.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objLogisticVehicleType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objLogisticVehicleType.NotesXid = NotesChanges(objNotes.Pid, objLogisticVehicleType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objLogisticVehicleType.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objLogisticVehicleType.NotesXid = NotesChanges(objNotes.Pid, objLogisticVehicleType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticVehicleType.Pid, objLogisticVehicleType.RoomTypeName, objLogisticVehicleType.MaxNoPpl, objLogisticVehicleType.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticVehicleType.LastEditByXid, objLogisticVehicleType.CompanyXid, "E");

            return objLogisticVehicleType;

        }

        internal GeneralMaster.LogisticVehicleType DLogisticVehicleType(int pid)
        {
            GeneralMaster.LogisticVehicleType objLogisticVehicleType = new GeneralMaster.LogisticVehicleType();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objLogisticVehicleType;
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
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("LogisticPickupArea", searchPid).Tables[0];

            objLogisticPickupArea.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.LogisticPickupArea
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objLogisticPickupArea.PagingValues.MaxRows)
                                .Take(objLogisticPickupArea.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objLogisticPickupArea.PagingValues.MaxRows));
            objLogisticPickupArea.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objLogisticPickupArea.PagingValues.CurrentPageIndex = currPage;

            return objLogisticPickupArea;

        }

        internal object SearchLogisticPickupArea(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listLogisticPickupArea = dt.AsEnumerable().Where(x => x.Field<string>("LogisticPickupArea").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.LogisticPickupArea
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("LogisticPickupArea")
                                        }).ToList();

            return listLogisticPickupArea;

        }

        internal GeneralMaster.LogisticPickupArea ALogisticPickupArea()
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            return objLogisticPickupArea;
        }

        internal GeneralMaster.LogisticPickupArea ALogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticPickupArea.Pid = objNotes.Pid = -1;
            objLogisticPickupArea.DiscountTypeName = model.DiscountTypeName;
            objLogisticPickupArea.Sequence = model.Sequence;

            objLogisticPickupArea.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticPickupArea.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objLogisticPickupArea.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objLogisticPickupArea.NotesXid = NotesChanges(objNotes.Pid, objRoomType.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticPickupArea.Pid, objLogisticPickupArea.DiscountTypeName, objLogisticPickupArea.Sequence, objLogisticPickupArea.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticPickupArea.LastEditByXid, objLogisticPickupArea.Companyxid, "A");

            return objLogisticPickupArea;

        }

        internal GeneralMaster.LogisticPickupArea ELogisticPickupArea(int pid)
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objLogisticPickupArea.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objLogisticPickupArea.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objLogisticPickupArea.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objLogisticPickupArea.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objLogisticPickupArea.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objLogisticPickupArea.NotesXid != -1)
            {
                objLogisticPickupArea.NotesDescription = GetNotesById(objLogisticPickupArea.NotesXid.GetValueOrDefault(-1));
            }
            return objLogisticPickupArea;

        }

        internal GeneralMaster.LogisticPickupArea ELogisticPickupArea(GeneralMaster.LogisticPickupArea model)
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticPickupArea.Pid = model.Pid;
            objLogisticPickupArea.RoomTypeName = model.RoomTypeName;
            objLogisticPickupArea.MaxNoPpl = model.MaxNoPpl;

            objLogisticPickupArea.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticPickupArea.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objLogisticPickupArea.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objLogisticPickupArea.NotesXid = NotesChanges(objNotes.Pid, objLogisticPickupArea.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objLogisticPickupArea.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objLogisticPickupArea.NotesXid = NotesChanges(objNotes.Pid, objLogisticPickupArea.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticPickupArea.Pid, objLogisticPickupArea.RoomTypeName, objLogisticPickupArea.MaxNoPpl, objLogisticPickupArea.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticPickupArea.LastEditByXid, objLogisticPickupArea.CompanyXid, "E");

            return objLogisticPickupArea;

        }

        internal GeneralMaster.LogisticPickupArea DLogisticPickupArea(int pid)
        {
            GeneralMaster.LogisticPickupArea objLogisticPickupArea = new GeneralMaster.LogisticPickupArea();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objLogisticPickupArea;
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
            getUserSettings(objUsergrouprights, "DisplayLogisticJourneyTimes");

            GeneralMaster.Paging objPaging = new GeneralMaster.Paging();
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes(objPaging, objUsergrouprights);
            DataTable dt = objBaseDataLayer.getDALGeneralMaster1("LogisticJourneyTimes", searchPid).Tables[0];

            objLogisticJourneyTimes.listRoomType = (from DataRow dr in dt.Rows
                                        select new GeneralMaster.LogisticJourneyTimes
                                        {
                                            Pid = Convert.ToInt32(dr["Pid"]),
                                            RoomTypeName = dr["RoomType"].ToString(),
                                            MaxNoPpl = Convert.ToInt32(dr["MaxNoPpl"]),
                                            LastEdit = Convert.ToDateTime(dr["lastEdit"])
                                        }
                               ).Skip((currPage - 1) * objLogisticJourneyTimes.PagingValues.MaxRows)
                                .Take(objLogisticJourneyTimes.PagingValues.MaxRows).ToList();

            double pageCount = (double)((decimal)dt.Rows.Count / Convert.ToDecimal(objLogisticJourneyTimes.PagingValues.MaxRows));
            objLogisticJourneyTimes.PagingValues.PageCount = (int)Math.Ceiling(pageCount);

            objLogisticJourneyTimes.PagingValues.CurrentPageIndex = currPage;

            return objLogisticJourneyTimes;

        }

        internal object SearchLogisticJourneyTimes(string prefix)
        {
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(-1, "P", prefix);
            var listLogisticJourneyTimes = dt.AsEnumerable().Where(x => x.Field<string>("RoomType").ToLower().Contains(prefix.ToLower()))
                                        .Select(x => new GeneralMaster.LogisticJourneyTimes
                                        {
                                            Pid = x.Field<int>("Pid"),
                                            RoomTypeName = x.Field<string>("LogisticJourneyTimes")
                                        }).ToList();

            return listLogisticJourneyTimes;

        }

        internal GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes()
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            return objLogisticJourneyTimes;
        }

        internal GeneralMaster.LogisticJourneyTimes ALogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticJourneyTimes.Pid = objNotes.Pid = -1;
            objLogisticJourneyTimes.DiscountTypeName = model.DiscountTypeName;
            objLogisticJourneyTimes.Sequence = model.Sequence;

            objLogisticJourneyTimes.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticJourneyTimes.Companyxid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesDescription != null)
            {
                objLogisticJourneyTimes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                objLogisticJourneyTimes.NotesXid = NotesChanges(objNotes.Pid, objLogisticJourneyTimes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticJourneyTimes.Pid, objLogisticJourneyTimes.DiscountTypeName, objLogisticJourneyTimes.Sequence, objLogisticJourneyTimes.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticJourneyTimes.LastEditByXid, objLogisticJourneyTimes.Companyxid, "A");

            return objLogisticJourneyTimes;

        }

        internal GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(int pid)
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            DataTable dt = objBaseDataLayer.getDALGetRoomTypeById(pid, "E", "");


            objLogisticJourneyTimes.Pid = Convert.ToInt32(dt.Rows[0]["Pid"]);
            objLogisticJourneyTimes.RoomTypeName = dt.Rows[0]["RoomType"].ToString();
            objLogisticJourneyTimes.MaxNoPpl = Convert.ToInt32(dt.Rows[0]["MaxNoPpl"]);
            objLogisticJourneyTimes.LastEdit = Convert.ToDateTime(dt.Rows[0]["lastEdit"]);
            objLogisticJourneyTimes.NotesXid = Convert.ToInt32(dt.Rows[0]["NotesXid"]);

            if (objLogisticJourneyTimes.NotesXid != -1)
            {
                objLogisticJourneyTimes.NotesDescription = GetNotesById(objLogisticJourneyTimes.NotesXid.GetValueOrDefault(-1));
            }
            return objLogisticJourneyTimes;

        }

        internal GeneralMaster.LogisticJourneyTimes ELogisticJourneyTimes(GeneralMaster.LogisticJourneyTimes model)
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            GeneralMaster.Notes objNotes = new GeneralMaster.Notes();

            objLogisticJourneyTimes.Pid = model.Pid;
            objLogisticJourneyTimes.RoomTypeName = model.RoomTypeName;
            objLogisticJourneyTimes.MaxNoPpl = model.MaxNoPpl;

            objLogisticJourneyTimes.LastEditByXid = objNotes.LastEditByXid = Convert.ToInt32(HttpContext.Current.Session["UserId"]);
            objLogisticJourneyTimes.CompanyXid = objNotes.CompanyXid = Convert.ToInt32(HttpContext.Current.Session["CompanyId"]);

            if (model.NotesXid != -1)
            {
                if (model.NotesDescription != null)
                {
                    objLogisticJourneyTimes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objNotes.Pid = model.NotesXid.GetValueOrDefault(-1);
                    objLogisticJourneyTimes.NotesXid = NotesChanges(objNotes.Pid, objLogisticJourneyTimes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "E");
                }
            }
            else
            {
                if (model.NotesDescription != null)
                {
                    objLogisticJourneyTimes.NotesDescription = objNotes.NotesName = model.NotesDescription;
                    objLogisticJourneyTimes.NotesXid = NotesChanges(objNotes.Pid, objLogisticJourneyTimes.NotesDescription, objNotes.LastEditByXid, objNotes.CompanyXid, "A");
                }
            }

            objBaseDataLayer.getDALInsertModifyRoomType(objLogisticJourneyTimes.Pid, objLogisticJourneyTimes.RoomTypeName, objLogisticJourneyTimes.MaxNoPpl, objLogisticJourneyTimes.NotesXid.GetValueOrDefault(-1),
                                                      objLogisticJourneyTimes.LastEditByXid, objLogisticJourneyTimes.CompanyXid, "E");

            return objLogisticJourneyTimes;

        }

        internal GeneralMaster.LogisticJourneyTimes DLogisticJourneyTimes(int pid)
        {
            GeneralMaster.LogisticJourneyTimes objLogisticJourneyTimes = new GeneralMaster.LogisticJourneyTimes();
            objBaseDataLayer.getDALGetRoomTypeById(id, "D", "");
            //string s =DeleteNotesById(notesxid);
            return objLogisticJourneyTimes;
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