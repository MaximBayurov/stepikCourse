using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task11 : MyTask
    {
        public Task11()
        {
            StringID = "1.1";
            Description = "Дан двумерный массив размером n*m, заполненный случайными числами. Определить элемент массива, имеющий наибольшее значение.";
            Order = 10;
        }
        public override void Execute()
        {
            int N, M;
            Console.WriteLine("Введите целые числа N и M, определяющие размерность массива");
            Console.Write("N:");
            InputHelper.ReadInt(out N);
            Console.Write("M:");
            InputHelper.ReadInt(out M);

            int[][] array = Generate.Array(N, M);

            TablePrinter printer = new TablePrinter(100);
            printer.PrintTable(array);

            Console.WriteLine("Самое большое число: {0}", FindMax(array));
        }

        private int FindMax(int[][] array)
        {
            int[] result = new int[array.Length];
            for(int index = 0; index < array.Length; index++)
            {
                result[index] = array[index].OrderBy(value => -value).First();
            }
            return result.OrderBy(value => -value).First();
        }
    }
}
