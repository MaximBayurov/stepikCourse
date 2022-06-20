using PractiveWork3.Extensions;
using System;
using System.Threading;

namespace PractiveWork3.Commands
{
    internal class FifthCommand : Command
    {
        private static Thread[] threads = new Thread[2];

        public FifthCommand()
        {
            id = 5;
            description = "Даны координаты точки на плоскости. Проверить, что точка входит в заштрихованную область.";
        }

        internal override void Execute()
        {
            ShowChart();

            double x = IOController.ReadDoubleFromConsole("Введите число x: ");
            double y = IOController.ReadDoubleFromConsole("Введите число y: ");

            bool isInclude;
            if (x.IsNegative())
            {
                isInclude = (y <= Math.Pow(x, 2) & y >= (2 - x));
            }
            else
            {
                isInclude = (y.IsPositive() & (Math.Pow(x, 2) >= y) & ((2 - x) >= y));
            }

            Console.WriteLine(
                "Данная точка {0} входит в штрихованную область",
                isInclude ? "" : "не"
                );

            return;
        }

        //TODO отрефакторить управление потоками для вывода графиков
        private void ShowChart()
        {
            if (threads[0] == null & threads[1] == null)
            {
                threads[0] = CreateChartThread();
                threads[0].Start();
                threads[1] = CreateChartImageThread();
                threads[1].Start();
                return;
            }

            if (!threads[0].IsAlive)
            {
                threads[0] = CreateChartThread();
                threads[0].Start();
            }

            if (!threads[1].IsAlive)
            {
                threads[1] = CreateChartImageThread();
                threads[1].Start();
            }

            return;

        }

        private static Thread CreateChartThread()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                ChartForm chartForm = new ChartForm();
                System.Windows.Forms.Application.Run(chartForm);
            }));
            thread.IsBackground = true;

            return thread;
        }

        private static Thread CreateChartImageThread()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                ChartImageForm chartImageForm = new ChartImageForm();
                System.Windows.Forms.Application.Run(chartImageForm);
            }));
            thread.IsBackground = true;

            return thread;
        }
    }
}