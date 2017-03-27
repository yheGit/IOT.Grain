using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


/******************************************
*Creater:yhw[]
*CreatTime:
*Description:
*==========================================
*Modifyier:sxf[]
*MidifyTime:
*Description:Net66.Comm
*******************************************/
namespace Net66.Workbook
{
    public class ExportExcel<T>:object
    {

        public static HSSFWorkbook OutExcel(List<T> queryData, string[] rowTile, string sheetName, string fileName)
        {

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet1 = book.CreateSheet(sheetName);
            IRow row1 = sheet1.CreateRow(0);
            //设置标题
            for (int j = 0; j < rowTile.Length; j++)
                row1.CreateCell(j).SetCellValue(rowTile[j]);
            int i = 0;
            System.Reflection.PropertyInfo[] pInfo = null;

            //给行填充值

            foreach (var info in queryData)
            {
                IRow rowtemp = sheet1.CreateRow(i + 1);
                if (i == 0)
                {
                    Type type = info.GetType();
                    try
                    {
                        pInfo = type.GetProperties();
                    }
                    catch { return null; }
                    if (pInfo.Length != rowTile.Length)
                        return null;
                }
                int k = 0;
                foreach (PropertyInfo ps in pInfo)
                {
                    object obj = ps.GetValue(info, null);
                    string vules = obj == null ? "" : obj.ToString();
                    rowtemp.CreateCell(k).SetCellValue(vules);
                    k++;
                }
                i++;
            }

            return book;


        }

    }
}
