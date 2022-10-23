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
                Response.Redirect("~/Default.aspx");
            }
        }
    }
}