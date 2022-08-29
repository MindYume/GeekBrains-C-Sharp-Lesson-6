using Homework.Utils;
using System;
using System.IO;

namespace Homework
{
    internal class Program
    {
        public delegate double Fun(double a, double x);
        public delegate double MathFunction(double a);

        static void Main(string[] args)
        {
            // Включение Кириллицы в консоли
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;


            bool runProgram = true;

            while (runProgram)
            {
                Console.WriteLine("=====================================================");
                Console.WriteLine("1 -> Задача 1");
                Console.WriteLine("2 -> Задача 2");
                Console.WriteLine("0 -> Выход");
                Console.WriteLine("=====================================================");

                int taskNumber;

                // Если пользователь неправильно ввёл номер задачи, то taskNumber будет равным -1, и в блоке switch
                // программа сообщит об этом пользователю
                if (!int.TryParse(CommonMethods.AskInfo("Введите номер задачи: "), out taskNumber))
                {
                    taskNumber = -1;
                }

                switch (taskNumber)
                {
                    default:
                        Console.WriteLine("Некорректый номер задачи. Повторие ввод.\n");
                        break;

                    case 0:
                        runProgram = false;
                        break;

                    case 1:
                        Task1();
                        break;

                    case 2:
                        Task2();
                        break;
                }
            }

            CommonMethods.Pause("\nНажмите любую клавишу, чтобы выйти...");
        }


        #region Методы для вызова задачь из домашней работы
        static void Task1()
        {
            CommonMethods.PrintTaskInfo(
                6,
                1,
                "Изменить программу вывода таблицы функции так, чтобы можно было передавать функции типа double (double, double)." +
                "Продемонстрировать работу на функции с функцией a*x^2 и функцией a*sin(x)",
                "Грачёв Виктор Алексеевич"
            );

            Console.WriteLine("Вызов функуии a*x^2, a = 1.5");
            Table(FuncAX2, 1.5, -2, 2);

            Console.WriteLine("Вызов функуии a*x^2, a = -5");
            Table(FuncAX2, -5, -2, 2);

            Console.WriteLine("Вызов функуии a*sin(x), a = 2.5");
            Table(FuncASinX, 2.5, -2, 2);

            Console.WriteLine("Вызов функуии a*sin(x), a = -0.1");
            Table(FuncASinX, -0.1, -2, 2);

            CommonMethods.Pause("\nНажмите любую клавишу, чтобы продолжить...");
        }

        static void Task2()
        {
            // SaveFunc(F, "data.bin", -100, 100, 0.5);
            // Console.WriteLine(Load("data.bin"));

            CommonMethods.PrintTaskInfo(
               6,
               2,
               "Модифицировать программу нахождения минимума функции так, чтобы можно было передавать функцию в виде делегата.\n" +
               "а) Сделать меню с различными функциями и представить пользователю выбор, для какой функции и на каком отрезке находить минимум.Использовать массив(или список) делегатов, в котором хранятся различные функции.\n" +
               "б) *Переделать функцию Load, чтобы она возвращала массив считанных значений.Пусть она возвращает минимум через параметр(с использованием модификатора out)",
               "Грачёв Виктор Алексеевич"
            );

            MathFunction[] FuncArray = { F1, F2, F3, F4 };

            MathFunction mathFunction = ChooseFunction(FuncArray);

            double startValue = CommonMethods.AskInfoDouble("Введите первое число: ");
            double endValue   = CommonMethods.AskInfoDouble("Введите последнее число: ");
            double step       = CommonMethods.AskInfoDouble("Введите размер шага: ");

            double funcMinimum;

            SaveFunc(mathFunction, "data.bin", startValue, endValue, step);
            double[] values = Load("data.bin", out funcMinimum);

            Console.WriteLine();
            Console.Write("Результаты выполнения функции: ");
            for (int i = 0; i < values.Length; i++)
            {
                Console.Write($"{values[i]:F2}\t");
            }

            Console.WriteLine($"\nМинимум функции на отрезке от {startValue} до {endValue} равен {funcMinimum}");

            CommonMethods.Pause("\nНажмите любую клавишу, чтобы продолжить...");
        }

        #endregion

        #region Вспомогательные методы

        public static void Table(Fun F, double a, double x, double b)
        {
            Console.WriteLine("----- X ----- Y -----");
            while (x <= b)
            {
                Console.WriteLine("| {0,7:0.000} | {1,7:0.000} |", x, F(a,x));
                x += 1;
            }
            Console.WriteLine("---------------------\n");
        }


        public static double FuncAX2(double a, double x) => a * x * x;


        public static double FuncASinX(double a, double x) => a * Math.Sin(x);



        public static double F1(double x)
        {
            return Math.Pow(x, 2);
        }

        public static double F2(double x)
        {
            return 2 * Math.Pow(x, 3) - x;
        }

        public static double F3(double x)
        {
            return 10 * Math.Pow(x, 2) - 5 * x + 1;
        }

        public static double F4(double x)
        {
            return Math.Pow(x, 3) - Math.Pow(x, 2) + x;
        }


        public static void SaveFunc(MathFunction Func, string fileName, double a, double b, double h)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create,
            FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);
            double x = a;
            while (x <= b)
            {
                bw.Write(Func(x));
                x += h;// x=x+h;
            }
            bw.Close();
            fs.Close();
        }

        public static double[] Load(string fileName, out double funcMinimum)
        {

            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader bw = new BinaryReader(fs);

            long valuesAmount = fs.Length / sizeof(double);

            double[] retValues = new double[valuesAmount];

            double min = double.MaxValue;
            double d;

            for (int i = 0; i < valuesAmount; i++)
            {
                // Считываем значение и переходим к следующему
                d = bw.ReadDouble();
                if (d < min) min = d;

                retValues[i] = d;
            }

            bw.Close();
            fs.Close();

            funcMinimum = min;

            return retValues;
        }

        public static MathFunction ChooseFunction(MathFunction[] FuncArray)
        {
            bool exitMethod = true;

            while (exitMethod)
            {
                Console.WriteLine("Выберите функцию");
                Console.WriteLine();
                Console.WriteLine("1 -> x^2");
                Console.WriteLine("2 -> 2x^3 - x");
                Console.WriteLine("3 -> 10x^2 - 5x + 1");
                Console.WriteLine("4 -> x^3 - x^2 + x");
                Console.WriteLine();

                int chooseNumber;
                if (!int.TryParse(CommonMethods.AskInfo("Введите номер функции: "), out chooseNumber))
                {
                    chooseNumber = -1;
                }

                switch (chooseNumber)
                {
                    default:
                        Console.WriteLine("Некорректый номер. Повторие ввод.\n");
                        break;

                    case 1:
                        return F1;

                    case 2:
                        return F2;

                    case 3:
                        return F3;

                    case 4:
                        return F4;
                }
            }

            return null;
        }

        #endregion
    }
}
