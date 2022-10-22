using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Linq;
using HyDrA.Data.SqlClient;

namespace SKC_Milk_Run.App_Data
{
    [Table("Data_User_Access")]
    public class Data_User_Access : MsSQL
    {
        # region Declare
        private int _accessId;
        private string _accessName;
        private string _admin;
        # endregion;
        public Data_User_Access()
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = true;
        }
        public Data_User_Access(int accessId)
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = false;
            ObjectDatabind(this.GetById(accessId));
        }
        //Init data from database to object members.
        private void ObjectDatabind(DataTable obj)
        {
            foreach (DataRow dr in obj.Rows)
            {
                if (dr["AccessId"] != DBNull.Value) { this.AccessId = (int)dr["AccessId"]; }
                if (dr["AccessName"] != DBNull.Value) { this.AccessName = (string)dr["AccessName"]; }
                if (dr["Admin"] != DBNull.Value) { this.Admin = (string)dr["Admin"]; }
            }
        }
        //Get single record all 
        public DataTable GetAll()
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@FLAG", "A");
            return (this.Fill("sp_Data_User_Access_Select", P, CommandType.StoredProcedure));
        }
        //Get single record by primarykey 
        public DataTable GetById(int accessId)
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@AccessId", accessId);
            P.AddParameters("@FLAG", "P");
            return (this.Fill("sp_Data_User_Access_Select", P, CommandType.StoredProcedure));
        }
        //Save changed record. 
        public object SaveToDatabase()
        {
            object ret = null;
            try
            {
                CommandParameters P = new CommandParameters();
                P.AddParameters("@AccessId", AccessId);
                P.AddParameters("@AccessName", AccessName);
                P.AddParameters("@Admin", Admin);
                if (this.IsObjectEmpty) { P.AddParameters("@FLAG", "I"); }
                else { P.AddParameters("@FLAG", "U"); }
                ret = this.ExecuteScalar("sp_Data_User_Access_Execute", P, CommandType.StoredProcedure);
                this.IsObjectEmpty = false;
                return (ret);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //Get single record by primarykey 
        public void Delete(int accessId)
        {
            string sqlCmd = base.CommandTextBuilder(this, CommandTextType.Delete);
            CommandParameters P = new CommandParameters();
            P.AddParameters("@AccessId", accessId);
            this.ExecuteNonQuery(sqlCmd, P, CommandType.Text);
        }
        //#########################BEGIN-MODIFY#######################################

        //#########################END-MODIFY#######################################
        public DataTable SQLSelect(string sqlCmmandText)
        {
            return (this.Fill(sqlCmmandText));
        }
        public void SQLExecute(string sqlCmmandText)
        {
            this.ExecuteNonQuery(sqlCmmandText);
        }
        # region Data Properties
        public bool IsObjectEmpty
        { get; set; }
        [Column("AccessId", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true)]
        public int AccessId
        { get { return (_accessId); } set { _accessId = value; } }
        [Column("AccessName", DbType = "varchar")]
        public string AccessName
        { get { return (_accessName); } set { _accessName = value; } }
        [Column("Admin", DbType = "varchar")]
        public string Admin
        { get { return (_admin); } set { _admin = value; } }
        # endregion;
    }
}