using System.Data;

namespace LittleBeard.Core.Lib.DBAccess;

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


    /// <summary>
    /// 获取一个查询语句的列名字
    /// 使用场景：可用于导出一个查询的excel
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbconnection"></param>
    /// <returns></returns>
    List<string> GetColumnNames(string sql, string dbconnection);


    /// <summary>
    /// 将一个sql查询结果保存为一个列表
    /// 测试过20万数据，没有什么问题
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="dbconnection"></param>
    /// <returns></returns>
    List<List<string>> LoadResultSetToList(string sql, string dbconnection);
}