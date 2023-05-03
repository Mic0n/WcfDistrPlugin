using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerteilteSysLib;

namespace Pierpont
{
    [Export(typeof(IPlugin))]
    public class Pierpont : IPlugin
    {
        public string DisplayPlugins()
        {
            return "PierpontPrimes";
        }

        public string DoOperation(string input)
        {
            string output = "";
            int lower = Int32.Parse(input.Split(' ')[0]);
            int upper = Int32.Parse(input.Split(' ')[1]);

            List<int> primes = new List<int>();
            int c, d, flag;
            for (c = lower; c <= upper; c++)
            {
                if (c == 1 || c == 0)
                    continue;
                flag = 1;
                for (d = 2; d <= c / 2; ++d)
                {
                    if (c % d == 0)
                    {
                        flag = 0;
                        break;
                    }
                }
                if (flag == 1)
                    primes.Add(c);
            }
            List<int> array = new List<int>();

            for (int i = 0; i < (Math.Log(upper) / Math.Log(2)); i++)
            {
                for (int j = 0; j < (Math.Log(upper) / Math.Log(3)); j++)
                {
                    int test = (int)(1 + Math.Pow(2, i) * Math.Pow(3, j));
                    if (!array.Contains(test) && test < upper && primes.Contains(test))
                    {
                        array.Add(test);
                    }
                }
            }
            array.Sort();
            for (int i = 0; i < array.Count; i++)
            {
                output += array.ElementAt(i) + " ";
            }
            return output;
        }

        public int[] nodeAmount(string command)
        {
            int number = Int32.Parse(command);
            int nodeAmount = 2;
            int[] output = new int[2];
            output[0] = nodeAmount;
            double dbl = number / nodeAmount;
            output[1] = (int)dbl;

            return output;
        }
    }
}
