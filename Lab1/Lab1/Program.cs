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
            Arma arma = new Arma();
            arma.setDimensions(3, 1);
            arma.setCoefficients(new double[]{ 0.005, 0.1, 0.2, 0.3}, new double[]{ 1.0, 0.01 });
            //arma.generateData(100);
            arma.setSize(100);
            arma.loadData("v.txt", false);
            arma.generateOutput();
            arma.calculateMNK();
            arma.printResult("result.txt");
        }
    }
}
