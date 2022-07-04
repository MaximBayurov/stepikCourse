using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PracticalWork11
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

    class FilesHelper
    {
        private static Random random = new Random();
        public static FileMode GetFileModeFor(string fileName)
        {
            FileInfo resultsFile = new FileInfo(fileName);
            return resultsFile.Exists ? FileMode.Truncate : FileMode.CreateNew;
        }
        public static string GenerateRealsFile()
        {
            string fileName = "reals.txt";
            int count = random.Next(100, 200);
            double[] content = new double[count];
            for (int i = 0; i < count; i++)
            {
                content[i] = Math.Round(random.NextDouble() * random.Next(10, 1000), 2);
            }
            GenerateFile(fileName, content);

            return fileName;
        }

        public static string GenerateIntsFile()
        {
            string fileName = "ints.txt";
            int count = random.Next(100, 200);
            int[] content = new int[count];
            for (int i = 0; i < count; i++)
            {
                content[i] = random.Next();
            }
            GenerateFile(fileName, content);

            return fileName;
        }

        public static string GenerateDaysFile(string format = "dd\\/MM\\/yyyy")
        {
            string fileName = "days.txt";
            DateTime start = new DateTime(1990, 1, 1);

            List<string> content = new List<string>();
            foreach(DateTime day in RandomDays(start, random.Next(100, 200)))
            {
                content.Add(day.ToString(format));
            }

            GenerateFile(fileName, content.ToArray());

            return fileName;
        }

        private static void GenerateFile<T>(string fileName, T[] content)
        {
            using (FileStream reals = new FileStream(fileName, GetFileModeFor(fileName), FileAccess.Write))
            {
                StreamWriter writer = new StreamWriter(reals);

                foreach (T element in content)
                {
                    writer.WriteLine(element);
                }
                writer.Flush();
                reals.Close();
            }
        }
        private static DateTime[] RandomDays(DateTime start, int count)
        {
            int range = (DateTime.Today - start).Days;
            DateTime[] days = new DateTime[count];
            for (int i = 0; i < count; i++)
            {
                days[i] = start.AddDays(random.Next(range));
            }
            return days;
        }
    }

    internal class Job1 : Job
    {
        public new static string GetDescription()
        {
            return "Дан файл вещественных чисел. Создать два новых файла, первый из которых содержит элементы исходного файла с нечетными номерами (1,3,...), а второй — с четными (2,4,...).";
        }
        public override void Execute()
        {
            using (StreamReader reals = new StreamReader(new FileStream(FilesHelper.GenerateRealsFile(), FileMode.Open, FileAccess.Read)))
            using (StreamWriter evenNumbers = new StreamWriter(new FileStream("evens.txt", FilesHelper.GetFileModeFor("evens.txt"), FileAccess.Write)))
            using (StreamWriter oddNumbers = new StreamWriter(new FileStream("odds.txt", FilesHelper.GetFileModeFor("odds.txt"), FileAccess.Write)))
            {
                int index = 0;
                string line;
                StreamWriter currentWriter = null;
                while ((line = reals.ReadLine()) != null)
                {
                    currentWriter = (index % 2 == 0) ? evenNumbers : oddNumbers;
                    currentWriter.WriteLine(line);
                    index++;
                }
            }
        }
    }

    internal class Job2 : Job
    {
        public new static string GetDescription()
        {
            return "Дан файл вещественных чисел.Создать файл целых чисел, содержащий длины всех монотонных последовательностей элементов исходного файла.Например, для исходного файла с элементами 1.7,4.5,3.4,2.2,8.5,1.2 содержимое результирующего файла должно быть следующим: 2,3,2,2.";
        }
        public override void Execute()
        {
            string sequencesLengthFilename = "sequencesLength.txt";

            using (StreamReader reals = new StreamReader(new FileStream(FilesHelper.GenerateRealsFile(), FileMode.Open, FileAccess.Read)))
            using (StreamWriter sequencesLength = new StreamWriter(new FileStream(sequencesLengthFilename, FilesHelper.GetFileModeFor(sequencesLengthFilename), FileAccess.Write)))
            {
                
                string line;
                double 
                    current,
                    previous = double.NaN;
                bool
                    wasGrowing = true,
                    isGrowing;
                int length = 0;

                while ((line = reals.ReadLine()) != null)
                {
                    Double.TryParse(line, out current);
                    if (!Double.IsNaN(previous))
                    {
                        isGrowing = previous <= current;
                        if (isGrowing != wasGrowing)
                        {
                            sequencesLength.WriteLine(length);
                            length = 1;
                        }
                        wasGrowing = isGrowing;
                    }
                    length++;
                    previous = current;
                }
                sequencesLength.WriteLine(length);
            }
        }
    }

    internal class Job3 : Job
    {
        public new static string GetDescription()
        {
            return "Дан файл целых чисел. Удвоить его размер, записав в конец файла все его исходные элементы (в обратном порядке).";
        }
        public override void Execute()
        {

            using (FileStream ints = new FileStream(FilesHelper.GenerateIntsFile(), FileMode.Open, FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(ints);

                Stack<int> integers = new Stack<int>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    integers.Push(Int32.Parse(line));
                }

                StreamWriter writer = new StreamWriter(ints);
                while (integers.Count > 0)
                {
                    writer.WriteLine(integers.Pop());
                }
                writer.Flush();
            }
        }
    }

    internal class Job4 : Job
    {
        public new static string GetDescription()
        {
            return "Дан строковый файл, содержащий даты в формате «день/месяц/год», причем под день и месяц отводится по две позиции, а под год — четыре (например, «16/04/2001»). Создать новый строковый файл, в котором даты из исходного файла располагались бы в порядке убывания";
        }
        public override void Execute()
        {
            string daysSortedFilename = "days_sorted.txt";
            using (StreamReader days = new StreamReader(new FileStream(FilesHelper.GenerateDaysFile("dd\\/MM\\/yyyy"), FileMode.Open, FileAccess.Read)))
            using (StreamWriter daysSorted = new StreamWriter(new FileStream(daysSortedFilename, FilesHelper.GetFileModeFor(daysSortedFilename), FileAccess.Write)))
            {
                string line;
                List<DateTime> daysListed = new List<DateTime>();
                while((line = days.ReadLine()) != null)
                {
                    daysListed.Add(DateTime.Parse(line));
                }
                daysListed.Sort((x, y) => {
                    return DateTime.Compare(y, x);
                });
                foreach(DateTime day in daysListed)
                {
                    daysSorted.WriteLine(day.ToString("dd\\/MM\\/yyyy"));
                }
                daysSorted.Flush();
            }
        }
    }
}
