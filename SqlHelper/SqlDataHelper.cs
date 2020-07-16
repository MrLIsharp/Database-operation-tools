using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseToolByLjj.SqlHelper
{
    public static class SqlDataHelper
    {
        /// <summary>
        /// SqlDataReader 转成 DataTable
        /// 源需要是结果集
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public static DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
        {
            ///定义DataTable  
            DataTable datatable = new DataTable();

            try
            {    ///动态添加表的数据列  
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = dataReader.GetFieldType(i);
                    myDataColumn.ColumnName = dataReader.GetName(i);
                    datatable.Columns.Add(myDataColumn);
                }

                ///添加表的数据  
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                ///关闭数据读取器  
                dataReader.Close();
                return datatable;
            }
            catch (Exception ex)
            {
                ///抛出类型转换错误  
                //SystemError.CreateErrorLog(ex.Message);  
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
