using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PracticalWork10
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
            return "Дано целое число K и текстовый файл. Вставить пустую строку после строки файла с номером K. Если строки с таким номером нет, то оставить файл без изменений.";
        }
        public override void Execute()
        {
            int K;
            string inputMessage = "Введите целое число K (0 < K):";

            Console.WriteLine(inputMessage);
            while (int.TryParse(Console.ReadLine(), out K) != true || K < 0)
            {
                Console.WriteLine(inputMessage);
            }

            using (FileStream file = new FileStream("TextFile.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(file);
                StreamWriter writer = new StreamWriter(file);

                string line;
                long writeOffset = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    K--;
                    writer.WriteLine(line);
                    if (K == 0)
                    {
                        writer.BaseStream.Seek(writeOffset, SeekOrigin.Begin);
                        writer.WriteLine();
                        writer.Flush();
                        break;
                    }
                }
                file.Close();
            }
        }
    }

    internal class Job2 : Job
    {
        public new static string GetDescription()
        {
            return "Дан символ C - строчная (маленькая) русская буква и текстовый файл. Создать строковый файл и записать в него все слова из исходного файла, содержащие хотя бы одну букву C (прописную или строчную). Словом, считать набор символов, не содержащий пробелов, знаков препинания и ограниченный пробелами, знаками препинания или началом/концом строки. Если исходный файл не содержит подходящих слов, то оставить результирующий файл пустым.";
        }
        public override void Execute()
        {
            char C;
            string inputMessage = "Введите русскую букву:";

            Regex isRussianLetter = new Regex("[а-яА-Я]{1}");
            Console.WriteLine(inputMessage);
            while (char.TryParse(Console.ReadLine(), out C) != true && isRussianLetter.IsMatch(C.ToString()) == false)
            {
                Console.WriteLine(inputMessage);
            }

            string resultsFileName = "results2.txt";
            FileInfo resultsFile = new FileInfo(resultsFileName);
            FileMode fileMode = resultsFile.Exists ? FileMode.Truncate : FileMode.CreateNew;
            using (FileStream resultFile = new FileStream(resultsFileName, fileMode, FileAccess.Write))
            using (FileStream file = new FileStream("TextFile.txt", FileMode.Open, FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(file);
                StreamWriter writer = new StreamWriter(resultFile);

                string line;
                int wordsCount = 0;
                Regex isContainChar = new Regex(
                    String.Format("[\\s]?([а-яА-Я]*[{0}{1}][а-яА-Я]*)[\\s]?", C.ToString().ToUpper(), C.ToString().ToLower())
                );
                writer.WriteLine("Слова, содержащие букву - {0}", C);
                while ((line = reader.ReadLine()) != null)
                {
                    foreach (Match word in isContainChar.Matches(line))
                    {
                        writer.WriteLine(word.Groups[1].Value);
                        wordsCount++;
                    }
                }
                file.Close();

                string statisctic = String.Format("Всего слов с буквой '{0}' - {1}", C, wordsCount);
                Console.WriteLine(statisctic);
                writer.WriteLine(statisctic);
            }
        }
    }

    internal class Job3 : Job
    {
        public Job3()
        {
            GenerateTextFile3();
        }

        public new static string GetDescription()
        {
            return " Дан текстовый файл. В каждой его строке первые 30 позиций отводятся под текст, а оставшаяся часть - под вещественное число. Создать два файла: строковый файл, содержащий текстовую часть исходного файла, и файл вещественных чисел, содержащий числа из исходного файла (в том же порядке).";
        }

        private void GenerateTextFile3()
        {
            using (FileStream file = new FileStream("TextFile3.txt", GetFileModeFor("TextFile3.txt"), FileAccess.ReadWrite))
            {
                StreamWriter writer = new StreamWriter(file);
                Random random = new Random();
                int length = 30;
                int rowsCount = random.Next(25, 100);

                const string chars = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮЁ";

                for(int i = 0; i<rowsCount; i++)
                {
                    writer.WriteLine(String.Format(
                        "{0,30}{1}",
                        new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray()),
                        random.NextDouble() * random.Next()
                    ));
                    file.Flush();
                }

                file.Close();
            }
        }

        public override void Execute()
        {
            string 
                stringsFileName = "strings.txt",
                doublesFileName = "doubles.txt";
            using (FileStream stringsFile = new FileStream(stringsFileName, GetFileModeFor(stringsFileName), FileAccess.Write))
            using (FileStream doublesFile = new FileStream(doublesFileName, GetFileModeFor(doublesFileName), FileAccess.Write))
            using (FileStream file = new FileStream("TextFile3.txt", FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(file);
                StreamWriter 
                    strings = new StreamWriter(stringsFile),
                    doubles = new StreamWriter(doublesFile);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    strings.WriteLine(line.Substring(0, 30));
                    doubles.WriteLine(line.Substring(30));
                    strings.Flush();
                    doubles.Flush();
                }
                strings.Close();
                doubles.Close();
                file.Close();
            }
        }

        public static FileMode GetFileModeFor(string fileName)
        {
            FileInfo resultsFile = new FileInfo(fileName);
            return resultsFile.Exists ? FileMode.Truncate : FileMode.CreateNew;
        }
    }

    internal class Job4 : Job
    {
        public new static string GetDescription()
        {
            return "Дан текстовый файл. Создать символьный файл, содержащий все символы, встретившиеся в тексте, включая пробел и знаки препинания (без повторений). Символы располагать в порядке их первого появления в тексте.";
        }
        public override void Execute()
        {
            using (FileStream file = new FileStream("TextFile.txt", FileMode.Open, FileAccess.ReadWrite))
            using (FileStream symbolsFile = new FileStream("symbols.txt", Job3.GetFileModeFor("symbols.txt"), FileAccess.ReadWrite))
            {
                StreamReader reader = new StreamReader(file);
                StreamWriter symbols = new StreamWriter(symbolsFile);
                Dictionary<char, bool> symbolsMap = new Dictionary<char, bool>();

                char symbol;
                int symbolNumber;
                while ((symbolNumber = reader.Read()) != -1)
                {
                    symbol = ((char)symbolNumber).ToString().ToLower().ToCharArray()[0];
                    if (char.IsControl(symbol) == true || symbolsMap.ContainsKey(symbol))
                    {
                        continue;
                    } else
                    {
                        symbolsMap.Add(symbol, true);
                    }
                }
                file.Close();

                foreach(char key in symbolsMap.Keys)
                {
                    symbols.WriteLine(key);
                }

                symbols.Flush();
                symbolsFile.Close();
            }
        }
    }
}
