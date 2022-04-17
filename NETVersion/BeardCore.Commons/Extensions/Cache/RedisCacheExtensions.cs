using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BeardCore.Commons.Extensions.Cache
{
    public static class RedisCacheExtensions
    {
        /// <summary>
        /// 将数据存入Redis缓存
        /// </summary>
        /// <typeparam name="T">保存数据的类型</typeparam>
        /// <param name="cache">缓存</param>
        /// <param name="recordId">记录的唯一ID</param>
        /// <param name="data">数据</param>
        /// <param name="absoluteExpireTime">过期时间，默认60秒</param>
        /// <param name="unusedExpireTime">未使用删除时间，如需使用注意小于绝对过期时间</param>
        /// <returns></returns>
        /// 保存的数据格式为：
        // 127.0.0.1:6379> hgetall  Redis_SZCredit_APP_LoginInfo
        //1) "sldexp"
        //2) "-1"
        //3) "data"
        //4) "{\"ID\":\"41ef7dda5c4a4539b52df88ef2a88a7f\",\"USERCODE\":\"uc001\",\"USERNAME\":\"rn001\",\"PASSWORD\":\"AYqDe9NVth2K\\u002BxqnOAO2CMQuIxUTg/Rc0n\\u002BJjjVfPMne\\u002BJUQyLuuVceoXSldJkV56w==\",\"MOBILE\":\"13631558389\",\"EMAIL\":\"bp5@qq.com\",\"CREATEDATE\":\"2021-04-02T10:43:48\",\"USERTYPE\":\"1\",\"ERRORTIMES\":0,\"LASTLOGINTIME\":\"2021-04-07T11:46:55\",\"STATUSCODE\":\"1\",\"VALIDDATE\":\"2021-10-02T10:43:48\",\"VALIDCODE\":\"0\"}"
        //5) "absexp"
        //6) "637533773104887414"
        public static async Task SetRecordAsync<T>(this IDistributedCache cache,
            string recordId,
            T data,
            TimeSpan? absoluteExpireTime = null,
            TimeSpan? unusedExpireTime = null)
        {
            var options = SetOptions(absoluteExpireTime, unusedExpireTime);
            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId, jsonData, options);
        }

        /// <summary>
        /// Helper Method
        /// </summary>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="unusedExpireTime"></param>
        /// <returns></returns>
        private static DistributedCacheEntryOptions SetOptions(TimeSpan? absoluteExpireTime, TimeSpan? unusedExpireTime)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(3600);
            options.SlidingExpiration = unusedExpireTime;
            return options;
        }

        /// <summary>
        /// 同步方法，将数据存入Redis缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <param name="data"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <param name="unusedExpireTime"></param>
        public static void SetRecord<T>(this IDistributedCache cache,
                                            string recordId,
                                            T data,
                                            TimeSpan? absoluteExpireTime = null,
                                            TimeSpan? unusedExpireTime = null)
        {
            var options = SetOptions(absoluteExpireTime, unusedExpireTime);
            var jsonData = JsonSerializer.Serialize(data);
            cache.SetString(recordId, jsonData, options);
        }

        /// <summary>
        /// 异步方法，从缓存中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);

        }

        /// <summary>
        /// 同步方法，从缓存中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="recordId"></param>
        /// <returns></returns>
        public static T GetRecord<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = cache.GetString(recordId);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);

        }
    }
}
