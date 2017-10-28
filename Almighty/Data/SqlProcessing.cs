using Almighty.Files;
using Almighty.Table;
using Almighty.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almighty.Data
{
    /// <summary>
    /// 错误的写法，暂时找不到更好的方式
    /// </summary>
    public class SqlData
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string connStr { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string honeybee { get; set; }
    }
    public class SqlProcessing<T> where T : new()
    {
        public static string Flie_Path = System.IO.Directory.GetCurrentDirectory() + "\\Data\\DATA.accdb";//当前路径
        public static string PassWD = null;//获取数据库密码
        private static DataTable dt;
        private static string connStr;//= String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Jet OLEDB:Database Password ={1}", Flie_Path, PassWD);//;
        private static OleDbConnection con = new OleDbConnection(connStr);   // TODO: 在此处添加构造函数逻辑

        public static object TextProcessing { get; private set; }

        private static void Init()
        {
            SqlData sd = new SqlData();
            string data = "";
            XmlOperate<SqlData>.DSerialize(ref sd, "Config.xml", ref data);
            PassWD = Encryption.SuperDesDecrypt(sd.honeybee, "zjsxzsta", "zjsxzstb");
            connStr = String.Format(Encryption.SuperDesDecrypt(sd.connStr, "zjsxzsta", "zjsxzstb"), Flie_Path, PassWD);
            con = new OleDbConnection(connStr);
            //SqlProcessing<SqlData>.Init(sd.connStr, sd.honeybee);
        }
        public static IList<T> ExeQuerys(string sql)
        {
            Init();
            OleDbDataAdapter oda = new OleDbDataAdapter(sql, con);
            dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
                return Transform<T>.DataTableToList(dt);
            return new List<T>();
        }
        /// <summary>
        /// 获取当前表
        /// </summary>
        /// <returns></returns>
        private static string GetT()
        {
            string[] Temp = typeof(T).ToString().Split('.');
            return Temp[Temp.Length - 1];
        }
        /// <summary>
        /// 基础分页
        /// </summary>
        /// <param name="num">单页显示数量</param>
        /// <param name="start">从第几条开始获取</param>
        /// <returns></returns>
        public static IList<T> Paging(int? num = 50, int start = 0)
        {
            string sql = "select top 50 * from " + GetT() + " where > (select max(id) from(select top "
                        + start.ToString() + " id from " + GetT() + " order by id)) order by id";
            return ExeQuerys(sql);

        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="num">单页显示数量,默认50</param>
        /// <param name="start">从第几条开始获取</param>
        /// <param name="IdName">ID名称(ID只限数字型),默认为ID</param>
        /// <param name="Where">筛选条件</param>
        /// <param name="OrderBy">根据那列排序,默认为ID</param>
        /// <returns></returns>
        public static IList<T> Paging(int num, int start, string IdName, string Where, string OrderBy)
        {
            if (string.IsNullOrWhiteSpace(OrderBy))
                OrderBy = "id";
            if (string.IsNullOrWhiteSpace(IdName))
                IdName = "id";
            if (num == 0)
                num = 50;
            Where = HandleWhere(Where);
            string sql = "select top 50 * from " + GetT() + " where " + IdName + "> (select max(id) from(select top "
                        + start.ToString() + " " + IdName + " from " + GetT() + " order by " + IdName + "))";
            if (string.IsNullOrWhiteSpace(Where))
                sql += "order by " + OrderBy;
            else
                sql += Where + " order by " + OrderBy;
            return ExeQuerys(sql);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="num">单页显示数量,默认50</param>
        /// <param name="start">从第几条开始获取</param>
        /// <param name="Where">筛选条件</param>
        /// <returns></returns>
        public static IList<T> Paging(int num, int start, string Where)
        {
            if (num == 0)
                num = 50;
            Where = HandleWhere(Where);
            string sql = "select top 50 * from " + GetT() + " where id> (select max(id) from(select top "
                        + start.ToString() + " id from " + GetT() + " order by id)) ";
            if (string.IsNullOrWhiteSpace(Where))
                sql += Where + "order by id";
            return ExeQuerys(sql);
        }
        /// <summary>
        /// where条件处理
        /// </summary>
        /// <param name="Where">where条件</param>
        /// <returns></returns>
        private static string HandleWhere(string Where)
        {
            try
            {
                if (Where.Substring(0, 3).ToLower() != "and")
                    Where = "and" + Where;
                else if (Where.ToLower() == "and")
                    Where = "";
            }
            catch (Exception ex)
            {
                if (Where.ToLower() == "and")
                    Where = "";
                else if (Where.ToLower() != "and")
                    Where = "and" + Where;
            }
            return Where;
        }


    }
    public class SqlProcessing
    {
        public static string Flie_Path = System.IO.Directory.GetCurrentDirectory() + "\\Data\\DATA.accdb";//当前路径
        public static string PassWD = null;//获取数据库密码
        private static DataTable dt;
        private static string connStr;//= String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}; Jet OLEDB:Database Password ={1}", Flie_Path, PassWD);//;
        private static OleDbConnection con = new OleDbConnection(connStr);   // TODO: 在此处添加构造函数逻辑
        private static void Init()
        {
            SqlData sd = new SqlData();
            string data = "";
            XmlOperate<SqlData>.DSerialize(ref sd, "Config.xml", ref data);
            PassWD = Encryption.SuperDesDecrypt(sd.honeybee, "abcdefgh", "abcdefgh");
            connStr = String.Format(Encryption.SuperDesDecrypt(sd.connStr, "abcdefgh", "abcdefgh"), Flie_Path, PassWD);
            con = new OleDbConnection(connStr);
            //SqlProcessing<SqlData>.Init(sd.connStr, sd.honeybee);
        }

        public static DataTable ExeQuery(String sql)//查询
        {
            Init();
            //if (con.State == ConnectionState.Closed)
            //    con.Open();
            OleDbDataAdapter oda = new OleDbDataAdapter(sql, con);
            dt = new DataTable();
            oda.Fill(dt);
            con.Close();
            return dt;
        }
        public bool ExeNoQuery(String sql)//添加、修改、删除
        {
            Init();
            if (con.State == ConnectionState.Closed)
                con.Open();
            OleDbCommand cmd = new OleDbCommand(sql, con);
            if (cmd.ExecuteNonQuery() > 0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }
        public static bool DiaoYongShiWu(String[] sql)//事务调用
        {
            Init();
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            OleDbTransaction trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
            OleDbCommand cmd = new OleDbCommand();
            cmd.Transaction = trans;
            cmd.Connection = con;
            try
            {
                foreach (String s in sql)
                {
                    cmd.CommandText = s;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                trans.Rollback();
                con.Close();
                return false;
            }
        }
        /// <summary>
        /// 基础分页
        /// </summary>
        /// <param name="num">单页显示数量</param>
        /// <param name="start">从第几条开始获取</param>
        /// <returns></returns>
        public static DataTable DTPaging(string Table, int? num = 50, int start = 0)
        {
            string sql = "select top 50 * from " + Table + " where > (select max(id) from(select top "
                      + start.ToString() + " id from " + Table + " order by id)) order by id";
            return ExeQuery(sql);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="num">单页显示数量,默认50</param>
        /// <param name="start">从第几条开始获取</param>
        /// <param name="IdName">ID名称(ID只限数字型),默认为ID</param>
        /// <param name="Where">筛选条件</param>
        /// <param name="OrderBy">根据那列排序,默认为ID</param>
        /// <returns></returns>
        public static DataTable DTPaging(string Table, int num, int start, string IdName, string Where, string OrderBy)
        {
            if (string.IsNullOrWhiteSpace(OrderBy))
                OrderBy = "id";
            if (string.IsNullOrWhiteSpace(IdName))
                IdName = "id";
            if (num == 0)
                num = 50;
            Where = HandleWhere(Where);
            string sql = "select top 50 * from " + Table + " where " + IdName + "> (select max(id) from(select top "
                        + start.ToString() + " " + IdName + " from " + Table + " order by " + IdName + "))";
            if (string.IsNullOrWhiteSpace(Where))
                sql += "order by " + OrderBy;
            else
                sql += Where + " order by " + OrderBy;
            return ExeQuery(sql);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="num">单页显示数量,默认50</param>
        /// <param name="start">从第几条开始获取</param>
        /// <param name="Where">筛选条件</param>
        /// <returns></returns>
        public static DataTable DTPaging(string Table, int num, int start, string Where)
        {
            if (num == 0)
                num = 50;
            Where = HandleWhere(Where);
            string sql = "select top 50 * from " + Table + " where id> (select max(id) from(select top "
                        + start.ToString() + " id from " + Table + " order by id)) ";
            if (string.IsNullOrWhiteSpace(Where))
                sql += Where + "order by id";
            return ExeQuery(sql);
        }
        /// <summary>
        /// where条件处理
        /// </summary>
        /// <param name="Where">where条件</param>
        /// <returns></returns>
        private static string HandleWhere(string Where)
        {
            try
            {
                if (Where.Substring(0, 3).ToLower() != "and")
                    Where = "and" + Where;
                else if (Where.ToLower() == "and")
                    Where = "";
            }
            catch (Exception ex)
            {
                if (Where.ToLower() == "and")
                    Where = "";
                else if (Where.ToLower() != "and")
                    Where = "and" + Where;
            }
            return Where;
        }
    }
}
