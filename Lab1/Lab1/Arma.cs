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
        private int n = 0;
        private int ard, mad;
        private double[] a, b;
        private double[] v, y;

        private Matrix<double> result;
        public Arma()
        {

        }
        public void setSize(int n) { this.n = n; }
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

        public void generateInput()
        {
            NormalDistribution d = new NormalDistribution();
            v = new double[n];
            
            for (int i = 0; i < n; i++)
                v[i] = d.Next();
        }

        public void generateOutput()
        {
            y = new double[n];

            for (int i = 0; i < n; i++)
            {
                double res = 0;
                for (int j = 0; j <= ard; j++)
                {
                    res += j == 0 ? a[j] : a[j] * outputValue(i - j);
                }

                for (int j = 0; j <= mad; j++)
                {
                    res += b[j] * inputValue(i - j);
                }

                y[i] = res;
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

        public void printResult(string filename)
        {
            string res = result.ToMatrixString();
            System.IO.File.WriteAllText(filename, res);
        }

        public void calculateMNK()
        {
            int maxDim = Math.Max(ard, mad);
            int size = n - 1 - maxDim;
            Matrix<double> X = DenseMatrix.Create(size, 1, 1.0);

            Matrix<double> yPart = DenseMatrix.Create(size, ard, 0);
            for (int i = 0; i < size; i++)
            {
                Vector<double> row = DenseVector.Create(ard, 0.0);
                for (int j = 0; j < ard; j++)
                {
                    row[j] = outputValue(i + maxDim - j - 1);
                }
                yPart.SetRow(i, row);
            }
            X = X.Append(yPart);

            Matrix<double> vPart = DenseMatrix.Create(size, mad + 1, 0);
            for (int i = 0; i < size; i++)
            {
                Vector<double> row = DenseVector.Create(mad + 1, 0.0);
                for (int j = 0; j < mad + 1; j++)
                {
                    row[j] = inputValue(i + maxDim - j);
                }
                vPart.SetRow(i, row);
            }
            X = X.Append(vPart);

            double[] yCopy = new double[size];
            Array.Copy(y, maxDim, yCopy, 0, size);
            Matrix<double> Y = DenseMatrix.OfColumnVectors(DenseVector.OfArray(yCopy));

            Matrix<double> c1 = X.Transpose();
            Matrix<double> c2 = c1 * X;
            Matrix<double> c3 = c2.Inverse();
            Matrix<double> c4 = c3 * c1;
            Matrix<double> c5 = c4 * Y;

            result = (X.Transpose() * X).Inverse()*X.Transpose()*Y;
        }
    }
}
