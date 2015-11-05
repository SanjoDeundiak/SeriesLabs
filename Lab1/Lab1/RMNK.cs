using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Lab1
{
    class RMNK : Arma
    {
        private Matrix<double> Theta, P;
        private int i = 1;
        public RMNK(int ard, int mad) : base(ard, mad)
        {
            int size = ard + mad + 2;
            Theta = DenseMatrix.Create(size, 1, 0.0);
            P = DenseMatrix.Create(size, size, 0.0);

            for (int i = 0; i < size; i++)
                P[i, i] = 1.0;

            double beta = 10000000;
            P = beta * P;
        }
        public void iterate()
        {
            int size = ard + mad + 2;

            Matrix<double> xi = DenseMatrix.Create(1, size, 0.0);
            xi[0, 0] = 1;
            for (int j = 1; j < ard + 1; j++)
                xi[0, j] = outputValue(i - j, false);

            int offset = ard + 1;
            for (int j = 0; j < mad + 1; j++)
                xi[0, offset + j] = inputValue(i - j);

            Matrix<double> xiT = xi.Transpose();

            Matrix<double> num = (P * xiT * xi * P);
            double denom = (1 + (xi * P * xiT)[0, 0]);

            P = P - num / denom;

            Theta = Theta + P * xiT * (outputValue(i, false) - (xi * Theta)[0, 0]);

            result = Theta;

            aEstimation = new double[ard + 1];
            for (int i = 0; i < ard + 1; i++)
                aEstimation[i] = result[i, 0];

            bEstimation = new double[mad + 1];
            for (int i = 0; i < mad + 1; i++)
                bEstimation[i] = result[i + ard + 1, 0];

            i++;
        }
    }
}
