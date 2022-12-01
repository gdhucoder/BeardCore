using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBeard.Core.Lib.DBAccess;
public class DmDataAccess : IDataAccess
{
    public Task<int> Execute<T>(string sql, T parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public List<string> GetColumnNames(string sql, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public List<List<string>> LoadResultSetToList(string sql, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public Task<T> QueryFirstOrDefault<T, U>(string sql, U parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> QueryList<T, U>(string sql, U parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }

    public Task<int> TranctionExecute<T>(string sql, T parameters, string dbconnection)
    {
        throw new NotImplementedException();
    }
}
