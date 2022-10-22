using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using HyDrA.Data.SqlClient;
namespace SKC_Milk_Run.App_Data
{
    [Table("Data_Object")]
    public class Data_Object : MsSQL
    {
        # region Declare
        public DataTable GetObject(string UserId)
        {
            string sqlCmmd = "SELECT [ObjectId], [ObjectName], [ObjectPath], [Description] FROM [dbo].[Data_Object]";
            sqlCmmd += " WHERE [ParentId] IS NOT NULL";
            return (this.Fill(sqlCmmd));
        }
        # endregion;
        public Data_Object() : base(Configs.ConnectionString)
        {

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