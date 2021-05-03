using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    class Program
    {
        static double[,] A;
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

                A = new double[valueEquation, valueEquation];
                Y = new double[valueEquation];

                //Вводим матрицу А
                for (int i = 0; i < A.GetUpperBound(0) + 1; i++)
                {
                    for (int j = 0; j < A.GetUpperBound(1) + 1; j++)
                    {
                        Console.Write($"A[{i + 1},{j + 1}] : ");
                        A[i, j] = Convert.ToDouble(Console.ReadLine());
                    }
                }

                //Вводим правую часть Y
                for (int i = 0; i < Y.GetUpperBound(0) + 1; i++)
                {
                    Console.Write($"Y[{i + 1}] : ");
                    Y[i] = Convert.ToDouble(Console.ReadLine());
                }

                //Выводим матрицу
                PrintMatrix(A);

                #region Гаусса
                //Получаем результаты с помощью метода Гаусса
                double[] Result1 = MethodGauss();

                Console.WriteLine("\nМетод Гауса: ");

                //Выводим ответы
                for (int i = 0; i < valueEquation; i++)
                    Console.WriteLine($"x[{i + 1}]={Result1[i]}");
                #endregion

                #region Жордана
                //Получаем результаты с помощью метода Жордана
                double[] Result2 = MethodJordana();

                //Выводим ответы
                for (int i = 0; i < valueEquation; i++)
                    Console.WriteLine($"x[{i + 1}]={Result2[i]}");
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
            }
        }

        static double[] MethodGauss()
        {
            double[] x = new double[valueEquation];
            //Точность 
            const double eps = 0.00001;

            double max;
            int index;
            for ( int k = 0; k < valueEquation; k++)
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

                (Y[k], Y[index]) = (Y[index], Y[k]);

                // Нормализация уравнений
                for (int i = k; i < valueEquation; i++)
                {
                    double temp = A[i, k];
                    if (Math.Abs(temp) < eps) continue; // для нулевого коэффициента пропустить
                    for (int j = 0; j < valueEquation; j++)
                        A[i, j] = A[i, j] / temp;
                    Y[i] = Y[i] / temp;
                    if (i == k) continue; // уравнение не вычитать само из себя
                    for (int j = 0; j < valueEquation; j++)
                        A[i, j] = A[i, j] - A[k, j];
                    Y[i] = Y[i] - Y[k];
                }
            }
            // обратная подстановка
            for ( int k = valueEquation - 1; k >= 0; k--)
            {
                x[k] = Y[k];
                for (int i = 0; i < k; i++)
                    Y[i] = Y[i] - A[i, k] * x[k];
            }

            return x;
        }


        static double[] MethodJordana()
        {

            return null;
        }
    }
}
