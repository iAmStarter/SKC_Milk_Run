using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SKC_Milk_Run.AuthModels
{
    public class AzureUserDetail
    {

        public string FullName { get; private set; }
        public string LoginUser { get; private set; }
        public string IPAddress { get;  private set; }
        public string Identifier { get; private set; }

        public AzureUserDetail(IAuthenticationManager authenticationManager)
        {
            FullName = authenticationManager.User.Claims.FirstOrDefault(x => x.Type == "name").Value.ToString();
            LoginUser = authenticationManager.User.Claims.FirstOrDefault(x => x.Type.Contains("upn")).Value.ToString();
            IPAddress = authenticationManager.User.Claims.FirstOrDefault(x => x.Type == "ipaddr").Value.ToString();
            Identifier = authenticationManager.User.Claims.FirstOrDefault(x => x.Type.Contains("nameidentifier")).Value.ToString();
        }

    }
}