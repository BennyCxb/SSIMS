using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BT.Manage.Core;
using BT.Manage.Tools.Utils;
using TZManage.Model;

namespace TZBaseFrame.Bll
{
    public class LoanCacheBll
    {
        /// <summary>
        /// 根据key获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCacheValue(string key)
        {
            //清理缓存
            CleanCache();
            LoanCacheInfo info= ModelOpretion.FirstOrDefault<LoanCacheInfo>(p=>p.FCacheKey==key && p.FIsDeleted.ToSafeInt32(0)==0);

            //更新缓存时间
            ReplaceCacheTime(key);

            return info.FCacheValue;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool SetCacheValue(string key,string value,int time)
        {
            //清理缓存
            CleanCache();
            LoanCacheInfo info = ModelOpretion.FirstOrDefault<LoanCacheInfo>(p => p.FCacheKey == key && p.FIsDeleted.ToSafeInt32(0) == 0);
            info.FCacheKey = key;
            info.FCacheValue = value;
            info.FCacheTime = time;
            info.FAddTime = DateTime.Now;
            info.FIsDeleted = 0;
            return info.SaveOnSubmit()>0;
        }

        /// <summary>
        /// 根据key清除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool RemoveCache(string key)
        {
            bool result = true;
            //清理缓存
            CleanCache();
            LoanCacheInfo info = ModelOpretion.FirstOrDefault<LoanCacheInfo>(p => p.FCacheKey == key && p.FIsDeleted.ToSafeInt32(0) == 0);
            if(info.FID>0)
            {
                info.FIsDeleted = 1;
                result= info.Update().Submit()>0;
            }

            return result;
        }

        /// <summary>
        /// 清理过期缓存
        /// </summary>
        private static void CleanCache()
        {
            ModelOpretion.ExecuteSqlNoneQuery(@" update  t_Loan_Cache set FIsDeleted=1
                                                    where DATEDIFF(s,DATEADD(n,FCacheTime,FAddTime),GETDATE())>0 ", null).Submit();

        }

        /// <summary>
        /// 更新缓存时间
        /// </summary>
        private static void ReplaceCacheTime(string key)
        {
            LoanCacheInfo info = ModelOpretion.FirstOrDefault<LoanCacheInfo>(p => p.FCacheKey == key && p.FIsDeleted.ToSafeInt32(0) == 0);

            //更新缓存时间
            if (info.FID > 0)
            {
                info.FAddTime = DateTime.Now;
                info.Update().Submit();
            }

        }
    }
}
