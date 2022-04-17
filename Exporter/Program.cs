using ClosedXML.Excel;
using Exporter.DTO;
using LumenWorks.Framework.IO.Csv;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Exporter
{
    class Program
    {
        static void Main(string[] args)
        {

            var date = DateTime.Now.ToShortDateString();
            var covidSummary = new List<CovidSummary> {
            new CovidSummary
            { 
                DeathsNumber=12,
                ActiveNumber=24,
                country="UAE",
                Date=date
            },
             new CovidSummary
             { 
                 DeathsNumber=56,
                 ActiveNumber=57,
                 country="UAE",
                 Date=date
             }
            };

            var covidHistories = new List<CovidHistory> {
            new CovidHistory{ DeathsNumber=1200,Country="UAE"},
              new CovidHistory{ DeathsNumber=3434989,Country="KSA"},
            };
            // to test covid histories, replace it in line 45
            WriteToExcel(covidSummary);
            Read();
        }

        static void WriteToExcel<T>(List<T> data)
        {
            List<string> savedData = new List<string>();
            var lines = new List<string>();
            DataTable dataTable = ToDataTable(data);
            string[] columnNames = dataTable.Columns
                .Cast<DataColumn>()
                .Select(column => column.ColumnName)
                .ToArray();

            var header = string.Join(",", columnNames.Select(name => $"\"{name}\""));
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable()
                .Select(row => string.Join(",", row.ItemArray.Select(val => $"\"{val}\"")));

            lines.AddRange(valueLines);


            var fileName = Directory.GetCurrentDirectory().Split("bin")[0] + "FileService\\CovidResult.csv";

            File.WriteAllLines(fileName, lines);
        }

        static void Read()
        {
            var FullFilePath = Directory.GetCurrentDirectory().Split("bin")[0] + "FileService\\CovidResult.csv";
            using (CsvReader csv =
                   new CsvReader(new StreamReader(FullFilePath), true))
            {
                int fieldCount = csv.FieldCount;
                string[] headers = csv.GetFieldHeaders();
        
                while (csv.ReadNextRecord())
                {
                    for (int i = 0; i < fieldCount; i++)
                        Console.Write(string.Format("{0} = {1};",
                                      headers[i], csv[i]));

                    Console.WriteLine();
                }
            }
        }

       public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {

                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}