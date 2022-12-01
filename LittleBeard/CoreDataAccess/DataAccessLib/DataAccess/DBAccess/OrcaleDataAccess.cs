using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBeard.Core.Lib.DBAccess;

public class OrcaleDataAccess : IDataAccess
{
    public Task<int> Execute<T>(string sql, T parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public Task<T> QueryFirstOrDefault<T, U>(string sql, U parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> QueryList<T, U>(string sql, U parameters, string dbconnection)
    {
        using (IDbConnection conn = new OracleConnection(dbconnection))
        {
            var rows = await conn.QueryAsync<T>(sql, parameters);
            return rows.ToList();
        }
    }

    public Task<int> TranctionExecute<T>(string sql, T parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public List<string> GetColumnNames(string sql, string dbconnection)
    {
        DataTable schema = null;
        List<string> res = new();
        using (var conn = new OracleConnection(dbconnection))
        {
            using (var schemaCommand = new OracleCommand(sql, conn))
            {
                conn.Open();
                using (var reader = schemaCommand.ExecuteReader(CommandBehavior.SchemaOnly))
                {
                    schema = reader.GetSchemaTable();
                }
            }
        }

        foreach (DataRow row in schema.Rows)
        {
            res.Add(row.Field<String>("ColumnName"));
        }

        return res;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbconnection"></param>
    /// <returns></returns>
    public List<List<string>> LoadResultSetToList(string sql, string dbconnection)
    {
        var headers = GetColumnNames(sql, dbconnection);
        int numOfCols = headers.Count;
        List<List<string>> res = new();
        using (var conn = new OracleConnection(dbconnection))
        {
            conn.Open();
            var cmd = new OracleCommand(sql, conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                List<string> aline = new();
                for (int i = 0; i < numOfCols; i++)
                {
                    var val = "";
                    try
                    {
                        val = reader.GetString(i);
                    }
                    catch (Exception ex)
                    {
                        // Console.WriteLine(ex.Message);
                    }
                    aline.Add(val);
                }
                res.Add(aline);
            }

            return res;
        }

    }
}
