using System;

namespace PractiveWork3.Commands
{
    internal class ThirdCommand : Command
    {
        public ThirdCommand()
        {
            id = 3;
            description = "Даны целочисленные координаты точки на плоскости. Если точка совпадает с началом координат, то вывести 0. Если точка не совпадает с началом координат, но лежит на оси OX или OY, то вывести соответственно 1 или 2. Если точка не лежит на координатных осях, то вывести 3.";
        }

        internal override void Execute()
        {
            Console.Write("Введите координату X: ");
            int X = Program.IOController.ReadNumer(9, true);

            Console.Write("Введите координату Y: ");
            int Y = Program.IOController.ReadNumer(9, true);

            int result;
            if (X.Equals(Y) && X.Equals(0))
            {
                result = 0;
            }
            else if (X.Equals(0) && !Y.Equals(0))
            {
                result = 1;
            }
            else if (!X.Equals(0) && Y.Equals(0))
            {
                result = 2;
            }
            else
            {
                result = 3;
            }

            Console.WriteLine("Результат: {0}", result);

            return;
        }
    }
}