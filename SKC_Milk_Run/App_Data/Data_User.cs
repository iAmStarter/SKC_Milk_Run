using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Linq;
using HyDrA.Data.SqlClient;

namespace SKC_Milk_Run.App_Data
{
    [Table("Data_User")]
    public class Data_User : MsSQL
    {
        # region Declare
        private int _UserId;
        private string _Username;
        private string _Password;
        private string _FirstName;
        private string _LastName;
        private string _SupplierCode;
        private int _AccessLevel;
        private int _UserLevel;
        # endregion;
        public Data_User()
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = true;
        }
        public Data_User(int UserId)
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = false;
            ObjectDatabind(this.GetById(UserId));
        }
        //Init data from database to object members.
        private void ObjectDatabind(DataTable obj)
        {
            foreach (DataRow dr in obj.Rows)
            {
                if (dr["UserId"] != DBNull.Value) { this.UserId = (int)dr["UserId"]; }
                if (dr["Username"] != DBNull.Value) { this.Username = (string)dr["Username"]; }
                if (dr["Password"] != DBNull.Value) { this.Password = (string)dr["Password"]; }
                if (dr["FirstName"] != DBNull.Value) { this.FirstName = (string)dr["FirstName"]; }
                if (dr["LastName"] != DBNull.Value) { this.LastName = (string)dr["LastName"]; }
                if (dr["SupplierCode"] != DBNull.Value) { this.SupplierCode = (string)dr["SupplierCode"]; }
                if (dr["AccessLevel"] != DBNull.Value) { this.AccessLevel = (int)dr["AccessLevel"]; }
                if (dr["UserLevel"] != DBNull.Value) { this.UserLevel = (int)dr["UserLevel"]; }
            }
        }
        //Get single record all 
        public DataTable GetAll()
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@FLAG", "A");
            return (this.Fill("sp_Data_User_Select", P, CommandType.StoredProcedure));
        }
        //Get single record by primarykey 
        public DataTable GetById(int UserId)
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@UserId", UserId);
            P.AddParameters("@FLAG", "P");
            return (this.Fill("sp_Data_User_Select", P, CommandType.StoredProcedure));
        }
        //Save changed record. 
        public object SaveToDatabase()
        {
            object ret = null;
            try
            {
                CommandParameters P = new CommandParameters();
                P.AddParameters("@UserId", UserId);
                P.AddParameters("@Username", Username);
                P.AddParameters("@Password", Password);
                P.AddParameters("@FirstName", FirstName);
                P.AddParameters("@LastName", LastName);
                P.AddParameters("@SupplierCode", SupplierCode);
                P.AddParameters("@AccessLevel", AccessLevel);
                P.AddParameters("@UserLevel", UserLevel);
                if (this.IsObjectEmpty) { P.AddParameters("@FLAG", "I"); }
                else { P.AddParameters("@FLAG", "U"); }
                ret = this.ExecuteScalar("sp_Data_User_Execute", P, CommandType.StoredProcedure);
                this.IsObjectEmpty = false;
                return (ret);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //Get single record by primarykey 
        public void Delete(int UserId)
        {
            string sqlCmd = base.CommandTextBuilder(this, CommandTextType.Delete);
            CommandParameters P = new CommandParameters();
            P.AddParameters("@UserId", UserId);
            this.ExecuteNonQuery(sqlCmd, P, CommandType.Text);
        }
        //#########################BEGIN-MODIFY#######################################

        # region Declare
        public DataTable CheckUserByKey(string Username, string Password)
        {
            string sqlCmmd = "SELECT UserId FROM [Data_User]";
            sqlCmmd += " WHERE [Username] = N'" + Username + "' AND [Password] = N'" + Password + "'";
            return (this.Fill(sqlCmmd));
        }
        public DataTable GetUserByKey(string Username, string Password)
        {
            string sqlCmmd = "SELECT [UserId], [Username], CONCAT([FirstName], ' '+ [LastName]) AS [FullName], [SupplierCode], [AccessLevel] ,UserLevel , (select [Supplier_Name] from Tbl_Supplier vs where vs.[Supplier_Code] COLLATE DATABASE_DEFAULT  = SupplierCode) as [Supplier_Name] FROM [Data_User]";
            sqlCmmd += " WHERE [Username] = N'" + Username + "' AND [Password] = N'" + Password + "'";
            return (this.Fill(sqlCmmd));
        }
        public DataTable GetUserByUserId(int UserId) 
        {
            string sqlCmmd = "SELECT COUNT(*) AS CheckLogin FROM [Data_User]";
            sqlCmmd += " WHERE [UserId] = " + UserId.ToString();
            return (this.Fill(sqlCmmd));
        }

        public DataTable GetUserByUserLogin(string Username)
        {
            string sqlCmmd = "SELECT [UserId], [Username], CONCAT([FirstName], ' '+ [LastName]) AS [FullName], [SupplierCode], [AccessLevel] ,UserLevel , (select [Supplier_Name] from Tbl_Supplier vs where vs.[Supplier_Code] COLLATE DATABASE_DEFAULT  = SupplierCode) as [Supplier_Name] FROM [Data_User]";
            sqlCmmd += " WHERE [Username] = N'" + Username + "'";
            return (this.Fill(sqlCmmd));
        }
        # endregion;

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
        [Column("UserId", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true)]
        public int UserId
        { get { return (_UserId); } set { _UserId = value; } }
        [Column("Username", DbType = "varchar")]
        public string Username
        { get { return (_Username); } set { _Username = value; } }
        [Column("Password", DbType = "varchar")]
        public string Password
        { get { return (_Password); } set { _Password = value; } }
        [Column("FirstName", DbType = "varchar")]
        public string FirstName
        { get { return (_FirstName); } set { _FirstName = value; } }
        [Column("LastName", DbType = "varchar")]
        public string LastName
        { get { return (_LastName); } set { _LastName = value; } }
        [Column("SupplierCode", DbType = "varchar")]
        public string SupplierCode
        { get { return (_SupplierCode); } set { _SupplierCode = value; } }
        [Column("AccessLevel", DbType = "int")]
        public int AccessLevel
        { get { return (_AccessLevel); } set { _AccessLevel = value; } }
        [Column("UserLevel", DbType = "varchar")]
        public int UserLevel
        { get { return (_UserLevel); } set { _UserLevel = value; } }
        # endregion;
    }
}