namespace PractiveWork3.Commands
{
    internal abstract class Command
    {
        protected int id;
        protected string description;
        protected bool isContinueExecute = true;

        internal double GetID()
        {
            return id;
        }

        internal virtual string GetPreviewText()
        {
            return $"Задача №{id}";
        }
        internal string GetDescription()
        {
            return description;
        }

        internal abstract void Execute();

        internal bool IsContinueExecute()
        {
            return isContinueExecute;
        }
    }
}