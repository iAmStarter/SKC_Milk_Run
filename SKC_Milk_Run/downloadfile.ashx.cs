using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using System.Drawing;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Drawing.Chart;
using SKC_Milk_Run.App_Data;

namespace SKC_Milk_Run
{
    /// <summary>
    /// Summary description for downloadfile
    /// </summary>
    public class downloadfile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {    
            string Expoprt_Name = context.Request.QueryString["Expoprt_Name"];

            if (Expoprt_Name == "Create_Truck_Monitoring_Show")
            {   
                string Supplier_Gate_ = context.Request.QueryString["Supplier_Gate"];
                string Type_ = context.Request.QueryString["Type"];
                string Transporter_Code_ = context.Request.QueryString["Transporter_Code"];
                string Time_ = context.Request.QueryString["Time"];
                string Showdate_ = context.Request.QueryString["Showdate"];

                string Mode_ = context.Request.QueryString["Mode"];
                string Fromdate_ = context.Request.QueryString["Fromdate"];
                string Todate_ = context.Request.QueryString["Todate"];

                Create_Truck_Monitoring_Show_Export(context, Supplier_Gate_, Type_, Transporter_Code_, Time_, Showdate_, Mode_, Fromdate_, Todate_);
            }

            if (Expoprt_Name == "OilPrice")
            {               
                string YYYY_ = context.Request.QueryString["YYYY"];
                string MM_ = context.Request.QueryString["MM"];
                string exportMM_ = context.Request.QueryString["exportMM"];

                OilPrice_Export(context, YYYY_, MM_, exportMM_);
            }

            if (Expoprt_Name == "Diagram")
            {
                Diagram_Export(context);
            }
        }

