namespace PracticalWork7
{
    internal abstract class MyTask
    {
        protected string StringID;
        protected string Description;
        protected int Order;

        public virtual string GetName()
        {
            return string.Format("Задача №{0}", StringID);
        }

        public virtual int GetOrder()
        {
            return Order;
        }
        public virtual string GetDescription()
        {
            return Description;
        }
        public virtual string GetStringID()
        {
            return StringID;
        }

        public abstract void Execute();
    }
}