using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7.Tasks
{
    internal class Task23 : MyTask
    {
        public Task23()
        {
            StringID = "2.3";
            Description = "Дана квадратная матрица порядка M. Обнулить элементы матрицы, лежащие выше побочной диагонали. Условный оператор не использовать.";
            Order = 60;
        }
        public override void Execute()
        {
            int M;
            Console.WriteLine("Введите целое число M, определяющее размерность массива");
            Console.Write("M:");
            InputHelper.ReadInt(out M);

            int[][] array = Generate.Array(M, M);

            TablePrinter printer = new TablePrinter((int.MaxValue.ToString().Length + 2) * M);
            printer.PrintTable(array);
            printer.PrintTable(ResetSideDiagonalFor(array));
        }

        private T[][] ResetSideDiagonalFor<T>(T[][] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                for(int j = 0; j < array[i].Length - 1 - i; j++)
                {
                    array[i][j] = default(T);
                }
            }
            return array;
        }
    }
}
