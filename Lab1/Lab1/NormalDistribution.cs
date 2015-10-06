using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1
{
    class NormalDistribution
    {
        private Random rand = new Random();

        public double Next()
        {
            double u = rand.NextDouble(), v = rand.NextDouble();
            return Math.Sqrt(-2 * Math.Log(u)) * Math.Sin(2 * Math.PI * v);
        }
    }
}
