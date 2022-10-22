using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using HyDrA.Data.SqlClient;

namespace SKC_Milk_Run.App_Data
{
    public class Global_Service : MsSQL
    {
        # region Declare

        # endregion;
        public Global_Service() : base(Configs.ConnectionString)
        {
            //
        }

        public DataTable SQLSelect(string sqlCmmd)
        {
            return (this.Fill(sqlCmmd));
        }
        public void SQLExecute(string sqlCmmd)
        {
            this.ExecuteNonQuery(sqlCmmd);
        }
    }

}