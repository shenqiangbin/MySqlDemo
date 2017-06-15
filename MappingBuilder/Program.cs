using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using Dapper;
using MySql.Data.MySqlClient;

namespace MappingBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("输入表名：");
            string tableName = Console.ReadLine();
            GetInfoFile(tableName);
        }

        static void GetInfoFile(string tableName)
        {
            var infoText = GetColumnsInfo(tableName);
            var text = GetMappingText(tableName);
            var classText = GenerateClass(tableName);
            var saveClassText = GenerateSaveModelClass(tableName);

            WriteTmpFileAndOpen(infoText + "\r\n" + text + "\r\n" + classText + "\r\n" + saveClassText);
        }

        static string GetMappingText(string tableName)
        {
            IEnumerable<Column> columns = GetTableColumns(tableName);

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < columns.Count(); i++)
            {
                var col = columns.ElementAt(i);

                if (i == 0)
                    builder.Append($"<property name=\"{col.ColumnName}\" mapping=\"{col.ColumnName}\" isKey=\"true\"/> \r\n");
                else
                    builder.Append($"<property name=\"{col.ColumnName}\" mapping=\"{col.ColumnName}\" /> \r\n");

            }

            return builder.ToString();
        }

        static string GenerateClass(string tableName)
        {
            IEnumerable<Column> columns = GetTableColumns(tableName);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public class {tableName}");
            builder.AppendLine("{");

            for (int i = 0; i < columns.Count(); i++)
            {
                Column col = columns.ElementAt(i);
                builder.AppendLine(col.GeneratePropertyString());
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        static string GenerateSaveModelClass(string tableName)
        {
            IEnumerable<Column> columns = GetTableColumns(tableName);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"public class {tableName}SaveModel");
            builder.AppendLine("{");
            for (int i = 0; i < columns.Count(); i++)
            {
                Column col = columns.ElementAt(i);
                builder.AppendLine("    " + col.GenerateSavePropertyString());
            }
            builder.AppendLine("");
            builder.AppendLine($"    public void SetValTo({tableName} model)");
            builder.AppendLine("    {");
            for (int i = 0; i < columns.Count(); i++)
            {
                Column col = columns.ElementAt(i);
                if (col.GenerateSavePropertyString().Contains("?"))
                    builder.AppendLine($"       model.{col.ColumnName} = {col.ColumnName} != null ? {col.ColumnName}.Value : model.{col.ColumnName};");
                else
                    builder.AppendLine($"       model.{col.ColumnName} = {col.ColumnName} != null ? {col.ColumnName} : model.{col.ColumnName};");
            }
            builder.AppendLine("    }");

            builder.AppendLine("}");
            return builder.ToString();
        }

        static string GetColumnsInfo(string tableName)
        {
            IEnumerable<Column> columns = GetTableColumns(tableName);

            StringBuilder builder = new StringBuilder();
            foreach (var item in columns)
            {
                builder.AppendLine(item.ToString());
            }
            return builder.ToString();
        }

        static IEnumerable<Column> GetTableColumns(string tableName)
        {
            string sql = $"select column_name as ColumnName,column_comment as ColumnComment,data_type as DataType from information_schema.columns where table_name = '{tableName}'";
            IEnumerable<Column> list = GetConn().Query<Column>(sql);
            return list;
        }

        static MySqlConnection GetConn()
        {
            string connStr = "server=192.168.103.90;database=thesismgmt;Uid=thesismgmt;Pwd=123456;";
            connStr = "server=127.0.0.1;database=thesisdb;Uid=root;Pwd=123456;";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }

        static void WriteTmpFileAndOpen(string text)
        {
            string file = System.IO.Path.GetTempFileName();
            System.IO.File.WriteAllText(file, text);
            System.Diagnostics.Process.Start(file);
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
