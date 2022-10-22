using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SKC_Milk_Run.App_Data;

namespace SKC_Milk_Run
{
    public partial class Default : System.Web.UI.Page
    {
        public string UserId;

        public string strConnect = Configs.ConnectionString;

        protected string UserLevel, supplier_code, user_name = "";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserId"] != null)
                UserId = Session["UserId"].ToString();

            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }

            if (!IsPostBack)
            {

                if (Session["UserLevel"] != null)
                    UserLevel = Session["UserLevel"].ToString();

                if (Session["SupplierCode"] != null)
                    supplier_code = Session["SupplierCode"].ToString();

                if (Session["Username"] != null)
                {
                    user_name = Session["Username"].ToString();
                }

            }

        }
    }
}