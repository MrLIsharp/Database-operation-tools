using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseToolByLjj.DataCommon
{
    public class Execute
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public static readonly string Customers = ConfigurationSettings.AppSettings["connString"].ToString();
        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Add<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            object oCompany = Activator.CreateInstance(type);
            // Id 是自动增长的，sql语句中应该去除Id的字段
            // GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)  过滤掉继承自父类的属性
            string props = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(a => $"[{a.Name}]"));//获取属性名不等于id的所有属性数组
            string paraValues = string.Join(",", type.GetProperties().Where(p => !p.Name.Equals("Id")).Select(a => $"@[{a.Name}]"));//获取属性名不等于id的所有参数化数组
            string sql = $"Insert [{type.Name}] ({props}) values({paraValues})";
            var parameters = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(item => new SqlParameter()
            {
                ParameterName = $"@{item.Name}",
                SqlValue = $"{item.GetValue(t)}"
            });
            //在拼接sql语句的时候，尽管ID 是Int类型，还是建议大家使用Sql语句参数化 （防止sql注入）
            using (SqlConnection connection = new SqlConnection(Customers))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                sqlCommand.Parameters.AddRange(parameters.ToArray());
                connection.Open();
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Delete<T>(T t) where T : BaseModel
        {
            Type type = t.GetType();
            string sql = SqlBuilder<T>.DeleteSql;
            //string sql = $"Delete from [{type.Name}] where Id=@Id";
            using (SqlConnection connection = new SqlConnection(Customers))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.Parameters.Add(new SqlParameter("@Id", t.Id));
                connection.Open();
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool Update<T>(T t) where T : BaseModel
        {
            Type type = typeof(T);
            object oCompany = Activator.CreateInstance(type);
            //string sql = $"update [{type.Name}] set {string.Join(",", type.GetProperties().Where(a => !a.Name.Equals("Id")).Select(a => $"[{a.Name}]=@ {a.Name}"))} where Id =@Id";
            string sql = SqlBuilder<T>.UpdateSql;
            var parameters = type.GetProperties().Select(item => new SqlParameter()
            {
                ParameterName = $"@{item.Name}",
                SqlValue = $"{item.GetValue(t)}"
            });
            //  在拼接sql语句的时候，尽管ID 是Int类型，还是建议大家使用Sql语句参数化防止sql注入）
            using (SqlConnection connection = new SqlConnection(Customers))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                sqlCommand.Parameters.AddRange(parameters.ToArray());
                connection.Open();
                return sqlCommand.ExecuteNonQuery() > 0;
            }
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>

        public T Find<T>(int id) where T : BaseModel
        {
            Type type = typeof(T);
            object oCompany = Activator.CreateInstance(type);

            //string sql = $"SELECT {string.Join(",", type.GetProperties().Select(a => $"[{a.Name}]")) } FROM [{type.Name}] where Id=@Id";

            string sql = SqlBuilder<T>.FindSql;


            //  在拼接sql语句的时候，尽管ID 是Int类型，还是建议大家使用Sql语句参数化 （防止sql注入）
            using (SqlConnection connection = new SqlConnection(Customers))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                sqlCommand.Parameters.Add(new SqlParameter("@Id", id));
                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.Read())
                {
                    ReaderToList(type, oCompany, reader);
                    return (T)oCompany;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>

        public List<T> FindAll<T>() where T : BaseModel
        {
            Type type = typeof(T);
            //string sql = $"SELECT {string.Join(",", type.GetProperties().Select(a => $"[{a.Name}]")) } FROM [{type.Name}]";

            string sql = SqlBuilder<T>.FindAllSql;

            using (SqlConnection connection = new SqlConnection(Customers))
            {
                SqlCommand sqlCommand = new SqlCommand(sql, connection);

                connection.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                List<T> datalist = new List<T>();
                while (reader.Read())
                {
                    object oCompany = Activator.CreateInstance(type);
                    ReaderToList(type, oCompany, reader);
                    datalist.Add((T)oCompany);
                }
                return datalist;
            }
        }

        //私有函数封装通用代码，引用类型可以不用返回

        private static void ReaderToList(Type type, object oCompany, SqlDataReader reader)
        {
            foreach (var prop in type.GetProperties())
            {
                prop.SetValue(oCompany, reader[prop.Name] is DBNull ? null : reader[prop.Name]);
            }
        }
    }
}
