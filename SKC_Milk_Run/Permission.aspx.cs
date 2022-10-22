using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SKC_Milk_Run.App_Data;
namespace SKC_Milk_Run
{
    public partial class Permission : System.Web.UI.Page
    {
        protected string UserId, yyMMddHHmmss;
        protected string UserLevel, supplier_code, user_name;

        protected void Page_Load(object sender, EventArgs e)
        {
            yyMMddHHmmss = DateTime.Now.ToString("yyMMddHHmmss"); // case sensitive

            if (Session["Username"] == null)
            {
                Response.Redirect(ResolveUrl("~/Login.aspx"));
                return;
            }

            // Get User ID
            UserId = Session["UserId"].ToString();
            UserLevel = "";
            supplier_code = "";

            // Start Page
            if (!Page.IsPostBack)
            {
                if (Session["UserLevel"] != null)
                    UserLevel = Session["UserLevel"].ToString();

                if (Session["SupplierCode"] != null)
                    supplier_code = Session["SupplierCode"].ToString();

                user_name = "";

                if (Session["Username"] != null)
                {
                    user_name = Session["Username"].ToString();
                }
            }
        }
    }
}