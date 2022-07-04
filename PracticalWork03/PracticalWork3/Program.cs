using PractiveWork3.Commands;
using System.Threading;

namespace PractiveWork3
{
    internal class Program
    {
        public static readonly IOController IOController = new IOController();

        static void Main(string[] args)
        {
            int commandID;
            Command command;
            CommandsManager commandsManager = CommandsManager.GetInstance();

            do
            {
                do
                {
                    IOController.PrintCommandsList();
                    commandID = IOController.ReadCommandID();
                } while (IOController.HasErrors());

                command = commandsManager.GetCommandByID(commandID);
                IOController.ExecuteCommand(command);

            } while (command.IsContinueExecute());
        }
    }
}
