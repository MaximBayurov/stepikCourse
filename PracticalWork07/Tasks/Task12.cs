using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task12 : MyTask
    {
        public Task12()
        {
            StringID = "1.2";
            Description = "Заполнить массив n*n по правилу";
            Order = 20;
        }
        public override void Execute()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Task12Form form = new Task12Form();
                System.Windows.Forms.Application.Run(form);
            }));
            thread.IsBackground = true;
            thread.Start();

            Console.WriteLine("Введите размерность массива N на N.");

            int N;
            Console.Write("N:");
            InputHelper.ReadInt(out N);

            TablePrinter printer = new TablePrinter((N.ToString().Length + 2) * N);
            printer.PrintTable(FillArray(N));
        }

        private int[,] FillArray(int N)
        {
            int[,] array = new int[N, N];
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    array[i, j] = i + 1;
                }
            }
            return array;
        }
    }
}