        public void Diagram_Export(HttpContext context)
        {           

            string Id_ = context.Request.QueryString["Id"];
            string Transport_ = context.Request.QueryString["Transport"];
            string Type_ = context.Request.QueryString["Type"];
            string Status_ = context.Request.QueryString["Status"];
            string Gate_ = context.Request.QueryString["Gate"];
            string Supplier_ = context.Request.QueryString["Supplier"];
            string sql_ = context.Request.QueryString["sql"];

            string ExcelName = "ExportDiagram_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

            string sql = " SELECT[Transporter_Code] AS[Transporter], [Route_Code] AS[Route Code], ";
            sql += " (CASE WHEN[Route_Type] = '1' THEN 'K-Epress' WHEN[Route_Type] = '2' THEN 'EXTRA' ";
            sql += " WHEN [Route_Type] = '3' THEN 'URGENT' WHEN [Route_Type] = '4' THEN 'SHORT TRUCK' END) AS[Route_Type], ";
            sql += " CONVERT(VARCHAR(10), Effective_Date, 103) AS[Effective Date], ";
            sql += " ISNULL(Effective_End_Date, [Effective_Date]) AS[Effective End Date],  ";
            sql += " CONVERT(VARCHAR(5), [Start_Time], 108) AS[Start Time], ";
            sql += " CONVERT(VARCHAR(5), [End_Time], 108) AS[End Time],  ";
            sql += "  [Fix_Cost] AS [FIX COST], (SELECT ROUND(avg([PRICE]),2) FROM [Tbl_OilPrice] AS P_B7 WHERE [PRODUCT] = 'Diesel B7' ";
            sql += "   AND CAST([PRICE_DATE] AS DATE) >= DATEADD(MONTH, -1, CONVERT(DATE, '21/' + substring(convert(varchar, getdate(), 103), 4, 7), 103)) ";
            sql += "  AND CAST([PRICE_DATE] AS DATE) <= CONVERT(DATE, '20/' + substring(convert(varchar, getdate(), 103), 4, 7), 103)) AS [FUEL AGV. PRICE],  ";
            sql += "  [Fuel_KM_Liter] AS[FUEL KM / LITER],[Maintenance_Cost] AS[MAINTENANCE COST],[Distance] as DISTANCE,  ";
            sql += "  (CASE WHEN Fuel_KM_Liter <= 0 THEN 0 ELSE((((DISTANCE) / (Fuel_KM_Liter)) * (";
            sql += " (SELECT ROUND(avg([PRICE]),2) FROM[Tbl_OilPrice] AS P_B7 WHERE[PRODUCT] = 'Diesel B7' ";
            sql += "   AND CAST([PRICE_DATE] AS DATE) >= DATEADD(MONTH, -1, CONVERT(DATE, '21/' + substring(convert(varchar, getdate(), 103), 4, 7), 103)) ";
            sql += "  AND CAST([PRICE_DATE] AS DATE) <= CONVERT(DATE, '20/' + substring(convert(varchar, getdate(), 103), 4, 7), 103))";
            sql += " )) + (([Distance]) * (MAINTENANCE_COST))) END) AS[FUEL + MAINTENANCE], ";
            sql += "  [Express_Way] AS[EXPRESS WAY],R.SHIFT,[OT], [Other] as OTHER,  ";
            sql += "  (FIX_COST + (CASE WHEN Fuel_KM_Liter <= 0 THEN 0 ELSE((((DISTANCE) / (Fuel_KM_Liter)) * (";
            sql += " (SELECT ROUND(avg([PRICE]),2) FROM[Tbl_OilPrice] AS P_B7 WHERE[PRODUCT] = 'Diesel B7' ";
            sql += "   AND CAST([PRICE_DATE] AS DATE) >= DATEADD(MONTH, -1, CONVERT(DATE, '21/' + substring(convert(varchar, getdate(), 103), 4, 7), 103)) ";
            sql += "  AND CAST([PRICE_DATE] AS DATE) <= CONVERT(DATE, '20/' + substring(convert(varchar, getdate(), 103), 4, 7), 103))";
            sql += ")) + (([Distance]) * (MAINTENANCE_COST))) END) +EXPRESS_WAY + R.SHIFT + OT + OTHER) AS[COST BAHT / DAY],  ";
            sql += "  [Utilize] as [UTILIZE(%)], ";
            sql += " (CASE WHEN R.[Status] = '1' ";
            sql += " THEN 'EFFECTIVE' ELSE 'DRAFT' END) AS[Status], ";
            sql += " ISNULL(Vehicle_Registration_No, '-') AS LICENCE, ";
            sql += " ISNULL(GPS_SIM, '-') AS[GPS SIM],  ";
            sql += " ISNULL(GPS_BlackBox, '-') AS[GPS BlackBox],  ";
            sql += " [Name] AS[Driver Name], D.[Tel_Number] AS[Tel Number], ";
            sql += " (CASE WHEN RL.[Type] = 'S' THEN 'SUPPLIER' ELSE 'GATE' END) as [Type] ,  ";
            sql += " [Supplier_Gate_Code] as [Supplier / Gate Code],  ";
            sql += " CONVERT(VARCHAR(5), [Time_In], 108) AS Time_In, ";
            sql += " CONVERT(VARCHAR(5), Time_Out, 108) AS Time_Out,[ROUND], ";
            sql += " ISNULL(( SELECT Short_Name ";
            sql += " FROM Tbl_Supplier ";
            sql += " WHERE Supplier_Code = [Supplier_Gate_Code] ";
            sql += " ), '-') AS Supplier_name, ";
            sql += " (CASE WHEN ISNULL(RL.Shift, '1') = '1' ";
            sql += " THEN 'DAY' ";
            sql += " ELSE 'NIGHT' ";
            sql += " END) AS[Shift], ";
            sql += " ISNULL((CASE WHEN RL.[Type] = 'S' THEN ISNULL( ";
            sql += " (SELECT ISNULL([GPS], '') ";
            sql += "  FROM[Tbl_Supplier_GPS] G ";
            sql += "  WHERE G.[Supplier_Code] = RL.Supplier_Gate_Code ";
            sql += " ), '') ";
            sql += "  ELSE ISNULL((SELECT GPS ";
            sql += " FROM Tbl_Gate ";
            sql += " WHERE Gate_No = RL.[Supplier_Gate_Code] ";
            sql += " ), '') END), '') AS[GPS] ";
            sql += " FROM[Tbl_TRANSPORT] R ";
            sql += "  LEFT JOIN[Tbl_TRANSPORT_ROUTE] RL ON R.Id = RL.Id ";
            sql += "    LEFT JOIN Tbl_Vehicle_Registration V ON V.[ID] = R.Vehicle_Id ";
            sql += "    LEFT JOIN Tbl_Driver D ON D.ID = V.Driver_ID ";            

            sql += " WHERE R.Transporter_Code " + Transport_ + " and CONVERT(VARCHAR,R.[Id]) " + Id_;
            sql += " and [Route_Type] " + Type_ + " and R.Status " + Status_ ;
            sql += " AND R.id in ( select Id from [Tbl_TRANSPORT_ROUTE] S1 where S1.[Supplier_Gate_Code] " + Supplier_ + " ) ";
            sql += " AND R.id in (select Id from[Tbl_TRANSPORT_ROUTE] G1 where G1.[Supplier_Gate_Code] " + Gate_ + " )";
            sql += " AND R.Transporter_Code IN (" + sql_ + ")";

            sql += "  ORDER BY[Transporter_Code],[Route_Code],ISNULL(RL.Shift, '1'),RL.[Time_In] ";

            Global_Service GS = new Global_Service();
            DataTable getDataMonitoring = GS.SQLSelect(sql);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create the worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("dataSheet");

                //load the datatable into the sheet, with headers
                worksheet.Cells["A1"].LoadFromDataTable(getDataMonitoring, true);

                //send the file to the browser
                byte[] bin = excelPackage.GetAsByteArray();
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("content-length", bin.Length.ToString());
                context.Response.AddHeader("content-disposition", "attachment; filename=\"" + ExcelName + ".xlsx\"");
                context.Response.OutputStream.Write(bin, 0, bin.Length);
                context.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public void OilPrice_Export(HttpContext context, string YYYY, string MM, string exportMM)
        {

            string ExcelName = "ExportOilPrice_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

            string sql = "";

            sql = "EXEC	[dbo].[Run_OilPrice_Show_EXPORT] '" + YYYY + "','" + MM + "','" + exportMM +"'";


            Global_Service GS = new Global_Service();
            DataTable getDataMonitoring = GS.SQLSelect(sql);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create the worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("dataSheet");

                //load the datatable into the sheet, with headers
                worksheet.Cells["A1"].LoadFromDataTable(getDataMonitoring, true);

                //send the file to the browser
                byte[] bin = excelPackage.GetAsByteArray();
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("content-length", bin.Length.ToString());
                context.Response.AddHeader("content-disposition", "attachment; filename=\"" + ExcelName + ".xlsx\"");
                context.Response.OutputStream.Write(bin, 0, bin.Length);
                context.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public void Create_Truck_Monitoring_Show_Export(HttpContext context, string Supplier_Gate_, string Type_, string Transporter_Code_,
            string Time_, string Showdate_, string Mode_, string Fromdate_, string Todate_)
        {

            string ExcelName = "ExportMonitoring_" + DateTime.Now.ToString("yyyyMMdd_HHmm");

            string sql = "";

            sql = "EXEC	[dbo].[Create_Truck_Monitoring_Show_Export] '" + Supplier_Gate_ + "','" + Type_ + "','" + Transporter_Code_ + "','" + Time_ + "','" + Showdate_ + "','" + Mode_ + "','" + Fromdate_ + "','" + Todate_ + "'";


            Global_Service GS = new Global_Service();
            DataTable getDataMonitoring = GS.SQLSelect(sql);

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                //create the worksheet
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("dataSheet");

                //load the datatable into the sheet, with headers
                worksheet.Cells["A1"].LoadFromDataTable(getDataMonitoring, true);

                //send the file to the browser
                byte[] bin = excelPackage.GetAsByteArray();
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                context.Response.AddHeader("content-length", bin.Length.ToString());
                context.Response.AddHeader("content-disposition", "attachment; filename=\"" + ExcelName + ".xlsx\"");
                context.Response.OutputStream.Write(bin, 0, bin.Length);
                context.Response.Flush();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}