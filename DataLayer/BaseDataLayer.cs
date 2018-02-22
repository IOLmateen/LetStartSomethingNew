using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace DataLayer
{
   public class BaseDataLayer
    {
         string connectionString = "Server=10.10.1.19;Database=travcostaging2016;uid=illusions;password=illusions";
        public DataSet  getDALLogin(string login,string password)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsRsLogin = new DataSet();
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                try
                {
                    sCon.Open();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText =// "Select pid from M_Passport where pid=733";
                    "Select M_PassPort.Pid,M_PassPort.EntityXid,M_PassPort.Usergroupxid,"+
                    " M_PassPort.CompanyXid,M_PassPort.EntityType" +
                    " From M_PassPort " +
                    " INNER JOIN M_Employee ON M_Passport.EntityXid = M_Employee.Pid " +
                    " Where Login ='"+login + "'"+
                    " And dbo.UF_Decrypt(password) ='" + password + "'" +
                    " And(EntityType = 'E' Or EntityType = 'C' Or EntityType = 'U' Or EntityType = 'H' Or EntityType = 'S')" +
                    " And M_Employee.Status = 'A' " +
                    " Order By M_PassPort.Pid  ";

                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsRsLogin);
                    return dsRsLogin;
                }
            }
        }
        public DataSet getDALEmployee(int emppid, int companyxid)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsRsEmployee = new DataSet();
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText =// "Select pid from M_Passport where pid=733";
                   " Select M_Employee.DepartmentXid,M_Employee.DesignationXid,M_Employee.Email, "+
                   "  M_Employee.FirstName + M_Employee.LastName as EmployeeName,M_Employee.CompanyXid, " +
                   "  M_Department.Department from M_Employee, M_Department " +
                   "  where M_Employee.DepartmentXid = M_Department.Pid " +
                   "  and M_Employee.Pid = " + emppid +
                   "  and m_employee.companyxid = " + companyxid ;
                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsRsEmployee);
                    return dsRsEmployee;
                }
            }
        }
        public DataSet getDALCompany(int companyxid)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsRsCompany = new DataSet();
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {

                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText =// "Select pid from M_Passport where pid=733";
                    " select M_Company.Company as Company,M_Company.CountryXid,M_Company.ODCompanyXid,M_Country.DateFormat from M_Company" +
                    " Inner Join M_Country on M_Company.CountryXid = M_Country.Pid Where M_Company.pid="+companyxid;    
                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsRsCompany);
                    return dsRsCompany;
                }
            }
        }


        public DataSet getDALUserPageSettings(string pagename)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsUserSettings = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {

                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText = "Select pageName,LinkName,RightHeader from UserGroupRights1_Mateen where LinkName != 'View' and pagename ='" +pagename + "'" ;

                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsUserSettings);
                    return dsUserSettings;
                }
            }
        }


        public DataSet getDALUserSettings()
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsUserSettings = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {

                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText = "Select pageName,LinkName,RightHeader from UserGroupRights1_Mateen where LinkName='View'";

                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsUserSettings);
                    return dsUserSettings;
                }
            }
        }

        public int getDALUserGroupRights(string pname,string lname,string rightheader,string usergroupid,string companyid)
        {
            int count;
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText = "select count(*) as count from  UserGroupRights1_Mateen where PageName='" + pname+"' and linkname= '"+lname+ "' and rightheader= '" + rightheader + "' and AllowYN='Y' and UserGroupXid='" + usergroupid+"' AND CompanyXid =" +companyid ;
                    count = Convert.ToInt32(sCmd.ExecuteScalar());
                    return count;
                }
            }
        }


        public DataSet getDALGeneralMaster(string KeyName)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsgetGeneralMaster = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_GetMaster_Mateen";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@lv_KeyName";
                    param.Value = KeyName;
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 20;
                    sCmd.Parameters.Add(param);


                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsgetGeneralMaster);
                    return dsgetGeneralMaster;
                }
            }
        }



        public DataSet getDALGeneralMaster1(string KeyName,int SearchPid)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsgetGeneralMaster = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_GetMaster_Mateen";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@lv_KeyName";
                    param.Value = KeyName;
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 20;
                    sCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@li_SearchPid";
                    param1.Value = SearchPid;
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Size = 20;
                    sCmd.Parameters.Add(param1);

                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsgetGeneralMaster);
                    return dsgetGeneralMaster;
                }
            }
        }


        public string getDALGetNotesById(int NotesXid,string action)
        {
            DataTable dtgetDiscountTypeById = new DataTable();
            string NotesDesc = null;
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_GetNotesById_Mateen";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@li_Pid";
                    param.Value = NotesXid;
                    param.SqlDbType = SqlDbType.Int;
                    param.Size = 100;
                    sCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@lv_action";
                    param1.Value = action;
                    param1.SqlDbType = SqlDbType.VarChar;
                    param1.Size = 1;
                    sCmd.Parameters.Add(param1);


                    NotesDesc = Convert.ToString(sCmd.ExecuteScalar());

                    return NotesDesc;
                }
            }
        }

        public DataTable getDALGetDiscountTypeById(int id,string action)
        {
            SqlDataReader reader = null;
            DataTable dtgetDiscountTypeById=new DataTable();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_GetDiscountTypeById_Mateen";

                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@li_Pid";
                    param.Value = id;
                    param.SqlDbType = SqlDbType.Int;
                    param.Size = 100;
                    sCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@lv_action";
                    param1.Value = action;
                    param1.SqlDbType = SqlDbType.VarChar;
                    param1.Size = 1;
                    sCmd.Parameters.Add(param1);



                    reader = sCmd.ExecuteReader();
                    dtgetDiscountTypeById.Load(reader);

                    return dtgetDiscountTypeById;
                }
            }
        }


                public void getDALInsertModifyDiscountType(int pid,string DiscountTypeName, int Sequence, int NotesXid,
                                                      int LastEditByXid, int Companyxid,string Action)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_InsertModifyDiscountType_Mateen";

                    SqlParameter para = new SqlParameter();
                    para.ParameterName = "@li_Pid";
                    para.Value = pid;
                    para.SqlDbType = SqlDbType.Int;
                    para.Size = 100;
                    sCmd.Parameters.Add(para);


                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@lv_DiscountTypeName";
                    param.Value = DiscountTypeName;
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 100;
                    sCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@li_Sequence";
                    param1.Value = Sequence;
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Size = 4;
                    sCmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@li_NotesXid";
                    param2.Value = NotesXid;
                    param2.SqlDbType = SqlDbType.Int;
                    param2.Size = 10;
                    sCmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@li_LastEditByXid";
                    param3.Value = LastEditByXid;
                    param3.SqlDbType = SqlDbType.Int;
                    param3.Size = 4;
                    sCmd.Parameters.Add(param3);

                    SqlParameter param4 = new SqlParameter();
                    param4.ParameterName = "@li_Companyxid";
                    param4.Value = Companyxid;
                    param4.SqlDbType = SqlDbType.Int;
                    param4.Size = 20;
                    sCmd.Parameters.Add(param4);

                    SqlParameter param5 = new SqlParameter();
                    param5.ParameterName = "@lc_Action";
                    param5.Value = Action;
                    param5.SqlDbType = SqlDbType.VarChar;
                    param5.Size = 20;
                    sCmd.Parameters.Add(param5);


                    sCmd.ExecuteNonQuery();

                }
            }
        }

        public int getDALNotesChanges(int pid,string notes,int lastedit, int companyxid,string action)
        {
            int pidvalue = 0;
            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.StoredProcedure;

                    sCmd.CommandText = "Usp_NotesChanges_Mateen";

                    SqlParameter para = new SqlParameter();
                    para.ParameterName = "@li_pid";
                    para.Value = pid;
                    para.SqlDbType = SqlDbType.Int;
                    para.Size = 100;
                    sCmd.Parameters.Add(para);


                    SqlParameter param = new SqlParameter();
                    param.ParameterName = "@lv_notes";
                    param.Value =notes;
                    param.SqlDbType = SqlDbType.VarChar;
                    param.Size = 16;
                    sCmd.Parameters.Add(param);

                    SqlParameter param1 = new SqlParameter();
                    param1.ParameterName = "@li_lastedit";
                    param1.Value =lastedit;
                    param1.SqlDbType = SqlDbType.Int;
                    param1.Size = 10;
                    sCmd.Parameters.Add(param1);

                    SqlParameter param2 = new SqlParameter();
                    param2.ParameterName = "@li_companyxid";
                    param2.Value =companyxid;
                    param2.SqlDbType = SqlDbType.Int;
                    param2.Size = 10;
                    sCmd.Parameters.Add(param2);

                    SqlParameter param3 = new SqlParameter();
                    param3.ParameterName = "@lc_action";
                    param3.Value = action;
                    param3.SqlDbType = SqlDbType.VarChar;
                    param3.Size = 1;
                    sCmd.Parameters.Add(param3);

                       pidvalue = Convert.ToInt32(sCmd.ExecuteScalar());

                 //  var pidvalue1 = sCmd.ExecuteScalar();
                }
            }
                return pidvalue;
        }

        //public DataSet getDALDiscountType()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetDiscountType = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //              //  if(id == null &&  prefix == null )
        //              //  { 
        //                sCmd.CommandText = "select top 10 Pid, DiscountType,Sequence,LastEdit  from m_discounttype";
        //                //  }
        //                //else if (id != null)
        //                //{
        //                //    sCmd.CommandText = "select Pid, DiscountType,Sequence,LastEdit from m_discounttype where pid=" + id;
        //                //}
        //                //else if (prefix != null)
        //                //{
        //                //    sCmd.CommandText = "select Pid, DiscountType from m_discounttype where DiscountType like '%" + prefix + "%'";
        //                //}

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetDiscountType);
        //                return dsgetDiscountType;
        //            }
        //        }
        //    }


        //    public DataSet getDALRoomType()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetRoomType = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 Pid, roomtype,isnull(MaxNoPpl,0) as MaxNoPpl,LastEdit  from m_roomtype";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetRoomType);
        //                return dsgetRoomType;
        //            }
        //        }
        //    }


        //    public DataSet getDALActivity()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetActivity = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "Select top 10 pid,code,activity,lastedit from m_activity";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetActivity);
        //                return dsgetActivity;
        //            }
        //        }
        //    }


        //    public DataSet getDALAddressType()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetAddressType = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "Select top 10  pid,addresstype,lastedit from m_addresstype";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetAddressType);
        //                return dsgetAddressType;
        //            }
        //        }
        //    }


        //        public DataSet getDALBank()
        //        {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetBank = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "Select top 10  pid,code,bank,BankBranch,GuichetCode,lastedit from m_bank";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetBank);
        //                return dsgetBank;
        //            }
        //        }
        //    }

        //    public DataSet getDALBookingNote()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetBookingNote = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "Select top 10  pid,code,NoteFor,Note from m_BookingNote";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetBookingNote);
        //                return dsgetBookingNote;
        //            }
        //        }
        //    }

        //    public DataSet getDALCardType()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetBookingNote = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "Select top 10  pid,CardType,Length,CCChargesYN,CCCharges,CCChargeApplyTo,LastEdit from m_cardtype";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetBookingNote);
        //                return dsgetBookingNote;
        //            }
        //        }
        //    }

        //    public DataSet getDALCurrency()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsgetCurrency = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10  pid,Code,Currency,CurrencySymbol,defaultcurrency,nominalcode,lastedit from m_currency";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsgetCurrency);
        //                return dsgetCurrency;
        //            }
        //        }
        //    }


        //    public DataSet getDALTradeFairsTypes()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsTradeFairsTypes = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,TradeFairsType,lastedit from M_TradeFairsTypes";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsTradeFairsTypes);
        //                return dsTradeFairsTypes;
        //            }
        //        }
        //    }


        //    public DataSet getDALFacility()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsFacility = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,facility,belongsto,lastedit from M_Facility";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsFacility);
        //                return dsFacility;
        //            }
        //        }
        //    }

        //    public DataSet getDALFinancialYear()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsFinancialYear = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,financialyear,fromdate,todate,lastedit from M_financialyear";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsFinancialYear);
        //                return dsFinancialYear;
        //            }
        //        }
        //    }


        //    public DataSet getDALHolidayDuration()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsHolidayDuration = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,holidayduration,lastedit from M_holidayduration";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsHolidayDuration);
        //                return dsHolidayDuration;
        //            }
        //        }
        //    }

        //    public DataSet getDALHolidayType()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsHolidayType = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,holidaytype from M_holidaytype";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsHolidayType);
        //                return dsHolidayType;
        //            }
        //        }
        //    }

        //    public DataSet getDALHotelStandard()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsHotelStandard = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,standard from M_standard";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsHotelStandard);
        //                return dsHotelStandard;
        //            }
        //        }
        //    }
        //    public DataSet getDALHotelChain()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsHotelChain = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,hotelchain from M_hotelchain";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsHotelChain);
        //                return dsHotelChain ;
        //            }
        //        }
        //    }

        //    public DataSet getDALInspectionCriteria()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsInspectionCriteria = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,inspectioncriteria,lastedit from M_InspectionCriteria";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsInspectionCriteria);
        //                return dsInspectionCriteria;
        //            }
        //        }
        //    }
        //    public DataSet getDALLanguage()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsLanguage = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,code,languagename,lastedit from M_language";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsLanguage);
        //                return dsLanguage;
        //            }
        //        }
        //    }




        //    public DataSet getDALMarket()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsMarket = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,market,lastedit from M_market";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsMarket);
        //                return dsMarket;
        //            }
        //        }
        //    }


        //    public DataSet getDALMealPlan()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsMealPlan = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,mealplan,lastedit from M_mealplan";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsMealPlan);
        //                return dsMealPlan;
        //            }
        //        }
        //    }


        //    public DataSet getDALNationality()
        //    {
        //        SqlDataAdapter sDa = new SqlDataAdapter();
        //        DataSet dsNationality = new DataSet();

        //        using (SqlConnection sCon = new SqlConnection(connectionString))
        //        {
        //            sCon.Open();
        //            using (SqlCommand sCmd = new SqlCommand())
        //            {
        //                sCmd.Connection = sCon;
        //                sCmd.CommandType = CommandType.Text;
        //                sCmd.CommandText = "select top 10 pid,nationality,lastedit from M_nationality";

        //                sDa.SelectCommand = sCmd;
        //                sDa.Fill(dsNationality);
        //                return dsNationality;
        //            }
        //        }
        //    }

        //    //public DataSet getDALPaymentSchedules()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsPaymentSchedules = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "select top 10 pid,paymentschedules,lastedit from M_PaymentSchedules";

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsPaymentSchedules);
        //    //            return dsPaymentSchedules;
        //    //        }
        //    //    }
        //    //}
        //    //public DataSet getDALPaymentTypes()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsPaymentTypes = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "select top 10 pid,PaymentType,nominalcode,lastedit from M_paymentTypes";

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsPaymentTypes);
        //    //            return dsPaymentTypes;
        //    //        }
        //    //    }
        //    //}
        //    //public DataSet getDALLogisticPickupType()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsLogisticPickupType = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "select top 10 pid,pickuptype,showbookingengine,arrivalpoint,lastedit from M_LogisticPickupType";

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsLogisticPickupType);
        //    //            return dsLogisticPickupType;
        //    //        }
        //    //    }
        //    //}


        //    //public DataSet getDALClientChain()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsgetClientChain = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "Select top 10  pid,code,clientchain,LastEdit from M_ClientChain";

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsgetClientChain);
        //    //            return dsgetClientChain;
        //    //        }
        //    //    }
        //    //}

        //    //public DataSet getDALAirport()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsgetAirport = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "Select m_airport.pid,m_airport.code,m_airport.airport,m_country.Country,m_city.City,m_airport.lastedit from m_airport "+
        //    //                               "inner join m_country on m_airport.countryxid = m_country.pid "+
        //    //                               "inner join m_city on m_airport.cityxid = m_city.pid"; 

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsgetAirport);
        //    //            return dsgetAirport;
        //    //        }
        //    //    }
        //    //}


        //    //public DataSet getDALHumanResource()
        //    //{
        //    //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    //    DataSet dsgetHumanResource = new DataSet();

        //    //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    //    {
        //    //        sCon.Open();
        //    //        using (SqlCommand sCmd = new SqlCommand())
        //    //        {
        //    //            sCmd.Connection = sCon;
        //    //            sCmd.CommandType = CommandType.Text;
        //    //            sCmd.CommandText = "select top 10 m_HumanResource.pid,m_HumanResource.id,m_HumanResource.Firstname,m_HumanResource.LastName, " +
        //    //                               "   m_HumanResource.category,m_HumanResource.doj,m_HumanResource.Mobileno,m_HumanResource.Email, " +
        //    //                               "   M_ResourceType.resourcetype,M_Designation.Designation" +
        //    //                               "   from m_HumanResource" +
        //    //                               "   inner join M_ResourceType on m_HumanResource.ResourceTypeXid = M_ResourceType.pid " +
        //    //                               "   inner join M_Designation on m_HumanResource.DesignationXid = m_Designation.pid ";

        //    //            sDa.SelectCommand = sCmd;
        //    //            sDa.Fill(dsgetHumanResource);
        //    //            return dsgetHumanResource;
        //    //        }
        //    //    }
        //    //}



        public DataSet getDALDiscountTypeById(int id)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsgetDiscountType = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText = "select Pid, DiscountType,Sequence,LastEdit from m_discounttype where pid=" + id;
                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsgetDiscountType);
                    return dsgetDiscountType;
                }
            }
        }

        public DataSet getDALDiscountTypeByPrefix(string prefix)
        {
            SqlDataAdapter sDa = new SqlDataAdapter();
            DataSet dsgetDiscountType = new DataSet();

            using (SqlConnection sCon = new SqlConnection(connectionString))
            {
                sCon.Open();
                using (SqlCommand sCmd = new SqlCommand())
                {
                    sCmd.Connection = sCon;
                    sCmd.CommandType = CommandType.Text;
                    sCmd.CommandText = "select Pid, DiscountType from m_discounttype where DiscountType like  '%" + prefix + "%'";
                    sDa.SelectCommand = sCmd;
                    sDa.Fill(dsgetDiscountType);
                    return dsgetDiscountType;
                }
            }
        }

        //public DataSet getDALRoomType()
        //{
        //    SqlDataAdapter sDa = new SqlDataAdapter();
        //    DataSet dsgetDiscountType = new DataSet();

        //    using (SqlConnection sCon = new SqlConnection(connectionString))
        //    {
        //        sCon.Open();
        //        using (SqlCommand sCmd = new SqlCommand())
        //        {
        //            sCmd.Connection = sCon;
        //            sCmd.CommandType = CommandType.Text;
        //            sCmd.CommandText = "select Pid, RoomType  from m_roomtype";
        //            sDa.SelectCommand = sCmd;
        //            sDa.Fill(dsgetDiscountType);
        //            return dsgetDiscountType;
        //        }
        //    }
        //}
    }
}

