using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace WeChat_Committee.Uitl
{
    /// <summary>
    /// 关于一些通用的类
    /// </summary>
    public class GeneralUtil
    {
        /// <summary>
        /// 通过DataRow 填充实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T GetModelByDataRow<T>(System.Data.DataRow dr) where T : new()
        {
            T model = new T();
            foreach (PropertyInfo pInfo in model.GetType().GetProperties())
            {
                object val = getValueByColumnName(dr, pInfo.Name);
                pInfo.SetValue(model, val, null);
            }
            return model;
        }

        /// <summary>
        /// 根据datarow中的列标签取值
        /// </summary>
        /// <param name="dr">datarow</param>
        /// <param name="columnName">列名</param>
        /// <returns></returns>
        public static object getValueByColumnName(System.Data.DataRow dr, string columnName)
        {
            if (dr.Table.Columns.IndexOf(columnName) >= 0)
            {
                if (dr[columnName] == DBNull.Value)
                    return null;
                return dr[columnName];
            }
            return null;
        }


        #region 将一个数据行转换为一个以列名为名称的字典集
        /// <summary>
        /// 将一个数据行转换为一个以列号为名称的字典集
        /// </summary>
        /// <param name="dr">数据行</param>
        /// <returns>两个情况 没有值时返回null，否则返回一个字典</returns>
        public static Dictionary<string, object> DataRowTODictionary(DataRow dr)
        {
            Dictionary<string, object> dic = null;
            if (dr != null && dr.Table.Columns.Count > 0)
            {
                dic = new Dictionary<string, object>();
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    string columnname = dr.Table.Columns[i].ColumnName;
                    object value = dr[columnname];
                    dic.Add(columnname, value);
                }
            }
            return dic;
        }
        #endregion

        #region 将dataset的数据集转换成list
        /// <summary>
        /// 将dataset的数据集转换成list
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>两种情况，一为null,二为填了数据的list<dictionary<string,object>></returns>
        public static List<Dictionary<string, object>> DataSetToList(DataSet ds)
        {
            List<Dictionary<string, object>> list = null;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                list = new List<Dictionary<string, object>>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic = DataRowTODictionary(ds.Tables[i].Rows[j]);
                        list.Add(dic);
                    }
                }
            }
            return list;
        }
        #endregion
    }
}