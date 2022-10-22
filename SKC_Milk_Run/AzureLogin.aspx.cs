using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
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
    public partial class AzureLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            if (!Request.IsAuthenticated)
            {
                authenticationManager.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
            else
            {
                SetLoginSession(authenticationManager);
                Response.Redirect("~/Default.aspx");
            }
        }

        public void SetLoginSession(IAuthenticationManager authenticationManager)
        {
            AzureUserDetail azureUserDetail = new AzureUserDetail(authenticationManager);

            Data_User Usr = new Data_User();
            DataTable GetUser = Usr.GetUserByUserLogin(azureUserDetail.LoginUser);
            Session["UserId"] = GetUser.Rows[0]["UserId"].ToString();
            Session["Username"] = GetUser.Rows[0]["Username"].ToString();
            Session["FullName"] = GetUser.Rows[0]["FullName"].ToString();
            Session["SupplierCode"] = GetUser.Rows[0]["SupplierCode"].ToString();
            Session["SupplierName"] = GetUser.Rows[0]["Supplier_Name"].ToString();
            Session["AccessLevel"] = GetUser.Rows[0]["AccessLevel"].ToString();
            Session["UserLevel"] = GetUser.Rows[0]["UserLevel"].ToString();

          
        }
    }
}