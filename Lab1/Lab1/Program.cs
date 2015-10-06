using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Globalization;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            double[] realCoefA = new double[] { 0.005, 0.1, 0.2, 0.3 };
            double[] realCoefB = new double[] { 1.0, 0.01 };

            System.IO.StreamWriter file = new System.IO.StreamWriter("analysis.txt");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("result.txt");

            for (int size = 10; size <= 100; size += 5)
            {
                for (int i = 3; i <= realCoefA.Length - 1; i++)
                {
                    for (int j = 1; j <= realCoefB.Length - 1; j++)
                    {
                        Arma arma = new Arma();
                        arma.setDimensions(i, j);

                        double[] coefA = new double[i + 1];
                        Array.Copy(realCoefA, 0, coefA, 0, i + 1);

                        double[] coefB = new double[j + 1];
                        Array.Copy(realCoefB, 0, coefB, 0, j + 1);

                        arma.setCoefficients(coefA, coefB);
                        arma.setSize(size);
                        arma.loadData("v.txt", false);
                        arma.loadData("y.txt", true);
                        //arma.generateOutput(false);
                        arma.calculateMNK();
                        file2.WriteLine(size.ToString() + " " + arma.resultString());
                        arma.generateOutput(true);
                        string res = arma.ToString();
                        file.WriteLine(res);
                    }
                }
            }
            file.Close();
            file2.Close();
        }
    }
}
