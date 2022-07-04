namespace PractiveWork3.Commands
{
    internal class ExitCommand : Command
    {
        public ExitCommand()
        {
            id = 9;
            description = "Завершение работы программы...";
        }

        internal override void Execute()
        {
            isContinueExecute = false;

            return;
        }

        internal override string GetPreviewText()
        {
            return "Выйти из программы";
        }
    }
}