using System.CommandLine;
using System.CommandLine.Parsing;

class Program
{
    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand("Практическая работа №4");

        var workDayCommand = new Command("week-day", "Выводит название дня недели, соответствующее данному числу");
        workDayCommand.AddAlias("wd");

        Option<int> dayNumberOption = new Option<int>(
            name: "--day-number",
            description: "Порядковый номер дня недели",
            isDefault: true,
            parseArgument: (result) =>
            {
                if (!result.Tokens.Any())
                {
                    return 1;
                }
                if (int.TryParse(result.Tokens.Single().Value, out var dayNumber) != true)
                {
                    result.ErrorMessage = "Опция --day-number требует один аргумент";
                    return -1;
                }
                return dayNumber;
            }
        );
        dayNumberOption
            .FromAmong(WeekDays.Keys.ToArray())
            .AddAlias("-d");

        workDayCommand.AddOption(dayNumberOption);
        workDayCommand.SetHandler((dayNumber) =>
        {
            Console.WriteLine(WeekDays[dayNumber.ToString()]);
        }, dayNumberOption);

        rootCommand.Add(workDayCommand);

        var locatorCommand = new Command("locator", "Выводит ориентацию локатора после выполнения заданных команд");
        locatorCommand.AddAlias("l");

        var locatorCommandsOption = new Option<string[]>(
            name: "--commands",
            description: "Комманды поворота",
            isDefault: false,
            parseArgument: (result) =>
            {
                if (result.Tokens.Count() < 1)
                {
                    result.ErrorMessage = "Введите хотя бы одну команду";
                    return new string[] { };
                }

                string[] arguments = new string[result.Tokens.Count];
                int index = 0;
                foreach (Token token in result.Tokens.ToArray())
                {
                    arguments[index] = token.Value.ToString();
                    index++;
                }

                return arguments;
            }
        );
        locatorCommandsOption.AllowMultipleArgumentsPerToken = true;
        locatorCommandsOption.AddAlias("-c");

        var locatorStartDirectionOption = new Option<string>(
            aliases: new string[2]
            {
                "--start-direction",
                "-sd",
            },
            description: "Начальная ориентация",
            getDefaultValue: () => Orientations.Keys.First()
        ).FromAmong(Orientations.Keys.ToArray());

        locatorCommand.AddOption(locatorCommandsOption);
        locatorCommand.AddOption(locatorStartDirectionOption);

        locatorCommand.SetHandler(
            (startDirection, commands) => {
                int result = Orientations[startDirection];
                foreach(string command in commands)
                {
                    result += Commands[command];
                }
                Console.WriteLine(Orientations.FirstOrDefault(x => x.Value == result % 360).Key);
            },
            locatorStartDirectionOption,
            locatorCommandsOption
        );

        rootCommand.Add(locatorCommand);

        await rootCommand.InvokeAsync(args);
    }

    static Dictionary<string, int> Orientations = new Dictionary<string, int>
    {
        { "Ю", 180 },
        { "З", 270 },
        { "С", 0 },
        { "В", 90 }
    };

    static Dictionary<string, int> Commands = new Dictionary<string, int>
    {
        { "-1", -90 },
        { "1", 90 },
        { "2", 180 }
    };

    static Dictionary<string, string> WeekDays = new Dictionary<string, string>(7)
    {
        { "1", "Понедельник" },
        { "2", "Вторник" },
        { "3", "Среда" },
        { "4", "Четверг" },
        { "5", "Пятница" },
        { "6", "Суббота" },
        { "7", "Воскресенье" }
    };
}