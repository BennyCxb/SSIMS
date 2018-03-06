using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TZBaseFrame.Bll;

namespace TZBaseFrame
{
    public class CacheHelper
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="obj">缓存的内容</param>
        /// <param name="time">缓存时间（分钟）</param>
        /// <returns></returns>
        public static bool CacheSet<T>(string key,T obj,int time)
        {
            if(time<=0)
            {
                time = 1;
            }
            string jsonValue = JsonConvert.SerializeObject(obj);
            return LoanCacheBll.SetCacheValue(key, jsonValue, time);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T CacheGet<T>(string key) where T : class
        {
            T resultT = null;
            string jsonValue= LoanCacheBll.GetCacheValue(key);
            if (!string.IsNullOrWhiteSpace(jsonValue))
            {
                resultT = JsonConvert.DeserializeObject<T>(jsonValue);
            }
            return resultT;
        }

        /// <summary>
        /// 根据key清除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveCache(string key)
        {
            return LoanCacheBll.RemoveCache(key);
        }
    }
}
