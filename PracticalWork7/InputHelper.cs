using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    internal class InputHelper
    {
        public static void ReadInt(out int var, Func<int, bool> additionalValidator = null)
        {
            Enter:
            while (!int.TryParse(Console.ReadLine(), out var))
            {
                Console.WriteLine(
                    String.Format("{0} не является целым числом. Введите целое число: ", var
                    )
                );
            }
            if (additionalValidator != null && additionalValidator(var) != true)
            {
                goto Enter;
            } 
        }
    }
}
