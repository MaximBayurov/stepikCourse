using System;

namespace PractiveWork3.Commands
{
    internal class FirstCommand : Command
    {
        public FirstCommand()
        {
            id = 1;
            description = "Дано целое число. Если оно является положительным, то прибавить к нему 1; в противном случае не изменять его. Вывести полученное число";
        }

        internal override void Execute()
        {
            Console.Write("Введите целое число: ");
            int number = Program.IOController.ReadNumer(9, true);

            number = (number > 0) ? number += 1 : number;
            Console.WriteLine($"Результат: {number.ToString(), 10}");

            return;
        }
    }
}