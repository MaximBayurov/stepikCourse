using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationRoots
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double a, b, c;
            Console.WriteLine("Введите три числа a, b и с, для вычисления корней");

            Console.WriteLine("Введите a:");
            do
            {

            }while(!Double.TryParse(Console.ReadLine(), out a));

            Console.WriteLine("Введите b:");
            do
            {

            }while(!Double.TryParse(Console.ReadLine(), out b));

            Console.WriteLine("Введите c:");
            do
            {

            }while(!Double.TryParse(Console.ReadLine(), out c));

            double D = Math.Pow(b, 2) - 4 * a * c;
            Console.WriteLine(
                "X1: {0};\nX2: {1};",
                (-b + Math.Sqrt(D)) / (2 * a),
                (-b - Math.Sqrt(D)) / (2 * a)
            );
            Console.WriteLine("Нажмите на любую кнопку...");
            Console.ReadKey();
        }
    }
}
