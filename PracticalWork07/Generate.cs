using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticalWork7
{
    internal class Generate
    {
        static public int[][] Array(int n, int m)
        {
            Random random = new Random();
            int[][] result = new int[n][];

            for (int i = 0; i < n; i++)
            {
                int[] row = new int[m];
                for (int j = 0; j < m; j++)
                {
                    row[j] = random.Next();
                }
                result[i] = row;
            }

            return result;
        }

        static public string[] NumberingFor<T>(T[] array)
        {
            string[] result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = (i + 1).ToString();
            }
            return result;
        }
    }
}
