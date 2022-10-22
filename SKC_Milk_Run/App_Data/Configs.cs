using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SKC_Milk_Run.App_Data
{
    public class Configs
    {

        public static string ConnectionString
        {
            get
            {
                //return (ConfigurationManager.ConnectionStrings["DbConnectionStringTEST"].ConnectionString.ToString());
                return (ConfigurationManager.ConnectionStrings["DbConnectionStringMilkRun"].ConnectionString.ToString());
            }
        }        
    }
}