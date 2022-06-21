using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task21 : MyTask
    {
        public Task21()
        {
            StringID = "2.1";
            Description = "Дана матрица размера M×N и целое число K (1≤K≤M). Найти сумму и произведение элементов K-й строки данной матрицы.";
            Order = 40;
        }
        public override void Execute()
        {

            int N, M, K;
            Console.WriteLine("Введите целые числа N и M, определяющие размерность массива");
            Console.Write("M:");
            InputHelper.ReadInt(out M);
            Console.Write("N:");
            InputHelper.ReadInt(out N);

            int[][] array = Generate.Array(M, N);

            TablePrinter printer = new TablePrinter((int.MaxValue.ToString().Length + 2) * N);
            printer.PrintTable(array, columnHeader: Generate.NumberingFor(array));

            Console.Write("K:");
            InputHelper.ReadInt(out K, (value) => {
                bool isValid = value >= 1 & value <= M;
                if (!isValid)
                {
                    Console.WriteLine("Целое число K должно быть между 1 и M (1 <= K <= {0})", value);
                }
                return isValid;
            });

            Console.WriteLine("Сумма {0} строки: {1}", K, EvaluateSum(array[K]));
        }

        private double EvaluateSum(int[] ints)
        {
            double sum = 0;
            foreach(int element in ints)
            {
                sum += element;
            }
            return sum;
        }
    }
}
