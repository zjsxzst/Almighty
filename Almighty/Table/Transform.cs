using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//|------------------------------------------------------------|
//|                    基础信息：                              |
//|     名称：Transform                                        |
//|     功能：类型转换                                         |
//|     最后修改时间：2017/10/27                               |
//|------------------------------------------------------------|
namespace Almighty.Table
{
    public class Transform<T> where T : new()
    {
        /// <summary>
        /// DataTable转List
        /// </summary>
        /// <param name="dt">数据</param>
        /// <returns></returns>
        public static IList<T> DataTableToList(DataTable dt)
        {
            // 定义集合    
            IList<T> ts = new List<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;

        }
    }
}
