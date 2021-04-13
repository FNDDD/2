using Client.SqliteTool;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Tool
{
   public class Log_Local
    {
       //public static Sqlite_Tools sqlite_Tools = new Sqlite_Tools(GetProjectRootPath()+ConfigurationManager.AppSettings["DB_PATH"]);
        public static Sqlite_Tools sqlite_Tools = new Sqlite_Tools( ConfigurationManager.AppSettings["DB_PATH"]);
        public static string GetProjectRootPath()
        {
            string rootpath = AppDomain.CurrentDomain.BaseDirectory;
            //string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            return rootpath;
        }

        /// <summary>
        /// 登录
        /// </summary>
        public static int LOGIN = 1;

        /// <summary>
        /// 注销
        /// </summary>
        public static int LOGOUT = 2;

        /// <summary>
        /// 收银
        /// </summary>
        public static int CASHIE = 3;

        /// <summary>
        /// 进货
        /// </summary>
        public static int STOCK = 4;

        /// <summary>
        /// 退货
        /// </summary>
        public static int RETURN = 5;

        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="UserID">当前用户Id</param>
        /// <param name="Type">操作类型</param>
        /// <param name="Object">操作对象ID</param>
        public static void LOG(string UserID,int Type,string Object)
        {
            string sqlstr = "INSERT INTO `sys_Log`(UserId,Type,ObjectID) VALUES('" + UserID+"',"+Type+",'"+ Object + "')";
            sqlite_Tools.Insert(sqlstr);
        }


        public static void LOG_FYZF(string UserID, string Req, string Resp,string code)
        {
            string sqlstr = "INSERT INTO `sys_FYZF`(UserId,code,Request,Response) VALUES('" + UserID + "','" + code + "','" + Req +"','"+ Resp + "')";
            sqlite_Tools.Insert(sqlstr);
        }

    }
}
