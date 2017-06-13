using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace demo02
{
    public abstract class MySqlBaseRepository<T> : IBaseRepository<T> where T : class
    {
        public void Delete(T model)
        {
            throw new NotImplementedException();
        }

        public void Insert(T model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> SelectBy(Dictionary<string, string> fields)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update AttachmentPack set ");
            builder.Append(GetSqlByDic(destField, ","));
            builder.Append(" where ");
            builder.Append(GetSqlByDic(whereField, "and"));

            string sql = builder.ToString();

            IEnumerable<T> models = GetConn().Query<T>($"select * from {typeof(T).Name} where");
            return models;
        }

        public T SelectBy(int Id)
        {
            var model = GetConn().QueryFirstOrDefault<T>($"select * from {typeof(T).Name} where id = @id", new { Id = Id });
            return model;
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }

        public void UpdateBy(Dictionary<string, string> destFields, Dictionary<string, string> whereFields)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("update AttachmentPack set ");
            builder.Append(GetSqlByDic(destField, ","));
            builder.Append(" where ");
            builder.Append(GetSqlByDic(whereField, "and"));

            string sql = builder.ToString();



            DynamicParameters para = new DynamicParameters();
            para.Add("@id", 123);
            return GetConn().Execute(sql,para);
        }

        private MySqlConnection GetConn()
        {
            string connStr = "server=192.168.103.90;database=thesismgmt;Uid=thesismgmt;Pwd=123456;";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }

        private IEnumerable<Column> GetTableColumns(string tableName)
        {
            string sql = $"select column_name as ColumnName,column_comment as ColumnComment,data_type as DataType from information_schema.columns where table_name = '{tableName}'";
            IEnumerable<Column> list = GetConn().Query<Column>(sql);
            return list;
        }
    }

    public class Column
    {
        public string ColumnName { get; set; }
        public string ColumnComment { get; set; }
        public string DataType { get; set; }

        public override string ToString()
        {
            return $"{ColumnName.PadRight(20)}  {DataType.PadRight(20)}  {ColumnComment}  ";
        }

        public string GeneratePropertyString()
        {
            return string.Format("public {0} {1} {{ get; set; }}", GetDataTypeString(), ColumnName);
        }

        private string GetDataTypeString()
        {
            switch (DataType)
            {
                case "int":
                    return "int";
                case "datetime":
                    return "DateTime";
                case "varchar":
                    return "string";
                default:
                    return "string";
            }
        }

        public string GenerateSavePropertyString()
        {
            return string.Format("public {0} {1} {{ get; set; }}", GetSaveDataTypeString(), ColumnName);
        }

        private string GetSaveDataTypeString()
        {
            switch (DataType)
            {
                case "int":
                    return "int?";
                case "datetime":
                    return "DateTime?";
                case "varchar":
                    return "string";
                default:
                    return "string";
            }
        }
    }
}
