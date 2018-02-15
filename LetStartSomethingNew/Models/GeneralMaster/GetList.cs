using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer;
using System.Data.SqlClient;
using System.Data;

namespace LetStartSomethingNew.Models.GeneralMaster
{
    public static class GetList
    {
        //public void ViewDiscountType()
        //{
        //    DataTable dt = objBaseDataLayer.getDALDiscountType().Tables[0];
        //    ListDiscountType = (from DataRow dr in dt.Rows
        //                        select new DiscountType
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            DiscountTypeName = dr["DiscountType"].ToString(),
        //                            Sequence = Convert.ToInt32(dr["Sequence"]),
        //                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                        }
        //                        ).ToList();
        //}


        //public static void getRooms(RoomType r,BaseDataLayer objBaseDataLayer)
        //{
        //    DataTable dt = objBaseDataLayer.getDALRoomType().Tables[0];
        //     r.listRoomType = (from DataRow dr in dt.Rows
        //                        select new RoomType
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            RoomTypeName = dr["RoomType"].ToString(),
        //                        }
        //                        ).ToList();

        // //   return r.listRoomType;
        //}


        //public static List<DiscountType> getDiscount(DiscountType d, BaseDataLayer objBaseDataLayer)
        //{
        //    DataTable dt = objBaseDataLayer.getDALDiscountType().Tables[0];
        //    d.listDiscountType = (from DataRow dr in dt.Rows
        //                        select new DiscountType
        //                        {
        //                            Pid = Convert.ToInt32(dr["Pid"]),
        //                            DiscountTypeName = dr["DiscountType"].ToString(),
        //                            Sequence = Convert.ToInt32(dr["Sequence"]),
        //                            LastEdit = Convert.ToDateTime(dr["LastEdit"])
        //                        }
        //                        ).ToList();
        //    return d.listDiscountType;
        //}

        //public static void GetByID(int id, DiscountType objDiscountType, BaseDataLayer objBaseDataLayer)
        //{
        //    DataTable dt = objBaseDataLayer.getDALDiscountTypeById(id).Tables[0];

        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        objDiscountType.Pid = Convert.ToInt32(dr["pid"]);
        //        objDiscountType.DiscountTypeName = dr["DiscountType"].ToString();
        //    }
        //}
    }
}