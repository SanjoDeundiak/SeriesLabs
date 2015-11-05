using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lab1
{
    class MNK : Arma
    {
        public MNK(int ard, int mad) : base(ard, mad)
        { }
        public void calculate()
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
                    row[j] = outputValue(i + maxDim - j - 1, false);
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

            if (Math.Abs(c2.Determinant()) <= 1E-6)
                throw new Exception("Determinant == 0");

            Matrix<double> c3 = c2.Inverse();
            Matrix<double> c4 = c3 * c1;
            Matrix<double> c5 = c4 * Y;

            result = (X.Transpose() * X).Inverse() * X.Transpose() * Y;

            aEstimation = new double[ard + 1];
            for (int i = 0; i < ard + 1; i++)
                aEstimation[i] = result[i, 0];

            bEstimation = new double[mad + 1];
            for (int i = 0; i < mad + 1; i++)
                bEstimation[i] = result[i + ard + 1, 0];
        }
    }
}
