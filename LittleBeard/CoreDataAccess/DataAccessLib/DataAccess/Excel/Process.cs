using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBeard.Core.Lib.Excel;

public class ExcelProcessor
{
    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <param name="file"></param>
    /// <param name="headers">列名</param>
    /// <param name="datalists">列对应的具体数据列表</param>
    /// <returns></returns>
    public async Task ExportExcel(FileInfo file, List<string> headers, List<List<string>> datalists)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        DeleteIfExists(file);
        // 在方法的结尾会调用 dispose
        using var package = new ExcelPackage(file);
        var ws = package.Workbook.Worksheets.Add("data");

        for (int i = 0; i < headers.Count; i++)
        {
            ws.Cells[1, i + 1].Value = headers[i];
        }

        int recordIdx = 2;
        foreach (var data in datalists)
        {
            for (int i = 0; i < data.Count; i++)
            {
                ws.Cells[recordIdx, i + 1].Value = data[i];
            }
            recordIdx++;
        }
        await package.SaveAsync();
        Console.WriteLine($"Done");
    }

    private static void DeleteIfExists(FileInfo file)
    {
        if (file.Exists)
        {
            file.Delete();
        }
    }

}
