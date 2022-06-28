using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PracticalWork9
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
            return "Из множества целых чисел 1.. 100 выделить множество чисел, на которые делится без остатка число 4. Вывести это множество на экран.";
        }
        public override void Execute()
        {
            int N;
            string inputMessage = "Введите целое число больше нуля и не больше 10000:";

            Console.WriteLine(inputMessage);
            while (int.TryParse(Console.ReadLine(), out N) != true || N < 1 || N > 10000)
            {
                Console.WriteLine(inputMessage);
            }
            Console.WriteLine("Целые числа в диапазоне от 1 до {0}:", N);

            List<int> fourMultiples = new List<int>();
            for (int i = 4; i <= N; i += 4)
            {
                fourMultiples.Add(i);
            }
            Console.WriteLine(
                String.Join(", ", fourMultiples)
            );
        }
    }

    internal class Job2 : Job
    {
        char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public new static string GetDescription()
        {
            return "Из множества латинских букв выделить множество, состоящее из символов, не входящих в множество символов, составляющих ваше имя. Вывести это множество на экран.";
        }
        public override void Execute()
        {
            string inputMessage = "Введите ваше имя:";
            string name;

            Console.WriteLine(inputMessage);
            name = Console.ReadLine();
            char[] result = alpha.Except(name.ToUpper().ToCharArray()).ToArray();

            string template = result.Length > 0
                ? "В вашем имени нет следующих букв латинского алфавита:\n{0}"
                : "В вашем имени все буквы латинского алфавита";

            Console.WriteLine(
                template,
                string.Join(", ", result)
            );
        }
    }

    internal class Job3 : Job
    {
        public new static string GetDescription()
        {
            return "Определить и вывести на экран множество символов, входящих одновременно в имя, отчество и фамилию, введённые с клавиатуры.";
        }
        public override void Execute()
        {
            string inputMessage = "Введите ваше ФИО через пробел:";
            string[] FIO;

            do
            {
                Console.WriteLine(inputMessage);
                FIO = Console.ReadLine().ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (FIO.Length == 3)
                {
                    break;
                }

                Console.Write("ФИО должно состоять из трёх частей: фамилии, имя и отчества. ");
            } while (true);

            char[] result = FIO[0].ToCharArray().Intersect(
                    FIO[1].ToCharArray().Intersect(
                        FIO[2].ToCharArray()
                    )
                ).OrderBy(x => {
                    return (int)x; 
                }).ToArray();

            Console.WriteLine(
                "Эти буквы одновременно есть в вашей фамилии, имени и отчестве:\n{0}",
                string.Join(", ", result)
            );
        }
    }
    internal class Job4 : Job
    {
        public new static string GetDescription()
        {
            return "Даны названия 26 – ти городов и стран, в которых они находятся. Среди них есть города, находящиеся в стране из списка, вводимой пользователем с клавиатуры. Напечатать их названия.";
        }
        public override void Execute()
        {
            List<CityInfo> citiesInfo;
            using (StreamReader r = new StreamReader("job4_config.json"))
            {
                string json = r.ReadToEnd();
                citiesInfo = JsonSerializer.Deserialize<List<CityInfo>>(json);
            }
            string[] countries = citiesInfo.GroupBy(x => x.country).Select(x => { 
                return x.Key; 
            }).ToArray();

            string country;
            do
            {
                Console.WriteLine("Список стран:");
                for (int i = 0; i < countries.Length; i++)
                {
                    Console.WriteLine(
                        "{0, 4}. {1}",
                        i + 1,
                        countries[i]
                    );
                }
                Console.WriteLine("Введите название страны из списка:");
                country = Console.ReadLine();
                if (countries.Contains(country, new CountryComparer()))
                {
                    break;
                }
                Console.Clear();
            } while (true);

            string[] result = citiesInfo.Where(x => {
                return x.country.ToLower() == country.ToLower(); 
            }).Select(x => x.city).ToArray();
            Console.WriteLine(
                "Города страны \"{1}\" в списке:\n{0}",
                string.Join(", ", result),
                country
            );
        }

        class CityInfo
        {
            public string city { get; set; }
            public string country { get; set; }
        }

        class CountryComparer: IEqualityComparer<string>
        {
            public bool Equals(string c1, string c2)
            {
                return string.Equals(c1.ToLower(), c2.ToLower());
            }
            public int GetHashCode(string country)
            {
                return country.GetHashCode();
            }
        }
    }
}
