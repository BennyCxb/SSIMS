using BT.Manage.Frame.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TZBaseFrame.Attributes;
using TZManageAPI.Base;
using System.Web.Http;
using System.IO;
using TZManageAPI.Common;
using BT.Manage.Core;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;
using System.Data;
using TZManageAPI.DTO;

namespace TZManageAPI.Controllers
{
    [JwtAuthActionFilter]
    public class FilesController : BaseController
    {
        /// <summary>
        /// 上传附件到七牛
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Result UploadFileForQiNiu()
        {
            Result result = new Result();
            result.code = 0;
            try
            {
                int AttachType = HttpContext.Current.Request["AttachType"].ToSafeInt32(0);
                int FBillTypeID = HttpContext.Current.Request["FBillTypeID"].ToSafeInt32(0);
                int FLoanID = HttpContext.Current.Request["FLoanID"].ToSafeInt32(0);
                HttpFileCollection files = HttpContext.Current.Request.Files;
                int successCount = 0;
                foreach (string key in files.AllKeys)
                {
                    
                    HttpPostedFile file = files[key];
                    if (!string.IsNullOrWhiteSpace(file.FileName))
                    {
                        Result uploadResult = new Result();
                        Stream fileStream = file.InputStream;
                        uploadResult = FileTools.UploadFileToQiNiu(fileStream, file.FileName, AttachType, FBillTypeID, FLoanID, UserInfo.UserId);
                        if(uploadResult.code==1)
                        {
                            successCount++;
                        }
                    }
                }
                result.code = 1;
                result.message = string.Format("{0}个附件上传成功！", successCount);
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("附件上传失败:",ex.Message,ex);
                result.code = 0;
                result.message = "附件上传失败:" + ex.Message;
            }
            
            return result;
        }


        /// <summary>
        /// 获取七牛的附件地址
        /// </summary>
        /// <param name="FLoanID">核准单ID</param>
        /// <param name="FBillTypeID">单据类型</param>
        /// <param name="FAttachType">附件类型</param>
        /// <returns></returns>
        [HttpGet]
        public Result GetFilesUrl(int FLoanID,int FBillTypeID,int FAttachType)
        {
            Result result = new Result();
            result.code = 0;
            List<FileDTO> fileList = new List<FileDTO>();

            DataTable dt= ModelOpretion.SearchDataRetunDataTable(@"select f.FQiNiuKey,FFileName 
                                                    from t_loan_Files f
                                                    where f.FLoanID=@FLoanID and f.FBillTypeID=@FBillTypeID and f.FAttachmentTypeID=@FAttachmentTypeID ",
                                                    new { FLoanID= FLoanID, FBillTypeID= FBillTypeID , FAttachmentTypeID = FAttachType });
            
            foreach(DataRow dr in dt.Rows)
            {
                string filekey = dr["FQiNiuKey"].ToString();
                //根据key获取私有空间文件地址
                Result urlResult= FileTools.GetPrivateUrl(filekey);
                if(urlResult.code==1)
                {
                    FileDTO file = new FileDTO()
                    {
                        FileName = dr["FFileName"].ToString(),
                        FileUrl = urlResult.@object
                    };
                    fileList.Add(file);
                }
            }

            result.code = 1;
            result.@object = fileList;

            return result;
        }
    }
}