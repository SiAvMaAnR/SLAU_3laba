using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    public class MethodJordan
    {
        private double[,] MatrixA;
        private double[] MatrixB;
        private int Dimension;

        public MethodJordan(double[,] MatrixA, double[] MatrixB, int Dimension)
        {
            this.MatrixA = (double[,])MatrixA.Clone();
            this.MatrixB = (double[])MatrixB.Clone();
            this.Dimension = Dimension;
        }

        public double[] Run()
        {
            int row = MatrixA.GetLength(0);
            int column = MatrixA.GetLength(1) + 1;

            double[,] returnMatrix = new double[row, column];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column - 1; j++)
                {
                    returnMatrix[i, j] = MatrixA[i, j];
                }
                returnMatrix[i, column - 1] = MatrixB[i];
            }

            for (int i = 0; i < row; i++)
            {
                double use = returnMatrix[i, i];
                if (use == 0)
                {
                    int j;
                    for (j = i + 1; j < row; j++)
                    {
                        if (returnMatrix[j, i] != 0)
                        {
                            double[] oldmatrix = new double[column];
                            double[] newmatrix = new double[column];

                            for (int k = 0; k < column; k++) oldmatrix[k] = returnMatrix[i, k];
                            for (int k = 0; k < column; k++)
                            {
                                returnMatrix[i, k] = returnMatrix[j, k];
                                returnMatrix[j, k] = oldmatrix[k];
                            }
                            use = returnMatrix[i, i];
                            //for (int k = 0; k < column; k++) Console.WriteLine($"x {i + 1}-{k + 1} = {Matrix[i, k]}");
                            //for (int k = 0; k < column; k++) Console.WriteLine($"x {j + 1}-{k + 1} = {Matrix[j, k]}");
                            break;
                        }
                    }

                    if (j == row)
                    {
                        Console.WriteLine("Решение получить невозможно из - за нулевого столбца матрицы А");
                        break;
                    }
                }
                if (use != 1)
                {
                    for (int j = 0; j < column; j++)
                    {
                        returnMatrix[i, j] /= use;
                        if (returnMatrix[i, j] == -0) returnMatrix[i, j] = 0;
                        //Console.WriteLine($"x{i + 1}-{j + 1}/{use} = {Matrix[i, j]}");
                    }
                }
                for (int j = i + 1; j < row; j++)
                {
                    double cons = returnMatrix[j, i];
                    if (cons != 0)
                    {
                        for (int k = i; k < column; k++)
                        {
                            returnMatrix[j, k] = -cons * returnMatrix[i, k] + returnMatrix[j, k];
                        }
                    }
                }
            }
            for (int i = row - 1; i >= 0; i--)
            {
                if (returnMatrix[i, i] == 0) break;
                for (int j = i - 1; j >= 0; j--)
                {
                    double cons = returnMatrix[j, i];
                    if (cons != 0)
                    {
                        for (int k = i; k < column; k++)
                        {
                            returnMatrix[j, k] = -cons * returnMatrix[i, k] + returnMatrix[j, k];
                        }
                    }
                }
            }

            double[] Return = new double[row];
            for (int i = 0; i < row; i++)
            {
                Return[i] = returnMatrix[i, row];
            }
            return Return;
        }
    }
}
