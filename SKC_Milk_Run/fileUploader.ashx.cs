using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using HyDrA.Data.SqlClient;
using System.Configuration;
using SKC_Milk_Run.App_Data;

namespace SKC_Milk_Run
{

    public class fileUploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string  ID_ = context.Request.QueryString["ID"].ToString() ;
            try
            {
                string dirFullPath = HttpContext.Current.Server.MapPath("~/UploadImages/");
                string[] files;
                int numFiles;
                files = System.IO.Directory.GetFiles(dirFullPath);
                numFiles = files.Length;
                numFiles = numFiles + 1;
                string str_image = "";

                foreach (string s in context.Request.Files)
                {
                    HttpPostedFile file = context.Request.Files[s];
                    string fileName = file.FileName;
                    string fileExtension = file.ContentType;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        fileExtension = Path.GetExtension(fileName);
                        str_image = "MyPHOTO_" + numFiles.ToString() + fileExtension;
                        string pathToSave_100 = HttpContext.Current.Server.MapPath("~/UploadImages/") + str_image;
                        //file.SaveAs(pathToSave_100);


                        byte[] imgBin = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            imgBin = binaryReader.ReadBytes(file.ContentLength);
                        }

                        string sql = " UPDATE [dbo].[Tbl_Driver] SET Driver_Image = @Driver_Image WHERE [ID] = " + ID_;

                        //string sql = " UPDATE [dbo].[Tbl_Vehicle_Registration] SET Driver_Image = @Driver_Image WHERE [ID] = " + ID_;

                        using (var connUpd = new SqlConnection(Global.db_Con))
                        {
                            connUpd.Open();
                            SqlCommand cmdImage = new SqlCommand(sql, connUpd);
                            cmdImage.CommandType = CommandType.Text;
                            cmdImage.Parameters.Add("@Driver_Image", SqlDbType.Image, imgBin.Length).Value = imgBin;
                            cmdImage.ExecuteNonQuery();
                        };

                    }
                        
                }
                //  database record update logic here  ()

                context.Response.Write(str_image);
            }
            catch (Exception ac)
            {

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