using PractiveWork3.Extensions;
using System;
using System.Collections.Generic;

namespace PractiveWork3.Commands
{
    internal class FourthCommand : Command
    {
        public FourthCommand()
        {
            id = 4;
            description = "Даны действительные числа x, y. Если х и у отрицательны, то каждое значение заменить его модулем; если отрицательно только одно из них, то оба значения увеличить на 0.5; если оба значения неотрицательны и ни одно из них не принадлежит отрезку [0.5, 2.0], то оба значения уменьшить в 10 раз; в остальных случаях х и у оставить без изменения.";
        }

        internal override void Execute()
        {
            Console.WriteLine("Примечание: вводите действительные числа, используя запятую как разделитель целой и дробной части.");

            double x = IOController.ReadDoubleFromConsole("Введите число x: ");
            double y = IOController.ReadDoubleFromConsole("Введите число y: ");

            List<String[]> rows = new List<String[]>();
            rows.Add(new string[] { "", "x", "y"});
            rows.Add(new string[] { "Было", x.ToString(), y.ToString()});

            if (x.IsNegative() & y.IsNegative())
            {
                x = Math.Abs(x);
                y = Math.Abs(y);
            }
            else if (x.IsNegative() | y.IsNegative())
            {
                x += 0.5;
                y += 0.5;
            }
            else if (!x.IsBetweenEE(0.5, 2) & !y.IsBetweenEE(0.5, 2))
            {
                x /= 10;
                y /= 10;
            }

            rows.Add(new string[] { "Стало", x.ToString(), y.ToString() });
            IOController.PrintTable(rows);

            return;
        }
    }
}