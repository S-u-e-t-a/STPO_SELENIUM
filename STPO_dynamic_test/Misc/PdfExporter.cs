using System.Collections;
using System.Collections.Generic;
using System.Linq;

using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using Microsoft.VisualBasic;

using STPO_dynamic_test.Model;
using STPO_dynamic_test.ParametersVM;


namespace STPO_dynamic_test.Misc
{
    public static class PdfExporter
    {
        public static void Export(string path, IEnumerable<TestInDataGrid> tests, TestParametersVM testParameters)
        {
            var writer = new PdfWriter(path);
            var pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(PageSize.A4.Rotate());
            var document = new Document(pdf);
            var fontFilename = "../../../resources/fonts/Times_New_Roman.ttf";


            var font = PdfFontFactory.CreateFont(fontFilename, PdfEncodings.IDENTITY_H);
            document.SetFont(font);

            var header = new Paragraph("Отчёт о тестировании").SetFont(font)
                                                              .SetTextAlignment(TextAlignment.CENTER)
                                                              .SetFontSize(20);

            document.Add(header);

            // document.Add(new Paragraph("Входные данные"));
            // var initialTable = new Table(UnitValue.CreatePercentArray(2)).UseAllAvailableWidth();
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph("Нач. значение").SetTextAlignment(TextAlignment.CENTER)));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph(VariableToString(testParameters.Left))));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph("Кон. значение").SetTextAlignment(TextAlignment.CENTER)));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph(VariableToString(testParameters.Right))));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph("Шаг").SetTextAlignment(TextAlignment.CENTER)));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph(VariableToString(testParameters.Step))));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph("Методы").SetTextAlignment(TextAlignment.CENTER)));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph(MethodsToString(testParameters.SelectedMethods))));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph("Коэффициенты").SetTextAlignment(TextAlignment.CENTER)));
            // initialTable.AddCell(new Cell(1, 1).Add(new Paragraph(VariableArrayToString(testParameters.Coefs))));
            // document.Add(initialTable);

            document.Add(new Paragraph("Тесты"));
            var testTable = new Table(UnitValue.CreatePercentArray(8)).UseAllAvailableWidth();
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Нач. значение").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Правая граница").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Шаг").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Метод").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Коэффициенты").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Ожидаемое значение").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Фактическое значение").SetTextAlignment(TextAlignment.CENTER)));
            testTable.AddHeaderCell(new Cell(1, 1).Add(new Paragraph("Пройден").SetTextAlignment(TextAlignment.CENTER)));

            // foreach (var test in tests)
            // {
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.Test.InitialTestData.Min)));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.Test.InitialTestData.Max)));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.Test.InitialTestData.Step)));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.Test.InitialTestData.IntegrateMethod)));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.Test.InitialTestData.CoefsString)));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(ResultToString(test.Test.ResultExpected))));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(ResultToString(test.Test.ResultFact))));
            //     testTable.AddCell(new Cell(1, 1).Add(new Paragraph(test.IsPassed.ToString())));
            // }

            document.Add(testTable);

            document.Close();
        }

        private static string VariableToString(VariableParameter p)
        {
            if (!p.IsVariable)
            {
                return $"Фиксированное значение {p.Value}";
            }

            return $"Диапазон: [{p.Min};{p.Max}]";
        }

        // private static string VariableArrayToString(VariableParameterArray p)
        // {
        //     if (!p.IsVariable)
        //     {
        //         return $"Фиксированное значение {p.Value}";
        //     }
        //
        //     return $"Диапазон: [{p.Min};{p.Max}]; Количество: {p.Count}";
        // }
        
        private static string ResultToString(Result res)
        {
            if (res is null)
            {
                return "-";
            }

            return res.ToString();
        }
    }
}
