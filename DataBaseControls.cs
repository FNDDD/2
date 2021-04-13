using Client.Entity;
using Client.Http.ForService;
using Client.Tool;
using Client.收银小票;
using Client.用户;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Threading;
using System.Windows;

namespace Client
{
    public class DataBaseControls
    {
        //public static string DB_PATH = GetProjectRootPath()+ ConfigurationManager.AppSettings["DB_PATH"];
        public static string DB_PATH = ConfigurationManager.AppSettings["DB_PATH"];
        public static string GetProjectRootPath()
        {
            string rootpath = AppDomain.CurrentDomain.BaseDirectory;
            //string rootpath = path.Substring(0, path.LastIndexOf("bin"));
            return rootpath;
        }

        #region 挂单

        /// <summary>
        /// 获取挂单信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static List<GuaDanObj> GetGuaDan()
        {
            List<GuaDanObj> ListGuaDanOBJ = new List<GuaDanObj>();

            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);

                string Sql = "Select * from smc_guadan where time >='" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and mark=0";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        GuaDanObj guaDanObj = new GuaDanObj();
                        guaDanObj.DateTime = Convert.ToDateTime(sr["time"].ToString());
                        guaDanObj.Code = sr["code"].ToString();
                        ListGuaDanOBJ.Add(guaDanObj);
                    }
                    sr.Close();
                    cmd.Clone();
                    
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }




            return ListGuaDanOBJ;
        }

        /// <summary>
        /// 获取挂单信息详情
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static GuaDanObj GetGuaDanItem(string code)
        {
            GuaDanObj ListGuaDanOBJ = new GuaDanObj();
            List<Goodsroder> goodsroders = new List<Goodsroder>();
            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);
                string Sql = "Select * from smc_guadan_item where code='" + code + "';";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        Goodsroder goodsroder = new Goodsroder();
                        ListGuaDanOBJ.Code= sr["code"].ToString();
                        goodsroder.Barcode = sr["barcode"].ToString();
                        goodsroder.GoodsStoreId = Convert.ToInt32(sr["goods_id"].ToString());
                        goodsroder.Name = sr["name"].ToString();
                        goodsroder.Num = Convert.ToInt32(sr["num"].ToString());
                        goodsroder.Sum = Convert.ToDouble(sr["sum"].ToString());
                        goodsroder.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                        goodsroder.Original_price = Convert.ToDouble(sr["original_price"].ToString());
                        goodsroders.Add(goodsroder);
                    }
                    ListGuaDanOBJ.ListGoodsGuaDan = goodsroders;
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }


            return ListGuaDanOBJ;
        }

        /// <summary>
        /// 取消挂单
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool UpdataGuadan(string code ,int i)
        {
            bool mark = false;

            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE smc_guadan SET mark= "+i+" where code='" + code + "';";



                int n = cmd_.ExecuteNonQuery();

                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }

        /// <summary>
        /// 新增挂单
        /// </summary>
        /// <param name="code"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        public static bool AddGuadan(string code,double sum)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_guadan " +
                    "(code,sum) values ('" + code + "',"+ sum + ");";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        /// <summary>
        /// 新增挂单详情
        /// </summary>
        /// <param name="OrderCode"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool AddGuadanInfo(string OrderCode, List<Goodsroder> list)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_guadan_item " +
                    "(code,barcode,goods_id,name," +
                    "num,sum,original_price,sale_price) values ";
                string value = string.Empty;
                foreach (Goodsroder goodsStore in list)
                {
                    value = value + "('" + OrderCode + "','" + goodsStore.Barcode + "','" + goodsStore.GoodsStoreId +
                        "','" + goodsStore.Name + "'," + goodsStore.Num + "," + goodsStore.Sum +
                        "," + goodsStore.Original_price + "," + goodsStore.Sale_price + "),";

                }
                value = value.Substring(0, value.Length - 1);
                value = value + ";";
                cmd.CommandText = sql + value;
                int i = cmd.ExecuteNonQuery();
                cmd.Clone();
                cmd.Dispose();
            }
            cn.Clone();
            cn.Close();
            cn.Dispose();
            return mark;
        }

        /// <summary>
        /// 查询今日挂单总数数
        /// </summary>
        /// <returns></returns>
        public static string GetGuaDanAllNum()
        {
            string num = "";

            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);

                string Sql = "Select count(code) as allnum ,SUM(C.sum) as allSum from smc_guadan where time >='" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00 ";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        
                       int Num = Convert.ToInt32(sr["allnum"].ToString());
                       double Sum= Convert.ToDouble(sr["allSum"].ToString());
                        num = Num.ToString() + "_" + Sum.ToString();
                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }




            return num;
        }

        /// <summary>
        /// 查询今日结账挂单数
        /// </summary>
        /// <returns></returns>
        public static string GetGuaDanrealyNum()
        {
            string num = "";

            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);

                string Sql = "Select count(code) as allnum,SUM(sum) as allSum from smc_guadan where time >='" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and mark=2";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {

                        try
                        {
                            int Num = Convert.ToInt32(sr["allnum"].ToString());
                            double Sum = Convert.ToDouble(sr["allSum"].ToString());
                            num = Num.ToString() + "_" + Sum.ToString();
                        }
                        catch (Exception ex)
                        {
                            num = "0_0";
                        }
                       
                        

                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }




            return num;
        }

        /// <summary>
        /// 查询今日未结账挂单数
        /// </summary>
        /// <returns></returns>
        public static string GetGuaDanNotrealyNum()
        {
            string num = "";

            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);

                string Sql = "Select count(code) as allnum,SUM(sum) as allSum from smc_guadan where time >='" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and mark=0";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {

                        try
                        {
                            int Num = Convert.ToInt32(sr["allnum"].ToString());
                            double Sum = Convert.ToDouble(sr["allSum"].ToString());
                            num = Num.ToString() + "_" + Sum.ToString();
                        }
                        catch (Exception ex)
                        {
                            num = "0_0";
                        }

                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }

            return num;
        }
        #endregion

        #region 用户

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserName">账户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public static User GetUser(string UserName)
        {
            
            User usert = new User();
            try
            {
                
                string Sql = "Select * from sys_user where user_name='" + UserName + "';";
               
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
               
                if (cn.State != System.Data.ConnectionState.Open)
                {
                  
                    cn.Open();
                  
                    SQLiteCommand cmd = cn.CreateCommand();
                   
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {

                        usert.User_id = Convert.ToInt16(sr["user_id"].ToString());
                        usert.Dept_id = Convert.ToInt16(sr["dept_id"].ToString());
                        usert.User_name = sr["user_name"].ToString();
                        usert.Nick_name = sr["nick_name"].ToString();
                        usert.User_type = sr["user_type"].ToString();
                        usert.Email = sr["email"].ToString();
                        usert.Phonenumber = sr["phonenumber"].ToString();
                        usert.Sex = sr["sex"].ToString();
                        usert.Avatar = sr["avatar"].ToString();
                        usert.Password = sr["password"].ToString();
                        usert.Status = sr["status"].ToString();
                        usert.Del_flag = sr["del_flag"].ToString();
                        //usert.Login_ip = sr["login_ip"].ToString();
                        //usert.Login_date = Convert.ToDateTime(sr["login_date"].ToString());
                        //usert.Create_by = sr["create_by"].ToString();
                        //usert.Create_time = Convert.ToDateTime(sr["update_time"].ToString());
                       //usert.Remark = sr["remark"].ToString();
                        //usert.Token = sr["token"].ToString();
                        //usert.Open_id = sr["open_id"].ToString();
                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }
            return usert;
        }

        /// <summary>
        /// 获取会员信息
        /// </summary>
        /// <param name="UserName">账户名</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public static VIPUser GetVIPUser(string word)
        {
            VIPUser usert = new VIPUser();
            try
            {
                string Sql = "Select * from user where user_name='" + word + "' or phone_number='" + word + "';";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        usert.User_id = Convert.ToInt16(sr["id"].ToString());
                        usert.User_name = sr["user_name"].ToString();
                        usert.Nick_name = sr["nick_name"].ToString();
                        usert.Phonenumber = sr["phone_number"].ToString();
                        usert.Avatar = sr["avatar"].ToString();
                        usert.Status = sr["status"].ToString();
                        if (!sr["integral"].ToString().Equals(""))
                        {
                            usert.Integral = Convert.ToDouble(sr["integral"].ToString());
                        }
                        if (!sr["suplus"].ToString().Equals(""))
                        {
                            usert.Suplus = Convert.ToDouble(sr["suplus"].ToString());
                        }
                    }
                    sr.Close();
                }
                cn.Clone();
                cn.Close();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }
            return usert;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="shopId">设备id</param>
        /// <param name="userName">用户账户</param>
        /// <param name="nickName">昵称</param>
        /// <param name="phonenumber">手机号</param>
        /// <param name="password">密码</param>
        /// <param name="status">状态</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public static bool AddUser(string userId, string Type, string shopId, string userName,
            string nickName, string phonenumber, string password, string status, string remark)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO sys_user " +
                    "(user_id,user_type,dept_id,user_name,nick_name,phonenumber,password,status,remark) values (" +
                    userId +",'"+ Type + "','" + shopId + "','" + userName + "','" + nickName + 
                    "','"+ phonenumber + "','" + password + "','" + status + "','" + remark + "');";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Type"></param>
        /// <param name="shopId"></param>
        /// <param name="userName"></param>
        /// <param name="nickName"></param>
        /// <param name="phonenumber"></param>
        /// <param name="password"></param>
        /// <param name="status"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static bool UpdataUser(string userId, string Type, string shopId, string userName,
            string nickName, string phonenumber, string password, string status, string remark)
        {
            bool mark = false;

            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE sys_user SET user_type='"+ Type + "',dept_id='"+ shopId 
                    + "',user_name='"+ userName + "',nick_name='"+ nickName + "',phonenumber='"+ phonenumber 
                    + "',password='"+ password + "',status='"+ status + "',remark='"+ remark+ "' where user_id=" + userId+";";
                  


                int n = cmd_.ExecuteNonQuery();

                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool DeleteUser(string userId)
        {
            bool mark = false;

            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "delete from sys_user  where user_id=" + userId + ";";



                int n = cmd_.ExecuteNonQuery();

                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }


        #endregion

        #region 商品查询
        /// <summary>
        /// 根据条码获取商品信息
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns></returns>
        public static GoodsStore GetGoodsStoreByBarcode(string barcode)
        {
            GoodsStore goodsStore = new GoodsStore();
            string Sql = "Select * from smc_goods_store where barcode='" + barcode + "';";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            try
            {
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    try
                    {
                        while (sr.Read())
                        {

                            goodsStore.Goods_store_id = Convert.ToInt16(sr["Goods_store_id"].ToString());
                            //goodsStore.Goods_id = Convert.ToInt16(sr["goods_id"].ToString());
                            goodsStore.Classify_id = Convert.ToInt16(sr["Classify_id"].ToString());
                            //goodsStore.Shop_id = Convert.ToInt16(sr["Shop_id"].ToString());
                            goodsStore.Goods_name = sr["Goods_name"].ToString();

                            goodsStore.Pinyin_code = sr["Pinyin_code"].ToString();
                            goodsStore.Barcode = sr["Barcode"].ToString();
                            if (sr["Show_img"].ToString() != null)
                            {
                                goodsStore.Show_img = sr["Show_img"].ToString();
                            }
                            if (sr["Img_url"].ToString() != null)
                            {
                                goodsStore.Img_url = sr["Img_url"].ToString();
                            }
                            if (sr["Main_img"].ToString() != null)
                            {
                                goodsStore.Main_img = sr["Main_img"].ToString();
                            }

                            if (sr["sale_price"].ToString() != null && !sr["sale_price"].ToString().Equals(""))
                            {
                                goodsStore.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                            }
                            else
                            {
                                goodsStore.Sale_price = 0;
                            }
                            if (sr["Cost_price"].ToString() != null&& !sr["Cost_price"].ToString().Equals(""))
                            {
                                goodsStore.Cost_price = Convert.ToDouble(sr["Cost_price"].ToString());
                            }
                            else
                            {
                                goodsStore.Cost_price = 0;
                            }

                            //goodsStore.Vip_price = Convert.ToDouble(sr["Sale_price"].ToString());
                            goodsStore.Spec_data = sr["Spec_data"].ToString();

                            if (sr["wholesale_price"].ToString() != null && !sr["wholesale_price"].ToString().Equals(""))
                            {
                                goodsStore.Wholesale_price = Convert.ToDouble(sr["wholesale_price"].ToString());
                            }
                            else
                            {
                                goodsStore.Wholesale_price = 0;
                            }


                            if (sr["original_price"].ToString() != null && !sr["original_price"].ToString().Equals(""))
                            {
                               goodsStore.Original_price = Convert.ToDouble(sr["original_price"].ToString());
                            }
                            else
                            {
                                goodsStore.Original_price = 0;

                            }

                           



                            goodsStore.Unit = sr["Unit"].ToString();
                            //goodsStore.Inventory_max = sr["Inventory_max"].ToString();
                           

                            if (sr["Inventory_now"].ToString() != null)
                            {
                                goodsStore.Inventory_now = sr["Inventory_now"].ToString();
                            }
                            else
                            {
                                goodsStore.Inventory_now = "0";
                            }

                            if (sr["Inventory_min"].ToString() != null)
                            {
                                goodsStore.Inventory_min =sr["Inventory_min"].ToString();
                            }
                            else
                            {
                                goodsStore.Inventory_min = "0";
                            }

                            goodsStore.Is_discount = sr["Is_discount"].ToString();

                            goodsStore.Integral_goods = sr["Integral_goods"].ToString();
                            //goodsStore.Make_date = Convert.ToDateTime(sr["Make_date"].ToString());
                            goodsStore.Shelf_life = sr["Shelf_life"].ToString();
                            goodsStore.Status = sr["status"].ToString();
                            goodsStore.Create_by = sr["Create_by"].ToString();

                            //goodsStore.Create_time = Convert.ToDateTime(sr["Create_time"].ToString());
                            goodsStore.Update_by = sr["Update_by"].ToString();
                            //goodsStore.Update_time = Convert.ToDateTime(sr["Update_time"].ToString());
                            goodsStore.Remark = sr["Remark"].ToString();
                            //goodsStore.Sales = Convert.ToInt16(sr["Sales"].ToString());

                            if (sr["Virtual_sales"].ToString() != null)
                            {
                                // goodsStore.Virtual_sales = Convert.ToInt16(sr["Virtual_sales"].ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        
                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                    cn.Clone();

                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return goodsStore;
        }

        /// <summary>
        /// 模糊查询商品信息
        /// </summary>
        /// <param name="likeWord">关键字</param>
        /// <returns></returns>
        public static List<GoodsStore> GetGoodsStore(string Word)
        {
            List<GoodsStore> list = new List<GoodsStore>();

           string Sql= "Select * from smc_goods_store where barcode like '%" + Word + "%' or goods_name like '%" + Word + "%' or pinyin_code like '%" + Word + "%'";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsStore goodsStore = new GoodsStore();
                    goodsStore.Goods_store_id = Convert.ToInt16(sr["Goods_store_id"].ToString());
                    //goodsStore.Goods_id = Convert.ToInt16(sr["goods_id"].ToString());
                    goodsStore.Classify_id = Convert.ToInt16(sr["Classify_id"].ToString());
                    //goodsStore.Shop_id = Convert.ToInt16(sr["Shop_id"].ToString());
                    goodsStore.Goods_name = sr["Goods_name"].ToString();

                    goodsStore.Pinyin_code = sr["Pinyin_code"].ToString();
                    goodsStore.Barcode = sr["Barcode"].ToString();
                    if (sr["Show_img"].ToString() != null)
                    {
                        goodsStore.Show_img = sr["Show_img"].ToString();
                    }
                    if (sr["Img_url"].ToString() != null)
                    {
                        goodsStore.Img_url = sr["Img_url"].ToString();
                    }
                    if (sr["Main_img"].ToString() != null)
                    {
                        goodsStore.Main_img = sr["Main_img"].ToString();
                    }

                    if (sr["sale_price"].ToString() != null && !sr["sale_price"].ToString().Equals(""))
                    {
                        goodsStore.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                    }
                    else
                    {
                        goodsStore.Sale_price = 0;
                    }
                    if (sr["Cost_price"].ToString() != null && !sr["Cost_price"].ToString().Equals(""))
                    {
                        goodsStore.Cost_price = Convert.ToDouble(sr["Cost_price"].ToString());
                    }
                    else
                    {
                        goodsStore.Cost_price = 0;
                    }
                    if (sr["wholesale_price"].ToString() != null && !sr["wholesale_price"].ToString().Equals(""))
                    {
                        goodsStore.Wholesale_price = Convert.ToDouble(sr["wholesale_price"].ToString());
                    }
                    else
                    {
                        goodsStore.Wholesale_price = 0;
                    }
                    //goodsStore.Vip_price = Convert.ToDouble(sr["Sale_price"].ToString());
                    goodsStore.Spec_data = sr["Spec_data"].ToString();


                    if (sr["original_price"].ToString() != null && !sr["original_price"].ToString().Equals(""))
                    {
                        goodsStore.Original_price = Convert.ToDouble(sr["original_price"].ToString());
                    }
                    else
                    {
                        goodsStore.Original_price = 0;

                    }





                    goodsStore.Unit = sr["Unit"].ToString();
                    //goodsStore.Inventory_max = sr["Inventory_max"].ToString();


                    if (sr["Inventory_now"].ToString() != null)
                    {
                        goodsStore.Inventory_now = sr["Inventory_now"].ToString();
                    }
                    else
                    {
                        goodsStore.Inventory_now = "0";
                    }

                    if (sr["Inventory_min"].ToString() != null)
                    {
                        goodsStore.Inventory_min = sr["Inventory_min"].ToString();
                    }
                    else
                    {
                        goodsStore.Inventory_min = "0";
                    }

                    goodsStore.Is_discount = sr["Is_discount"].ToString();

                    goodsStore.Integral_goods = sr["Integral_goods"].ToString();
                    //goodsStore.Make_date = Convert.ToDateTime(sr["Make_date"].ToString());
                    goodsStore.Shelf_life = sr["Shelf_life"].ToString();
                    goodsStore.Status = sr["status"].ToString();
                    goodsStore.Create_by = sr["Create_by"].ToString();

                    //goodsStore.Create_time = Convert.ToDateTime(sr["Create_time"].ToString());
                    goodsStore.Update_by = sr["Update_by"].ToString();
                    //goodsStore.Update_time = Convert.ToDateTime(sr["Update_time"].ToString());
                    goodsStore.Remark = sr["Remark"].ToString();
                    //goodsStore.Sales = Convert.ToInt16(sr["Sales"].ToString());

                    if (sr["Virtual_sales"].ToString() != null)
                    {
                        // goodsStore.Virtual_sales = Convert.ToInt16(sr["Virtual_sales"].ToString());
                    }
                    list.Add(goodsStore);
                }
                sr.Close();
                cmd.Clone();
                cmd.Dispose();
            }
            cn.Clone();
            cn.Close();
            cn.Dispose();
            return list;
        }
        #endregion

        #region 商品

        /// <summary>
        /// 更新单品库存
        /// </summary>
        /// <param name="AddOrDelete">增获减  true增  false减</param>
        /// <param name="num">数量</param>
        /// <param name="GoodsId">单品Id</param>
        /// <returns></returns>
        public static bool UpdataStock(bool AddOrDelete, int num, string barcode)
        {
            bool mark = false;
            GoodsStore goodsStore = new GoodsStore();
            Int64 NowInventory = -1;//更新后库存
            string Sql = "Select * from smc_goods_store where barcode=" + barcode + ";";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);

            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    goodsStore.Inventory_now = sr["Inventory_now"].ToString();
                    //goodsStore.Inventory_max = sr["Inventory_max"].ToString();
                    goodsStore.Inventory_min = sr["Inventory_min"].ToString();
                   // goodsStore.Sales = Convert.ToInt32(sr["Sales"].ToString());

                    //if (sr["Virtual_sales"].ToString() != null)
                    //{
                    //    goodsStore.Virtual_sales = Convert.ToInt16(sr["Virtual_sales"].ToString());
                    //}
                    if (AddOrDelete)
                    {
                        //增加库存
                        NowInventory = Convert.ToInt64(goodsStore.Inventory_now) + num;
                    }
                    else
                    {
                        //减少库存
                        NowInventory = Convert.ToInt64(goodsStore.Inventory_now) - num;
                    }
                }
                sr.Close();
               
                cmd.Clone();
                cmd.Dispose();
                cn.Close();
                cn.Dispose();
            }
            Thread.Sleep(30);
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE smc_goods_store SET Inventory_now='" + NowInventory + "' WHERE barcode=" + barcode + ";";
                
                int n = cmd_.ExecuteNonQuery();

                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }

        /// <summary>
        /// 新增供应商
        /// </summary>
        /// <param name="SupplierId">供应商Id</param>
        /// <param name="SupplierName">供应商名称</param>
        /// <param name="SupplierPhone">供应商联系方式</param>
        /// <param name="SupplierTrademark">供应品牌名称</param>
        /// <returns></returns>
        public static bool AddSupplier(string SupplierId, string SupplierName, string SupplierPhone, string SupplierTrademark)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_supplier " +
                    "(id,name,phone,Trademark) values (" +
                    SupplierId + ",'" + SupplierName + "','" + SupplierPhone + "','" + SupplierTrademark + "');";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
            }
            return mark;
        }

        /// <summary>
        /// 获取供应商名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string GetGoodsSupplierName(string id)
        {
            string GoodsSupplierName = string.Empty;
            string Sql = "Select * from smc_supplier where id=" + id + ";";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {

                    GoodsSupplierName = sr["name"].ToString();

                }
                sr.Close();
            }
            return GoodsSupplierName;
        }

        /// <summary>
        /// 获取供应商集合
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> GetGoodsSupplier()
        {
            List<Supplier> lt = new List<Supplier>();
            string Sql = "Select * from smc_supplier ;";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Supplier supplier = new Supplier();
                    supplier.Id =Convert.ToInt32( sr["id"].ToString());
                    supplier.Name = sr["name"].ToString();
                    supplier.Phone = sr["Phone"].ToString();
                    supplier.Trademark = sr["trademark"].ToString();
                    lt.Add(supplier);
                }
                sr.Close();
            }
            return lt;
        }



        /// <summary>
        /// 新增类型
        /// </summary>
        /// <param name="Id">类型ID</param>
        /// <param name="Parent_id">父级Id</param>
        /// <param name="Classify_name">类型名称</param>
        /// <param name="Classify_type">分类类型</param>
        /// <param name="classify_icon">类型图标</param>
        /// <param name="classify_title">标题</param>
        /// <param name="sort">排序</param>
        /// <param name="status">状态（0=正常。1=失效）默认0  </param>
        /// <param name="create_by">创建人</param>
        /// <param name="create_time">创建时间</param>
        /// <param name="update_by">更新人</param>
        /// <param name="update_time">更新时间</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public static bool Addclassify(string Id, string Parent_id, string Classify_name, string Classify_type
            , string classify_icon, string classify_title, string sort, string status, string create_by, string create_time, string update_by, string update_time, string remark)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_supplier " +
                    "(id,parent_id,classify_name,classify_type," +
                    "classify_icon,classify_title,sort," +
                    "status,create_by,create_time" +
                    "update_by,update_time,remark) values (" +
                    Id + "," + Parent_id + ",'" + Classify_name + "','" + Classify_type +
                    "','" + classify_icon + "','" + classify_title + "'," + sort +
                    ",'" + status + "','" + create_by + "','" + create_time +
                    "','" + update_by + "','" + update_time + "','" + remark + "');";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
            }
            return mark;
        }

        public static string GetGoodsClassName(string id)
        {
            string GoodsClassName = string.Empty;
            string Sql = "Select * from smc_goods_classify where id=" + id + ";";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsClassName = sr["classify_name"].ToString();
                }
                sr.Close();
            }
            return GoodsClassName;
        }

        /// <summary>
        /// 获取所有商品类型
        /// </summary>
        /// <returns></returns>
        public static List<Goods_Class> GetGoodsClass()
        {
            string GoodsClassName = string.Empty;
            List<Goods_Class> lt = new List<Goods_Class>();
            string Sql = "Select * from smc_goods_classify;";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Goods_Class goods_Class = new Goods_Class();
                    goods_Class.Id = Convert.ToInt32(sr["id"].ToString());
                    goods_Class.Name = sr["classify_name"].ToString();
                    goods_Class.ParentId = Convert.ToInt32( sr["parent_id"].ToString());
                    lt.Add(goods_Class);
                }
                sr.Close();
                cn.Clone();
                cn.Close();
            }
            return lt;
        }

        public static bool InsertGoods(SmcGoodsStore goodsStore)
        {
            bool mark = false;//新增成功标识
            string sql = string.Empty;
            try
            {
                bool ISExist = true;

                SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
                if (cn_.State != System.Data.ConnectionState.Open)
                {
                    cn_.Open();
                    SQLiteCommand cmd_ = cn_.CreateCommand();
                    cmd_.CommandText = "select count(*) as num from smc_goods_store where barcode = '"+ goodsStore.Barcode+"';";
                    SQLiteDataReader sr_ = cmd_.ExecuteReader();
                    while (sr_.Read())
                    {
                        if (sr_["num"].ToString() != null)
                        {          
                            if (Convert.ToInt32(sr_["num"].ToString())>0)
                            {
                                ISExist = false;
                            }
                        }
                        else
                        {
                            ISExist = true;
                        } 
                    }
                    sr_.Close();
                    cmd_.Clone();
                    cmd_.Dispose();
                    cn_.Clone();
                    cn_.Close();
                    cn_.Dispose();
                }

                if (ISExist) { 
                    SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                    if (cn.State != System.Data.ConnectionState.Open)
                    {
                        cn.Open();
                        SQLiteCommand cmd = cn.CreateCommand();
                   
                        sql = "INSERT INTO smc_goods_store " +
                         "(original_price,goods_id,goods_store_id,goods_name,pinyin_code,barcode," +
                         "classify_id,class_name,cost_price," +
                         "sale_price,inventory_now,inventory_min," +
                         "make_date,shelf_life,remark) values (";
                        if (goodsStore.OriginalPrice != null)
                        {
                            sql = sql + goodsStore.OriginalPrice + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.GoodsId != null)
                        {
                            sql = sql + goodsStore.GoodsId + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.GoodsStoreId != null)
                        {
                            sql = sql + goodsStore.GoodsStoreId + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.GoodsName != null)
                        {
                            sql = sql + "'" + goodsStore.GoodsName + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.PinyinCode != null)
                        {
                            sql = sql + "'" + goodsStore.PinyinCode + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.Barcode != null)
                        {
                            sql = sql + "'" + goodsStore.Barcode + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.ClassifyId != null)
                        {
                            sql = sql + goodsStore.ClassifyId + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }


                        if (goodsStore.ClassifyName != null)
                        {
                            sql = sql + "'" + goodsStore.ClassifyName + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.CostPrice != null)
                        {
                            sql = sql + goodsStore.CostPrice + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.SalePrice != null)
                        {
                            sql = sql + goodsStore.SalePrice + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.Wholesale_price != null)
                        {
                            sql = sql + goodsStore.Wholesale_price + ",";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.InventoryNow != null)
                        {
                            sql = sql + "'" + goodsStore.InventoryNow + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.InventoryMin != null)
                        {
                            sql = sql + "'" + goodsStore.InventoryMin + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.MakeDate != null)
                        {
                            sql = sql + "'" + goodsStore.MakeDate + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.ShelfLife != null)
                        {
                            sql = sql + "'" + goodsStore.ShelfLife + "',";
                        }
                        else
                        {
                            sql = sql + "null,";
                        }

                        if (goodsStore.Remark != null)
                        {
                            sql = sql + "'" + goodsStore.Remark + "');";
                        }
                        else
                        {
                            sql = sql + "null);";
                        }

                        cmd.CommandText = sql;
                        int i = cmd.ExecuteNonQuery();
                        mark = i > 0 ? true : false;
                        cmd.Clone();
                        cmd.Dispose();
                        cn.Clone();
                        cn.Close();
                        cn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Local.LOG(sql, 101, ex.ToString());
            }
            return mark;
        }

        public static bool UpdateGoods(SmcGoodsStore goodsStore)
        {
            bool mark = false;//新增成功标识
            string sql = string.Empty;
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                  
                    sql = "UPDATE  smc_goods_store  set " ;
                    
                    if (goodsStore.GoodsId != null)
                    {
                        sql = sql + "goods_id=" + goodsStore.GoodsId + ",";
                    }
                    else
                    {
                        sql = sql + "goods_id=" + "null,";
                    }

                    if (goodsStore.GoodsStoreId != null)
                    {
                        sql = sql + "goods_store_id="+ goodsStore.GoodsStoreId + ",";
                    }
                    else
                    {
                        sql = sql + "goods_store_id=null,";
                    }

                    if (goodsStore.GoodsName != null)
                    {
                        sql = sql + "goods_name='" + goodsStore.GoodsName + "',";
                    }
                    else
                    {
                        sql = sql + "goods_name=null,";
                    }

                    if (goodsStore.PinyinCode != null)
                    {
                        sql = sql + "pinyin_code='" + goodsStore.PinyinCode + "',";
                    }
                    else
                    {
                        sql = sql + "pinyin_code=null,";
                    }

                    if (goodsStore.Wholesale_price != null)
                    {
                        sql = sql + "wholesale_price=" + goodsStore.Wholesale_price + ",";
                    }
                    else
                    {
                        sql = sql + "wholesale_price=null,";
                    }

                    if (goodsStore.Barcode != null)
                    {
                        sql = sql + "barcode='" + goodsStore.Barcode + "',";
                    }
                    else
                    {
                        sql = sql + "barcode=null,";
                    }

                    if (goodsStore.ClassifyId != null)
                    {
                        sql = sql + "classify_id="+ goodsStore.ClassifyId + ",";
                    }
                    else
                    {
                        sql = sql + "classify_id=null,";
                    }


                    if (goodsStore.ClassifyName != null)
                    {
                        sql = sql + "class_name='" + goodsStore.ClassifyName + "',";
                    }
                    else
                    {
                        sql = sql + "class_name=null,";
                    }

                    if (goodsStore.CostPrice != null)
                    {
                        sql = sql + "cost_price="+ goodsStore.CostPrice + ",";
                    }
                    else
                    {
                        sql = sql + "cost_price=null,";
                    }

                    if (goodsStore.SalePrice != null)
                    {
                        sql = sql + "sale_price="+goodsStore.SalePrice + ",";
                        
                    }
                    else
                    {
                        sql = sql + "sale_price=null,";
                    }

                    if (goodsStore.InventoryNow != null)
                    {
                        sql = sql + "inventory_now='" + goodsStore.InventoryNow + "',";
                    }
                    else
                    {
                        sql = sql + "inventory_now=null,";
                    }

                    if (goodsStore.InventoryMin != null)
                    {
                        sql = sql + "inventory_min='" + goodsStore.InventoryMin + "',";
                    }
                    else
                    {
                        sql = sql + "inventory_min=null,";
                    }

                    if (goodsStore.MakeDate != null)
                    {
                        sql = sql + "make_date='" + goodsStore.MakeDate + "',";
                    }
                    else
                    {
                        sql = sql + "make_date=null,";
                    }

                    if (goodsStore.ShelfLife != null)
                    {
                        sql = sql + "shelf_life='" + goodsStore.ShelfLife + "',";
                    }
                    else
                    {
                        sql = sql + "shelf_life=null,";
                    }

                    if (goodsStore.Remark != null)
                    {
                        sql = sql + "remark='" + goodsStore.Remark + "' where goods_id=" + goodsStore.GoodsId + ";";
                    }
                    else
                    {
                        sql = sql + "remark=null where goods_id="+ goodsStore.GoodsId+ ";";
                    }

                    cmd.CommandText = sql;
                    int i = cmd.ExecuteNonQuery();
                    mark = i > 0 ? true : false;
                    cmd.Clone();
                    cmd.Dispose();
                    cn.Clone();
                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log_Local.LOG(sql, 101, ex.ToString());
            }
            return mark;
        }

        public static bool DeleteGoods(string goodsStore)
        {
            bool mark = false;
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "delete from smc_goods_store  where goods_store_id=" + goodsStore+ ";";
                int n = cmd_.ExecuteNonQuery();
                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();
            mark = true;
            return mark;
        }
        #endregion

        #region 收银相关
        /// <summary>
        /// 新增订单
        /// </summary>
        /// <param name="order">订单对象</param>
        /// <returns></returns>
        public static bool AddOrder(Order order)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = "INSERT INTO smc_order " +
                    "(order_number,actual_payment,order_status," +
                    "pay_ment,customer_id,order_source," +
                    "dept_id,return_reason,coupons_id," +
                    "order_type,coupons_price,product_all_price," +
                    "create_by,remark," +
                    "user_id) VALUES('" +
                    order.Order_number + "'," + order.Actual_payment + "," + order.Order_status +
                    ",'" + order.Pay_ment + "','" + order.Customer_id + "','" + order.Order_source +
                    "'," + order.Dept_id + ",'" + order.Return_reason + "','" + order.Coupons_id +
                     "'," + order.Order_type + "," + order.Coupons_price + "," + order.Product_all_price +
                     ",'" + order.Create_by + "','" + order.Remark +
                    "'," + order.User_id + ")";
                int i = cmd.ExecuteNonQuery();
            }
            cn.Close();
            return mark;
        }

        /// <summary>
        /// 新增订单详情
        /// </summary>
        /// <param name="OrderCode">订单编号</param>
        /// <param name="UserId">用户id</param>
        /// <param name="remark">备注</param>
        /// <param name="list">商品集合</param>
        /// <returns></returns>
        public static bool AddOrderInfo(string OrderCode, string UserId, string remark, List<Goodsroder> list)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_order_item " +
                    "(order_number,barcode,goods_name," +
                    "goods_num,product_price,remark) values ";
                string value = string.Empty;
                foreach (Goodsroder goodsStore in list)
                {
                    value = value + "('" + OrderCode + "','" + goodsStore.Barcode + "','" + goodsStore.Name +
                        "'," + goodsStore.Num + "," + goodsStore.Sale_price + ",'" + remark + "'),";

                }
                value = value.Substring(0, value.Length - 1);
                value = value + ";";
                cmd.CommandText = sql + value;
                int i = cmd.ExecuteNonQuery();
                cmd.Clone();
                cmd.Dispose();
            }
            cn.Clone();
            cn.Close();
            cn.Dispose();
            return mark;
        } 
        #endregion

        #region 增加新品
        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="UserId">用户id</param>
        /// <param name="goodsStore">新商品</param>
        /// <returns></returns>
        public static bool AddNewGoods(string UserId, GoodsStore goodsStore)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO smc_goods_store " +
                    "(goods_id,goods_name,pinyin_code,barcode," +
                    "classify_id,supplier_id,cost_price," +
                    "sale_price,inventory_now,inventory_min," +
                    "make_date,shelf_life,remark) values ("+ goodsStore .Goods_id+ ",'" +
                    goodsStore.Goods_name + "','" + goodsStore.Pinyin_code + "','" + goodsStore.Barcode + "'," +
                    goodsStore.Classify_id + "," + goodsStore.Supplier_id + "," + goodsStore.Cost_price + "," +
                    goodsStore.Sale_price + ",'" + goodsStore.Inventory_now + "','" + goodsStore.Inventory_min + "','" +
                    goodsStore.Make_date.ToString("s") + "','" + goodsStore.Shelf_life + "','" + goodsStore.Remark + "');";
                cmd.CommandText = sql;
                int i = cmd.ExecuteNonQuery();
                mark = i > 0 ? true : false;
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        
        #endregion

        #region 入库
        /// <summary>
        /// 新增入库单
        /// </summary>
        /// <param name="stockInOrder">入库单对象</param>
        /// <returns></returns>
        public static bool AddStockInOrder(StockInOrder stockInOrder)
        {
            bool mark = false;//新增成功标识
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = "INSERT INTO smc_order_in_stock " +
                        "(order_code,User_id,orther_order_code," +
                        "all_in_price,all_out_price,all_goods_sum," +
                        "delivery_by,delivery_phone,remark) VALUES('" +
                        stockInOrder.Order_code + "','" + stockInOrder.User_id + "','" + stockInOrder.Orther_order_code +
                        "'," + stockInOrder.All_in_price + "," + stockInOrder.All_out_price + "," + stockInOrder.All_goods_sum +
                        ",'" + stockInOrder.Delivery_by + "','" + stockInOrder.Delivery_phone + "','" + stockInOrder.Remark + "')";
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0) { mark = true; }
                    cmd.Clone(); cmd.Dispose();
                    cn.Close(); cn.Dispose();
                }
            }
            catch (Exception ex)
            {

                Log_Local.LOG("AddStockInOrder（）", 101,"入库单信息插入失败。行号637");
            }


            return mark;
        }

        /// <summary>
        /// 新增入库单详情
        /// </summary>
        /// <param name="orderCode">入库单</param>
        /// <param name="list">详情集合</param>
        /// <returns></returns>
        public static bool AddStockInOrderItem(string orderCode, List<AddGoodsInfo> list)
        {
            bool mark = false;//新增成功标识
            try
            {
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    string value = string.Empty;
                    string sql = "INSERT INTO smc_order " +
                        "(order_code,goodsname,barcode" +
                        "unit,supplier,price_in," +
                        "price_out,number) VALUES('";
                    foreach (AddGoodsInfo addGoodsInfo in list)
                    {
                        value = value + "('" + orderCode + "','" + addGoodsInfo.Name + "','" + addGoodsInfo.Barcode +
                       "','" + addGoodsInfo.Guige + "','" + addGoodsInfo.Supplier1 + "'," + addGoodsInfo.Sale_price +
                       "," + 0 + "," + addGoodsInfo.Num + "),";
                    }
                    value = value.Substring(0, value.Length - 1);
                    value = value + ";";
                    cmd.CommandText = sql + value;
                    int i = cmd.ExecuteNonQuery();
                    if (i > 0) { mark = true; }
                    cmd.Clone();cmd.Dispose();
                    cn.Close(); cn.Dispose();
                }
            }
            catch (Exception ex)
            {
               
                //  cn.Close();
                Log_Local.LOG("", 101, ex.ToString());
            }


            return mark;
        }
        #endregion

        #region 订单

        /// <summary>
        /// 获取现金统计数据
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static string GetCasheNum(string userid,bool mark)
        {
            string num = "";

            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);

                string Sql = "Select count(order_number) as allnum,SUM(actual_payment) as allSum from smc_order where create_time >='" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and pay_ment='现金' and user_id="+userid+";";
                if (mark) {Sql= "Select count(order_number) as allnum,SUM(actual_payment) as allSum from smc_order where create_time >= '" +
                    dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" +
                    yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and pay_ment='现金'"; }
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {

                        try
                        {
                            int Num = Convert.ToInt32(sr["allnum"].ToString());
                            double Sum = Convert.ToDouble(sr["allSum"].ToString());
                            num = Num.ToString() + "_" + Sum.ToString();
                        }
                        catch (Exception ex)
                        {
                            num = "0_0";
                        }



                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }




            return num;
        }

        /// <summary>
        /// 获取今日销售总数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string GetProductAllPrice(string UserId, bool mark)
        {
            string GoodsClassName = string.Empty;
            DateTime dt = DateTime.Now;
            DateTime yd = DateTime.Now.AddDays(1);
            string Sql = "Select  COUNT(1) as num , SUM(A.product_all_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' and user_id=" + UserId + ";";
            if (mark)
            {
                Sql = "Select  COUNT(1) as num , SUM(A.product_all_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00';";
            }
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsClassName = sr["price_sum"].ToString() + "_" + sr["num"].ToString();
                }
                sr.Close();
                cn.Close();
            }
            return GoodsClassName;
        }

        /// <summary>
        /// 获取今日优惠总数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string GetCouponAllPrice(string UserId, bool mark)
        {
            string GoodsClassName = string.Empty;
            DateTime dt = DateTime.Now;
            DateTime yd = DateTime.Now.AddDays(1);
            string Sql = "Select  COUNT(1) as num , SUM(A.coupons_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and coupons_price > 0 and user_id=" + UserId + ";";
            if (mark)
            {
                Sql =  "Select  COUNT(1) as num , SUM(A.coupons_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and coupons_price > 0 ;";
            }
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsClassName = sr["price_sum"].ToString() + "_" + sr["num"].ToString();
                }
                sr.Close();
                cn.Close();
            }
            return GoodsClassName;
        }

        /// <summary>
        /// 获取今日会员优惠总数据
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static string GetVIPAllPrice(string UserId, bool mark)
        {
            string GoodsClassName = string.Empty;
            DateTime dt = DateTime.Now;
            DateTime yd = DateTime.Now.AddDays(1);
            string Sql = "Select  COUNT(1) as num , SUM(A.vip_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "'  and vip_price > 0 and user_id=" + UserId + ";";
            if (mark)
            {
                Sql = "Select  COUNT(1) as num , SUM(A.vip_price) as price_sum from smc_order as A where create_time >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "'  and vip_price > 0 ;";
            }
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsClassName = sr["price_sum"].ToString() + "_" + sr["num"].ToString();
                }
                sr.Close();
                cn.Close();
            }
            return GoodsClassName;
        }

        /// <summary>
        /// 获取销售报表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<SalesReport> GetSalesReport(string UserId)
        {
            List<SalesReport> ts = new List<SalesReport>();

            DateTime dt = DateTime.Now;
            DateTime yd = DateTime.Now.AddDays(1);
            //string Sql = "select F.goods_name,F.num,F.sum ,G.classify_name from smc_goods_classify as G " +
            //    "join (select  D.goods_name,D.num,D.sum ,E.classify_id from smc_goods_store as E " +
            //    "join(select C.goods_name, SUM(C.goods_num) as num, SUM(C.product_price) as sum, C.barcode from smc_order_item as C " +
            //    "join (Select order_number from smc_order as A where create_time >= '" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time <= '" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and user_id = " + UserId + ") as B " +
            //    "on B.order_number = C.order_number group by C.goods_name)  as D on D.barcode = E.barcode group by D.goods_name) as F ON G.id=F.classify_id  ;";


            string Sql = "select  D.goods_name,D.num,D.sum ,E.class_name from smc_goods_store as E " +
    "join(select C.goods_name, SUM(C.goods_num) as num, product_price as sum, C.barcode from smc_order_item as C " +
    "join (Select order_number from smc_order as A where create_time >= '" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time <= '" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and user_id = " + UserId + ") as B " +
    "on B.order_number = C.order_number group by C.goods_name)  as D on D.barcode = E.barcode group by D.goods_name ;";

            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    SalesReport salesReport = new SalesReport();
                    salesReport.Name = sr["goods_name"].ToString();
                    salesReport.ClassName = sr["class_name"].ToString();
                    salesReport.Num = Convert.ToInt32(sr["num"].ToString());
                    salesReport.Sum = Convert.ToDouble(sr["sum"].ToString());
                    ts.Add(salesReport);
                }
                sr.Close();
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return ts;
        }

        /// <summary>
        /// 根据时间获取销售单据
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="star">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static List<Order> GetSalesReport(DateTime star, DateTime end, string Word)
        {
            List<Order> ts = new List<Order>();
            DateTime dt = star;
            DateTime yd = end;
            string Sql = " Select order_number,pay_ment,product_all_price ,order_status,create_time,actual_payment,customer_id from smc_order as A   where create_time >= '" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time <= '" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' order by A.create_time desc";
            if (!Word.Equals("")) 
            {
                Sql = " Select order_number,pay_ment ,product_all_price,order_status,create_time,actual_payment,customer_id from smc_order as A   where order_number like '%" + Word+"%' ;";
            }
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    Order salesReport = new Order();
                    salesReport.Order_number = sr["order_number"].ToString();
                    salesReport.Customer_id = sr["customer_id"].ToString();
                    salesReport.Order_status = Convert.ToInt32(sr["order_status"].ToString());
                    salesReport.Actual_payment = Convert.ToDouble(sr["actual_payment"].ToString());
                    salesReport.Payment_time = Convert.ToDateTime(sr["create_time"].ToString());
                    salesReport.Pay_ment = sr["pay_ment"].ToString();
                    string asd = sr["product_all_price"].ToString();
                    salesReport.Product_all_price= Convert.ToDouble(sr["product_all_price"].ToString());
                    salesReport.Return_reason = salesReport.Order_status == 1 ? "退" : "结";
                    
                    ts.Add(salesReport);
                }
                sr.Close();
                cn.Close();
            }
            if (ts.Count == 0)
            {
                return null;
            }
            else
            {
                return ts;
            }
        }

        /// <summary>
        /// 根据单号获取订单详情
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static List<OrderItems> GetSalesReportItems(string order,string zhekou)
        {
            List<OrderItems> ts = new List<OrderItems>();
            string Sql = " Select * from smc_order_item  where order_number='" + order + "';";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    OrderItems salesReport = new OrderItems();
                    salesReport.Goods_name = sr["goods_name"].ToString();
                    salesReport.Product_price = Convert.ToDouble(sr["product_price"].ToString());
                    salesReport.Goods_num = Convert.ToInt32(sr["goods_num"].ToString());
                    salesReport.Barcode = sr["barcode"].ToString();
                    salesReport.Remark = sr["remark"].ToString();
                    if (!zhekou.Equals("1.00"))
                    {
                        salesReport.Zhekou = zhekou;
                    }
                    else
                    {
                        salesReport.Zhekou = "";
                    }
                    ts.Add(salesReport);
                }
                sr.Close();
                cn.Close();
            }
            return ts;
        }

        /// <summary>
        /// 退单
        /// </summary>
        /// <param name="order">订单号</param>
        /// <returns></returns>
        public static bool TuiDan(string order)
        {
            bool mark = false;
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE smc_order SET order_status=1 WHERE order_number= '" + order + "';";
                int n = cmd_.ExecuteNonQuery();
                
                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();
            mark = true;
            return mark;

        }




        #endregion

        #region 库存

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="Word">关键字</param>
        /// <returns></returns>
        public static List<GoodsStore> GetKunCun(DateTime start, DateTime end, string Word)
        {
            List<GoodsStore> ts = new List<GoodsStore>();
            DateTime dt = start;
            DateTime yd = end;

            string Sql_Word = " select * from " +
                "(select * from smc_goods_store as D " +
                "join  (Select C.barcode  from smc_order_item as C " +
                "join (Select A.order_number from smc_order as A where create_time >= '" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time <= '" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00') as B " +
                "on C.order_number=B.order_number group by C.barcode) as E " +
                "on d.barcode=e.barcode   group by d.barcode) as h " +
                " where h.barcode like '%" + Word + "%' or h.goods_name like '%" + Word + "%' or h.pinyin_code like '%" + Word + "%'";

            string Sql = "select * from smc_goods_store as D join  " +
                "(Select C.barcode  from smc_order_item as C join " +
                "(Select A.order_number from smc_order as A where create_time >= '" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00" + "' and create_time <= '" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00') as B " +
                "on C.order_number=B.order_number group by C.barcode) as E on d.barcode=e.barcode   group by d.barcode";

            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                if (!Word.Equals(""))
                {
                    cmd.CommandText = Sql_Word;
                }
                else
                {
                    cmd.CommandText = Sql;
                }

                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    GoodsStore goodsroder = new GoodsStore();
                    goodsroder.Barcode = sr["barcode"].ToString();
                    if (goodsroder.Barcode.Equals("0000000"))
                    {
                        continue;
                    }
                    goodsroder.Inventory_now = sr["inventory_now"].ToString();
                    goodsroder.Goods_name = sr["goods_name"].ToString();
                    goodsroder.Spec_data = sr["spec_data"].ToString();
                    if (!sr["sale_price"].ToString().Equals("") && sr["sale_price"].ToString() != null)
                    {
                        goodsroder.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                    }
                    else
                    {
                        goodsroder.Sale_price = 0;
                    }
                   
                    ts.Add(goodsroder);
                }
                sr.Close();
                cn.Close();
            }
            return ts;
        }

        public static List<GoodsStore> GetKunCun(string Word)
        {
            List<GoodsStore> ts = new List<GoodsStore>();
            
            string Sql = "Select * from smc_goods_store where barcode like '%" + Word + "%' or goods_name like '%" + Word + "%' or pinyin_code like '%" + Word + "%'";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            try
            {
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    try
                    {
                        while (sr.Read())
                        {

                            GoodsStore goodsroder = new GoodsStore();
                            goodsroder.Barcode = sr["barcode"].ToString();
                            goodsroder.Inventory_now = sr["inventory_now"].ToString();
                            goodsroder.Goods_name = sr["goods_name"].ToString();
                            goodsroder.Spec_data = sr["spec_data"].ToString();
                            goodsroder.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                            ts.Add(goodsroder);
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                    cn.Clone();

                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return ts;
        }

        /// <summary>
        /// 分页查询库存
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static List<GoodsStore> GetSomeKunCun(int page)
        {
            List<GoodsStore> ts = new List<GoodsStore>();


            string Sql_Word = " select * from smc_goods_store order by inventory_now desc limit " + page+ " ,"+(page+20)+";";


            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql_Word;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    try
                    {
                        GoodsStore goodsroder = new GoodsStore();
                        goodsroder.Barcode = sr["barcode"].ToString();
                        goodsroder.Inventory_now = sr["inventory_now"].ToString();
                        goodsroder.Goods_name = sr["goods_name"].ToString();
                        goodsroder.Spec_data = sr["spec_data"].ToString();
                        goodsroder.Sale_price = Convert.ToDouble(sr["sale_price"].ToString());
                        ts.Add(goodsroder);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                sr.Close();
                cn.Close();
            }
            return ts;
        }

        /// <summary>
        /// 库存预警
        /// </summary>
        /// <returns></returns>
        public static List<GoodsStore> GetKunCunYuJing()
        {
            List<GoodsStore> ts = new List<GoodsStore>();
            string Sql = "select goods_name,barcode,inventory_now,inventory_min from smc_goods_store order by create_time desc  ;" +
                "";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    try
                    {
                        GoodsStore goodsroder = new GoodsStore();
                        goodsroder.Barcode = sr["barcode"].ToString();
                        goodsroder.Inventory_now = sr["inventory_now"].ToString();
                        goodsroder.Goods_name = sr["goods_name"].ToString();
                        goodsroder.Inventory_min = sr["inventory_min"].ToString();
                        if (Convert.ToDouble(goodsroder.Inventory_now) <= Convert.ToDouble(goodsroder.Inventory_min))
                        {
                            ts.Add(goodsroder);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                sr.Close();
                cn.Close();
            }
            return ts;
        }

        /// <summary>
        /// 单个预警
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool GetKunCunYuJing(string code)
        {
            bool mark = false;
            string Sql = "select goods_name,barcode,inventory_now,inventory_min from smc_goods_store where  barcode='"+ code + "';" +
                "";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    try
                    {
                        GoodsStore goodsroder = new GoodsStore();
                        goodsroder.Barcode = sr["barcode"].ToString();
                        goodsroder.Inventory_now = sr["inventory_now"].ToString();
                        goodsroder.Goods_name = sr["goods_name"].ToString();
                        goodsroder.Inventory_min = sr["inventory_min"].ToString();
                        if (Convert.ToDouble(goodsroder.Inventory_now) <= Convert.ToDouble(goodsroder.Inventory_min))
                        {
                            mark = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                sr.Close();
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        /// <summary>
        /// 获取商品数量
        /// </summary>
        /// <returns></returns>
        public static int GetGoodsCount()
        {
            int num = 0;
            string Sql = " select count(goods_name) as sum from smc_goods_store";
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                cmd.CommandText = Sql;
                SQLiteDataReader sr = cmd.ExecuteReader();
                while (sr.Read())
                {
                    try
                    {
                       num = Convert.ToInt32( sr["sum"].ToString());                   
                    }
                    catch (Exception ex)
                    {
                    }
                }
                sr.Close();
                cn.Close();
            }
            return num;
        }


        #endregion

        #region 线上订单

        public static List<smc_order> GetOnlineOrder()
        {
            List<smc_order> list = new List<smc_order>();
            
            try
            {
                DateTime dt = DateTime.Now;
                DateTime yd = DateTime.Now.AddDays(1);
               
                string Sql = "Select * from Online_Order where creattime >='" + dt.Date.ToString("yyyy-MM-dd") + " 00:00:00' and creattime<='" + yd.Date.ToString("yyyy-MM-dd") + " 00:00:00' ;";

                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);

                if (cn.State != System.Data.ConnectionState.Open)
                {

                    cn.Open();

                    SQLiteCommand cmd = cn.CreateCommand();

                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        smc_order order = new smc_order();
                        order.ActualPayment = Convert.ToDouble(sr["Sum"].ToString());
                        order.Id = sr["Id"].ToString();
                        order.OrderNumber = sr["ordercode"].ToString();
                        order.StrType = sr["type"].ToString();
                        order.NickName = sr["username"].ToString();
                        order.CustomerPhone = sr["Phone"].ToString();
                        order.DeliveryDate = sr["GetTime"].ToString();
                        order.Status = sr["statu"].ToString();

                        list.Add(order);
                    }
                    sr.Close();
                    cmd.Clone();
                    cmd.Dispose();
                }
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }
            return list;
        }

        /// <summary>
        /// 增加线上订单
        /// </summary>
        /// <param name="id">订单id  </param>
        /// <param name="orderType">订单类型</param>
        /// <param name="ordercode">订单编码</param>
        /// <param name="Sum">订单金额</param>
        /// <param name="userId">会员id</param>
        /// <param name="username">会员名</param>
        /// <param name="phone">电话</param>
        /// <param name="GetTime">配送日期</param>
        /// <returns></returns>
        public static bool AddOnlineOrder(string id, string orderType, string ordercode, 
            string Sum, string userId, string username,  string phone, string GetTime)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO Online_Order " +
                    "(id,userId,type,ordercode,Sum,username,Phone,GetTime) values ('" +
                    id + "','" + userId + "','" + orderType + "','" + ordercode +
                    "'," + Sum + ",'" + username + "','" + phone + "','" + GetTime + "');";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        /// <summary>
        /// 新增线上订单明细
        /// </summary>
        /// <param name="OnlineOrderId">订单编号</param>
        /// <param name="barcode">条形码</param>
        /// <param name="SalePrice">价格</param>
        /// <param name="Num">数量</param>
        /// <returns></returns>
        public static bool AddOnlineOrderItem(string OnlineOrderId, string barcode, string SalePrice, string Num)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO Online_Order_item " +
                   "(OnlineOrderId,barcode,SalePrice,Num) values ('" +
                   OnlineOrderId + "','" + barcode + "'," + SalePrice + "," + Num + "');";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
                cmd.Clone();
                cmd.Dispose();
                cn.Clone();
                cn.Close();
                cn.Dispose();
            }
            return mark;
        }

        /// <summary>
        /// 更新线上订单状态
        /// </summary>
        /// <param name="id">线上订单Id</param>
        /// <param name="statu">状态</param>
        /// <param name="casherId">备货人</param>
        /// <returns></returns>
        public static bool UpdataOnlineOrder(string id, string statu, string casherId)
        {
            bool mark = false;
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE Online_Order SET statu=" + statu + " WHERE ordercode='" + id + "';";
                int n = cmd_.ExecuteNonQuery();
                mark = true;
                cmd_.Clone();
                cmd_.Dispose();
                cn_.Clone();
                cn_.Close();
                cn_.Dispose();
            }
            
            return mark;
        }
        #endregion

        /// <summary>
        /// 添加服务端消息
        /// </summary>
        /// <param name="messageId">消息Id</param>
        /// <param name="deviceId">设备Id</param>
        /// <param name="serviceType">操作类型</param>
        /// <param name="data">数据</param>
        /// <param name="sign">签名</param>
        /// <param name="Mark">是否已处理 0为 未处理  1为已处理</param>
        /// <returns></returns>
        public static bool AddServiceMQ(string messageId, string deviceId, string serviceType, string data,string sign,string Mark)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO ActiveMQ_Service " +
                    "(messageId,deviceId,serviceType,data,sign,Mark) values ('" +
                    messageId + "','" + deviceId + "'," + serviceType + ",'" + data +"','"+ sign + "',"+ Mark + ");";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
            }
            return mark;
        }

        /// <summary>
        /// 添加本地消息
        /// </summary>
        /// <param name="messageId">消息Id</param>
        /// <param name="deviceId">设备Id</param>
        /// <param name="serviceType">操作类型</param>
        /// <param name="data">数据</param>
        /// <param name="sign">签名</param>
        /// <param name="Mark">是否成功消费</param>
        /// <returns></returns>
        public static bool AddProducerMQ(string messageId, string deviceId, string serviceType, string data, string sign, string Mark)
        {
            bool mark = false;//新增成功标识
            SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
            if (cn.State != System.Data.ConnectionState.Open)
            {
                cn.Open();
                SQLiteCommand cmd = cn.CreateCommand();
                string sql = "INSERT INTO ActiveMQ_Producer " +
                    "(messageId,deviceId,serviceType,data,sign,Mark) values ('" +
                    messageId + "','" + deviceId + "'," + serviceType + ",'" + data + "','" + sign + "'," + Mark + ");";
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    mark = true;
                }
            }
            return mark;
        }

        /// <summary>
        /// 更新服务端消息状态
        /// </summary>
        /// <param name="messageId">消息ID</param>
        /// <param name="Mark"></param>
        /// <returns></returns>
        public static bool UpdataServiceMQ(string messageId, string Mark)
        {
            bool mark = false;
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE ActiveMQ_Service SET Mark=" + Mark + " WHERE Id=" + messageId + ";";
                int n = cmd_.ExecuteNonQuery();
                cmd_.Clone();
            }
            cn_.Close();
            mark = true;
            return mark;
        }

        /// <summary>
        /// 更新本地消息状态
        /// </summary>
        /// <param name="messageId">消息Id</param>
        /// <param name="Mark"></param>
        /// <returns></returns>
        public static bool UpdataProducer(string messageId, string Mark)
        {
            bool mark = false;
            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "UPDATE ActiveMQ_Producer SET Mark=" + Mark + " WHERE Id=" + messageId + ";";
                int n = cmd_.ExecuteNonQuery();
                cmd_.Clone();
            }
            cn_.Close();
            mark = true;
            return mark;
        }

        /// <summary>
        /// 获取未处理服务端消息
        /// </summary>
        /// <returns></returns>
        public static MessageObj GetServiceMQ()
        {
            MessageObj obj = new MessageObj();
            try
            {
                string Sql = "Select * from ActiveMQ_Service where Mark=0 limit 1;";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        obj.MessageId= sr["messageId"].ToString();
                        obj.ServiceType = sr["serviceType"].ToString();
                        obj.Data = sr["Data"].ToString();
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }
            return obj;
        }

        /// <summary>
        /// 获取处理失败的本地消息
        /// </summary>
        /// <returns></returns>
        public static MessageObj GetProducerMQ()
        {
            MessageObj obj = new MessageObj();
            try
            {
                string Sql = "Select * from ActiveMQ_Producer where Mark=0 limit 1;";
                SQLiteConnection cn = new SQLiteConnection("data source=" + DB_PATH);
                if (cn.State != System.Data.ConnectionState.Open)
                {
                    cn.Open();
                    SQLiteCommand cmd = cn.CreateCommand();
                    cmd.CommandText = Sql;
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    while (sr.Read())
                    {
                        obj.MessageId = sr["messageId"].ToString();
                        obj.ServiceType = sr["serviceType"].ToString();
                        obj.Data = sr["Data"].ToString();
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Log_Local.LOG("", 101, ex.ToString());
                return null;
            }
            return obj;
        }

        /// <summary>
        /// 清空商品表
        /// </summary>
        /// <returns></returns>
        public static bool DeleteGoods()
        {
            bool mark = false;

            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "delete  from smc_goods_store where goods_store_id>0;";



                int n = cmd_.ExecuteNonQuery();
                
                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }

        /// <summary>
        /// 清空用户表
        /// </summary>
        /// <returns></returns>
        public static bool DeleteUser()
        {
            bool mark = false;

            SQLiteConnection cn_ = new SQLiteConnection("data source=" + DB_PATH);
            if (cn_.State != System.Data.ConnectionState.Open)
            {
                cn_.Open();
                SQLiteCommand cmd_ = cn_.CreateCommand();
                cmd_.CommandText = "delete  from sys_user where rowid>0;";



                int n = cmd_.ExecuteNonQuery();

                cmd_.Clone();
                cmd_.Dispose();
            }
            cn_.Close();
            cn_.Dispose();

            mark = true;
            return mark;
        }

    }






}
