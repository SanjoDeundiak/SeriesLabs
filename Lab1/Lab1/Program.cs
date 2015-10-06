using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Arma arma = new Arma();
            arma.setDimensions(3, 3);
            arma.setCoefficients(new double[]{ 1.0, 2.0, 3.0, 4.0}, new double[]{ 1.0, 2.0, 3.0, 4.0 });
            arma.generateData(100);
        }
    }
}
