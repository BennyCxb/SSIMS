using BT.Manage.Frame.Base;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using Qiniu.Storage;
using Qiniu.Http;
using System.IO;
using System.Text;

namespace TZBaseFrame
{
    public class UploadFiles
    {
        /// <summary>
        /// 通行钥
        /// </summary>
        private static string AccessKey
        {
            get { return ConfigurationManager.AppSettings["QiNiuAccessKey"].ToSafeString(); }
        }
        //密钥
        private static string SecretKey
        {
            get { return ConfigurationManager.AppSettings["QiNiuSecretKey"].ToSafeString(); }
        }
        /// <summary>
        /// 存储空间名
        /// </summary>
        private static string Bucket
        {
            get { return ConfigurationManager.AppSettings["QiNiuBucket"].ToSafeString(); }
        }

        private static string DomainUrl
        {
            get { return ConfigurationManager.AppSettings["QiNiuDomainUrl"].ToSafeString(); }
        }

        /// <summary>
        /// 上传文件到七牛
        /// </summary>
        /// <param name="fileStream">文件流</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static Result UploadFilesForQiNiu(Stream fileStream,string fileName)
        {
            Result result = new Result();
            result.code = 0;
            try
            {
                string key = fileName;
                Mac mac = new Mac(AccessKey, SecretKey);
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = Bucket;
                putPolicy.SetExpires(3600);
                putPolicy.InsertOnly = 1;
                string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

                Config config = new Config();
                // 设置上传区域
                config.Zone = Zone.ZONE_CN_South;
                // 设置 http 或者 https 上传
                config.UseHttps = true;
                //config.UseCdnDomains = true;
                config.ChunkSize = ChunkUnit.U512K;

                // 表单上传
                FormUploader target = new FormUploader(config);
                HttpResult uploadResult = target.UploadStream(fileStream, key, token, null);
                Console.WriteLine("form upload result: " + uploadResult.ToString());
                if (uploadResult.Code == 200)
                {
                    result.code = 1;
                }
                
                result.message = "form upload result: " + uploadResult.ToString();

            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("七牛上传文件报错：", ex.Message, ex);
                result.code = 0;
                result.message = "七牛上传文件报错：" + ex.Message;

            }


            return result;
        }


        /// <summary>
        /// 上传文件到七牛
        /// </summary>
        /// <param name="filesPath">本地文件地址</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static Result UploadFilesForQiNiu(string filesPath, string fileName)
        {
            Result result = new Result();
            result.code = 0;
            try
            {
                string key = fileName;
                Mac mac = new Mac(AccessKey, SecretKey);
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = Bucket;
                putPolicy.SetExpires(3600);
                putPolicy.InsertOnly = 1;
                string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

                Config config = new Config();
                // 设置上传区域
                config.Zone = Zone.ZONE_CN_South;
                // 设置 http 或者 https 上传
                config.UseHttps = true;
                //config.UseCdnDomains = true;
                config.ChunkSize = ChunkUnit.U512K;

                // 表单上传
                FormUploader target = new FormUploader(config);
                HttpResult uploadResult = target.UploadFile(filesPath, key, token, null);
                Console.WriteLine("form upload result: " + uploadResult.ToString());
                if(uploadResult.Code==200)
                {
                    result.code = 1;
                }
                result.message = "form upload result: " + uploadResult.ToString();

            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("七牛上传文件报错：", ex.Message, ex);
                result.code = 0;
                result.message = "七牛上传文件报错：" + ex.Message;

            }


            return result;
        }

        /// <summary>
        /// 获取私有空间文件地址
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static Result GetQiNiuPrivateUrl(string fileKey)
        {
            Result result = new Result();
            try
            {
                Mac mac = new Mac(AccessKey, SecretKey);
                string domain = DomainUrl;
                string key = fileKey;
                string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, key, 3600);
                result.code = 1;
                result.@object = privateUrl;
            }
            catch( Exception ex)
            {
                LogService.Default.Fatal("获取图片地址失败：",ex.Message,ex);
                result.code = 0;
                result.message = "获取图片地址失败：" + ex.Message;
            }
            

            return result;
        }

        /// <summary>
        /// 获取公共空间文件地址
        /// </summary>
        /// <param name="fileKey"></param>
        /// <returns></returns>
        public static Result GetQiNiuPublishUrl(string fileKey)
        {
            Result result = new Result();
            try
            {
                Mac mac = new Mac(AccessKey, SecretKey);
                string domain = DomainUrl;
                string key = fileKey;
                

                string publicUrl = DownloadManager.CreatePublishUrl(domain, key);
                result.code = 1;
                result.@object = publicUrl;
            }
            catch (Exception ex)
            {
                LogService.Default.Fatal("获取图片地址失败：", ex.Message, ex);
                result.code = 0;
                result.message = "获取图片地址失败：" + ex.Message;
            }


            return result;
        }

    }
}
