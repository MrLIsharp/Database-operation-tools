using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseToolByLjj.DataCommon
{
    /// <summary>
    /// 通用数据库字符串
    /// </summary>
    public class SqlBuilder<T> where T: BaseModel
    {
        public static readonly string FindSql = null;
        public static readonly string DeleteSql = null;
        public static readonly string FindAllSql = null;
        public static readonly string UpdateSql = null;

        static SqlBuilder()
        {
            Type type = typeof(T);

            FindSql = $"SELECT {string.Join(",", type.GetProperties().Select(a => $"[{a.Name}]")) } FROM [{type.Name}] where Id=@Id";

            DeleteSql = $"Delete from [{type.Name}] where Id=@Id"; 

            FindAllSql = $"SELECT {string.Join(",", type.GetProperties().Select(a => $"[{a.Name}]")) } FROM [{type.Name}]";

            UpdateSql = $"update [{type.Name}] set {string.Join(",", type.GetProperties().Where(a => !a.Name.Equals("Id")).Select(a => $"[{a.Name}]=@ {a.Name}"))} where Id =@Id";
        }
    }
}
