using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Linq;
using HyDrA.Data.SqlClient;
namespace SKC_Milk_Run.App_Data
{
    [Table("Config_Access_Object")]
    public class Config_Access_Object : MsSQL
    {
        # region Declare
        private int _Id;
        private int _AccessId;
        private int _ObjectId;
        # endregion;
        public Config_Access_Object()
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = true;
        }
        public Config_Access_Object(int Id)
            : base(Configs.ConnectionString)
        {
            this.IsObjectEmpty = false;
            ObjectDatabind(this.GetById(Id));
        }
        //Init data from database to object members.
        private void ObjectDatabind(DataTable obj)
        {
            foreach (DataRow dr in obj.Rows)
            {
                if (dr["Id"] != DBNull.Value) { this.Id = (int)dr["Id"]; }
                if (dr["AccessId"] != DBNull.Value) { this.AccessId = (int)dr["AccessId"]; }
                if (dr["ObjectId"] != DBNull.Value) { this.ObjectId = (int)dr["ObjectId"]; }
            }
        }
        //Get single record all 
        public DataTable GetAll()
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@FLAG", "A");
            return (this.Fill("sp_Config_Access_Object_Select", P, CommandType.StoredProcedure));
        }
        //Get single record by primarykey 
        public DataTable GetById(int Id)
        {
            CommandParameters P = new CommandParameters();
            P.AddParameters("@Id", Id);
            P.AddParameters("@FLAG", "P");
            return (this.Fill("sp_Config_Access_Object_Select", P, CommandType.StoredProcedure));
        }
        //Save changed record. 
        public object SaveToDatabase()
        {
            object ret = null;
            try
            {
                CommandParameters P = new CommandParameters();
                P.AddParameters("@Id", Id);
                P.AddParameters("@AccessId", AccessId);
                P.AddParameters("@ObjectId", ObjectId);
                if (this.IsObjectEmpty) { P.AddParameters("@FLAG", "I"); }
                else { P.AddParameters("@FLAG", "U"); }
                ret = this.ExecuteScalar("sp_Config_Access_Object_Execute", P, CommandType.StoredProcedure);
                this.IsObjectEmpty = false;
                return (ret);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //Get single record by primarykey 
        public void Delete(int Id)
        {
            string sqlCmd = base.CommandTextBuilder(this, CommandTextType.Delete);
            CommandParameters P = new CommandParameters();
            P.AddParameters("@Id", Id);
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
        [Column("Id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        { get { return (_Id); } set { _Id = value; } }
        [Column("AccessId", DbType = "int")]
        public int AccessId
        { get { return (_AccessId); } set { _AccessId = value; } }
        [Column("ObjectId", DbType = "int")]
        public int ObjectId
        { get { return (_ObjectId); } set { _ObjectId = value; } }
        # endregion;
    }
}