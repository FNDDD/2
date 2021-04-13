using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.SqliteTool
{
   public class Sqlite_Tools
    {
        public Sqlite_Tools(string path)
        {
            PATH = path;
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="Path">创建数据库地址</param>
        public void CreateDB(string Path)
        {
            string path = @""+ Path;
            SQLiteConnection cn = new SQLiteConnection("data source=" + path);
            cn.Open();
            cn.Close();
        }

        /// <summary>
        /// 数据库地址
        /// </summary>
        public String PATH = "";

        /// <summary>
        /// 插入数据库
        /// </summary>
        /// <param name="SqlStr">Sql语句</param>
        public void Insert(string SqlStr)
        {
            SQLiteConnection cn = new SQLiteConnection("data source=" + PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                try
                {

                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = SqlStr;
                    cmd.ExecuteNonQuery();
                    cmd.Clone();
                    cmd.Dispose();
                    cn.Clone();
                    cn.Close();
                    cn.Dispose();
                }
                catch (Exception EX)
                {
                }
            }
        }

        /// <summary>
        /// 查
        /// </summary>
        public DataTable Select(string SqlStr)
        {
            DataTable table=new DataTable();
            SQLiteConnection cn = new SQLiteConnection("data source=" + PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = SqlStr;
                SQLiteDataReader sr = cmd.ExecuteReader();

                while (sr.Read())
                { 
                    string s=sr["userid"].ToString();
                    string ss = sr["type"].ToString();
                    string sss = sr["Objectid"].ToString();
                }
                sr.Close();             
            }
            return table;
        }
    }
}
