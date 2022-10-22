using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SKC_Milk_Run
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public string UserName = "";
        public string UserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (Session["Username"] == null)
            {
                Response.Redirect(ResolveUrl("~/Login.aspx"));
                return;
            }

            if (!IsPostBack)
            {
               
                UserId = Session["UserId"].ToString();

                if (Session["Username"] != null)
                {
                    UserName = Session["Username"].ToString();
                }
            }
        }        
    }
}