﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    class Program
    {
        static double[,] M;
        
        
        static double[] Y;
        static int valueEquation;

        static void PrintMatrix(double[,] Matrix)
        {
            Console.WriteLine($"Вывод СЛАУ: ");
            for (int i = 0; i < Matrix.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < Matrix.GetUpperBound(1) + 1; j++)
                {
                    if (j != 0)
                    {
                        Console.Write($" + {String.Format("{0,3}", Matrix[i, j])}*x{j + 1}");
                    }
                    else
                    {
                        Console.Write($"   {String.Format("{0,3}", Matrix[i, j])}*x{j + 1}");
                    }
                }
                Console.WriteLine($" = { String.Format("{0,3}", Y[i])}");
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.Write("Кол-во уравнений: ");
                valueEquation = int.Parse(Console.ReadLine());

                M = new double[valueEquation, valueEquation];
                Y = new double[valueEquation];

                //Вводим матрицу А
                for (int i = 0; i < M.GetUpperBound(0) + 1; i++)
                {
                    for (int j = 0; j < M.GetUpperBound(1) + 1; j++)
                    {
                        Console.Write($"A[{i + 1},{j + 1}] : ");
                        M[i, j] = Convert.ToDouble(Console.ReadLine());
                    }
                }

                //Вводим правую часть Y
                for (int i = 0; i < Y.GetUpperBound(0) + 1; i++)
                {
                    Console.Write($"Y[{i + 1}] : ");
                    Y[i] = Convert.ToDouble(Console.ReadLine());
                }

                //Выводим матрицу
                PrintMatrix(M);

                #region Гаусса
                Console.WriteLine("\nМетод Гауса: ");

                double[] Result1 = MethodGauss();

                //Выводим ответы
                for (int i = 0; i < valueEquation; i++)
                    Console.WriteLine($"x[{i + 1}] = {Result1[i]}");
                #endregion

                #region Жордана
                Console.WriteLine("\nМетод Жордана: ");

                double[] Result2 = MethodJordana();

                // Выводим ответы
                for (int i = 0; i < valueEquation; i++)
                    Console.WriteLine($"x[{i + 1}] = {Result2[i]}");
                #endregion

                #region Обратная Матрица
                Console.WriteLine("\nМетод Жордана / Обратная матрица: ");

                double[,] C = (double[,])M.Clone();
                double[,] Result3 = InverseMatrix(C);

                for (int i = 0; i < Result3.GetLength(0); i++)
                {
                    for (int j = 0; j < Result3.GetLength(1); j++)
                    {
                        Console.Write($"{String.Format("{0,3}",Result3[i, j])} ");
                    }
                    Console.WriteLine();
                }
                #endregion
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        static double[] MethodGauss()
        {
            double[,] A = (double[,])M.Clone();
            double[] Y1 = (double[])Y.Clone();

            double[] x = new double[valueEquation];
            //Точность 
            const double eps = 0.00001;

            double max;
            int index;
            for (int k = 0; k < valueEquation; k++)
            {
                //Поиск строки с максимальным A[i,k]
                max = Math.Abs(A[k, k]);
                index = k;
                for (int i = k + 1; i < valueEquation; i++)
                {
                    if (Math.Abs(A[i, k]) > max)
                    {
                        max = Math.Abs(A[i, k]);
                        index = i;
                    }
                }

                // Перестановка строк
                if (max < eps)
                {
                    // нет ненулевых диагональных элементов
                    throw new Exception($"Решение получить невозможно из-за нулевого столбца {index} матрицы А");
                }

                for (int j = 0; j < valueEquation; j++)
                {
                    (A[k, j], A[index, j]) = (A[index, j], A[k, j]);
                }

                (Y1[k], Y1[index]) = (Y1[index], Y1[k]);

                // Нормализация уравнений
                for (int i = k; i < valueEquation; i++)
                {
                    double temp = A[i, k];
                    if (Math.Abs(temp) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < valueEquation; j++)
                        A[i, j] = A[i, j] / temp;
                    Y1[i] = Y1[i] / temp;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < valueEquation; j++)
                        A[i, j] = A[i, j] - A[k, j];
                    Y1[i] = Y1[i] - Y1[k];
                }
            }
            // обратная подстановка
            for (int k = valueEquation - 1; k >= 0; k--)
            {
                x[k] = Y1[k];
                for (int i = 0; i < k; i++)
                    Y1[i] = Y1[i] - A[i, k] * x[k];
            }

            return x;
        }

        static double[] MethodJordana()
        {
            int row = M.GetLength(0);
            int column = M.GetLength(1) + 1;

            double[,] B = new double[row, column];
            double[] Y2 = (double[])Y.Clone();

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column - 1; j++)
                {
                    B[i, j] = M[i, j];
                }
                B[i, column - 1] = Y2[i];
            }

            for (int i = 0; i < row; i++)
            {
                double use = B[i, i];
                if (use == 0) 
                {
                    int j;
                    for (j = i + 1; j < row; j++) 
                    {
                        if (B[j, i] != 0)
                        {
                            double[] oldmatrix = new double[column];
                            double[] newmatrix = new double[column];

                            for (int k = 0; k < column; k++) oldmatrix[k] = B[i, k];
                            for (int k = 0; k < column; k++)
                            {
                                B[i, k] = B[j, k];
                                B[j, k] = oldmatrix[k];
                            }
                            use = B[i, i];
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
                        B[i, j] /= use;
                        if (B[i, j] == -0) B[i, j] = 0;
                        //Console.WriteLine($"x{i + 1}-{j + 1}/{use} = {Matrix[i, j]}");
                    }
                }
                for (int j = i + 1; j < row; j++) 
                {
                    double cons = B[j, i];
                    if (cons != 0) 
                    {
                        for (int k = i; k < column; k++)
                        {
                            B[j, k] = -cons * B[i, k] + B[j, k];
                        }
                    }
                }
            }
            for (int i = row - 1; i >= 0; i--)
            {
                if (B[i, i] == 0) break; 
                for (int j = i - 1; j >= 0; j--)
                {
                    double cons = B[j, i];
                    if (cons != 0) 
                    {
                        for (int k = i; k < column; k++)
                        {
                            B[j, k] = -cons * B[i, k] + B[j, k];
                        }
                    }
                }
            }

            double[] Return = new double[row];
            for (int i = 0; i < row; i++)
            {
                Return[i] = B[i, row];
            }
            return Return;
        }

        static double[,] InverseMatrix(double[,] Matrix)
        {
            int n = Matrix.GetLength(0); //Размерность начальной матрицы

            double[,] xirtaM = new double[n, n]; //Единичная матрица (искомая обратная матрица)
            for (int i = 0; i < n; i++)
                xirtaM[i, i] = 1;

            double[,] Matrix_Big = new double[n, 2 * n]; //Общая матрица, получаемая скреплением Начальной матрицы и единичной
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    Matrix_Big[i, j] = Matrix[i, j];
                    Matrix_Big[i, j + n] = xirtaM[i, j];
                }

            //Прямой ход (Зануление нижнего левого угла)
            for (int k = 0; k < n; k++) //k-номер строки
            {
                for (int i = 0; i < 2 * n; i++) //i-номер столбца
                    Matrix_Big[k, i] = Matrix_Big[k, i] / Matrix[k, k]; //Деление k-строки на первый член !=0 для преобразования его в единицу
                for (int i = k + 1; i < n; i++) //i-номер следующей строки после k
                {
                    double K = Matrix_Big[i, k] / Matrix_Big[k, k]; //Коэффициент
                    for (int j = 0; j < 2 * n; j++) //j-номер столбца следующей строки после k
                        Matrix_Big[i, j] = Matrix_Big[i, j] - Matrix_Big[k, j] * K; //Зануление элементов матрицы ниже первого члена, преобразованного в единицу
                }
                for (int i = 0; i < n; i++) //Обновление, внесение изменений в начальную матрицу
                    for (int j = 0; j < n; j++)
                        Matrix[i, j] = Matrix_Big[i, j];
            }

            //Обратный ход (Зануление верхнего правого угла)
            for (int k = n - 1; k > -1; k--) //k-номер строки
            {
                for (int i = 2 * n - 1; i > -1; i--) //i-номер столбца
                    Matrix_Big[k, i] = Matrix_Big[k, i] / Matrix[k, k];
                for (int i = k - 1; i > -1; i--) //i-номер следующей строки после k
                {
                    double K = Matrix_Big[i, k] / Matrix_Big[k, k];
                    for (int j = 2 * n - 1; j > -1; j--) //j-номер столбца следующей строки после k
                        Matrix_Big[i, j] = Matrix_Big[i, j] - Matrix_Big[k, j] * K;
                }
            }

            //Отделяем от общей матрицы
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    xirtaM[i, j] = Matrix_Big[i, j + n];

            return xirtaM;
        }
    }
}
