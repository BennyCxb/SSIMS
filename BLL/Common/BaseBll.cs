using FluentData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class BaseBll
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["AkData.Reader"].ConnectionString; }
        }

        /// <summary>
        /// 上下文
        /// </summary>
        public static IDbContext DbContext
        {
            get
            {
                var context = new DbContext().ConnectionString(ConnectionString, new SqlServerProvider());
                return context;
            }
        }
    }
}
