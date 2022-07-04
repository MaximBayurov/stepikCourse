using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task22 : MyTask
    {
        public Task22()
        {
            StringID = "2.2";
            Description = "Дана матрица размера M×N и целые числа K1 и K2 (1≤K1<K2≤M). Поменять местами строки матрицы с номерами K1 и K2 .";
            Order = 50;
        }
        public override void Execute()
        {
            int N, M, K2, K1;
            Console.WriteLine("Введите целые числа M и N, определяющие размерность массива");
            Console.Write("M:");
            InputHelper.ReadInt(out M);
            Console.Write("N:");
            InputHelper.ReadInt(out N);

            int[][] array = Generate.Array(M, N);

            TablePrinter printer = new TablePrinter((int.MaxValue.ToString().Length + 2) * N);
            printer.PrintTable(array, columnHeader: Generate.NumberingFor(array));

            Console.Write("K2:");
            InputHelper.ReadInt(out K2, (value) => {
                bool isValid = 1 <= value & value <= M;
                if (!isValid)
                {
                    Console.WriteLine("Целое число K2 должно быть между 1 и M (1 <= K2 <= {0})", M);
                }
                return isValid;
            });

            Console.Write("K1:");
            InputHelper.ReadInt(out K1, (value) => {
                bool isValid = 1 <= value & value <= K2;
                if (!isValid)
                {
                    Console.WriteLine("Целое число K1 должно быть между 1 и К2 (1 <= K1 <= {0})", K2);
                }
                return isValid;
            });

            printer.PrintTable(SwipeRows(array, K1-1, K2-1), columnHeader: Generate.NumberingFor(array));
        }

        private int[][] SwipeRows(int[][] array, int k1, int k2)
        {
            int[] bufferRow = array[k1];
            array[k1] = array[k2];
            array[k2] = bufferRow;

            return array;
        }
    }
}
