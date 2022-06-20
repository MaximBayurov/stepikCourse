using PractiveWork3.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PractiveWork3
{
    internal class IOController
    {
        private List<string> Errors = new List<string>();
        public void PrintCommandsList()
        {
            Console.Clear();
            Console.WriteLine("Список доступных программ:");

            CommandsManager manager = CommandsManager.GetInstance();
            foreach (Command command in manager.CommandsList)
            {
                Console.WriteLine($"{command.GetID(),2}. {command.GetPreviewText()}");
            }
        }

        public int ReadCommandID()
        {
            if (HasErrors())
            {
                DumpErrors();
            }

            Console.Write("Введите идентификатор команды: ");
            int commandID = ReadNumer(1);

            CommandsManager manager = CommandsManager.GetInstance();
            if (!manager.HasCommandWithID(Convert.ToDouble(commandID)))
            {
                Errors.Add($"Некорректно введен идентификатор команды. Команды №{commandID} не существует.");
            }

            if (Errors.Count > 0)
            {
                return -1;
            }

            return Convert.ToInt16(commandID);
        }

        internal static void PrintTable(List<string[]> rows)
        {
            string rowTemplate = "|";
            List<int> cellSizes = new List<int>();
            int index;

            foreach (string[] row in rows)
            {
                index = 0;
                foreach (string cell in row)
                {
                    if (cellSizes.Count > index)
                    {
                        cellSizes[index] = Math.Max(cellSizes[index], cell.Length);
                    }
                    else
                    {
                        cellSizes.Add(cell.Length);
                    }
                    index++;
                }
            }

            index = 0;
            foreach (int cellSize in cellSizes)
            {
                rowTemplate += "{" + index + ", -" + (cellSize + 2) + "}|";
                index++;
            }

            index = 0;
            string rowFormatted, separatorTemplate;
            foreach (string[] row in rows)
            {
                rowFormatted = String.Format(rowTemplate, row);

                if (index == 0 || index == (row.Length - 1))
                {
                    StringBuilder sb = new StringBuilder("");
                    for (int i = 1; i < rowFormatted.Length; i++)
                    {
                        sb.Append('—');
                    }
                    separatorTemplate = (index == 0)
                        ? "{0}\n{1}\n{0}"
                        : "{1}\n{0}";
                    rowFormatted = string.Format(
                            separatorTemplate,
                            sb.ToString(),
                            rowFormatted
                        );
                }
                Console.WriteLine(rowFormatted);
                index++;
            }
        }

        internal static double ReadDoubleFromConsole(string label = null)
        {
            if (String.IsNullOrEmpty(label) != true)
            {
                Console.WriteLine(label);
            }

            double doubleNumber;
            string stringNumber;
            bool isIncorrectInput;

            do
            {
                stringNumber = Console.ReadLine();
                isIncorrectInput = double.TryParse(stringNumber, out doubleNumber) != true;

                if (isIncorrectInput)
                {
                    Console.WriteLine("Попробуйте ввести число ещё раз");
                }
            } while (isIncorrectInput);

            return doubleNumber;
        }

        public int ReadNumer(int numberOrder, bool allowNegative = false)
        {
            int charsLimit = allowNegative ? numberOrder + 1 : numberOrder;
            StringBuilder sb = new StringBuilder(charsLimit);
            int curStart = Console.CursorLeft;
            int curOffset = 0;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

                bool isCorrectType = char.IsDigit(keyInfo.KeyChar);
                if (allowNegative)
                {
                    isCorrectType = isCorrectType || (curOffset == 0 && keyInfo.KeyChar == '-');
                    charsLimit = (sb.Length > 0 && (sb[0] == '-' || keyInfo.KeyChar == '-')) ? numberOrder + 1 : numberOrder;
                }

                if (isCorrectType && sb.Length < charsLimit)
                {
                    sb.Insert(curOffset, keyInfo.KeyChar);
                    curOffset++;
                    Console.CursorLeft = curStart;
                    Console.Write(sb.ToString().PadRight(charsLimit));
                }
                if (keyInfo.Key == ConsoleKey.LeftArrow && curOffset > 0) curOffset--;
                if (keyInfo.Key == ConsoleKey.RightArrow && curOffset < sb.Length) curOffset++;
                if (keyInfo.Key == ConsoleKey.Backspace && curOffset > 0)
                {
                    curOffset--;
                    sb.Remove(curOffset, 1);
                    Console.CursorLeft = curStart;
                    Console.Write(sb.ToString().PadRight(charsLimit));
                }
                if (keyInfo.Key == ConsoleKey.Delete && curOffset < sb.Length)
                {
                    sb.Remove(curOffset, 1);
                    Console.CursorLeft = curStart;
                    Console.Write(sb.ToString().PadRight(charsLimit));
                }
                Console.CursorLeft = curStart + curOffset;
            }
            while (!(keyInfo.Key == ConsoleKey.Enter && sb.Length > 0));
            Console.WriteLine();
            return int.Parse(sb.ToString());
        }

        internal void ExecuteCommand(Command Command)
        {
            Console.Clear();
            Console.WriteLine(
                "{0}\n{1}\n",
                Command.GetPreviewText(),
                Command.GetDescription()
            );

            Command.Execute();

            Console.WriteLine("\nНажмите любую кнопку чтобы продолжить.");
            Console.ReadKey();
        }

        private void DumpErrors()
        {
            foreach (string error in Errors)
            {
                Console.WriteLine(error);
            }

            Errors.Clear();
        }

        internal bool HasErrors()
        {
            return Errors.Count > 0;
        }
    }
}