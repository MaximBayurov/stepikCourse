using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    internal class InputHelper
    {
        public static void ReadInt(out int var, Func<int, bool> additionalValidator = null, bool allowNegative = false)
        {
            while (true)
            {
                while (!int.TryParse(Console.ReadLine(), out var))
                {
                    Console.WriteLine(
                        String.Format("{0} не является целым числом. Введите целое число: ", var
                        )
                    );
                }

                if (allowNegative == false & var <= 0)
                {
                    Console.WriteLine(
                        String.Format("{0} отрицательное число. Введите положительное число, больше нуля: ", var
                        )
                    );
                    continue;
                }

                if (additionalValidator != null && additionalValidator(var) != true)
                {
                    continue;
                }

                break;
            }
        }
    }
}
