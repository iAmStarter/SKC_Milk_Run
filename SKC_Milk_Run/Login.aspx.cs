using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SKC_Milk_Run.App_Data;
using SKC_Milk_Run.AuthModels;

namespace SKC_Milk_Run
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
        }
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string Type = "", Msg = "";

            try
            {
                GeneralClass GC = new GeneralClass();
                string Username = GC.CheckSQL(this.TextUsername.Text);
                string Password = GC.CheckSQL(this.TextPassword.Text);

                Data_User Usr = new Data_User();
                DataTable CheckUser = Usr.CheckUserByKey(Username, Password);

                if (CheckUser.Rows.Count == 1)
                {
                    DataTable GetUser = Usr.GetUserByKey(Username, Password);
                    Session["UserId"] = GetUser.Rows[0]["UserId"].ToString();
                    Session["Username"] = GetUser.Rows[0]["Username"].ToString();
                    Session["FullName"] = GetUser.Rows[0]["FullName"].ToString();
                    Session["SupplierCode"] = GetUser.Rows[0]["SupplierCode"].ToString();
                    Session["SupplierName"] = GetUser.Rows[0]["Supplier_Name"].ToString();
                    Session["AccessLevel"] = GetUser.Rows[0]["AccessLevel"].ToString();
                    Session["UserLevel"] = GetUser.Rows[0]["UserLevel"].ToString();
                    Labelmsg.Text = "";
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    Type = "[Error Message]";
                    Msg = " User Name or Password was wrong ";
                    Labelmsg.Text = Msg ;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);

                Type = "Error";
                Msg = err.ToString();
                Labelmsg.Text = err.ToString() + " !"; //.Substring(0,106)
            }

            string JS = "alert(\"" + Type + "\\n" + Msg + "\")";
            ScriptManager.RegisterStartupScript(this, GetType(), "modelUser", JS, true);
        }
    }
}