using DataBaseToolByLjj.SqlHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseToolByLjj.DataCommon.CreatClassBySql
{

    public class CreateBySqlServer
    {
        public string connStr=string.Empty;
        public string FieldPath = string.Empty;
        public CreateBySqlServer(string connString)
        {
            connStr = connString;
        }

        public CreateBySqlServer(string connString,string filePath)
        {
            connStr = connString;
            FieldPath = filePath;
        }

        //获取所有的数据库名
        private readonly static string GetAllDataSql = "SELECT NAME FROM MASTER.DBO.SYSDATABASES ORDER BY NAME";
        //获取所有的表名
        private static string GetAllTableSql = "SELECT name FROM sys.tables where type ='U'";
        /// <summary>
        /// 获取所有的表名和表说明 minor_id
        /// </summary>
        private static string GetAllTableInfo = @"SELECT tbs.name 表名,ds.des 描述 
                                                    FROM
                                                    (
                                                    SELECT ds.value des, ds.minor_id, ds.major_id
                                                    FROM sys.extended_properties ds where ds.minor_id= 0
                                                    ) ds
                                                    RIGHT JOIN sysobjects tbs ON ds.major_id=tbs.id

                                                    where tbs.xtype='U'
                                                    order by 表名";

        //获取所有的表信息
        private static string GetTableInfoSql = @"SELECT DISTINCT a.COLUMN_NAME columnName, 
                                          a.DATA_TYPE typeName, a.IS_NULLABLE isnullAble
                                          From INFORMATION_SCHEMA.Columns a LEFT JOIN 
                                          INFORMATION_SCHEMA.KEY_COLUMN_USAGE b ON a.TABLE_NAME=b.TABLE_NAME ";

        private static string GetTableInfoByTableName = @"SELECT DISTINCT a.COLUMN_NAME 字段名, a.DATA_TYPE 类型, a.IS_NULLABLE 可空,a.COLUMN_DEFAULT 默认值,B.value 描述
                                                            From INFORMATION_SCHEMA.Columns a 
                                                            LEFT JOIN sys.extended_properties B ON object_id(a.TABLE_NAME)=b.major_id and a.ORDINAL_POSITION=B.minor_id
                                                            where a.TABLE_NAME=@TableName";
        //根据表名创建模型
        public void CreateSingleModel(string tableName)
        {
            string sql = $"{GetTableInfoSql}  where a.table_name='{tableName}'";
            using (SqlConnection conn = new SqlConnection(connStr))//ConnectionString为自己连接字符串
            {
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"public class {tableName} \r\n{{\r\n");
                while (reader.Read())
                {
                    stringBuilder.Append($"  public {GetTypeOfColumn(reader["typeName"].ToString(), reader["isnullAble"].ToString())} {reader["columnName"]} {{get;set;}}\r\n");
                }
                stringBuilder.Append("} \r\n");
                string directory = string.IsNullOrEmpty(FieldPath) ? AppDomain.CurrentDomain.BaseDirectory + "\\Model\\" : FieldPath;//FieldPath为自己文件路径
                StreamWriter sr;
                //是否存在文件夹,不存在则创建
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string path = directory + "\\" + tableName + ".cs";
                //如果该文件存在则追加内容，否则创建文件
                if (File.Exists(path))
                {
                    sr = File.AppendText(path);
                }
                else
                {
                    sr = File.CreateText(path);
                }
                sr.Write(stringBuilder.ToString());
                sr.Flush();
                sr.Close();
            }
        }

        /// <summary>
        /// 根据数据库直接生成所有模型
        /// </summary>
        public DataTable GetTableName()
        {
            using (SqlConnection conn = new SqlConnection(connStr))//ConnectionString为自己连接字符串
            {
                SqlCommand sqlCommand = new SqlCommand(GetAllTableInfo, conn);
                conn.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                return SqlDataHelper.ConvertDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 获取数据表字段信息
        /// </summary>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public DataTable TableInfoByTableName(string TableName)
        {
            using (SqlConnection conn = new SqlConnection(connStr))//ConnectionString为自己连接字符串
            {
                SqlCommand sqlCommand = new SqlCommand(GetTableInfoByTableName, conn);
                sqlCommand.Parameters.Add(new SqlParameter("@TableName", TableName));
                conn.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                return SqlDataHelper.ConvertDataReaderToDataTable(reader);
            }
        }

        /// <summary>
        /// 根据数据库直接生成所有模型
        /// </summary>
        public void BatchCreateModel()
        {
            using (SqlConnection conn = new SqlConnection(connStr))//ConnectionString为自己连接字符串
            {
                SqlCommand sqlCommand = new SqlCommand(GetAllTableSql, conn);
                conn.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    CreateSingleModel(reader["name"].ToString());
                }
            }
        }

        /// <summary>
        /// 获取列的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="nullAble"></param>
        /// <returns></returns>
        private static string GetTypeOfColumn(string type, string nullAble)
        {
            if (type == "int" && nullAble == "NO")
                return "int";
            if (type == "int" && nullAble == "YES")
                return "int?";
            if (type == "bigint" && nullAble == "NO")
                return "long";
            if (type == "bigint" && nullAble == "YES")
                return "long?";
            if (type == "tinyint" && nullAble == "NO")
                return "byte";
            if (type == "tinyint" && nullAble == "YES")
                return "byte?";
            if (type == "smallint" && nullAble == "NO")
                return "short";
            if (type == "smallint" && nullAble == "YES")
                return "short?";
            if (type == "bit" && nullAble == "NO")
                return "bool";
            if (type == "bit" && nullAble == "YES")
                return "bool?";
            if (type == "real" && nullAble == "NO")
                return "decimal";
            if (type == "real" && nullAble == "Yes")
                return "decimal?";
            if (type == "float" && nullAble == "NO")
                return "decimal";
            if (type == "float" && nullAble == "YES")
                return "decimal";
            if ((type == "decimal" || type == "money" || type == "smallmoney") && nullAble == "NO")
                return "decimal";
            if ((type == "decimal" || type == "money" || type == "smallmoney") && nullAble == "YES")
                return "decimal";
            if (type == "numeric" && nullAble == "NO")
                return "decimal";
            if (type == "numeric" && nullAble == "YES")
                return "decimal";
            if ((type == "datetime" || type == "smalldatetime") && nullAble == "YES")
                return "DateTime?";
            if ((type == "datetime" || type == "smalldatetime") && nullAble == "NO")
                return "DateTime";
            if (type == "date" && nullAble == "YES")
                return "DateTime?";
            if (type == "date" && nullAble == "NO")
                return "DateTime";
            if (type == "nchar" || type == "char" || type == "nvarchar" || type == "varchar" || type == "text" || type == "ntext")
                return "string";
            if (type == "sql_variant")
                return "object";
            if (type == "image")
                return "byte[]";
            throw new Exception("无此类型:" + type);
            //return "string";
        }
    }
}
