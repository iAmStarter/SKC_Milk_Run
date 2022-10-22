using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using SKC_Milk_Run.App_Data;

namespace SKC_Milk_Run
{
    public class GeneralClass
    {
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
                
        public string CheckSQL(string text)
        {
            if (text.Contains("'") || text.Contains("*") || text.Contains("--") || text.Contains("\"") || text.Contains("=") || text.Contains("(") || text.Contains(")") || text.Contains(";"))
            {
                text = "";
            }
            return text;

        }
        public static string Check_Menu(string menu, string usr)
        {
            string return_ = "#", sql = "";

            Data_User_Access USRA = new Data_User_Access();

            if (usr == "D")
            {
                sql = " select distinct ObjectId, '1'  as selected,isnull(Link,'#') as Link from [dbo].[Data_Object] obj where ObjectId =" + menu + " and ObjectId in (18,24) ";
            }
            else
            {
                sql = " select distinct ObjectId, (case when exists(select ObjectId from [dbo].[Config_Access_Object] conf inner join [dbo].[Data_User] usr on usr.AccessLevel = conf.AccessId ";
                sql += "  where usr.UserId = '" + usr + @"' and conf.ObjectId = obj.ObjectId) then '1' else '0' end) as selected,isnull(Link,'#') as Link ";
                sql += "  from [dbo].[Data_Object] obj where ObjectId =" + menu;
            }

            DataTable GetObject = USRA.SQLSelect(sql);
            foreach (DataRow row in GetObject.Rows)
            {
                if (row["Selected"].ToString() == "1")
                {
                    return_ = row["Link"].ToString();
                }
                else
                {
                    return_ = "#";
                }
            }

            return return_;
        }
        public static void SetDDLs(DropDownList d, string val)
        {
            ListItem li;
            for (int i = 0; i < d.Items.Count; i++)
            {
                li = d.Items[i];
                if (li.Value == val)
                {
                    d.SelectedIndex = i;
                    break;
                }
                else
                {
                    d.SelectedIndex = 0;
                }
            }
        }
        public static string Supplier_code(string UserLevel_, string SupplierCode)
        {
            string UserLevel = "", supplier_ = "";
            if (UserLevel_ != "")
                UserLevel = UserLevel_;

            if (SupplierCode != "")
                supplier_ = SupplierCode;

            if (UserLevel == "3")
            {
                supplier_ = "";
            }

            return supplier_;
        }
        public static bool checkfilexlsx(FileUpload FileUpload_)
        {
            string NameString = Path.GetExtension(FileUpload_.FileName);

            if (NameString == ".xlsx")
            {
                return true;
            }
            else
            {
                return false;
            }
        }             

