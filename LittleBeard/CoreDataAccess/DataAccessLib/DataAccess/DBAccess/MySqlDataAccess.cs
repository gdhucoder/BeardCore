using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LittleBeard.Core.Lib.DBAccess;

public class MySqlDataAccess : IDataAccess
{
    private readonly IConfiguration _config;

    public MySqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<T> QueryFirstOrDefault<T, U>(string sql, U parameters, string dbconnection)
    {
        using (IDbConnection conn = new MySqlConnection(dbconnection))
        {
            var res = await conn.QueryFirstOrDefaultAsync<T>(sql, parameters);
            return res;
        }
    }

    public async Task<List<T>> QueryList<T, U>(string sql, U parameters, string dbconnection)
    {
        using (IDbConnection conn = new MySqlConnection(dbconnection))
        {
            var rows = await conn.QueryAsync<T>(sql, parameters);
            return rows.ToList();
        }
    }

    public async Task<int> Execute<T>(string sql, T parameters, string dbconnection)
    {
        using (IDbConnection conn = new MySqlConnection(dbconnection))
        {
            return await conn.ExecuteAsync(sql, parameters);
        }
    }

    public async Task<int> TranctionExecute<T>(string sql, T parameters, string dbconnection)
    {
        using (var tran = new TransactionScope())
        using (IDbConnection conn = new MySqlConnection(dbconnection))
        {
            var res = await conn.ExecuteAsync(sql, parameters);
            tran.Complete();
            return res;
        }
    }

    public List<string> GetColumnNames(string sql, string dbconnection)
    {
        DataTable schema = null;
        List<string> res = new();
        using (var conn = new MySqlConnection(dbconnection))
        {
            using (var schemaCommand = new MySqlCommand(sql, conn))
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

    public List<List<string>> LoadResultSetToList(string sql, string dbconnection)
    {
        var headers = GetColumnNames(sql, dbconnection);
        int numOfCols = headers.Count;
        List<List<string>> res = new();
        using (var conn = new MySqlConnection(dbconnection))
        {
            conn.Open();
            var cmd = new MySqlCommand(sql, conn);
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
