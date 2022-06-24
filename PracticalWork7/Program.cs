using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            TaskManager taskManager = TaskManager.GetInstance();
            var rootCommand = new RootCommand("Практическая работа №7");

            Option<MyTask> taskOption = new Option<MyTask>(
                name: "--task",
                description: "Строковый идентификатор задачи",
                isDefault: true,
                parseArgument: (result) =>
                {
                    if (!result.Tokens.Any())
                    {
                        return taskManager.GetFirst();
                    }

                    string task = result.Tokens.Single().Value;
                    return taskManager.GetByStringID(task);
                }
            );
            taskOption
                .FromAmong(taskManager.GetStringIDsArray())
                .AddAlias("-t");

            rootCommand.AddOption(taskOption);
            rootCommand.SetHandler((task) =>
            {
                Console.WriteLine(task.GetDescription());

                task.Execute();

                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
            }, taskOption);

            await rootCommand.InvokeAsync(args);
        }
    }
}
