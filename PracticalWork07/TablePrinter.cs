using System;
using System.Linq;

namespace PracticalWork7
{
    internal class TablePrinter
    {
        int _tableWidth;

        public TablePrinter(int width = 73)
        {
            _tableWidth = width;
        }

        public void PrintTable<T>(T[][] array, string[] header = null, string[] columnHeader = null)
        {
            if (header != null)
            {
                header = PrepareArray(header, array[0].Length);
            }
            if (columnHeader != null)
            {
                columnHeader = PrepareArray(columnHeader, array.Length);
            }

            int additionalWidth = 0;
            if (columnHeader != null && columnHeader.Length > 0)
            {
                additionalWidth = columnHeader.OrderBy((value) => { return (value != null) ? -value.Length : 0; }).First().Length + 2;
            }
            PrintLine(additionalWidth);

            if (header != null)
            {
                PrintColumnHeadCell(null, additionalWidth);
                PrintRow(header);
                PrintLine(additionalWidth);
            }

            for (int index = 0; index < array.Length; index++)
            {
                if (columnHeader != null && columnHeader.Length > 0)
                {
                    PrintColumnHeadCell(columnHeader[index], additionalWidth);
                }
                PrintRow(array[index]);
            }
            PrintLine(additionalWidth);
        }

        private T[] PrepareArray<T>(T[] array, int length)
        {
            if (array.Length > length)
            {
                return array.Take(length-1).ToArray();
            }

            if (array.Length < length)
            {
                Array.Resize(ref array, length);
                return array;
            }

            return array;
        }

        private void PrintColumnHeadCell(string cell, int width)
        {
            Console.Write($"|{{{0}, -{width-1}}}", cell);
        }

        public void PrintTable<T>(T[,] array, string[] header = null, string[] columnHeader = null)
        {
            int rows = array.GetUpperBound(0) + 1;
            int columns = array.Length / rows;

            T[][] convertedArray = new T[rows][];
            for (int i = 0; i < rows; i++)
            {
                convertedArray[i] = new T[columns];
                for (int j = 0; j < columns; j++)
                {
                    convertedArray[i][j] = array[i, j];
                }
            }

            PrintTable(convertedArray, header, columnHeader);
        }

        private void PrintLine(int additionalWidth = 0)
        {
            Console.WriteLine(new string('-', _tableWidth + additionalWidth));
        }

        private void PrintRow<T>(T[] columns)
        {
            int width = (_tableWidth - columns.Length) / columns.Length;
            string row = "|", columnString;

            foreach (T column in columns)
            {
                columnString = column != null ? column.ToString() : " ";
                row += AlignCentre(columnString, width) + "|";
            }

            Console.WriteLine(row);
        }

        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
