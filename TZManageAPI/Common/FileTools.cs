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
                //文件名（去除扩展名）
                string fileFirstName =  Path.GetFileNameWithoutExtension(fileName);
                string FQiNiuKey = fileFirstName+"A" + FLoanID.ToString()+"B"+FBillTypeID.ToString()+"T"+DateTime.Now.ToString("yyyyMMdd");
                loanFilesInfo fileInfo = new loanFilesInfo()
                {
                    FAddTime = DateTime.Now,
                    FAddUserID = FUserID,
                    FBillTypeID = FBillTypeID,
                    FFileName = fileName,
                    FIsQiNiu = 1,
                    FLoanID = FLoanID,
                    FQiNiuKey = FQiNiuKey
                };
                
                Result upLoadresult = UploadFiles.UploadFilesForQiNiu(file, FQiNiuKey);

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


        public static Result GetPrivateUrl(string fileKey)
        {
            Result result= UploadFiles.GetQiNiuPrivateUrl(fileKey);

            return result;
        }
    }
}