        public static void Supplier_list(DropDownList ddl, string UserLevel_, string SupplierCode, string mode_, string user_name, string PDS_Status = "")
        {
            using (SqlConnection con = new SqlConnection(Global.db_Con))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        string SQLCommand = "", supplier_ = "", where_S_B = "";

                        string UserLevel = "";
                        if (UserLevel_.Trim() != "")
                            UserLevel = UserLevel_.Trim();

                        if (SupplierCode.Trim() != "")
                            supplier_ = SupplierCode.Trim();

                        if (supplier_ == "SKC")
                        {
                            if (user_name.Trim() != "")
                            {
                                supplier_ = "";
                                where_S_B = " AND  Supplier_Code IN ( SELECT Supplier_Code FROM Tbl_Supplier_S_Base WHERE S_Base like '%" + user_name.Trim() + "%' ) ";
                            }
                        }

                        if (UserLevel == "D")
                        {
                            supplier_ = "";
                            where_S_B = "";
                        }

                        if (UserLevel.Trim() == "3")
                        {
                            supplier_ = "";
                            where_S_B = "";
                        }

                        SQLCommand = "SELECT Supplier_Code as Supplier_No ,[Supplier_Name] +'('+[Supplier_Code]+')' as Supplier_Name FROM [Tbl_Supplier] WHERE Status Not in ('" + PDS_Status + "') and Supplier_Code LIKE '%" + supplier_.Trim() + "%' " + where_S_B;


                        cmd.CommandTimeout = 0;
                        cmd.CommandText = SQLCommand;
                        con.Open();

                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;

                        DataTable dtSupplier = new DataTable();
                        sda.Fill(dtSupplier);

                        ddl.Items.Clear();
                        ddl.AppendDataBoundItems = true;

                        int index = 0;

                        if (UserLevel.Trim() == "3")
                        {
                            if (mode_ != "S")
                            {
                                ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                index = 1;
                            }
                        }
                        else
                        {
                            if (UserLevel.Trim() == "D")
                            {
                                if (mode_ != "S")
                                {
                                    ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                    index = 1;
                                }
                            }
                            else
                            {
                                if (SupplierCode.Trim() == "SKC")
                                {
                                    if (mode_ != "S")
                                    {
                                        ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                        index = 1;
                                    }
                                }
                            }
                        }

                        foreach (DataRow rdt in dtSupplier.Rows)
                        {
                            ddl.Items.Insert(index, new ListItem(rdt["Supplier_Name"].ToString(), rdt["Supplier_No"].ToString()));
                            index++;
                        }

                        if (mode_ == "S")
                        {
                            if (SupplierCode.Trim() != "")
                            {
                                SetDDLs(ddl, SupplierCode.Trim());
                            }
                        }
                        else
                        {
                            if (supplier_.Trim() != "")
                            {
                                SetDDLs(ddl, supplier_.Trim());
                            }
                        }

                        if (UserLevel.Trim() == "3")
                        {
                            ddl.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = true; //false;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

        
        public static void Supplier_list_ShortName(DropDownList ddl, string UserLevel_, string SupplierCode, string mode_, string user_name, string PDS_Status = "", string user_id = "")
        {
            using (SqlConnection con = new SqlConnection(Global.db_Con))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    try
                    {
                        string SQLCommand = "", supplier_ = "", where_S_B = "";

                        string UserLevel = "";
                        if (UserLevel_.Trim() != "")
                            UserLevel = UserLevel_.Trim();

                        if (SupplierCode.Trim() != "")
                            supplier_ = SupplierCode.Trim();

                        if (supplier_ == "SKC")
                        {
                            if (user_name.Trim() != "")
                            {
                                supplier_ = "";
                                where_S_B = " AND  Supplier_Code IN ( SELECT Supplier_Code FROM Tbl_Supplier_S_Base WHERE S_Base like '%" + user_name.Trim() + "%' ) ";
                            }
                        }

                        if (UserLevel == "D")
                        {
                            supplier_ = "";
                            where_S_B = " AND Supplier_Code IN (SELECT [Supplier_Code] FROM [dbo].[Tbl_Vehicle_Supplier] where [ID] =" + user_id + ")";
                        }

                        if (UserLevel.Trim() == "3")
                        {
                            supplier_ = "";
                            where_S_B = "";
                        }

                        SQLCommand = "SELECT Supplier_Code as Supplier_No ,[Supplier_Name] +'('+[Supplier_Code]+')' as Supplier_Name ,Short_Name FROM [Tbl_Supplier] WHERE Status Not in ('" + PDS_Status + "') and Supplier_Code LIKE '%" + supplier_.Trim() + "%' " + where_S_B;


                        cmd.CommandTimeout = 0;
                        cmd.CommandText = SQLCommand;
                        con.Open();

                        SqlDataAdapter sda = new SqlDataAdapter();
                        sda.SelectCommand = cmd;

                        DataTable dtSupplier = new DataTable();
                        sda.Fill(dtSupplier);

                        ddl.Items.Clear();
                        ddl.AppendDataBoundItems = true;

                        int index = 0;

                        if (UserLevel.Trim() == "3")
                        {
                            if (mode_ != "S")
                            {
                                ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                index = 1;
                            }
                        }
                        else
                        {
                            if (UserLevel.Trim() == "D")
                            {
                                if (mode_ != "S")
                                {
                                    ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                    index = 1;
                                }
                            }
                            else
                            {
                                if (SupplierCode.Trim() == "SKC")
                                {
                                    if (mode_ != "S")
                                    {
                                        ddl.Items.Insert(0, new ListItem("Please Select ", ""));
                                        index = 1;
                                    }
                                }
                            }
                        }

                        foreach (DataRow rdt in dtSupplier.Rows)
                        {
                            ddl.Items.Insert(index, new ListItem(rdt["Short_Name"].ToString(), rdt["Supplier_No"].ToString()));
                            index++;
                        }

                        if (mode_ == "S")
                        {
                            if (SupplierCode.Trim() != "")
                            {
                                SetDDLs(ddl, SupplierCode.Trim());
                            }
                        }
                        else
                        {
                            if (supplier_.Trim() != "")
                            {
                                SetDDLs(ddl, supplier_.Trim());
                            }
                        }

                        if (UserLevel.Trim() == "3")
                        {
                            ddl.Enabled = true;
                        }
                        else
                        {
                            ddl.Enabled = true; //false;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }
        public static string S_Base_SQL(string UserLevel_, string SupplierCode, string user_name, string sql_)
        {
            string supplier_ = "", where_ = "";

            string UserLevel = "";

            try
            {

                if (UserLevel_.Trim() != "")
                    UserLevel = UserLevel_.Trim();

                if (SupplierCode.Trim() != "")
                    supplier_ = SupplierCode.Trim();

                if (UserLevel.Trim() == "3")
                {
                    supplier_ = "";
                    where_ = sql_ + " IN ( select VS.Supplier_Code FROM [dbo].[Tbl_Supplier] VS WHERE VS.Supplier_Code LIKE '%" + supplier_ + "%')";
                }
                else
                {
                    if (supplier_ == "SKC")
                    {
                        if (user_name.Trim() != "")
                        {
                            where_ = sql_ + " IN ( SELECT Supplier_Code FROM Tbl_Supplier_S_Base WHERE S_Base like '%" + user_name.Trim() + "%' ) ";
                        }
                        else
                        {
                            where_ = sql_ + " IN ( select VS.Supplier_Code FROM [dbo].[Tbl_Supplier] VS WHERE VS.Supplier_Code = '" + supplier_ + "')";
                        }
                    }
                    else
                    {
                        where_ = sql_ + " IN ( select VS.Supplier_Code FROM [dbo].[Tbl_Supplier] VS WHERE VS.Supplier_Code = '" + supplier_ + "')";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return where_;

        }
    }
}