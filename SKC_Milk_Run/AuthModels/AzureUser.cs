using Microsoft.Owin.Security;
using SKC_Milk_Run.App_Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SKC_Milk_Run.AuthModels
{
    public class AzureUser
    {

        public string FullName { get; private set; }
        public string LoginUser { get; private set; }
        public string IPAddress { get;  private set; }
        public string Identifier { get; private set; }

        public AzureUser(IAuthenticationManager authenticationManager)
        {
            FullName = authenticationManager.User.Claims.FirstOrDefault(x => x.Type == "name").Value.ToString();
            LoginUser = authenticationManager.User.Claims.FirstOrDefault(x => x.Type.Contains("upn")).Value.ToString();
            IPAddress = authenticationManager.User.Claims.FirstOrDefault(x => x.Type == "ipaddr").Value.ToString();
            Identifier = authenticationManager.User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value.ToString();
        }

        public void SetLoginSession(HttpSessionState Session) { 

            Data_User Usr = new Data_User();
            DataTable GetUser = Usr.GetUserByUserLogin(LoginUser);
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