using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    public class MethodGauss_Row
    {
        private double[,] MatrixA;
        private double[] MatrixB;
        private int Dimension;
        private double[] X;
        int[] order;

        public MethodGauss_Row(double[,] MatrixA, double[] MatrixB, int Dimension)
        {
            this.MatrixA = (double[,])MatrixA.Clone();
            this.MatrixB = (double[])MatrixB.Clone();
            this.Dimension = Dimension;
        }

        public double[] Run()
        {
            //Результат
            X = new double[Dimension];

            //Прямой ход
            StraightRun(Dimension, MatrixA, MatrixB);

            //Обратный ход
            ReverseRun(Dimension, MatrixA, MatrixB, X);

            return X;
        }

        void ShowVector(int Dimension, double[] vec)
        {
            for (int i = 0; i < Dimension; i++)
                Console.WriteLine(vec[i]);
            Console.WriteLine();
        }

        void StraightRun(int Dimension, double[,] MatrixA, double[] MatrixB)
        {
            order = new int[Dimension];
            for (int i = 0; i < order.Length; i++)
            {
                order[i] = i;
            }

            double v;
            for (int k = 0, im, i, j; k < Dimension - 1; k++)
            {
                im = k;
                for (i = k + 1; i < Dimension; i++)
                {
                    if (Math.Abs(MatrixA[k, im]) < Math.Abs(MatrixA[k, i]))
                    {
                        im = i;
                    }
                }
                //if (im != k)
                {
                    for (j = 0; j < Dimension; j++)
                    {
                        (MatrixA[j,im], MatrixA[j,k]) = (MatrixA[j, k], MatrixA[j, im]);
                        (order[im], order[k]) = (order[k], order[im]);
                    }
                    //(MatrixB[im], MatrixB[k]) = (MatrixB[k], MatrixB[im]);
                }


                for (i = k + 1; i < Dimension; i++)
                {
                    v = MatrixA[i, k] / MatrixA[k, k];
                    MatrixA[i, k] = 0;
                    MatrixB[i] = MatrixB[i] - v * MatrixB[k];
                    if (v != 0)
                    {
                        for (j = k + 1; j < Dimension; j++)
                        {
                            MatrixA[i, j] = MatrixA[i, j] - v * MatrixA[k, j];
                        }
                    }
                }
            }
        }

        void ReverseRun(int Dimension, double[,] MatrixA, double[] MatrixB, double[] x)
        {
            double s = 0;
            x[Dimension - 1] = MatrixB[Dimension - 1] / MatrixA[Dimension - 1, Dimension - 1];
            for (int i = Dimension - 2, j; 0 <= i; i--)
            {
                s = 0;
                for (j = i + 1; j < Dimension; j++)
                {
                    s = s + MatrixA[i, j] * x[j];
                }
                x[i] = (MatrixB[i] - s) / MatrixA[i, i];
            }
            OrderSort(x);
        }

        void OrderSort(double[] x)
        {
            var CopyX = (double[])x.Clone();
            for (int i = 0; i < CopyX.Length; i++)
            {
                x[i] = CopyX[order[i]];
            }
        }
    }
}
