using System;

namespace PractiveWork3.Commands
{
    internal class SecondCommand : Command
    {
        public SecondCommand()
        {
            id = 2;
            description = "Даны две переменные целого типа: A и B. Если их значения не равны, то присвоить каждой переменной большее из этих значений, а если равны, то присвоить переменным нулевые значения. Вывести новые значения переменных A и B.";
        }

        internal override void Execute()
        {
            Console.Write("Введите целое число A: ");
            int A = Program.IOController.ReadNumer(9, true);

            Console.Write("Введите целое число B: ");
            int B = Program.IOController.ReadNumer(9, true);

            if (A != B)
            {
                int max = Math.Max(A, B);
                A = B = max;
            } else
            {
                A = B = 0;
            }

            Console.WriteLine("Результаты:\nA = {0}\nB = {1}", A, B);

            return;
        }
    }
}