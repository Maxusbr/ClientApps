using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace DTCDev.Client.Cars.Controls.Models
{
    public class ExportToExcel<T>
        where T : class
    {
        public List<T> DataToPrint;

        private Excel.Application _excelApp = null;
        private Excel.Workbooks _books = null;
        private Excel._Workbook _book = null;
        private Excel.Sheets _sheets = null;
        private Excel._Worksheet _sheet = null;
        private Excel.Range _range = null;
        private Excel.Font _font = null;

        private object _optionalValue = Missing.Value;
        private object[] header;

        public void GenerateReport(object[] headerToAdd = null)
        {
            try
            {
                header = headerToAdd;
                if (DataToPrint == null || DataToPrint.Count == 0) return;
                Mouse.SetCursor(Cursors.Wait);
                CreateExcelRef();
                FillSheet();
                OpenReport();
                Mouse.SetCursor(Cursors.Arrow);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while generating Excel report");
            }
            finally
            {
                ReleaseObject(_sheet);
                ReleaseObject(_sheets);
                ReleaseObject(_book);
                ReleaseObject(_books);
                ReleaseObject(_excelApp);
            }
        }

        private void OpenReport()
        {
            _excelApp.Visible = true;
        }

        private void FillSheet()
        {
            header = header == null ? CreateHeader() : CreateHeader(header);
            WriteData(header);
        }

        private void WriteData(IList<object> header)
        {
            var objData = new object[DataToPrint.Count, header.Count];

            for (var j = 0; j < DataToPrint.Count; j++)
            {
                var item = DataToPrint[j];
                for (var i = 0; i < header.Count; i++)
                {
                    var y = typeof(T).InvokeMember(header[i].ToString(),
                    BindingFlags.GetProperty, null, item, null);
                    objData[j, i] = (y == null) ? "" : ObjToString(y);
                }
            }
            AddExcelRows("A2", DataToPrint.Count, header.Count, objData);
            AutoFitColumns("A1", DataToPrint.Count + 1, header.Count);
        }

        private static object ObjToString(object obj)
        {
            if (obj is double) return obj;
            if (obj is int) return obj;
            return obj.ToString();
        }

        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.Range[startRange, _optionalValue];
            _range = _range.Resize[rowCount, colCount];
            _range.Columns.AutoFit();
        }

        private object[] CreateHeader()
        {
            var headerInfo = typeof(T).GetProperties();

            var headerToAdd = headerInfo.Select(t => t.Name).Cast<object>().ToArray();
            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle();

            return headerToAdd;
        }

        private object[] CreateHeader(object[] headerToAdd)
        {
            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle();
            var headerInfo = typeof(T).GetProperties();

            return headerInfo.Select(t => t.Name).Cast<object>().ToArray();
        }
        private void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }

        private void AddExcelRows(string startRange, int rowCount,
        int colCount, object values)
        {
            _range = _sheet.Range[startRange, _optionalValue];
            _range = _range.Resize[rowCount, colCount];
            _range.set_Value(_optionalValue, values);
        }

        private void CreateExcelRef()
        {
            _excelApp = new Excel.Application();
            _books = (Excel.Workbooks)_excelApp.Workbooks;
            _book = (Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Excel.Sheets)_book.Worksheets;
            _sheet = (Excel._Worksheet)(_sheets.Item[1]);
        }

        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
