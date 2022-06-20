using PractiveWork3.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PractiveWork3
{
    internal class CommandsManager
    {
        private static CommandsManager commandsManager;

        public List<Command> CommandsList = new List<Command>();
        private Dictionary<double, int> MapForCommandsByID;

        private CommandsManager()
        {
            Type[] typelist = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where((type) =>
                {
                    return type.IsSubclassOf(typeof(Command));
                })
                .ToArray();

            Command command;
            foreach (Type type in typelist)
            {
                command = (Command)Activator.CreateInstance(Type.GetType(type.FullName));
                CommandsList.Add(command);
            }
            CommandsList = CommandsList
                .OrderBy((Command element) =>
                {
                    return element.GetID();
                })
                .ToList<Command>();
        }

        public static CommandsManager GetInstance()
        {
            if (commandsManager == null)
            {
                commandsManager = new CommandsManager();
            }

            return commandsManager;
        }

        public Command GetCommandByID(int commandID)
        {
            int index = MapForCommandsByID[commandID];
            return CommandsList[index];
        }

        public bool HasCommandWithID(double commandID)
        {
            if (MapForCommandsByID == null || MapForCommandsByID.Count == 0)
            {
                MapForCommandsByID = new Dictionary<double, int>();

                int index = 0;
                foreach (Command task in CommandsList)
                {
                    MapForCommandsByID.Add(task.GetID(), index);
                    index++;
                }
            }

            return MapForCommandsByID.ContainsKey(commandID);
        }
    }
}