using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI;
using NPOI.XSSF.UserModel;
using NPOI.HPSF;
using System.IO;
using System.Data;
using NPOI.SS.Util;
using BT.Manage.Tools.Utils;
using BT.Manage.Tools;

namespace TZBaseFrame
{
    public class NPOIHelper
    {
        public static Stream ExportExcel(DataTable dt,string title,bool haveTitle=true)
        {
            //双标题标识
            bool haveDoubleTitle = false;
            int indexRow = 1;
            HSSFWorkbook wk = new HSSFWorkbook();
            //创建一个Sheet  
            ISheet sheet = wk.CreateSheet("Sheet1");
            
            //判断是否存在双标题
            foreach(DataColumn dc in dt.Columns)
            {
                if(dc.ColumnName.Contains('|'))
                {
                    haveDoubleTitle = true;
                    break;
                }
            }
            IRow TitleRow = sheet.CreateRow(0);
            TitleRow.HeightInPoints = 25;
            TitleRow.CreateCell(0).SetCellValue(title);

            ICellStyle headStyle = wk.CreateCellStyle();
            headStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            IFont font = wk.CreateFont();
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            headStyle.SetFont(font);
            TitleRow.GetCell(0).CellStyle = headStyle;
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));

            //创建标题
            //存在双标题情况
            if (haveDoubleTitle)
            {
                string regionName = string.Empty;
                int regionBegin = -1;
                IRow dbheaderRow = sheet.CreateRow(indexRow);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = dbheaderRow.CreateCell(i);
                    DataColumn dc = dt.Columns[i];
                    string[] cname = dc.ColumnName.Split('|');
                    //双标题合并单元格
                    string fName = string.Empty;
                    if (cname.Length > 1)
                    {
                        fName = cname[0];
                    }

                    if (fName != regionName && !string.IsNullOrWhiteSpace(fName))
                    {
                        if (regionBegin > -1)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(indexRow, indexRow, regionBegin, i - 1));

                        }
                        cell.SetCellValue(fName);
                        regionName = fName;
                        regionBegin = i;
                    }
                    else
                    {
                        cell.SetCellValue(string.Empty);
                    }
                }
                sheet.AddMergedRegion(new CellRangeAddress(indexRow, indexRow, regionBegin, dt.Columns.Count-1));

                indexRow++;
            }
            IRow headerRow = sheet.CreateRow(indexRow);
            for (int i=0;i<dt.Columns.Count;i++)
            {
                ICell cell = headerRow.CreateCell(i);
                DataColumn dc = dt.Columns[i];
                string[] cname = dc.ColumnName.Split('|');
                string sName = dc.ColumnName;
                if (cname.Length > 1)
                {
                    sName = cname[1];
                }
                cell.SetCellValue(sName);
            }
            indexRow++;

            //填充内容
            for(int i=0;i<dt.Rows.Count;i++)
            {
                IRow contentRow = sheet.CreateRow(indexRow);

                DataRow dr = dt.Rows[i];
                for(int j=0;j<dt.Columns.Count;j++)
                {
                    ICell newCell = contentRow.CreateCell(j);
                    DataColumn column = dt.Columns[j];
                    string drValue = dr[column].ToString();
                    newCell.SetCellValue(drValue);

                }

                indexRow++;
            }
            


            Stream str = new MemoryStream();
            wk.Write(str);
            return str;
        }


    }
}
