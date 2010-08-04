using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.Common;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography;

namespace chuiwenchiu {
	/// <summary>
	/// 
	/// </summary>
    /// <example>
    /// public virtual IList<EC_Type> SelectListByAll(string order, string fieldList, bool useCache) {
    ///     string cacheKey = string.Format("EC_TypeCache_SelectListByAll_{0}_{1}", order, fieldList); IList<EC_Type> list = null;
    ///     if (useCache) {
    /// 	list = (IList<EC_Type>)Cache2.Get(cacheKey);
    /// 	if (list != null) return list;
    ///     }
    /// 
    ///     DbParameter[] dbParams = {
    /// 	Data.MakeInParam("@Fields", (DbType)SqlDbType.VarChar, 1000, fieldList),
    /// 	Data.MakeInParam("@Order", (DbType)SqlDbType.VarChar, 100, order),
    ///     };
    ///     list = Data.GetDbDataReader("EC_Type_SelectListByAll", dbParams).ToList<EC_Type>();
    ///     if (useCache) Cache2.Insert(cacheKey, list, cacheSeconds);
    ///     return list;
    /// }
    /// </example>
    public static class IDataReaderExtensions {

        public static List<TResult> ToList<TResult>(this IDataReader dr) where TResult : class,new() {   
            List<TResult> list= new List<TResult>();
            if (dr == null) return list;
            int len = dr.FieldCount;

            while (dr.Read()) { 
                TResult info = new TResult();
                for (int j = 0; j < len; j++) {
                    if (dr[j] == null || string.IsNullOrEmpty(dr[j].ToString())) continue;
                    System.Reflection.PropertyInfo pi = info.GetType().GetProperty(dr.GetName(j).Trim());
                    pi.SetValue(info, dr[j], null);
                }
                list.Add(info);
            }
            dr.Close(); dr.Dispose(); dr = null;
            return list;
        }   

        public static T Get<T>(this IDataReader reader, string field) {
            return reader.Get<T>(field, default(T));
        }
        public static T Get<T>(this IDataReader reader, string field, T defaultValue) {
            var value = reader[field];
            if(value == DBNull.Value) return defaultValue;
            return value.ConvertTo<T>(defaultValue);
        }

    } 
}