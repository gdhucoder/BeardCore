using System.Data;

namespace LittleBeard.DataAccess.Lib;

public interface IDataAccess
{
    Task<List<T>> QueryList<T, U>(string sql, U parameters, string dbconnection);

    Task<int> Execute<T>(string sql, T parameters, string dbconnection);

    Task<T> QueryFirstOrDefault<T, U>(string sql, U parameters, string dbconnection);

    /// <summary>
    /// 执行事务提交
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <param name="connectionDB"></param>
    /// <returns></returns>
    Task<int> TranctionExecute<T>(string sql, T parameters, string dbconnection);



}