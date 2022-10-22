using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace SKC_Milk_Run
{
    /// <summary>
    /// Summary description for HandlerImg
    /// </summary>
    public class HandlerImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SqlConnection con = new SqlConnection(Global.db_Con);
            con.Open();
            SqlCommand cmd = new SqlCommand("select Driver_Image from Tbl_Driver where ID =" + context.Request.QueryString["Id"] , con);
            DataTable dt = new DataTable();
            SqlDataAdapter sdr = new SqlDataAdapter(cmd);
            sdr.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                if (dt.Rows[i][0].ToString() == "")
                {

                    context.Response.ContentType = "image/png";
                    context.Response.Clear();
                    context.Response.BufferOutput = true;

                    MemoryStream m = new MemoryStream();

                    string img_str  = HttpContext.Current.Server.MapPath(@"~/Images/Driver.png");

                    Image img = Image.FromFile(img_str);
                    img.Save(m, System.Drawing.Imaging.ImageFormat.Png);

                    context.Response.BinaryWrite(m.ToArray());
                }
                else
                {
                    context.Response.ContentType = "image/jpg";
                    context.Response.BinaryWrite((byte[])dt.Rows[i][0]);
                }

            }
            con.Close();
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