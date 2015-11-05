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
        static double[] realCoefA = new double[] { 0.0, -0.045, -0.079, 0.525 };
        static double[] realCoefB = new double[] { 1.0, 0.3, 0.4, 0.3 };
        static void TestMNK()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            System.IO.StreamWriter file = new System.IO.StreamWriter("modelMNK.txt");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("resultMNK.txt");

            {
                for (int size = 15; size <= 100; size += 5)
                {
                    for (int i = 3; i <= realCoefA.Length - 1; i++)
                    {
                        for (int j = 3; j <= realCoefB.Length - 1; j++)
                        {
                            MNK mnk = new MNK(i, j);

                            double[] coefA = new double[i + 1];
                            Array.Copy(realCoefA, 0, coefA, 0, i + 1);

                            double[] coefB = new double[j + 1];
                            Array.Copy(realCoefB, 0, coefB, 0, j + 1);

                            mnk.setCoefficients(coefA, coefB);
                            mnk.setSize(size);
                            //arma.loadData("v.txt", false);
                            //arma.loadData("y.txt", true);
                            mnk.generateInput();
                            mnk.generateOutput(false);
                            mnk.calculate();
                            file2.WriteLine(size.ToString() + " " + mnk.resultString());
                            mnk.generateOutput(true);
                            string res = mnk.ToString();
                            file.WriteLine(res);
                        }
                    }
                }
                file.Close();
                file2.Close();
            }
        }
        static void TestRMNK()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            System.IO.StreamWriter file = new System.IO.StreamWriter("modelRMNK.txt");
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("resultRMNK.txt");

            {
                for (int size = 10; size <= 100; size += 5)
                {
                    for (int i = 3; i <= realCoefA.Length - 1; i++)
                    {
                        for (int j = 3; j <= realCoefB.Length - 1; j++)
                        {
                            RMNK rmnk = new RMNK(i, j);

                            double[] coefA = new double[i + 1];
                            Array.Copy(realCoefA, 0, coefA, 0, i + 1);

                            double[] coefB = new double[j + 1];
                            Array.Copy(realCoefB, 0, coefB, 0, j + 1);

                            rmnk.setCoefficients(coefA, coefB);
                            rmnk.setSize(size);
                            //arma.loadData("v.txt", false);
                            //arma.loadData("y.txt", true);
                            rmnk.generateInput();
                            rmnk.generateOutput(false);
                            for (int it = 1; it < size; it++)
                            {
                                rmnk.iterate();
                            }

                            file2.WriteLine(rmnk.resultString());
                            rmnk.generateOutput(true);
                            string res = rmnk.ToString();
                            file.WriteLine(res);
                        }
                    }
                }
            }
            file.Close();
            file2.Close();
        }
        static void Main(string[] args)
        {
            TestMNK();
            //TestRMNK();    
        }
    }
}
