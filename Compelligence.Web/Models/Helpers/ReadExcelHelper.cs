using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Data.OleDb;
using NPOI.HSSF.UserModel;

namespace Compelligence.Web.Models.Helpers
{
    public static class ReadExcelHelper
    {
        /// <summary>
        /// Render a Excel 2007 (xlsx) Worksheet to NPOI Excel 2003 Worksheet, all excel formatting
        /// from XLSX will be lost when converted.  NPOI roadmap shows v1.6 will support Excel 2007 (xlsx).
        /// NPOI Roadmap  : http://npoi.codeplex.com/wikipage?title=NPOI%20Road%20Map&referringTitle=Documentation
        /// NPOI Homepage : http://npoi.codeplex.com/
        /// </summary>
        /// <param name="excelFileStream">XLSX FileStream</param>
        /// <param name="sheetName">Excel worksheet to convert</param>
        /// <returns>MemoryStream containing NPOI Excel workbook</returns>
        /// 
        public static IList<string> SheetTotal(Stream excelFileStream, string location)
        {
            string sheetName = "";
            IList<string> valueList = new List<string>();
            try
            {
                string Connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + location + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                OleDbConnection con = new OleDbConnection(Connection);
                con.Open();
                DataTable dtSheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                    {
                        sheetName = drSheet["TABLE_NAME"].ToString();

                        valueList.Add(sheetName);
                    }
                }
                con.Close();
            }
            finally
            {
                // Make sure temp file is deleted
                File.Delete(location);
            }
            // Return a new POI Excel 2003 Workbook
            return valueList;
        }
        public static Stream ConvertXLSXWorksheetToXLSWorksheet(Stream excelFileStream, string sheetName, string location)
        {
            IList<string> valueList = new List<string>();
            IList<DataTable> dtlist = new List<DataTable>();
            // Temp data container (using DataTable to leverage existing RenderDataTableToExcel function)

            try
            {
                // Create temp XLSX file
                FileStream fileStream = new FileStream(location, FileMode.Create, FileAccess.Write);
                const int length = 256;
                Byte[] buffer = new Byte[length];
                int bytesRead = excelFileStream.Read(buffer, 0, length);
                while (bytesRead > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    bytesRead = excelFileStream.Read(buffer, 0, length);
                }
                excelFileStream.Close();
                fileStream.Close();
                string Connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + location + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
                OleDbConnection con = new OleDbConnection(Connection);
                con.Open();
                DataTable dtSheet = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                foreach (DataRow drSheet in dtSheet.Rows)
                {
                    if (drSheet["TABLE_NAME"].ToString().Contains("$"))//checks whether row contains '_xlnm#_FilterDatabase' or sheet name(i.e. sheet name always ends with $ sign)
                    {
                        DataTable dt = new DataTable();
                        sheetName = drSheet["TABLE_NAME"].ToString();
                        valueList.Add(sheetName);
                        OleDbCommand command = new OleDbCommand("SELECT * FROM[@tblName]", con);
                        command.Parameters.AddWithValue("@tblName", sheetName);

                        //OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", con);
                        OleDbDataAdapter myCommand = new OleDbDataAdapter(command);
                        myCommand.Fill(dt);
                        dtlist.Add(dt);
                    }
                }

                con.Close();
            }
            finally
            {
                // Make sure temp file is deleted

            }
            // Return a new POI Excel 2003 Workbook

            return RenderDataTableToExcel(dtlist, valueList);

        }

        /// <summary>
        /// Render DataTable to NPOI Excel 2003 MemoryStream
        /// NOTE:  Limitation of 65,536 rows suppored by XLS
        /// </summary>
        /// <param name="sourceTable">Source DataTable</param>
        /// <returns>MemoryStream containing NPOI Excel workbook</returns>
        public static Stream RenderDataTableToExcel(IList<DataTable> dtlist, IList<string> valueList)
        {
            int i = 0;
            int j = valueList.Count - 1;
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream memoryStream = new MemoryStream();
            // By default NPOI creates "Sheet0" which is inconsistent with Excel using "Sheet1"
            foreach (DataTable dt in dtlist)
            {
                HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet(valueList[i]);
                HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                // Header Row
                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                // Detail Rows
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in dt.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    rowIndex++;
                }
                workbook.Write(memoryStream);
                //memoryStream.Flush();
                memoryStream.Position = 0;
                i++;
                j--;
            }
            return memoryStream;
        }
    }
}
