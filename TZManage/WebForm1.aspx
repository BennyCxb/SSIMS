<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="TZManage.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="http://libs.baidu.com/jquery/1.11.1/jquery.min.js"></script>
    <script>
        $(function () {
            //$.ajax({
            //    type: "GET",
            //    url: "http://localhost:8088/api/Login/GetOldCityChangeSchExcelByAgency?FYear=2018",
            //    data: { username: $("#username").val(), content: $("#content").val() },
            //    dataType: "blob",
            //    success: function (data) {
            //        console.log(typeof (data))
            //        blob = new Blob([data])
            //        var a = document.createElement('a');
            //        a.download = 'data.xls';
            //        a.href = window.URL.createObjectURL(blob)
            //        a.click()
            //    }
            //});

            download();
        })

        function download() {
            var url = 'http://localhost:8088/api/Login/GetOldCityChangeSchExcelByAgency?FYear=2018';
            var xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);    // 也可以使用POST方式，根据接口
            xhr.responseType = "blob";  // 返回类型blob
            // 定义请求完成的处理函数，请求前也可以增加加载框/禁用下载按钮逻辑
            xhr.onload = function () {
                // 请求完成
                if (this.status === 200) {
                    // 返回200
                    var blob = this.response;
                    var reader = new FileReader();
                    reader.readAsDataURL(blob);  // 转换为base64，可以直接放入a表情href
                    reader.onload = function (e) {
                        // 转换完成，创建一个a标签用于下载
                        var a = document.createElement('a');
                        a.download = 'data.xlsx';
                        a.href = e.target.result;
                        $("body").append(a);  // 修复firefox中无法触发click
                        a.click();
                        $(a).remove();
                    }
                }
            };
            // 发送ajax请求
            xhr.send();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
