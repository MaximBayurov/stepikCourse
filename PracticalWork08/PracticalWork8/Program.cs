using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PracticalWork8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string jobsPrefix = "Job";
            Type[] typelist = GetTypesInNamespace(
                Assembly.GetExecutingAssembly(),
                typeof(Program).Namespace,
                (Type type) =>
                {
                    return type.Name.StartsWith(jobsPrefix) && type.BaseType == typeof(Job);
                }
                );
            for (int i = 0; i < typelist.Length; i++)
            {
                Console.WriteLine("{0}. {1}", i + 1, typelist[i].GetMethod("GetDescription").Invoke(null, null));
            }

            short number;
            do
            {
                Console.WriteLine("Введите номер задачи:");
                if (Int16.TryParse(System.Console.ReadLine(), out number) != true)
                {
                    continue;
                }
                number--;
                if (0 <= number && number < typelist.Length)
                {
                    break;
                }
            } while (true);
            Job job = (Job)Activator.CreateInstance(typelist[number]);
            Console.Clear();
            Console.WriteLine("Задача №{0}\n{1}",
                number + 1,
                job.GetType().GetMethod("GetDescription").Invoke(null, null)
            );
            job.Execute();
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey(false);
        }
        static private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace, Func<Type, bool> addFilter = null)
        {
            IEnumerable<Type> typesEnum = assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal));
            if (addFilter != null)
            {
                typesEnum = typesEnum.Where(addFilter);
            }
            return typesEnum.ToArray();
        }
    }

    abstract class Job
    {
        public abstract void Execute();
        public static string GetDescription()
        {
            throw new NotImplementedException();
        }
    }

    internal class Job1 : Job
    {
        public new static string GetDescription()
        {
            return "Дан символ C. Вывести его код.";
        }
        public override void Execute()
        {

            Console.Write("Введите символ C: ");
            char ch;
            string input;
            while (Char.TryParse(input = Console.ReadLine(), out ch) != true)
            {
                Console.WriteLine("Введите один символ. Строка '{0}' не является символом", input);
                Console.Write("Введите символ: ");
            }
            Console.WriteLine("С: '{0}'; Код: '\\u{1,4:X4}'", ch, (int)ch);
        }
    }

    internal class Job2 : Job
    {
        public new static string GetDescription()
        {
            return "Дана строка. Подсчитать количество содержащихся в ней цифр.";
        }
        public override void Execute()
        {

            Console.Write("Введите строку: ");
            string input = System.Console.ReadLine();
            int numbersCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                numbersCount += Char.IsNumber((Char)input[i])
                    ? 1
                    : 0;
            }
            Console.WriteLine("Всего {0} цифр в строке {1}", numbersCount, input);
        }
    }

    internal class Job3 : Job
    {
        public new static string GetDescription()
        {
            return "Дано целое число N(>0) и строка S. Преобразовать строку S в строку длины N следующим образом: если длина строки S больше N, то отбросить первые символы, если длина строки S меньше N, то в ее начало добавить символы «.» (точка).";
        }
        public override void Execute()
        {

            Console.Write("Введите строку S: ");
            string S = System.Console.ReadLine();
            int N;

            Console.Write("Введите число N: ");
            while (Int32.TryParse(Console.ReadLine(), out N) != true || N <= 0)
            {
                Console.Write("Введите число N: ");
            }

            string result = null;
            if (S.Length >= N)
            {
                result = S.Substring(S.Length - N);
            }
            else
            {
                result = string.Concat(Enumerable.Repeat(".", N - S.Length));
                result = string.Concat(result, S);
            }
            Console.WriteLine("Результат: {0}", result);
        }
    }

    internal class Job4 : Job
    {
        public new static string GetDescription()
        {
            return "Дана строка, состоящая из русских слов, разделенных пробелами (одним или несколькими). Найти количество слов в строке";
        }
        public override void Execute()
        {

            Console.Write("Введите строку: ");
            string S = System.Console.ReadLine();
            int result = S.Split(
                new char[]
                {
                    ' ',
                    char.Parse("\t"),
                    '.',
                    ',',
                    ';',
                    ':'
                },
                StringSplitOptions.RemoveEmptyEntries
            ).Length;

            Console.WriteLine("Количество слов в строке: {0}", result);
        }
    }

    internal class Job5 : Job
    {
        private static char[,] _brakets = new char[3, 2]
            {
                { '[', ']' },
                { '(', ')' },
                { '{', '}' },
            };

        private char[] _openBrakets;
        private char[] _closeBrakets;

        public Job5()
        {
            _openBrakets = new char[_brakets.GetLength(0)];
            _closeBrakets = new char[_brakets.GetLength(0)];
            for (int i = 0; i < _brakets.GetLength(0); i++)
            {
                _openBrakets[i] = (_brakets[i, 0]);
                _closeBrakets[i] = (_brakets[i, 1]);
            }
        }

        public new static string GetDescription()
        {
            return "Дана строка, содержащая латинские буквы и скобки трех видов: \"()\", \"[]\", \"{}\". Если скобки расставлены правильно (то есть каждой открывающей соответствует закрывающая скобка того же вида), то вывести число 0. В противном случае вывести или номер позиции, в которой расположена первая ошибочная скобка, или, если закрывающих скобок не хватает, число -1.";
        }
        public override void Execute()
        {

            Console.Write("Введите строку: ");
            string S = System.Console.ReadLine();

            List<CharInfo> chars = MakeCharInfoListFrom(S.ToCharArray());

            Console.WriteLine(CheckIsCorrectBrackets(chars));
        }

        private int CheckIsCorrectBrackets(List<CharInfo> chars)
        {

            var braketsQuery =
                from element in chars
                where element.isBracket == true
                select new
                {
                    index = element.index,
                    value = element.value,
                };
            int[] depths = new int[_brakets.GetLength(0)];
            int braketType;

            foreach (var braket in braketsQuery.ToArray())
            {
                braketType = GetBraketType(braket.value);
                depths[braketType] += IsOpenBraket(braket.value) ? 1 : -1;
                if (depths[braketType] < 0)
                {
                    return braket.index + 1;
                }
            }

            foreach (int depth in depths)
            {
                if (depth != 0)
                {
                    return -1;
                }
            }

            return 0;
        }

        private int GetBraketType(char value)
        {
            char[] types = _closeBrakets.Contains(value) ? _closeBrakets : _openBrakets;
            return Array.IndexOf(types, value);
        }

        private bool IsCloseBraket(char value)
        {
            return _closeBrakets.Contains(value);
        }

        private bool IsOpenBraket(char value)
        {
            return _openBrakets.Contains(value);
        }

        private List<CharInfo> MakeCharInfoListFrom(char[] chars)
        {
            List<CharInfo> result = new List<CharInfo>();
            int index = 0;

            foreach (char c in chars)
            {
                result.Add(new CharInfo(
                    IsBraket(c),
                    c,
                    index++
                    )
                );
            }
            return result;
        }

        private bool IsBraket(char element)
        {
            if (_openBrakets.Contains(element) || _closeBrakets.Contains(element))
            {
                return true;
            }
            return false;
        }

        internal class CharInfo
        {
            public bool isBracket;
            public char value;
            public int index;

            internal CharInfo(bool isBracket, char value, int index)
            {
                this.isBracket = isBracket;
                this.value = value;
                this.index = index;
            }
        }
    }
}
