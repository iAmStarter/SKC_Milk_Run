using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HyDrA.Data.SqlClient;
using System.Configuration;
using SKC_Milk_Run.App_Data;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Device;
using System.Net.Mail;
using System.Text.RegularExpressions;
using RestSharp;
using OfficeOpenXml;

namespace SKC_Milk_Run
{
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

    [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        public string strConnect = Configs.ConnectionString;

        public WebService()
        {

        }

        public string ConvertDataTabletoString(DataTable dt)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }

            return serializer.Serialize(rows);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CheckGate_Time(string TimeIn_ = "00:00", string TimeOut_ = "00:00", string Gate_ = "", string Type_work_ = "")
        {
            Global_Service data_ = new Global_Service();
            string sql = "";
            if (Type_work_ == "2")
            {
                sql = "SELECT [Supplier_Gate_Code] FROM [dbo].[Tbl_TRANSPORT_ROUTE] where Type= 'XX'";
            }
            else if (Type_work_ == "3")
            {
                sql = "SELECT [Supplier_Gate_Code] FROM [dbo].[Tbl_TRANSPORT_ROUTE] where Type= 'XX'";
            }
            else
            {
                sql = "exec [dbo].[CheckGate_Time] N'" + TimeIn_ + "',N'" + TimeOut_ + "',N'" + Gate_ + "'";
            }
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVehiclelabel(string DataID = "", string UserId = "")
        {
            Global_Service _Label = new Global_Service();

            string[] ListDataProcessID = DataID.Split(',');

            DataTable GetVehicle_Label = _Label.SQLSelect("exec [dbo].[GetVehiclelabel] '" + DataID + "'");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetVehicle_Label));

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Check_Gate_Exist(string Gate_Code)
        {
            string JsonMsg = "";

            try
            {
                string sql = @"SELECT [Gate_No] FROM  [dbo].[Tbl_Gate]  WHERE Gate_No = '" + Gate_Code + "'";
                Global_Service data_ = new Global_Service();
                DataTable Getdata = data_.SQLSelect(sql);
                if (Getdata.Rows.Count > 0)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Gate : "+ Gate_Code + " , already exist!" } });
                }
                else
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "OK !" } });
                }
                
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetDrivers(string TRANSPORTER_, string ID_)
        {
            string sql = @"SELECT [ID],[Supplier_Code],[Name],[Tel_Number] FROM [dbo].[Tbl_Driver] WHERE Supplier_Code = '" + TRANSPORTER_ + "' and CONVERT(VARCHAR,[ID]) like '%" + ID_ + "' ORDER BY [Supplier_Code],[Name]";
            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVehicle(string TRANSPORTER_, string CAR_LICENSE_, string DRIVER_NAME_, string SIM_
            , string BlackBox_, string Vehicle_Type_, string status_, string Vehicle_STATUS, string sql_supplier)
        {
            string sql = @"SELECT V.[ID],[Vehicle_Registration_No],[Vehicle_Supplier_No],D.Name as [Driver_Name],isnull([QR_Code_Encrypt],'') as QR_Code_Encrypt ,V.[Create_Date],V.[Status] ";
            sql += ",(CASE WHEN V.[Status] = '1' THEN 'Active' ELSE 'N/A' END) as Status_txt,D.[Tel_Number],[Vehicle_Type],isnull([GPS_SIM],'') as GPS_SIM,";
            sql += " isnull([GPS_BlackBox],'') as GPS_BlackBox ,isnull((SELECT [Supplier_Name] FROM [Tbl_Supplier] s where s.[Supplier_Code] COLLATE DATABASE_DEFAULT  = V.[Vehicle_Supplier_No]),[Vehicle_Supplier_No]) as Vehicle_Supplier_Name ";
            sql += " ,isnull(Type,'1') as [Type] ,Driver_ID  FROM [Tbl_Vehicle_Registration] V LEFT JOIN  Tbl_Driver D ON V.Driver_ID = D.ID ";
            sql += "  Where Vehicle_Registration_No like N'%" + CAR_LICENSE_ + "%' ";
            sql += "and Vehicle_Supplier_No in (select Supplier_Code COLLATE DATABASE_DEFAULT from Tbl_Supplier where (Supplier_Name like '%" + TRANSPORTER_ + "%' or Short_Name like '%" + TRANSPORTER_ + "%'  or Supplier_Code like '%" + TRANSPORTER_ + "%' )) ";
            sql += " and D.Name like N'%" + DRIVER_NAME_ + "%' and V.Status like '%" + status_ + "%' and isnull(V.Type,'1') like '%" + Vehicle_STATUS + "%' ";
            sql += " and Vehicle_Type like N'%" + Vehicle_Type_ + "%' and ISNULL(GPS_SIM,'') like '%" + SIM_ + "%' and ISNULL(GPS_BlackBox,'') like '%" + BlackBox_ + "%'";
            sql += " and Vehicle_Supplier_No IN (" + sql_supplier + ") ORDER BY [Vehicle_Supplier_No],D.Name,[Vehicle_Registration_No] ";
            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Suppliers_list_Vehicle(string ID)
        {
            string SQLCommand = "";

            SQLCommand = @"SELECT Supplier_Code ,Short_Name ,Supplier_Name FROM [Tbl_Supplier] WHERE Supplier_Code not in ( SELECT [Supplier_Code] FROM [Tbl_Vehicle_Supplier]  where [ID] =" + ID + " ) ";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(SQLCommand);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get_Suppliers(string ID)
        {
            string sql = "";

            sql = @"SELECT ID,[Supplier_Code],(SELECT [Supplier_Name] FROM [Tbl_Supplier] S where S.[Supplier_Code] = V.[Supplier_Code]) as Supplier_Name  FROM [Tbl_Vehicle_Supplier] V  where [ID] =" + ID;

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get_Supplier_Contact_info(string Supplier_)
        {
            string sql = "";

            sql = @"SELECT s.[Supplier_Code],[Name],[Position],[Tel_Number],[Email],'('+[Short_Name] COLLATE DATABASE_DEFAULT +') '+[Supplier_Name] as Supplier_Name  FROM [Tbl_Supplier] s left join [dbo].[Tbl_Supplier_Part_Del_Contact] c ON s.Supplier_Code = c.Supplier_Code  where s.[Supplier_Code] ='" + Supplier_ + "'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get_Supplier_Contact(string Supplier_)
        {
            string sql = "";

            sql = @"SELECT [Supplier_Code],[Name],[Position],[Tel_Number],[Email],(SELECT '('+[Short_Name] COLLATE DATABASE_DEFAULT +') '+[Supplier_Name] FROM [Tbl_Supplier] s where s.Supplier_Code = c.Supplier_Code ) as Supplier_Name  FROM [dbo].[Tbl_Supplier_Part_Del_Contact] c where [Supplier_Code] ='" + Supplier_ + "'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get_Supplier_Gate_GPS(string Code_, string Mode_)
        {
            string sql = "";

            sql = @"SELECT [Code],[Type],[No],[GPS],Isnull(Scope_distance,0) as Scope_distance ,Isnull([Note],'') as Note FROM [dbo].[Tbl_Supplier_Gate_GPS] where [Code] ='" + Code_ + "' AND Type = '" + Mode_ + "'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Budget_Year(string Year)
        {
            string sql = "";

            sql = @"SELECT [Year], [Month], SUM(Budget_MILKRUN) as Budget_MILKRUN , SUM(Budget_EXTRA) as Budget_EXTRA , SUM(Budget_URGENT) as Budget_URGENT , SUM(Budget_SHORT) as Budget_SHORT ";
            sql += " FROM ( SELECT [Year], [Month], (CASE WHEN [Group] = '1' THEN  SUM(ISNULL(Budget, 0)) ELSE 0 END) AS Budget_MILKRUN,  ";
            sql += " (CASE WHEN [Group] = '2' THEN SUM(ISNULL(Budget, 0)) ELSE 0 END) AS Budget_EXTRA,  ";
            sql += " (CASE WHEN [Group] = '3' THEN SUM(ISNULL(Budget, 0)) ELSE 0 END) AS Budget_URGENT,  ";
            sql += " (CASE WHEN [Group] = '4' THEN SUM(ISNULL(Budget, 0)) ELSE 0 END) AS Budget_SHORT ";
            sql += " FROM [dbo].[Tbl_Budget] WHERE [Year] = '" + Year + "' GROUP BY [Year],[Month],[Group] ) V GROUP BY [Year],[Month] ";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Config_Access_Object(string Access)
        {
            string sql = "";

            sql = @"SELECT [Id] ,[AccessId] ,[ObjectId],(SELECT [Description] FROM [Data_Object] obj where obj.[ObjectId] = A.[ObjectId]) as Obj_Desc  FROM [Config_Access_Object] A Where AccessId =" + Access;

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Data_Object(string Access)
        {
            string sql = "";

            sql = @"SELECT [ObjectId],[Description],ObjectName FROM [Data_Object] Where ObjectId not in (SELECT [ObjectId]  FROM [Config_Access_Object] where AccessId =" + Access + ")";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void User_Data_Object(string UserId)
        {
            string sql = "";

            sql = @"SELECT DISTINCT ObjectId, (CASE  WHEN EXISTS ( SELECT ObjectId FROM [dbo].[Config_Access_Object] conf  INNER JOIN [dbo].[Data_User] usr ON usr.AccessLevel = conf.AccessId WHERE usr.UserId = '" + UserId + "' AND conf.ObjectId = obj.ObjectId) THEN '1' ELSE '0' END) AS selected,ObjectName FROM [dbo].[Data_Object] obj ";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Check_User_Data(string User_name)
        {
            string sql = "";

            sql = @"SELECT Username FROM [dbo].[Data_User] WHERE [Username] = '" + User_name + "'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void User_Access(string UserLevel, string Supplier)
        {
            string sql = "", sql_where = "";

            sql_where = " where [Admin] in ('1','0') ";

            if (Supplier == "SKC")
                sql_where = " where [Admin] in ('1','0') ";

            if (UserLevel != "3")
                sql_where = " where [Admin] in ('0') ";

            sql = @"select AccessId, AccessName from [dbo].[Data_User_Access] " + sql_where + " order by AccessId desc";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void User_Level(string UserLevel)
        {
            string sql = "", sql_where = "";

            if (UserLevel != "3")
                sql_where = " where [LevelId] not in (3) ";


            sql = @"select LevelId, LevelName from [dbo].[Data_User_Level] " + sql_where + " order by Hierarchy";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUser(string Username, string Name, string SupplierCode, string UserLevel, string AccessLevel, string Status_, string sql_supplier)
        {
            string sql = "", sql_where = "";
            sql_where = " where SupplierCode like '%" + SupplierCode + "%' ";
            sql_where += " and Username like '%" + Username + "%'";
            sql_where += " and (FirstName+LastName) like '%" + Name + "%'";
            sql_where += " and convert(varchar, isnull(AccessLevel,''))  like '%" + AccessLevel + "%'";
            sql_where += " and convert(varchar, isnull(AccessLevel,'')) like '%" + UserLevel + "%'";
            sql_where += " and Status like '%" + Status_ + "%'";
            sql_where += " and SupplierCode IN (" + sql_supplier + ")";

            sql = @"select UserId, Username,'*******' as Password_txt, Password, FirstName, LastName
                                    , SupplierCode, isnull(AccessLevel,'') as AccessLevel, isnull(UserLevel,'') as UserLevel
                                    , isnull((select top 1 AccessName from [dbo].[Data_User_Access] where AccessId = isnull(AccessLevel,0)), '') as AccessName
                                    , isnull((select top 1 LevelName from [dbo].[Data_User_Level] where LevelId = isnull(UserLevel,0)), '') as LevelName,Status,isnull(Email,'') as Email
                                from [dbo].[Data_User] " + sql_where + @"                                
                                order by UserId ";


            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }
        public void Excel_Export(string ID, string User, string path)
        {
            string sql = "";

            sql = @"SELECT [Id],[Supplier_Code],[Route_Code],[Round_No],[Round_Name],[Del_Time_Name],CONVERT(VARCHAR(5), [Del_Time_From], 108) AS [Del_Time_From] ";
            sql += ",CONVERT(VARCHAR(5), [Del_Time_To], 108) AS [Del_Time_To],CONVERT(VARCHAR, [Scope_From], 120) as [Scope_From],CONVERT(VARCHAR, [Scope_To], 120) as [Scope_To] ";
            sql += " ,[Create_By],CONVERT(VARCHAR,[Create_Date], 120) as Create_Date,[Update_By],CONVERT(VARCHAR,[Update_Date], 120) as Update_Date ";
            sql += " FROM [dbo].[Tbl_Time_Order] where [Id] IN ('" + ID.Replace(",", "', '") + "') AND Status IN ('N','I','E','R') ";

            if (User != "")
            {
                sql += "AND REPLACE([Route_Code], Supplier_Code +'-', '') IN (SELECT Gate_No FROM [Tbl_User_Gate] UG left join Tbl_Gate G ON UG.[Gate_Id] = G.Id WHERE [UserId] =" + User + " ) ";
            }

            sql += " Order by [Id],[Supplier_Code],[Route_Code],[Round_No],[Round_Name],[Del_Time_Name] ";

            Global_Service GS = new Global_Service();
            DataTable getDataMonitoring = GS.SQLSelect(sql);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create the worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("dataSheet");

                //load the datatable into the sheet, with headers
                worksheet.Cells["A1"].LoadFromDataTable(getDataMonitoring, true);

                //send the file to the browser
                byte[] data = excelPackage.GetAsByteArray();
                File.WriteAllBytes(path, data);
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Send_mail(string ID, string Status, string host, string port, string userid, string mail_sender)
        {
            string JsonMsg = "";
            string mail_from = "", mail_to = "";
            string subject = "K-Express : Time Order", body = "" , status_txt ="";

            try
            {
                string ExcelName = "Time_Order_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string path = Server.MapPath("~/Excel_Files/" + ExcelName + ".xlsx");

                Excel_Export(ID, userid, path);

                string sqlmail = "";

                sqlmail = @"SELECT [FirstName]+' '+ [LastName] +' ('+[Username] + ' - ' +ISNULL([Email],'')+')' FROM [dbo].[Data_User] WHERE UserId =" + userid;

                Global_Service datamail_ = new Global_Service();
                DataTable Getdatamail = datamail_.SQLSelect(sqlmail);

                if (Getdatamail.Rows.Count > 0)
                {
                    mail_from = Getdatamail.Rows[0][0].ToString();

                }

                body = " Dear all concerned, <br /><br /> K-Express system. <br /> Please login for continue next process. <br /> " + mail_from;

                string sql = "";

                sql = @"SELECT ISNULL([Email],'') FROM [dbo].[Data_User] WHERE UserId NOT IN (" + userid + ") AND UserId IN (SELECT DISTINCT UserId FROM [Tbl_User_Gate] UG LEFT JOIN Tbl_Gate G ON UG.[Gate_Id] = G.Id ";
                sql += " WHERE Gate_No IN ( SELECT DISTINCT REPLACE([Route_Code], Supplier_Code + '-', '') FROM [dbo].[Tbl_Time_Order] ";
                sql += " WHERE [Id] IN('" + ID.Replace(",", "', '") + "') AND STATUS <> 'D')) AND RTRIM(LTRIM(ISNULL([Email], ''))) <> ''  ";

                Global_Service data_ = new Global_Service();
                DataTable Getdata = data_.SQLSelect(sql);

                if (Getdata.Rows.Count > 0)
                {
                    for (int i = 0; i < Getdata.Rows.Count; i++)
                    {
                        if (Getdata.Rows[i][0].ToString() != "")
                        {
                            if (mail_to == "")
                            {
                                mail_to = Regex.Replace(Getdata.Rows[i][0].ToString().Trim().Replace(System.Environment.NewLine, ""), @"(\r\n?|\n)", "");
                            }
                            else
                            {
                                mail_to += "," + Regex.Replace(Getdata.Rows[i][0].ToString().Trim().Replace(System.Environment.NewLine, ""), @"(\r\n?|\n)", "");
                            }
                        }
                    }
                }

                if (mail_to != "")
                {

                    string smtp_host = host;
                    int smtp_port = int.Parse(port);

                    Byte[] buffer = null;
                    if (ExcelName != "")
                    {
                        string pdfPath = Server.MapPath("~/Excel_Files/" + ExcelName + ".xlsx");
                        WebClient client = new WebClient();
                        buffer = client.DownloadData(pdfPath);
                    }

                    string f_mail;

                    f_mail = mail_sender;

                    using (MailMessage mm = new MailMessage(f_mail, mail_to))
                    {
                        if (ExcelName != "")
                        {
                            mm.Attachments.Add(new Attachment(new MemoryStream(buffer), ExcelName + ".xlsx"));
                        }



                        if (Status == "I")
                        {
                            status_txt = "Issue";
                        }

                        if (Status == "E")
                        {
                            status_txt = "Effective";
                        }

                        if (Status == "R")
                        {
                            status_txt = "Reject";
                        }

                        mm.Subject = status_txt + " : " + subject + "(" + ID + ")";
                        mm.Body = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head></head><body style='margin: 0; padding: 0;'> " + status_txt + " : " + subject + "(" + ID + ")" + "<br />" + body.Replace(Environment.NewLine, "<br />") + "</body></html>";

                        mm.SubjectEncoding = System.Text.Encoding.GetEncoding("Windows-874");
                        mm.BodyEncoding = System.Text.Encoding.GetEncoding("Windows-874");
                        mm.IsBodyHtml = true;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtp_host;
                        smtp.EnableSsl = true;

                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
                        credentials.UserName = mail_sender;
                        credentials.Password = "";
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = credentials;

                        smtp.Port = smtp_port;
                        smtp.Send(mm);

                        JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Email sent conpleted !" } });
                    }

                }
                else
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Email to is null !" } });
                }

            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }
        private bool RemoteServerCertificateValidationCallback(object sender,System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //Console.WriteLine(certificate);
            return true;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetTime_OrderID(string User)
        {
            string sql_where = "";

            if (User != "")
            {
                sql_where = " WHERE REPLACE([Route_Code], Supplier_Code +'-', '') IN (SELECT Gate_No FROM [Tbl_User_Gate] UG left join Tbl_Gate G ON UG.[Gate_Id] = G.Id WHERE [UserId] =" + User + " ) ";
            }

            string sql = "SELECT DISTINCT [Id] FROM [dbo].[Tbl_Time_Order] " + sql_where + " Order by [Id]";
            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }


        public void MFG_API_POST(string MODE, string ID, string supplierCode, string routeCode, string roundNo
           , string roundName, string deliveryTimeName, string deliveryTimeFrom, string deliveryTimeTo,
           string scopeFrom, string scopeTo, string createDate, string createBy, string updateDate, string updateBy, string pathValue, string MFG_API_Bearer)
        {
            string sql_upd = "";

            var client = new RestClient(pathValue);
            client.Timeout = -1;
            var body = "{\"supplierCode\":\"" + supplierCode + "\",\"routeCode\":\"" + routeCode + "\",";
            body += "\"roundNo\":\"" + roundNo + "\",\"roundName\":\"" + roundName + "\",\"deliveryTimeName\":\"" + deliveryTimeName + "\",";
            body += "\"deliveryTimeFrom\":\"" + deliveryTimeFrom + "\",\"deliveryTimeTo\":\"" + deliveryTimeTo + "\",\"scopeFrom\"" + scopeFrom + "\",";
            body += "\"scopeTo\":\"" + scopeTo + "\",\"createDate\":\"" + createDate + "\",\"createBy\":\"" + createBy + "\",";
            body += "\"updateDate\":\"" + updateDate + "\",\"updateBy\":\"" + updateBy + "\"}";

            if (MODE == "POST")
            {
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", MFG_API_Bearer);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorException != null)
                {
                    sql_upd = " UPDATE Tbl_Time_Order SET [Message] = '" + response.ErrorException.Message.ToString() + "' ";
                    sql_upd += " WHERE [Id]+'#' +[Supplier_Code] + '#' +[Route_Code] + '#' +[Round_No] + '#' +[Round_Name] + '#' +[Del_Time_Name] = '" + ID + '#' + supplierCode + '#' + routeCode + '#' + roundNo + '#' + roundName + '#' + deliveryTimeName + "' ";

                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql_upd);

                }
            }

            if (MODE == "PUT")
            {
                var request = new RestRequest(Method.PUT);
                request.AddHeader("Authorization", MFG_API_Bearer);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorException != null)
                {
                    sql_upd = " UPDATE Tbl_Time_Order SET [Message] = '" + response.ErrorException.Message.ToString() + "' ";
                    sql_upd += " WHERE [Id]+'#' +[Supplier_Code] + '#' +[Route_Code] + '#' +[Round_No] + '#' +[Round_Name] + '#' +[Del_Time_Name] = '" + ID + '#' + supplierCode + '#' + routeCode + '#' + roundNo + '#' + roundName + '#' + deliveryTimeName + "' ";

                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql_upd);

                }
            }

            if (MODE == "DELETE")
            {
                var request = new RestRequest(Method.DELETE);
                request.AddHeader("Authorization", MFG_API_Bearer);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                if (response.ErrorException != null)
                {
                    sql_upd = " UPDATE Tbl_Time_Order SET [Message] = '" + response.ErrorException.Message.ToString() + "' ";
                    sql_upd += " WHERE [Id]+'#' +[Supplier_Code] + '#' +[Route_Code] + '#' +[Round_No] + '#' +[Round_Name] + '#' +[Del_Time_Name] = '" + ID + '#' + supplierCode + '#' + routeCode + '#' + roundNo + '#' + roundName + '#' + deliveryTimeName + "' ";

                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql_upd);

                }
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetTime_Order_ByID(string ID, string User, string URL, string Key)
        {
            string sql = "";
            string JsonMsg = "";
            try
            {
                sql = @"SELECT [Id],[Supplier_Code],[Route_Code],[Round_No],[Round_Name],[Del_Time_Name],CONVERT(VARCHAR(5), [Del_Time_From], 108) AS [Del_Time_From] ";
                sql += ",CONVERT(VARCHAR(5), [Del_Time_To], 108) AS [Del_Time_To],CONVERT(VARCHAR, [Scope_From], 120) as [Scope_From],CONVERT(VARCHAR, [Scope_To], 120) as [Scope_To] ";
                sql += " ,[Create_By],CONVERT(VARCHAR,[Create_Date], 120) as Create_Date,[Update_By],CONVERT(VARCHAR,[Update_Date], 120) as Update_Date ";
                sql += " FROM [dbo].[Tbl_Time_Order] where [Id] IN ('" + ID.Replace(",", "', '") + "') AND Status ='E'";

                if (User != "")
                {
                    sql += "AND REPLACE([Route_Code], Supplier_Code +'-', '') IN (SELECT Gate_No FROM [Tbl_User_Gate] UG left join Tbl_Gate G ON UG.[Gate_Id] = G.Id WHERE [UserId] =" + User + " ) ";
                }

                Global_Service data_ = new Global_Service();
                DataTable Getdata = data_.SQLSelect(sql);

                if (Getdata.Rows.Count > 0)
                {
                    for (int i = 0; i < Getdata.Rows.Count; i++)
                    {
                        if (Getdata.Rows[i][0].ToString() != "")
                        {
                            MFG_API_POST("DELETE", Getdata.Rows[i][0].ToString(), Getdata.Rows[i][1].ToString(), Getdata.Rows[i][2].ToString(), Getdata.Rows[i][3].ToString()
                               , Getdata.Rows[i][4].ToString(), Getdata.Rows[i][5].ToString(), Getdata.Rows[i][6].ToString(), Getdata.Rows[i][7].ToString()
                               , Getdata.Rows[i][8].ToString(), Getdata.Rows[i][9].ToString(), Getdata.Rows[i][11].ToString(), Getdata.Rows[i][10].ToString()
                               , Getdata.Rows[i][13].ToString(), Getdata.Rows[i][12].ToString(), URL, Key);

                            MFG_API_POST("POST", Getdata.Rows[i][0].ToString(), Getdata.Rows[i][1].ToString(), Getdata.Rows[i][2].ToString(), Getdata.Rows[i][3].ToString()
                               , Getdata.Rows[i][4].ToString(), Getdata.Rows[i][5].ToString(), Getdata.Rows[i][6].ToString(), Getdata.Rows[i][7].ToString()
                               , Getdata.Rows[i][8].ToString(), Getdata.Rows[i][9].ToString(), Getdata.Rows[i][11].ToString(), Getdata.Rows[i][10].ToString()
                               , Getdata.Rows[i][13].ToString(), Getdata.Rows[i][12].ToString(), URL, Key);
                        }
                    }
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Interface MFG completed !" } });
                }
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetTime_Order(string ID, string SupplierCode, string GateCode, string Scope_From, string Scope_To, string User, string Mode, string Status)
        {
            string sql = "";

            if (Mode == "NEW")
            {
                sql = @"SELECT [Id],[Supplier_Code],[Route_Code],[Round_No],[Round_Name],[Del_Time_Name],CONVERT(VARCHAR(5), [Del_Time_From], 108) AS [Del_Time_From] ";
                sql += ",CONVERT(VARCHAR(5), [Del_Time_To], 108) AS [Del_Time_To],CONVERT(VARCHAR, [Scope_From], 103) as [Scope_From],CONVERT(VARCHAR, [Scope_To], 103) as [Scope_To] ";
                sql += " ,[Create_By],(CASE WHEN [Status] = 'N' THEN 'New' WHEN [Status] = 'I' THEN 'Issue'  WHEN [Status] = 'E' THEN 'Effective'  WHEN [Status] = 'R' THEN 'Reject' ELSE 'Draft' END) as [Status] , [Status] as [Status_val]";
                sql += " ,[Id]+'#' +[Supplier_Code] + '#' +[Route_Code] + '#' +[Round_No] + '#' +[Round_Name] + '#' +[Del_Time_Name] as Key_ID ";
                sql += " ,CONVERT(VARCHAR,[Create_Date], 103) as Create_Date,[Message],[Update_By],CONVERT(VARCHAR,[Update_Date], 103) as Update_Date FROM [dbo].[Tbl_Time_Order] where [Id] IN ('" + ID.Replace(",", "', '") + "') AND Status ='N'";
                sql += " ORDER BY [Id],[Supplier_Code],[Route_Code], [Round_No],[Round_Name],[Del_Time_Name]";
            }
            else
            {
                sql = @"SELECT [Id],[Supplier_Code],[Route_Code],[Round_No],[Round_Name],[Del_Time_Name],CONVERT(VARCHAR(5), [Del_Time_From], 108) AS [Del_Time_From] ";
                sql += ",CONVERT(VARCHAR(5), [Del_Time_To], 108) AS [Del_Time_To],CONVERT(VARCHAR, [Scope_From], 103) as [Scope_From],CONVERT(VARCHAR, [Scope_To], 103) as [Scope_To] ";
                sql += " ,[Create_By],(CASE WHEN [Status] = 'N' THEN 'New' WHEN [Status] = 'I' THEN 'Issue'  WHEN [Status] = 'E' THEN 'Effective'  WHEN [Status] = 'R' THEN 'Reject' ELSE 'Draft' END) as [Status]  , [Status] as [Status_val] ";
                sql += " ,[Id]+'#' +[Supplier_Code] + '#' +[Route_Code] + '#' +[Round_No] + '#' +[Round_Name] + '#' +[Del_Time_Name] as Key_ID ";
                sql += " ,CONVERT(VARCHAR,[Create_Date], 103) as Create_Date,[Message],[Update_By],CONVERT(VARCHAR,[Update_Date], 103) as Update_Date FROM [dbo].[Tbl_Time_Order] ";



                if (SupplierCode == "")
                {
                    sql += " where Supplier_Code like '%' ";
                }
                else
                {
                    sql += " where Supplier_Code IN ('" + SupplierCode.Replace(",", "','") + "')";
                }

                if (ID.Trim() != "")
                {
                    sql += " AND [Id] IN ('" + ID.Replace(",", "', '") + "')";
                }

                if (GateCode == "")
                {
                    sql += "AND [Route_Code] like '%'";
                }
                else
                {
                    sql += "AND REPLACE([Route_Code], Supplier_Code +'-', '') IN ('" + GateCode.Replace(",", "','") + "') ";
                }

                if (User != "")
                {
                    sql += "AND REPLACE([Route_Code], Supplier_Code +'-', '') IN (SELECT Gate_No FROM [Tbl_User_Gate] UG left join Tbl_Gate G ON UG.[Gate_Id] = G.Id WHERE [UserId] =" + User + " ) ";
                }

                if (Scope_From != "")
                {
                    sql += " AND Scope_From >= CONVERT(DATETIME, '" + Scope_From + "', 103) ";
                }

                if (Scope_To != "")
                {
                    sql += " AND Scope_To <= CONVERT(DATETIME, '" + Scope_To + "', 103) ";
                }

                if (Status == "")
                {
                    sql += " AND Status IN ('N','I','E','R') ";
                }
                else
                {
                    sql += " AND Status IN ('" + Status.Replace(",", "','") + "') ";
                }
                sql += " ORDER BY [Id],[Supplier_Code],[Route_Code],[Round_Name],[Round_No],[Del_Time_Name]";

            }

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void CreateTime_Order(string ID, string SupplierCode, string GateCode, string Scope_From, string Scope_To, string User, string Mode)
        {
            string JsonMsg = "";

            string sql = "EXEC [dbo].[Create_TIME_ORDERING] '" + ID + "','" + SupplierCode + "','" + GateCode + "','" + Scope_From + "','" + Scope_To + "','" + User + "','" + Mode + "'";

            try
            {
                Global_Service GS = new Global_Service();

                GS.SQLExecute(sql);

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Create completed !" } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUser_Access(string AccessName, string Admin)
        {
            string sql = @"SELECT [AccessId] ,[AccessName] ,[Admin] FROM [Data_User_Access] Where AccessName like '%" + AccessName + "%' and Admin like '%" + Admin + "%'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Supplier_list(string UserLevel_, string SupplierCode, string user_name)
        {
            string SQLCommand = "", supplier_ = "";

            string UserLevel = "";

            if (UserLevel_.Trim() != "")
                UserLevel = UserLevel_.Trim();

            if (SupplierCode.Trim() != "")
                supplier_ = SupplierCode.Trim();


            if (supplier_ == "SKC")
            {
                supplier_ = "";
            }


            if (UserLevel.Trim() == "3")
            {
                supplier_ = "";
            }


            SQLCommand = @"SELECT Supplier_Code as Supplier_No,Short_Name ,Supplier_Name,RTRIM(LTRIM([Supplier_Name]))  + '(' + [Supplier_Code] COLLATE DATABASE_DEFAULT + ')' AS L_Supplier_Name FROM [Tbl_Supplier] WHERE Supplier_Code LIKE '%" + supplier_.Trim() + "%'";

            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(SQLCommand);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        public static string Supplier_list_sql(string UserLevel_, string SupplierCode, string user_name = "")
        {
            string SQLCommand = "", supplier_ = "";

            string UserLevel = "";

            if (UserLevel_.Trim() != "")
                UserLevel = UserLevel_.Trim();

            if (SupplierCode.Trim() != "")
                supplier_ = SupplierCode.Trim();


            if (supplier_ == "SKC")
            {
                supplier_ = "";
            }


            if (UserLevel.Trim() == "3")
            {
                supplier_ = "";
            }


            SQLCommand = "SELECT Supplier_Code  FROM [Tbl_Supplier] WHERE Supplier_Code LIKE '%" + supplier_.Trim() + "%'";

            return SQLCommand;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetRound(string Round_, string Status_)
        {
            Global_Service data_ = new Global_Service();
            DataTable Getdata = data_.SQLSelect(@"SELECT [Id],[Round_Code],[Status],[Create_Date] FROM [Tbl_Round] where Round_Code like '%" + Round_ + "%' and Status like '%" + Status_ + "%'");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(Getdata));
        }

        // List Supplier Transport
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSupplierTransport(string supplier_code, string Type, string userlevel_, string user_name_)
        {
            string user_supplier_sql = "";

            user_supplier_sql = Supplier_list_sql(userlevel_, supplier_code, "");

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(@"SELECT S.*,isnull((Select GPS from [Tbl_Supplier_GPS] GPS WHERE GPS.Supplier_Code = S.Supplier_Code),'-') as [GPS_CODE] FROM [Tbl_Supplier] S where [Type] like '%" + Type + "' and [Status] ='1' and [Supplier_Code] IN ( " + user_supplier_sql + ")");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGPS(string code)
        {
            string sql = "";
            sql = " SELECT isnull(MAX(GPS),'') as GPS FROM(SELECT isnull(GPS,'') as GPS  from Tbl_Gate where Gate_No = '" + code + "' ";
            sql += " UNION SELECT isnull(GPS,'') as GPS  from[Tbl_Supplier_GPS] where Supplier_Code = '" + code + "') G ";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSupplier(string supplier_code, string supplier_name, string supplier_short, string GPS_, string Type_, string status_, string sql_supplier)
        {
            string sql = "";

            sql = @"SELECT S.*,'' as GPS_CODE ";
            sql += " FROM [Tbl_Supplier] S where [Type] like '%" + Type_ + "%' and [Status] like '%" + status_ + "%' ";
            sql += " and [Supplier_Code] like '%" + supplier_code + "%' and Supplier_Name like '%" + supplier_name + "%' ";
            sql += " and Short_Name like '%" + supplier_short + "%' AND Supplier_Code IN ( " + sql_supplier + ")";
            if (GPS_ != "")
            {
                sql += " and Supplier_Code in (Select Supplier_Code from [Tbl_Supplier_GPS] Where GPS like '%" + GPS_ + "%')";
            }

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetVehicle_Id(string ID_)
        {


            string sql = @" SELECT isnull([Vehicle_Id],0) as [Vehicle_Id] ";
            sql += " FROM [Tbl_TRANSPORT] ";
            sql += " where ID = " + ID_;

            Global_Service Vehicle = new Global_Service();
            DataTable GetVehicle = Vehicle.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetVehicle));
        }

        // List ROUTE Transport
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetROUTETransportListDiagram(string supplier_code, string user_supplier_code, string userlevel_, string user_name_)
        {
            string user_supplier_sql = "";

            string sql = "";
            user_supplier_sql = Supplier_list_sql(userlevel_, user_supplier_code, "");

            sql = @" SELECT [Id],[Transporter_Code],[Route_Code],[Route_Type],[Start_Time],[End_Time],isnull([Vehicle_Id],0) as [Vehicle_Id],";
            sql += " [Fix_Cost],[Fuel_AGV_Price],[Fuel_KM_Liter],[Maintenance_Cost],[Distance],[Express_Way],[Shift],[OT],[Other],[Utilize],";
            sql += " [Effective_Date],[Create_date],[Update_Date],[Create_By],[Update_By],[Status] FROM [Tbl_TRANSPORT] ";
            sql += " where [Transporter_Code] " + supplier_code + "  and [Transporter_Code] IN (" + user_supplier_sql + ") ";
            sql += " order by [Transporter_Code],[Route_Code] ";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        // List ROUTE Transport
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetROUTETransport(string supplier_code, string user_supplier_code, string userlevel_, string user_name_)
        {
            string user_supplier_sql = "";

            string sql = "";
            user_supplier_sql = Supplier_list_sql(userlevel_, user_supplier_code, "");

            sql = @" SELECT [Id],[Transporter_Code],[Route_Code],[Route_Type],[Start_Time],[End_Time],isnull([Vehicle_Id],0) as [Vehicle_Id],";
            sql += " [Fix_Cost],[Fuel_AGV_Price],[Fuel_KM_Liter],[Maintenance_Cost],[Distance],[Express_Way],[Shift],[OT],[Other],[Utilize],";
            sql += " [Effective_Date],[Create_date],[Update_Date],[Create_By],[Update_By],[Status] FROM [Tbl_TRANSPORT] ";
            sql += " where [Transporter_Code] like '%" + supplier_code + "%'  and [Transporter_Code] IN (" + user_supplier_sql + ") ";
            sql += " order by [Transporter_Code],[Route_Code] ";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        // List ROUTE Transport
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCarLicense(string Supplier_)
        {
            string sql = @"SELECT V.[ID],[Vehicle_Registration_No],[Vehicle_Supplier_No],D.Name as [Driver_Name], ";
            sql += " (select Short_Name from Tbl_Supplier where Supplier_Code COLLATE DATABASE_DEFAULT = [Vehicle_Supplier_No] COLLATE DATABASE_DEFAULT) as Supplier_Name ";
            sql += " FROM [Tbl_Vehicle_Registration]  V LEFT JOIN Tbl_Driver D ON V.Driver_ID = D.ID  Where Vehicle_Supplier_No ='" + Supplier_ + "'";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetCarLicense_By_Supplier_license(string Supplier_, string license_No)
        {
            string sql_ = "";

            string sql_where = "";
            if (license_No == "")
            {
                sql_where = " and V.[ID] = 0 ";
            }
            else
            {
                sql_where = " and V.[ID] = " + license_No;
            }

            sql_ = @"SELECT V.[ID],Vehicle_Type,[Vehicle_Registration_No],[Vehicle_Supplier_No],D.Name as [Driver_Name],";
            sql_ += " (select Short_Name from Tbl_Supplier where Supplier_Code COLLATE DATABASE_DEFAULT = [Vehicle_Supplier_No] COLLATE DATABASE_DEFAULT) as Supplier_Name ";
            sql_ += ",D.Tel_Number,[GPS_SIM],[GPS_BlackBox],isnull(V.Type,'1') as [Type],(CASE WHEN isnull(Type,'1') = '1' THEN 'Normal' ELSE 'Back up' END) as [Type_txt] ";
            sql_ += ",Driver_ID FROM [Tbl_Vehicle_Registration] V ";
            sql_ += " LEFT JOIN  Tbl_Driver D ON V.Driver_ID = D.ID Where Vehicle_Supplier_No ='" + Supplier_ + "'";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql_ + sql_where);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetSupplierCode(string supplier_code)
        {
            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(@"SELECT * FROM [Tbl_Supplier] where [Type] = '1' and [Status] ='1' and [Supplier_Code] like '%" + supplier_code + "%' order by Short_Name ");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGate(string code, string area_gate, string gps, string status, string gate_color)
        {
            string sql = "";

            sql = @"SELECT [Id],[Area_Gate],[Gate_No],[Gate_Name],[GPS],[Status],[Create_Date], Gate_Color ";
            sql += ",(CASE WHEN isnull(Gate_Color,'#ffffff')='' THEN '#ffffff' ELSE isnull(Gate_Color,'#ffffff') END) as B_Gate_Color ";
            sql += ",(CASE WHEN [Status] = '1' THEN 'Active' ELSE 'N/A' END) as Status_txt FROM [Tbl_Gate] where Gate_No like '%" + code + "%' ";
            sql += " and isnull(GPS,'') like '%" + gps + "%' and Status like '%" + status + "%'";
            sql += " and Gate_Color like '%" + gate_color + "%'";
            if (area_gate.Trim() != "")
            {
                sql += " and [Area_Gate] = '" + area_gate + "'";
            }

            sql += " order by Gate_No";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGateGroup()
        {
            string sql = "";

            sql = @"SELECT DISTINCT [Area_Gate] FROM [Tbl_Gate] WHERE ISNULL(Area_Gate,'') <> '-' AND ISNULL(Area_Gate,'') <> '' ";
            sql += " order by Area_Gate";

            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGate_UserGate(string Code, string UserId)
        {
            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(@"SELECT [Id],[Area_Gate],[Gate_No],[Gate_Name] FROM [Tbl_Gate] where Gate_No like '%" + Code + "%' and Id in ( SELECT [Gate_Id] FROM [dbo].[Tbl_User_Gate] where [UserId] =" + UserId + " )  order by Gate_No");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Get_Gate_Supplier_Destina(string Code_)
        {
            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(@"SELECT [Id],[Area_Gate],[Gate_No],[Gate_Name] FROM [Tbl_Gate] where Id Not in ( SELECT [Gate_Id] FROM [dbo].[Tbl_Supplier_Gate_Destination] where [Supplier_Code] =" + Code_ + " )  order by Gate_No");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetROUND(string code)
        {
            Global_Service Supplier = new Global_Service();
            DataTable GetSupplier = Supplier.SQLSelect(@"SELECT * FROM [Tbl_Round] where [Status] ='1' order by Round_Code ");

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetSupplier));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGateTime(string code)
        {
            string sql = "";

            sql = @"SELECT G.Gate_No ,ISNULL(Gate_Color, '#D4D4D4') AS Color,CONVERT(VARCHAR(5), [Time_In], 108) AS Time_In, ";
            sql += " CONVERT(VARCHAR(5), [Time_Out], 108) AS Time_Out,ISNULL([ROUND],'-') as [ROUND], ";
            sql += " ISNULL((select H.Route_Code from Tbl_TRANSPORT H WHERE H.Id = R.Id),'-') as Route_Code , ";
            sql += " ISNULL((select H.Transporter_Code from Tbl_TRANSPORT H WHERE H.Id = R.Id),'-') as Transporter_Code , ";
            sql += " CONVERT(VARCHAR(5), ISNULL((";
            sql += " SELECT MAX(Start_Time) FROM Tbl_Gate_Time RD WHERE RD.Gate_No = G.Gate_No AND[TYPE] = '1'), '00:00'), 108) AS Max_Time_In,";
            sql += "  CONVERT(VARCHAR(5), ISNULL((";
            sql += " SELECT MAX(End_Time) FROM Tbl_Gate_Time RD WHERE RD.Gate_No = G.Gate_No AND[TYPE] = '1'), '17:00'), 108) AS Max_Time_Out";
            sql += " FROM Tbl_Gate G LEFT JOIN [Tbl_TRANSPORT_ROUTE] R ON  G.Gate_No = R.Supplier_Gate_Code and Type = 'G' ";
            sql += " WHERE G.Gate_No = '" + code + "' AND R.Id IN (SELECT RR.Id FROM [Tbl_TRANSPORT] RR WHERE RR.Route_Type NOT IN ('3','2'))";
            sql += " UNION ";
            sql += " SELECT Gate_No ,('#ffff00') AS Color,";
            sql += " CONVERT(VARCHAR(5), Start_Time, 108) AS Time_In,";
            sql += " CONVERT(VARCHAR(5), End_Time, 108) AS Time_Out,";
            sql += " (CASE WHEN [TYPE] = '1' THEN N'เวลาทำงาน' WHEN [TYPE] = 'L' THEN N'พัก(เที่ยง)' ";
            sql += " WHEN [TYPE] = 'S' THEN '5S' WHEN [TYPE] = 'B' THEN 'Break'  WHEN [TYPE] = 'D' THEN N'พัก(เย็น)'";
            sql += " ELSE 'N/A' END) AS [ROUND], (CASE WHEN [TYPE] = '1' THEN N'เวลาทำงาน' WHEN [TYPE] = 'L' THEN N'พัก(เที่ยง)' ";
            sql += " WHEN [TYPE] = 'S' THEN '5S' WHEN [TYPE] = 'B' THEN 'Break'  WHEN [TYPE] = 'D' THEN N'พัก(เย็น)'";
            sql += " ELSE 'N/A' END) AS Route_Code, ";
            sql += "  '-' AS Transporter_Code, ";
            sql += " CONVERT(VARCHAR(5), ( SELECT MAX(Start_Time) ";
            sql += " FROM Tbl_Gate_Time RD ";
            sql += "  WHERE RD.Gate_No = R.Gate_No AND[TYPE] = '1'), 108) AS Max_Time_In, ";
            sql += "  CONVERT(VARCHAR(5), (SELECT MAX(End_Time) FROM Tbl_Gate_Time RD ";

            sql += " WHERE RD.Gate_No = R.Gate_No AND[TYPE] = '1'), 108) AS Max_Time_Out ";
            sql += " FROM Tbl_Gate_Time R ";
            sql += " WHERE [TYPE] <> '1' AND Gate_No ='" + code + "'";
            sql += " ORDER BY Gate_No, [Time_In] ";

            Global_Service GateTime = new Global_Service();
            DataTable GetGateTime = GateTime.SQLSelect(sql);

            Context.Response.ContentType = "application /json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetGateTime));
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGateColor(string Gate_code)
        {
            string sql = "";

            sql = @"SELECT isnull(Max(Gate_Color),'#D4D4D4') as Gate_Color FROM [Tbl_Gate] where [Gate_No] = '" + Gate_code + "'";

            Global_Service GS = new Global_Service();
            DataTable GetGateColor = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetGateColor));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ShowROUTEList(string ID, string Transporter_Code, string Route_Type, string Status, string Gate, string Supplier, string sql_supplier)
        {
            string sql = "";

            if (ID == "")
            {
                ID = "";
            }

            sql = @"select R.[Id],[Transporter_Code],[Route_Code],[Route_Type], ";

            sql += "CONVERT(VARCHAR(5), [Start_Time], 108) as [Start_Time], ";
            sql += "CONVERT(VARCHAR(5), [End_Time], 108) as [End_Time], ";
            sql += "CONVERT(VARCHAR(10), CAST(GETDATE() AS DATE), 103) AS [Start_date],  ";
            sql += "(CASE WHEN [End_Time] >= CONVERT(TIME, '01:00:00 AM') ";
            sql += " AND [End_Time] < CONVERT(TIME, '06:00:00 AM') ";
            sql += " THEN CONVERT(VARCHAR(10), CAST(DATEADD(Day, 1, GETDATE()) AS DATE), 103)  ";
            sql += " ELSE CONVERT(VARCHAR(10), CAST(GETDATE() AS DATE), 103)  ";
            sql += " END) AS End_date  ";

            sql += " ,[Vehicle_Id],[Fix_Cost],[Fuel_AGV_Price],[Fuel_KM_Liter]";
            sql += " ,[Maintenance_Cost],[Distance],[Express_Way],R.[Shift],[OT],[Other]";
            sql += " ,[Utilize],Convert(Varchar,[Effective_Date],103) as Effective_Date ,R.[Status],[Line_Id],[Type],[Supplier_Gate_Code]";

            sql += " ,(CASE WHEN [Time_In] >= CONVERT(TIME, '01:00:00 AM')";
            sql += "  AND [Time_In] < CONVERT(TIME, '06:00:00 AM')";
            sql += "  THEN CONVERT(VARCHAR(10), CAST(DATEADD(Day, 1, GETDATE()) AS DATE), 103)";
            sql += "  ELSE CONVERT(VARCHAR(10), CAST(GETDATE() AS DATE), 103)";
            sql += "  END) AS [Start_In], ";
            sql += "  (CASE WHEN Time_Out >= CONVERT(TIME, '01:00:00 AM')";
            sql += "   AND Time_Out<CONVERT(TIME, '06:00:00 AM')";
            sql += "  THEN CONVERT(VARCHAR(10), CAST(DATEADD(Day, 1, GETDATE()) AS DATE), 103)";
            sql += "  ELSE CONVERT(VARCHAR(10), CAST(GETDATE() AS DATE), 103)";
            sql += "  END) AS End_Out";

            sql += " ,CONVERT(VARCHAR(5),[Time_In], 108) as Time_In ,CONVERT(VARCHAR(5), Time_Out, 108) as Time_Out,[GPS],[ROUND]";
            sql += " , isnull((select Short_Name from Tbl_Supplier where Supplier_Code = [Supplier_Gate_Code]),'-') as Supplier_name ";
            sql += " , (SELECT ISNULL(MAX(Gate_Color), '#D4D4D4') AS Gate_Color FROM [Tbl_Gate]  WHERE[Gate_No] = [Supplier_Gate_Code]) AS Color ";
            sql += " ,convert(varchar(5),Max(Time_In) OVER (PARTITION BY R.Id),108)  as MaxTimeIn ,(CASE WHEN ISNULL(RL.Shift,'1') ='1' THEN 'DAY'  ELSE  'NIGHT' END) as L_Shift ";
            sql += " FROM [Tbl_TRANSPORT] R left join [Tbl_TRANSPORT_ROUTE] RL ON R.Id = RL.Id";

            sql += " WHERE R.Transporter_Code " + Transporter_Code + " and CONVERT(VARCHAR,R.[Id]) " + ID;
            sql += " and [Route_Type] " + Route_Type + " and R.Status " + Status;
            sql += " AND R.id in ( select Id from [Tbl_TRANSPORT_ROUTE] S1 where S1.[Supplier_Gate_Code] " + Supplier + " ) ";
            sql += " AND R.id in (select Id from[Tbl_TRANSPORT_ROUTE] G1 where G1.[Supplier_Gate_Code] " + Gate + " )";
            sql += " AND R.Transporter_Code IN (" + sql_supplier + ")";

            sql += " ORDER BY [Route_Code],[Transporter_Code],R.[Id],ISNULL(RL.Shift, '1'),[Time_In]";


            Global_Service GS = new Global_Service();
            DataTable GetROUTEData = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetROUTEData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetUserGate(string Id_)
        {
            string sql = "";

            sql = @"SELECT UG.[UserId],[Gate_Id],ISNULL(G.Gate_No,'') as Gate_No FROM [Tbl_User_Gate] UG ";
            sql += " left join Tbl_Gate G on UG.Gate_Id = G.Id";
            sql += " WHERE UG.UserId = " + Id_ + " order by G.Gate_No";

            Global_Service GS = new Global_Service();
            DataTable GateBreakTime = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GateBreakTime));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGateDestination(string Supplier_Code)
        {
            string sql = "";

            sql = @"SELECT [Supplier_Code],[Gate_Id],[Gate_No] FROM [Tbl_Supplier_Gate_Destination] left join [Tbl_Gate] ON Id = Gate_Id where Supplier_Code ='" + Supplier_Code + "' order by [Gate_No] ";

            Global_Service GS = new Global_Service();
            DataTable GateBreakTime = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GateBreakTime));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetGateBreakTime(string Gate_)
        {
            string sql = "";

            sql = @"SELECT [Id],[Gate_No],[TYPE],CONVERT(VARCHAR(5),[Start_Time], 108) as [Start_Time],";
            sql += " CONVERT(VARCHAR(5),[End_Time], 108) as [End_Time],[Status],[Create_Date]";
            sql += " ,(CASE WHEN [TYPE] = '1' THEN N'เวลาทำงาน' WHEN [TYPE] = 'L' THEN N'พัก(เที่ยง)' ";
            sql += " WHEN [TYPE] = 'S' THEN '5S' WHEN [TYPE] = 'B' THEN 'Break'  WHEN [TYPE] = 'D' THEN N'พัก(เย็น)'";
            sql += " ELSE 'N/A' END) as TYPE_TXT";
            sql += " FROM [Tbl_Gate_Time] Where [Gate_No] = '" + Gate_ + "' order by [Start_Time]";

            Global_Service GS = new Global_Service();
            DataTable GateBreakTime = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GateBreakTime));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetROUTE(string ID, string Transporter_Code)
        {
            string sql = "";

            if (ID == "")
            {
                ID = "0";
            }

            sql = @"select R.[Id],[Transporter_Code],[Route_Code],[Route_Type],CONVERT(VARCHAR(5),[Start_Time], 108) as Start_Time";
            sql += " ,CONVERT(VARCHAR(5),[End_Time], 108) as End_Time,[Vehicle_Id],[Fix_Cost],[Fuel_AGV_Price],[Fuel_KM_Liter]";
            sql += " ,[Maintenance_Cost],[Distance],[Express_Way],R.[Shift],[OT],[Other]";
            sql += " ,[Utilize],Convert(Varchar,[Effective_Date],103) as Effective_Date ,R.[Status],[Line_Id],[Type],[Supplier_Gate_Code]";
            sql += " ,CONVERT(VARCHAR(5),[Time_In], 108) as Time_In ,CONVERT(VARCHAR(5), Time_Out, 108) as Time_Out,(CASE WHEN isnull([ROUND],'') = '' AND [Type] ='G' THEN 'UNLOAD PART' ELSE isnull([ROUND],'') END) as[ROUND]";
            sql += " , isnull((select Short_Name from Tbl_Supplier where Supplier_Code = [Supplier_Gate_Code]),'-') as Supplier_name ";
            sql += " , ISNULL((SELECT ISNULL([GPS],'') FROM [Tbl_Supplier_GPS] G where G.[Supplier_Code] = RL.Supplier_Gate_Code),'') as GPS_S ";
            sql += " , ISNULL((SELECT ISNULL([GPS],'') FROM [Tbl_Gate] GG where GG.[Gate_No] = RL.Supplier_Gate_Code),'') as GPS_G ";
            sql += " ,(CASE WHEN ISNULL(RL.Shift,'1') ='1' THEN 'DAY'  ELSE  'NIGHT' END) as L_Shift,ISNULL(RL.Shift,'1') as Shift_Code ,Convert(Varchar,ISNULL([Effective_End_Date],Getdate()),103) as Effective_End_Date";
            sql += " FROM [Tbl_TRANSPORT] R left join [Tbl_TRANSPORT_ROUTE] RL ON R.Id = RL.Id";
            sql += " WHERE R.Transporter_Code = '" + Transporter_Code + "' and R.[Id] =" + ID + " ORDER BY R.[Id],[Transporter_Code],[Route_Code],[Type],ISNULL(RL.Shift,'1'),[Time_In]";

            Global_Service GS = new Global_Service();
            DataTable GetROUTEData = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetROUTEData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getOilPrice(string YYYY, string MM)
        {
            string sql = "";

            sql = @" EXEC [dbo].[Run_OilPrice_Show] '" + YYYY + "', '" + MM + "' ";

            Global_Service GS = new Global_Service();
            DataTable GetOilPriceData = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetOilPriceData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getCalendarMonth(string yyyy, string mm)
        {

            string Sql = " SELECT C.[SCHEDULED_LINE_CODE],(CASE WHEN C.[SCHEDULED_LINE_CODE] = 300000 THEN 'SH95' ";
            Sql += " WHEN C.[SCHEDULED_LINE_CODE] = 400000 THEN 'L-TRACTOR'";
            Sql += " WHEN C.[SCHEDULED_LINE_CODE] = 500000 THEN 'COMBINE'";
            Sql += " WHEN C.[SCHEDULED_LINE_CODE] = 800000 THEN 'B-TRACTOR'";
            Sql += " WHEN C.[SCHEDULED_LINE_CODE] = 700000 THEN 'ROTARY'";
            Sql += " END) as PRODUCT ,";
            Sql += " substring([ACTUAL_DATE], 1, 4) +'-'+substring([ACTUAL_DATE], 5, 2)+'-'+substring([ACTUAL_DATE], 7, 2) as [ACTUAL_DATE], ";
            Sql += "  isnull([ACTUAL_SIGN],0) as [ACTUAL_SIGN],";
            Sql += " (CASE WHEN ISNULL(H.[WORK_TIME_BAND_CODE], '') = '0002'THEN 'OT' ELSE '' END ) as HDay";
            Sql += " FROM [Tbl_Calendar] C ";
            Sql += " LEFT JOIN Tbl_Calendar_Holiday AS H ON LTRIM(RTRIM(C.[SCHEDULED_LINE_CODE])) = LTRIM(RTRIM(H.[SCHEDULED_LINE_CODE])) ";
            Sql += " AND LTRIM(RTRIM(C.[ACTUAL_DATE])) = REPLACE(LTRIM(RTRIM(H.[WORK_DATE])), '-', '') ";
            Sql += " AND ISNULL(H.[WORK_TIME_BAND_CODE], '-') = '0002'";
            Sql += "  WHERE [ACTUAL_DATE] LIKE '" + yyyy + mm + "%' AND C.[SCHEDULED_LINE_CODE] IN  (400000,500000,800000,700000) ";

            Global_Service GS = new Global_Service();
            DataTable GetOilPriceData = GS.SQLSelect(Sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetOilPriceData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Run_OnTimeRatio_Report(string YYYY, string TYPE_, string Report_, string Transporter_Code)
        {
            string sql = "";
            sql = "EXEC	[dbo].[Run_OnTimeRatio_Report_v2] '" + YYYY + "','" + Report_ + "','" + TYPE_ + "','" + Transporter_Code + "'";

            Global_Service GS = new Global_Service();
            DataTable getCost_Report = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(getCost_Report));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Cost_Report(string YYYY, string TYPE_WORK, string Transporter_Code)
        {
            string sql = "";
            sql = "EXEC	[dbo].[Run_Cost_Report_v2] '" + YYYY + "','" + TYPE_WORK + "','" + Transporter_Code + "'";

            Global_Service GS = new Global_Service();
            DataTable getCost_Report = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(getCost_Report));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getCalendar(string yyyy, string mm)
        {
            int year = 2021;
            int month = 1;

            year = Convert.ToInt32(yyyy);

            month = Convert.ToInt32(mm);

            IEnumerable<int> daysInMonths = Enumerable.Range(1, DateTime.DaysInMonth(year, month));

            int days = 31;

            string Sql = "";

            string SqlPIVOT = "";

            string SqlSELECT = "";

            string dayCounter_dd = "";

            for (int dayCounter = 1; dayCounter <= days; dayCounter++)
            {
                dayCounter_dd = "0" + dayCounter;

                dayCounter_dd = dayCounter_dd.Substring(dayCounter_dd.Length - 2, 2);

                if (dayCounter == 1)
                {
                    SqlSELECT = " isnull([" + dayCounter_dd + "],'-') as [D" + dayCounter_dd + "] ";
                    SqlPIVOT = " [" + dayCounter_dd + "]";
                }
                else
                {
                    SqlSELECT += " ,isnull([" + dayCounter_dd + "],'-') as [D" + dayCounter_dd + "] ";
                    SqlPIVOT += " ,[" + dayCounter_dd + "]";
                }
            }

            Sql = " SELECT SCHEDULED_LINE_CODE , (CASE WHEN [SCHEDULED_LINE_CODE] = 300000 THEN 'SH95' ";
            Sql += " WHEN [SCHEDULED_LINE_CODE] = 400000 THEN 'L-TRACTOR'";
            Sql += " WHEN [SCHEDULED_LINE_CODE] = 500000 THEN 'COMBINE'";
            Sql += " WHEN [SCHEDULED_LINE_CODE] = 800000 THEN 'B-TRACTOR'";
            Sql += " WHEN [SCHEDULED_LINE_CODE] = 700000 THEN 'ROTARY'";
            Sql += " END) as PRODUCT ";
            Sql += " ,(CASE WHEN OT = '0002' THEN '(OT)' ELSE '' END) as OT";
            Sql += "," + SqlSELECT;
            Sql += " FROM ( SELECT C.[SCHEDULED_LINE_CODE], ";
            Sql += "  substring([ACTUAL_DATE], 7, 2) as [ACTUAL_DATE], ";
            Sql += "  isnull([ACTUAL_SIGN]+'#'+C.UPDATE_FLAG+'#'+(CASE WHEN ISNULL(H.[WORK_TIME_BAND_CODE], '') = '0002'THEN 'OT' ELSE '' END ),'0#0#0') as [ACTUAL_SIGN]";
            Sql += " ,(SELECT MAX(WORK_TIME_BAND_CODE) from Tbl_Calendar_Holiday AS HH WHERE LTRIM(RTRIM(C.[SCHEDULED_LINE_CODE])) = LTRIM(RTRIM(HH.[SCHEDULED_LINE_CODE])) ";
            Sql += " AND REPLACE(LTRIM(RTRIM(HH.[WORK_DATE])), '-', '') LIKE '" + yyyy + mm + "%' ";
            Sql += " AND ISNULL(HH.[WORK_TIME_BAND_CODE], '-') = '0002') as OT";
            Sql += " FROM [Tbl_Calendar] C ";
            Sql += " LEFT JOIN Tbl_Calendar_Holiday AS H ON LTRIM(RTRIM(C.[SCHEDULED_LINE_CODE])) = LTRIM(RTRIM(H.[SCHEDULED_LINE_CODE])) ";
            Sql += " AND LTRIM(RTRIM(C.[ACTUAL_DATE])) = REPLACE(LTRIM(RTRIM(H.[WORK_DATE])), '-', '') ";
            Sql += " AND ISNULL(H.[WORK_TIME_BAND_CODE], '-') = '0002'";
            Sql += "  WHERE [ACTUAL_DATE] LIKE '" + yyyy + mm + "%'  AND C.[SCHEDULED_LINE_CODE] IN  (400000,500000,800000,700000) ";
            Sql += " ) AS src PIVOT(MAX([ACTUAL_SIGN]) FOR [ACTUAL_DATE] IN(" + SqlPIVOT + ")) AS pvt";

            Global_Service GS = new Global_Service();
            DataTable GetOilPriceData = GS.SQLSelect(Sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(GetOilPriceData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getCurrentOilPrice(string YYYY, string MM)
        {
            string sql = "";
            sql = @"EXEC Run_OilPrice_Avg_Show '" + YYYY + "','" + MM + "'";

            Global_Service GS = new Global_Service();
            DataTable getCurrentOilPriceData = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(getCurrentOilPriceData));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void ShowDataMonitoring(string Supplier_Gate, string Type, string Transporter_Code, string Time, string Showdate = "", string Gate_Group = "")
        {
            string sql = "";
            sql = "EXEC	[dbo].[Create_Truck_Monitoring_Show] '" + Supplier_Gate + "','" + Type + "','" + Transporter_Code + "','" + Time + "','" + Showdate + "','" + Gate_Group + "'";

            Global_Service GS = new Global_Service();
            DataTable getDataMonitoring = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(getDataMonitoring));
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetID()
        {
            string JsonMsg = "";
            try
            {
                Global_Service GS = new Global_Service();
                DataTable ResultGetID = GS.SQLSelect(" exec [dbo].[GetID]  ");

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { IdNumber = ResultGetID.Rows[0]["IdNumber"] } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { IdNumber = 0 } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void getCurrentOilPriceYear(string YYYY)
        {

            string sql = " EXEC Run_OilPrice_Show12Months '" + YYYY + "'";

            Global_Service GS = new Global_Service();
            DataTable getCurrentOilPriceDataYear = GS.SQLSelect(sql);

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(this.ConvertDataTabletoString(getCurrentOilPriceDataYear));

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDriver(string VehicleID, string Supplier, string Driver, string TelNumber, string Type, string SIM, string BlackBox, string Status_Truck)
        {
            string JsonMsg = "";
            string ID = "";
            string sql = "";

            using (var conn = new SqlConnection(Global.db_Con))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT NEXT VALUE FOR SequenceVehicle ;";
                cmd.CommandType = CommandType.Text;

                var result = cmd.ExecuteScalar();

                ID = Convert.ToString(result);
                conn.Close();
            };

            if (ID != "")
            {

                sql = " INSERT INTO [dbo].[Tbl_Vehicle_Registration] ([ID],[Vehicle_Registration_No],[Vehicle_Supplier_No],[Driver_Name],[QR_Code_Encrypt],Tel_Number,Vehicle_Type,GPS_SIM,GPS_BlackBox,Type,Driver_ID) ";
                sql += "  VALUES (" + ID + ",N'" + VehicleID.Trim() + "',N'" + Supplier.Trim() + "',N''";
                sql += " ,CONVERT(VARCHAR," + ID + ")+N'#" + GeneralClass.Base64Encode(ID) + "'";
                sql += " ,'" + TelNumber.Trim() + "',N'" + Type.Trim() + "','" + SIM.Trim() + "','" + BlackBox.Trim() + "','" + Status_Truck + "'," + Driver.Trim() + ")";

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Insert completed !", ID_RE = ID } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!", ID_RE = "" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Insert not completed !", ID_RE = "" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDriverMaster(string VehicleID, string Supplier, string Driver, string TelNumber, string Type, string SIM, string BlackBox, string Status_, string Vehicle_STATUS)
        {
            string JsonMsg = "";
            string ID = "";
            string sql = "";

            using (var conn = new SqlConnection(Global.db_Con))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT NEXT VALUE FOR SequenceVehicle ;";
                cmd.CommandType = CommandType.Text;

                var result = cmd.ExecuteScalar();

                ID = Convert.ToString(result);
                conn.Close();
            };

            if (ID != "")
            {

                sql = " INSERT INTO [dbo].[Tbl_Vehicle_Registration] ([ID],[Vehicle_Registration_No],[Vehicle_Supplier_No],[Driver_Name],[QR_Code_Encrypt],Tel_Number,Vehicle_Type,GPS_SIM,GPS_BlackBox,Status,Type,Driver_ID) ";
                sql += "  VALUES (" + ID + ",N'" + VehicleID.Trim() + "',N'" + Supplier.Trim() + "',N''";
                sql += " ,CONVERT(VARCHAR," + ID + ")+N'#" + GeneralClass.Base64Encode(ID) + "'";
                sql += " ,'" + TelNumber.Trim() + "',N'" + Type.Trim() + "','" + SIM.Trim() + "','" + BlackBox.Trim() + "','" + Status_ + "','" + Vehicle_STATUS + "'," + Driver + ")";

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Insert completed !", ID_RE = ID } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!", ID_RE = "" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Insert not completed !", ID_RE = "" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertDrivers(string Supplier, string Driver, string TelNumber)
        {
            string JsonMsg = "";
            string ID = "";
            string sql = "";

            using (var conn = new SqlConnection(Global.db_Con))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT NEXT VALUE FOR SequenceDriver ;";
                cmd.CommandType = CommandType.Text;

                var result = cmd.ExecuteScalar();

                ID = Convert.ToString(result);
                conn.Close();
            };

            if (ID != "")
            {

                sql = " INSERT INTO [dbo].[Tbl_Driver] ([ID],[Supplier_Code],[Name],[Tel_Number]) ";
                sql += "  VALUES (" + ID + ",N'" + Supplier.Trim() + "',N'" + Driver.Trim() + "',N'" + TelNumber.Trim() + "')";

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Insert completed !", ID_RE = ID } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!", ID_RE = "" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Insert not completed !", ID_RE = "" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void DeleteDriver(string ID)
        {
            string JsonMsg = "";
            string sql = "";

            if (ID != "")
            {

                sql = " DELETE FROM [dbo].[Tbl_Vehicle_Registration] ";
                sql += " WHERE [ID] = " + ID;

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Delete completed !" } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Delete not completed !" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertUpdateNote(string ID, string Note)
        {
            string JsonMsg = "";
            string sql = "";

            if (ID != "")
            {

                sql = " DELETE FROM [dbo].[Tbl_ROUTE_NOTE] ";
                sql += " WHERE [ID] = " + ID + " AND CAST(Running_Date as DATE) = CAST(GETDATE() as DATE)";

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    if (ID != "")
                    {

                        sql = " INSERT INTO [dbo].[Tbl_ROUTE_NOTE]([ID],[Running_Date],[Note]) VALUES (" + ID + ",CAST(GETDATE() as DATE),'" + Note + "') ";

                        try
                        {
                            Global_Service GSINS = new Global_Service();

                            GSINS.SQLExecute(sql);

                            JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Update Note completed !" } });
                        }
                        catch (Exception ex)
                        {
                            JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
                        }
                    }
                    else
                    {
                        JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Update Note not completed !" } });
                    }
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Delete Note not completed !" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateDriver(string ID, string VehicleID, string Supplier, string Driver, string TelNumber, string Type, string SIM, string BlackBox, string Status_Truck)
        {
            string JsonMsg = "";
            string sql = "";

            if (ID != "")
            {

                sql = " UPDATE [dbo].[Tbl_Vehicle_Registration] SET [Vehicle_Registration_No] = N'" + VehicleID.Trim() + "',[Vehicle_Supplier_No] =N'" + Supplier.Trim() + "' ";
                sql += ",[Driver_Name] =N'',[QR_Code_Encrypt]=CONVERT(VARCHAR," + ID + ")+N'#" + GeneralClass.Base64Encode(ID) + "' ";
                sql += ",Tel_Number='" + TelNumber.Trim() + "',Vehicle_Type=N'" + Type.Trim() + "',GPS_SIM='" + SIM.Trim() + "',GPS_BlackBox='" + BlackBox.Trim() + "'";
                sql += ",Type='" + Status_Truck + "', Driver_ID=" + Driver + "  WHERE [ID] = " + ID;

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Update completed !" } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Update not completed !" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateDriverMaster(string ID, string VehicleID, string Supplier, string Driver, string TelNumber, string Type, string SIM, string BlackBox, string Status_, string Vehicle_STATUS)
        {
            string JsonMsg = "";
            string sql = "";

            if (ID != "")
            {

                sql = " UPDATE [dbo].[Tbl_Vehicle_Registration] SET [Vehicle_Registration_No] = N'" + VehicleID.Trim() + "',[Vehicle_Supplier_No] =N'" + Supplier.Trim() + "' ";
                sql += ",[Driver_Name] =N'',[QR_Code_Encrypt]=CONVERT(VARCHAR," + ID + ")+N'#" + GeneralClass.Base64Encode(ID) + "' ";
                sql += ",Tel_Number='" + TelNumber.Trim() + "',Vehicle_Type=N'" + Type.Trim() + "',GPS_SIM='" + SIM.Trim() + "',GPS_BlackBox='" + BlackBox.Trim() + "',Status = '" + Status_ + "'";
                sql += ",Type = '" + Vehicle_STATUS + "' , Driver_ID=" + Driver + " WHERE [ID] = " + ID;

                try
                {
                    Global_Service GS = new Global_Service();

                    GS.SQLExecute(sql);

                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Update completed !" } });
                }
                catch (Exception ex)
                {
                    JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
                }
            }
            else
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = "Update not completed !" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertROUTE(string SQL)
        {
            string JsonMsg = "";

            string sql = "";
            sql += "INSERT INTO [dbo].[Tbl_TRANSPORT]";
            sql += "(Id,[Transporter_Code]";
            sql += ",[Route_Code]";
            sql += ",[Route_Type]";
            sql += ",[Start_Time]";
            sql += ",[End_Time]";
            sql += ",[Vehicle_Id]";
            sql += ",[Fix_Cost]";
            sql += ",[Fuel_AGV_Price]";
            sql += ",[Fuel_KM_Liter]";
            sql += ",[Maintenance_Cost]";
            sql += ",[Distance]";
            sql += ",[Express_Way]";
            sql += ",[Shift]";
            sql += ",[OT]";
            sql += ",[Other]";
            sql += ",[Utilize]";
            sql += ",[Effective_Date]";
            sql += ",[Create_By]";
            sql += ",[Update_By]";
            sql += ",[Status],[Effective_End_Date])";
            sql += " VALUES (" + SQL + ")";

            try
            {
                Global_Service GS = new Global_Service();

                GS.SQLExecute(sql);

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Insert completed !" } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateROUTE(string SQL)
        {
            string JsonMsg = "";

            string sql = SQL;

            try
            {
                Global_Service GS = new Global_Service();

                GS.SQLExecute(sql);

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Update completed !" } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void InsertROUTE_Line(string SQL)
        {
            string JsonMsg = "";

            string sql = "";

            sql += "INSERT INTO[dbo].[Tbl_TRANSPORT_ROUTE]([Id],[Supplier_Gate_Code],[Type],[Time_In],[Time_Out],[GPS],[ROUND],[Create_By],[Shift])";
            sql += " VALUES (" + SQL + ") ";

            try
            {
                Global_Service GS = new Global_Service();

                GS.SQLExecute(sql);

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Insert completed !" } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Update_Table_Data(string SQL)
        {
            string JsonMsg = "";

            string sql = "";

            sql = SQL;

            try
            {
                Global_Service GS = new Global_Service();

                GS.SQLExecute(sql);

                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "Y", Message = "Update data completed !" } });
            }
            catch (Exception ex)
            {
                JsonMsg = new JavaScriptSerializer().Serialize(new Object[] { new { Status = "N", Message = ex.ToString() + "!" } });
            }

            Context.Response.ContentType = "application/json; charset=utf-8";
            Context.Response.Write(JsonMsg);

        }
    }
}
