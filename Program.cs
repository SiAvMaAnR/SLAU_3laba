using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLAU
{
    class Program
    {
        //Левая часть
        static double[,] MatrixA;
        //Правая часть
        static double[] MatrixB;
        //Размерность
        static int Dimension;

        static void PrintSLAU(double[,] Matrix)
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
                Console.WriteLine($" = { String.Format("{0,3}", MatrixB[i])}");
            }
        }

        static void PrintAnswer(double[] Matrix, string text, string Return = "")
        {
            Console.WriteLine($"\n{text}");
            for (int i = 0; i < Dimension; i++)
                Console.WriteLine($"x[{i + 1}] = {Matrix[i]}");
        }

        static void PrintAnswer(double[,] Matrix, string text, string Return = "")
        {
            Console.WriteLine($"\n{text}");
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    Console.Write($"{String.Format("{0,3}", Matrix[i, j])} ");
                }
                Console.WriteLine();
            }
        }

        static void SetMatrix()
        {
            Console.Write("Кол-во уравнений: ");
            Dimension = int.Parse(Console.ReadLine());

            MatrixA = new double[Dimension, Dimension];
            MatrixB = new double[Dimension];

            //Вводим матрицу А
            for (int i = 0; i < MatrixA.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < MatrixA.GetUpperBound(1) + 1; j++)
                {
                    Console.Write($"A[{i + 1},{j + 1}] : ");
                    MatrixA[i, j] = Convert.ToDouble(Console.ReadLine());
                }
            }

            //Вводим правую часть MatrixB
            for (int i = 0; i < MatrixB.GetUpperBound(0) + 1; i++)
            {
                Console.Write($"B[{i + 1}] : ");
                MatrixB[i] = Convert.ToDouble(Console.ReadLine());
            }

        }

        static void Main(string[] args)
        {
            try
            {
                //SetMatrix();
                Dimension = 3;

                MatrixA = TestA();
                MatrixB = TestB();
                PrintSLAU(MatrixA);

                double[] Result1 = new MethodGauss(MatrixA, MatrixB, Dimension).Run();
                PrintAnswer(Result1,"Метод Гаусса: ");

                double[] Result2 = new MethodGauss_Column(MatrixA, MatrixB, Dimension).Run();
                PrintAnswer(Result2, "Метод Гаусса (колонки): ");

                double[] Result3 = new MethodGauss_Row(MatrixA, MatrixB, Dimension).Run();
                PrintAnswer(Result3, "Метод Гаусса (строки): ");

                double[] Result4 = new MethodJordan(MatrixA, MatrixB, Dimension).Run();
                PrintAnswer(Result4, "Метод Жордана: ");

                double[,] Result5 = new InverseJordanMatrix(MatrixA, MatrixB, Dimension).Run();
                PrintAnswer(Result5, "Метод Жордана / Обратная матрица: ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не корректные данные\n" + ex.Message);
            }
            Console.ReadKey();
        }


        static double[,] TestA()
        {
            double[,] matrix = new double[Dimension, Dimension];

            matrix[0, 0] = 5;
            matrix[0, 1] = 0;
            matrix[0, 2] = 1;
            matrix[1, 0] = 1;
            matrix[1, 1] = 3;
            matrix[1, 2] = -1;
            matrix[2, 0] = -3;
            matrix[2, 1] = 2;
            matrix[2, 2] = 10;

            return matrix;
        }


        static double[]TestB()
        {
            double[] matrix = new double[Dimension];

            matrix[0] =24;
            matrix[1] =9;
            matrix[2] =-23;

            return matrix;
        }

    }
}
