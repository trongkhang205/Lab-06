using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Lab_06.DAL
{
    public class DatabaseAccess
    {
        private static string _connectionString;
        private static string ConfigFile = "db_config.txt";

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    LoadConnectionString();
                }
                return _connectionString;
            }
        }

        public static void LoadConnectionString()
        {
            if (File.Exists(ConfigFile))
            {
                _connectionString = File.ReadAllText(ConfigFile).Trim();
            }
            else
            {
                // Default fallback if no file exists (User's specific local setup)
                _connectionString = "Data Source=TRONGKHANG-IT;Initial Catalog=BookstoreDB;Integrated Security=True";
            }
        }

        public static void SaveConnectionString(string serverName, string dbName)
        {
            _connectionString = $"Data Source={serverName};Initial Catalog={dbName};Integrated Security=True";
            File.WriteAllText(ConfigFile, _connectionString);
        }

        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    // If connection fails, it might be due to wrong connection string. 
                    // In a real app we might throw a custom exception or log it.
                    throw new Exception("Database Error: " + ex.Message);
                }
            }
        }

        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Database Error: " + ex.Message);
                }
            }
        }
        
        // Check connection validity
        public static bool TestConnection(string tempConnString = null)
        {
            string connStr = tempConnString ?? ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
