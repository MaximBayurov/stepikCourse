using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task13 : MyTask
    {
        public Task13()
        {
            StringID = "1.3";
            Description = "Дан двумерный массив размером n*m, заполненный случайным образом. Заменить максимальный по модулю элемент каждой строки на противоположный по знаку.";
            Order = 30;
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
            printer.PrintTable(SwipeMax(array));
        }

        private int[][] SwipeMax(int[][] array)
        {
            int[][] result = new int[array.Length][];
            int max;

            for(int i = 0; i < array.Length; i++)
            {
                max = array[i].OrderBy(value => -Math.Abs(value)).First();
                result[i] = new int[array[i].Length];
                for (int j = 0; j < array[i].Length; j++)
                {
                    result[i][j] = Math.Abs(array[i][j]) == max 
                        ? -array[i][j]
                        : array[i][j];
                }
            }
            return result;
        }
    }
}
