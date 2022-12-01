// See https://aka.ms/new-console-template for more information
using LittleBeard.Core.Lib.DBAccess;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Drawing.Text;
using System;
using LittleBeard.Core.Lib.Excel;

var dbconnection = "server=10.0.11.1;user id=DEV_USER;password=dev?1234;persistsecurityinfo=True;database=UserDB;SslMode=none;Charset=utf8mb4;MinimumPoolSize=10;MaximumPoolSize=50";

var mysql = new MySqlDataAccess(null);

var sql = @"select ID as '身份',s.* from USER_USERS s";

var header = mysql.GetColumnNames(sql, dbconnection);
Console.WriteLine(JsonConvert.SerializeObject(header));

var res = mysql.LoadResultSetToList(sql, dbconnection);
Console.WriteLine(JsonConvert.SerializeObject(res));

ExcelProcessor p = new ExcelProcessor();
var file = @"E:\dev\BeardCore\LittleBeard\ConsoleUI\ConsoleApp\reslut.xlsx";

await p.ExportExcel(new FileInfo(file), header, res);

//Console.WriteLine("Hello, World!");
//DataTable schema = null;
//using (var conn = new MySqlConnection(dbconnection))
//{
//    using (var schemaCommand = new MySqlCommand("select ID as '身份',s.* from USER_USERS s", conn))
//    {
//        conn.Open();
//        using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
//        {
//            schema = reader.GetSchemaTable();
//        }    
//    }
//}

//foreach (DataRow row in schema.Rows)
//{
//    Console.WriteLine(row.Field<String>("ColumnName"));
//}

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
