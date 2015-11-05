using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lab1
{
    class Arma
    {
        protected int n = 0;
        protected int ard, mad;
        protected double[] a, b;
        protected double[] v, y;

        protected Matrix<double> result;

        protected double[] aEstimation;
        protected double[] bEstimation;

        protected double[] estimations;

        public override string ToString()
        {
            string res = "";

            res += "n = " + n.ToString() + ", ard = " + ard.ToString() + ", mad = " + mad.ToString() + ", S = " + S().ToString() + ", R2 = " + R2().ToString() + ", IKA = " + IKA().ToString();

            return res;
        }
        protected Arma(int ard, int mad)
        {
            this.ard = ard; this.mad = mad;
        }
        public void setSize(int n) { this.n = n; }
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

            n = n == 0 ? lines.Length : n;

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
            generateOutput(false);
        }
        public double inputValue(int i)
        {
            if (i >= n)
                throw new Exception("out of range");

            return i >= 0 ? v[i] : 0;
        }
        public double outputValue(int i, bool estimation)
        {
            if (i >= n)
                throw new Exception("out of range");

            return i >= 0 ? (estimation ? estimations[i] : y[i]) : 0;
        }
        public void generateInput()
        {
            NormalDistribution d = new NormalDistribution();
            v = new double[n];
            
            for (int i = 0; i < n; i++)
                v[i] = d.Next();
        }
        public void generateOutput(bool estimation)
        {
            NormalDistribution d = new NormalDistribution();

            double[] array = new double[n];
            double[] a, b;

            if (estimation)
            {
                estimations = array;
                a = aEstimation;
                b = bEstimation;
            }
            else
            {
                y = array;
                a = this.a;
                b = this.b;
            }

            for (int i = 0; i < n; i++)
            {
                double res = 0;

                for (int j = 0; j <= ard; j++)
                {
                    res += j == 0 ? a[j] : a[j] * outputValue(i - j, estimation);
                }

                for (int j = 0; j <= mad; j++)
                {
                    res += b[j] * inputValue(i - j);
                }

                //if (!estimation)
                //    res += d.Next();

                array[i] = res;
            }
        }
        public void printData(string filename, bool output)
        {
            double[] array = output ? y : v;
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                for (int i = 0; i < n; i++)
                    file.WriteLine(array[i]);
            }
        }
        public string resultString()
        {
            return "n=" + n.ToString() + "\n" + result.ToMatrixString();
        }
        #region Errors
        public double S()
        {
            double res = 0;
            for (int i = 0; i < n; i++)
                res += Math.Pow(estimations[i] - y[i], 2);

            return res / n;
        }
        public double R2()
        {
            double yAv = 0;
            double yEstAv = 0;
            double dispy = 0;
            double dispyEst = 0;

            for (int i = 0; i < n; i++)
                yAv += y[i];
            yAv /= (double)n;

            for (int i = 0; i < n; i++)
                yEstAv += estimations[i];
            yEstAv /= (double)n;

            for (int i = 0; i < n; i++)
                dispy += Math.Pow(y[i] - yAv, 2);

            for (int i = 0; i < n; i++)
                dispyEst += Math.Pow(estimations[i] - yEstAv, 2);

            double res = dispyEst / dispy;

            return res < 1 ? res : 1 / res;
        }
        public double IKA()
        {
            double res = 0;
            for (int i = 0; i < n; i++)
                res += Math.Pow(estimations[i] - y[i], 2);

            return n*Math.Log(res) + ard + mad + 1;
        }
        #endregion
    }
}
