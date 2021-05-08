using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    public class MethodGauss
    {
        private double[,] MatrixA;
        private double[] MatrixB;
        private int Dimension;

        public MethodGauss(double[,] MatrixA, double[] MatrixB, int Dimension)
        {
            this.MatrixA = (double[,])MatrixA.Clone();
            this.MatrixB = (double[])MatrixB.Clone();
            this.Dimension = Dimension;
        }

        public double[] Run()
        {
           var x =  new double[Dimension];
            //Точность 
            const double eps = 0.00001;

            double max;
            int index;
            for (int k = 0; k < Dimension; k++)
            {
                //Поиск строки с максимальным MatrixA[i,k]
                max = Math.Abs(MatrixA[k, k]);
                index = k;
                for (int i = k + 1; i < Dimension; i++)
                {
                    if (Math.Abs(MatrixA[i, k]) > max)
                    {
                        max = Math.Abs(MatrixA[i, k]);
                        index = i;
                    }
                }

                // Перестановка строк
                if (max < eps)
                {
                    // нет ненулевых диагональных элементов
                    throw new Exception($"Решение получить невозможно из-за нулевого столбца {index} матрицы А");
                }

                for (int j = 0; j < Dimension; j++)
                {
                    (MatrixA[k, j], MatrixA[index, j]) = (MatrixA[index, j], MatrixA[k, j]);
                }

                (MatrixB[k], MatrixB[index]) = (MatrixB[index], MatrixB[k]);

                // Нормализация уравнений
                for (int i = k; i < Dimension; i++)
                {
                    double temp = MatrixA[i, k];
                    if (Math.Abs(temp) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < Dimension; j++)
                        MatrixA[i, j] = MatrixA[i, j] / temp;
                    MatrixB[i] = MatrixB[i] / temp;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < Dimension; j++)
                        MatrixA[i, j] = MatrixA[i, j] - MatrixA[k, j];
                    MatrixB[i] = MatrixB[i] - MatrixB[k];
                }
            }
            // обратная подстановка
            for (int k = Dimension - 1; k >= 0; k--)
            {
                x[k] = MatrixB[k];
                for (int i = 0; i < k; i++)
                    MatrixB[i] = MatrixB[i] - MatrixA[i, k] * x[k];
            }

            return x;
        }
    }
}
