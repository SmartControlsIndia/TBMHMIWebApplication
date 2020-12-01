using System;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace TBMHMIWebApplication
{
    public class DBConnection : System.Web.UI.Page
    {
        private IDbCommand cmd = new SqlCommand();
        private string strConnectionString = "";
        private bool handleErrors = true;
        private string strLastError = "";
        SqlConnection cnn = new SqlConnection();

        public DBConnection()
        {
            // 
            // ConnectionStringSettings objConnectionStringSettings = ConfigurationManager.ConnectionStrings["connectionStr"];
            //strConnectionString = Session["connection_session"].ToString();
             strConnectionString = (System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"]).ToString();
          
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = strConnectionString;
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
        }


        public IDataReader ExecuteReader()
        {
            IDataReader reader = null;
            try
            {
                this.Open();
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {

                throw ex;

            }

            return reader;
        }

        public IDataReader ExecuteReader(string commandtext)
        {
            IDataReader reader = null;
            try
            {
                cmd.CommandText = commandtext;
                reader = this.ExecuteReader();
            }
            catch (Exception ex)
            {

                throw ex;

            }

            return reader;
        }

        public object ExecuteScalar()
        {
            object obj = null;
            try
            {
                this.Open();
                obj = cmd.ExecuteScalar();
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return obj;
        }

        public object ExecuteScalar(string commandtext)
        {
            object obj = null;
            try
            {
                cmd.CommandText = commandtext;
                obj = this.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return obj;
        }

        public int ExecuteNonQuery()
        {
            int i = -1;
            try
            {
                this.Open();
                i = cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return i;
        }

        public int ExecuteNonQuery(string commandtext)
        {
            int i = -1;
            try
            {
                cmd.CommandText = commandtext;
                i = this.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return i;
        }

        public DataSet ExecuteDataSet()
        {
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                ds = new DataSet();
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public DataTable ExecuteDataTable()
        {
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                da = new SqlDataAdapter();
                da.SelectCommand = (SqlCommand)cmd;
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return dt;
        }

        public DataTable ExecuteDataTable(string commandtext)
        {
            DataTable dt = null;
            try
            {
                cmd.CommandText = commandtext;
                dt = this.ExecuteDataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public DataSet ExecuteDataSet(string commandtext)
        {
            DataSet ds = null;
            try
            {
                cmd.CommandText = commandtext;
                ds = this.ExecuteDataSet();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public string CommandText
        {
            get { return cmd.CommandText; }
            set
            {
                cmd.CommandText = value;
                cmd.Parameters.Clear();
            }
        }

        public IDataParameterCollection Parameters
        {
            get { return cmd.Parameters; }
        }

        public void AddParameter(string paramname, object paramvalue)
        {
            SqlParameter param = new SqlParameter(paramname, paramvalue);
            cmd.Parameters.Add(param);
        }

        public void AddParameter(IDataParameter param)
        {
            cmd.Parameters.Add(param);
        }

        public string ConnectionString
        {
            get { return strConnectionString; }
            set { strConnectionString = value; }
        }

        private void Open()
        {
            cmd.Connection.Open();
        }

        private void Close()
        {
            cmd.Connection.Close();
        }

        public bool HandleExceptions
        {
            get { return handleErrors; }
            set { handleErrors = value; }
        }

        public string LastError
        {

            get { return strLastError; }
        }

        public void Dispose()
        {
            cmd.Dispose();
        }

        public DataTable FetchMultipleRecords(string sQuery)
        {
            DBConnection dbObj = new DBConnection();
            return dbObj.ExecuteDataTable(sQuery);
        }

        //#region IDisposable Members

        //void IDisposable.Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}