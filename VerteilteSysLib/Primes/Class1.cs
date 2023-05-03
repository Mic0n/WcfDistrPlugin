using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using VerteilteSysLib;

namespace Primes
{
    [Export(typeof(IPlugin))]
    public class Plugin1 : IPlugin
    {
        public string DisplayPlugins()
        {
            return "Primfaktorzerlegung";
        }

        public string DoOperation(string command)
        {
            int lower = Int32.Parse(command.Split(' ')[0]);
            int upper = Int32.Parse(command.Split(' ')[1]);
            if(lower < 2)
            {
                lower = 2;
            }
            string output = "";

            for (int i = lower; i < upper; i++)
            {
                long[] primes = primeFac(i);
                long temp = primes[0];
                long potenz = 1;
                for (int j = 1; j < primes.Length; j++)
                {
                    if (temp != primes[j])
                    {
                        output += " " + temp + "^" + potenz + " *";
                        temp = primes[j];
                        potenz = 1;
                    }
                    else
                    {
                        potenz++;
                    }
                }
                output += " " + temp + "^" + potenz;
            }
            return output;
        }

        private long[] primeFac (long zahl)
        {
            int maxFactors = (int)Math.Ceiling(Math.Log10(zahl) / Math.Log10(2));
            long[] temp = new long[maxFactors];
            int FacCount = 0;
            for (long j = 2; j <= zahl; j++)
            {
                if (zahl % j == 0)
                {
                    temp[FacCount++] = j;
                    zahl = zahl / j;
                    j--;
                }
            }
            long[] prf = new long[FacCount];
            for (int i = 0; i < FacCount; i++)
            {
                prf[i] = temp[i];
            }
            return prf;
        }

        public int[] nodeAmount (string command)
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
