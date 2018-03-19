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

namespace TZManageAPI.Controllers
{
    [JwtAuthActionFilter]
    public class FilesController : BaseController
    {
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
    }
}