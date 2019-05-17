using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleClosedXml
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello ClosedXML World!");
            var app = new Program();
            // app.GoRead();
            app.GoWrite();
        }

        void GoRead()
        {
            string path = @"c:\azfunc\sample.xlsx";
            using (var st = System.IO.File.OpenRead(path))
            {
                var wb = new XLWorkbook(st);
                var sh = wb.Worksheets.First();

                var id = sh.Cell(1, 2).Value;           // ID
                var company = sh.Cell(2, 2).Value;      // 会社
                var person = sh.Cell(3, 2).Value;       // 担当者
                var apartment = sh.Cell(4, 2).Value;    // 部署

                Console.WriteLine($"{id} {company} {person} {apartment}");
            }
        }

        public class AddressBook
        {
            public int ID { get; set; }
            public string Company { get; set; }
            public string Person { get; set; }
            public string Apartment { get; set; }
        }

        /// <summary>
        /// データの書き込み
        /// </summary>
        void GoWrite()
        {
            var templatepath = @"c:\azfunc\template.xlsx";
            using (var st = System.IO.File.OpenRead(templatepath))
            {
                var wb = new XLWorkbook(st);
                var sh = wb.Worksheets.First();

                var items = new List<AddressBook>();
                items.Add(new AddressBook()
                {
                    ID = 1,
                    Company = "日経BP",
                    Person = "日経太郎",
                    Apartment = "出版部"
                });
                items.Add(new AddressBook()
                {
                    ID = 2,
                    Company = "日経BP",
                    Person = "日経次郎",
                    Apartment = "営業部"
                });
                items.Add(new AddressBook()
                {
                    ID = 3,
                    Company = "Microsoft",
                    Person = "アジュール花子",
                    Apartment = "開発部"
                });
                int row = 2;
                foreach (var it in items)
                {
                    sh.Cell(row, 1).Value = it.ID;
                    sh.Cell(row, 2).Value = it.Company;
                    sh.Cell(row, 3).Value = it.Person;
                    sh.Cell(row, 4).Value = it.Apartment;
                    row++;
                }
                var path = @"c:\azfunc\sample_output.xlsx";
                wb.SaveAs(path);
            }
        }
    }
}
