using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzService.DB.Utils
{
    public class OrmTransfer
    {
        public static IEnumerable<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            List<T> result = new List<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return result;
            }
            Type objType = typeof(T);
            foreach (DataRow dr in dt.Rows)
            {
                object item = System.Activator.CreateInstance(objType);
                foreach (System.Reflection.PropertyInfo pi in objType.GetProperties())
                {
                    if (pi.PropertyType.IsPublic && pi.CanWrite && dt.Columns.Contains(pi.Name))
                    {
                        Type pType = Type.GetType(pi.PropertyType.FullName);
                        if (dr[pi.Name] != string.Empty && dr[pi.Name] != null && dr[pi.Name] != DBNull.Value)
                        {
                            object value = ChanageType(dr[pi.Name], pType);
                            objType.GetProperty(pi.Name).SetValue(item, value, null);
                        }

                    }
                }
                result.Add((T)item);
            }
            return result;
        }
        public static T GetModel<T>(DataRow dr) where T : new()
        {
            T result = new T();
            if (dr == null)
            {
                return result;
            }
            Type objType = typeof(T);
            foreach (System.Reflection.PropertyInfo pi in objType.GetProperties())
            {
                if (pi.PropertyType.IsPublic && pi.CanWrite && dr.Table.Columns.Contains(pi.Name))
                {
                    Type pType = Type.GetType(pi.PropertyType.FullName);
                    if (dr[pi.Name] != string.Empty && dr[pi.Name] != null && dr[pi.Name] != DBNull.Value)
                    {
                        object value = ChanageType(dr[pi.Name], pType);
                        objType.GetProperty(pi.Name).SetValue(result, value, null);
                    }

                }
            }
            return result;
        }

        private static object ChanageType(object value, Type convertsionType)
        {
            if (convertsionType.IsGenericType && convertsionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || value.ToString().Length == 0)
                {
                    return null;
                }
                NullableConverter nullableConverter = new NullableConverter(convertsionType);
                convertsionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, convertsionType);
        }
    }
}
