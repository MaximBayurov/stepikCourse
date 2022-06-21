using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PracticalWork7
{
    internal class TaskManager
    {
        private static TaskManager _taskManager = null;
        private List<MyTask> _taskList;
        internal static TaskManager GetInstance()
        {
            if (_taskManager == null)
            {
                _taskManager = new TaskManager();
            }
            return _taskManager;
        }
        private TaskManager()
        {
            _taskList = new List<MyTask>();

            Type[] typelist = Assembly
               .GetExecutingAssembly()
               .GetTypes()
               .Where((type) =>
               {
                   return type.IsSubclassOf(typeof(MyTask));
               })
               .ToArray();

            MyTask task;
            foreach (Type type in typelist)
            {
                task = (MyTask)Activator.CreateInstance(Type.GetType(type.FullName));
                _taskList.Add(task);
            }
            _taskList = _taskList
                .OrderBy((MyTask element) =>
                {
                    return element.GetOrder();
                })
                .ToList();

            ValidateTaskList();
        }

        internal MyTask GetByStringID(string stringID)
        {
            return _taskList.Where((task) =>
            {
                return task.GetStringID() == stringID;
            }).Single();
        }

        internal MyTask GetFirst()
        {
            if (_taskList.Count < 1)
            {
                throw new Exception("Нет ни одной задачи!");
            }
            return _taskList[0];
        }

        internal string[] GetStringIDsArray()
        {
            string[] result = new string[_taskList.Count];
            for (int index = 0; index < _taskList.Count; index++)
            {
                result[index] = _taskList[index].GetStringID();
            }
            return result;
        }

        void ValidateTaskList()
        {
            string[] result = new string[_taskList.Count];
            for (int index = 0; index < _taskList.Count; index++)
            {
                if (result.Contains(_taskList[index].GetStringID()))
                {
                    throw new Exception(
                        String.Format(
                            "Не уникальный строковый идентификатор задачи\n",
                            _taskList[index].ToString()
                            )
                        );
                }
                result[index] = _taskList[index].GetStringID();
            }
        }
    }
}