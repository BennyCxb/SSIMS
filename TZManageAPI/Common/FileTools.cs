using BT.Manage.Frame.Base;
using BT.Manage.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TZBaseFrame;
using BT.Manage.Core;
using BT.Manage.Tools;
using BT.Manage.Tools.Utils;

namespace TZManageAPI.Common
{
    public class FileTools
    {
        public static Result UploadFileToQiNiu(Stream file,string fileName,int AttachTypeID,int FBillTypeID,int FLoanID,int FUserID)
        {
            Result result = new Result();
            result.code = 0;
            try
            {
                loanFilesInfo fileInfo = new loanFilesInfo()
                {
                    FAddTime = DateTime.Now,
                    FAddUserID = FUserID,
                    FBillTypeID = FBillTypeID,
                    FFileName = fileName,
                    FIsQiNiu = 1,
                    FLoanID = FLoanID,
                    FQiNiuKey = fileName
                };

                Result upLoadresult = UploadFiles.UploadFilesForQiNiu(file, fileName);

                if (upLoadresult.code == 1)
                {
                    int k = fileInfo.SaveOnSubmit();
                    result.code = 1;
                }
                else
                {
                    result.code = 0;
                    result.message = upLoadresult.message;
                }
            }
            catch(Exception ex)
            {
                LogService.Default.Fatal("图片上传失败:",ex.Message,ex);
                result.code = 0;
                result.message = ex.Message;
            }

            return result;
            
        }
    }
}