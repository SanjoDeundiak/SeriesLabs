using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Arma
    {
        private int n = 0;
        private int ard, mad;
        private double[] a, b;
        private double[] v, y;
        public Arma()
        {

        }

        public void setDimensions(int ard, int mad) { this.ard = ard;  this.mad = mad; }
        public void setCoefficients(double[] a, double[] b)
        {
            if (a.Length != ard + 1 || b.Length != mad + 1)
                throw new Exception("wrong coefficients");

            this.a = a;
            this.b = b;
        }

        public void loadData(string filename, bool output)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            if (n != 0 && lines.Length != n)
                throw new Exception("wrong data in " + filename);

            n = lines.Length;

            double[] array = new double[n];

            if (output)
                y = array;
            else
                v = array;

            for (int i = 0; i < n; i++)
                array[i] = Convert.ToDouble(lines[i]);
        }

        public void generateData(int n)
        {
            this.n = n;
            generateInput();
            generateOutput();
        }

        public double inputValue(int i)
        {
            if (i >= n)
                throw new Exception("out of range");

            return i >= 0 ? v[i] : 0;
        }
        public double outputValue(int i)
        {
            if (i >= n)
                throw new Exception("out of range");

            return i >= 0 ? y[i] : 0;
        }

        private void generateInput()
        {
            NormalDistribution d = new NormalDistribution();
            v = new double[n];
            
            for (int i = 0; i < n; i++)
                v[i] = d.Next();
        }

        private void generateOutput()
        {
            y = new double[n];

            for (int i = 0; i < n; i++)
            {
                double res = 0;
                for (int j = 0; j < ard; j++)
                {
                    res += j == 0 ? a[j] : a[j] * outputValue(i - j);
                }

                for (int j = 0; j < mad; j++)
                {
                    res += b[j] * inputValue(i - j);
                }

                y[i] = res;
            }
        }
    }
}
