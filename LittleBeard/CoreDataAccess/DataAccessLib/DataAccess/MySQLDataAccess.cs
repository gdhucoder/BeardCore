using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LittleBeard.DataAccess.Lib;

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
}
