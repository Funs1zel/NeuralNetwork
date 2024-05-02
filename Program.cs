using System;

namespace NeuralNetwork
{
    class Program
    {
        static int weights;     //кол-во весов
        static double[] weightsArray;       //сами веса
        static double[] example;            //пример, который вводит пользователь

        static void Main(string[] args)
        {
            Random rand = new Random();

            Console.Write("Введите количество чисел в каждом из примеров: ");
            weights = int.Parse(Console.ReadLine());
            weightsArray = new double[weights];

            for(int i = 0; i < weightsArray.Length; i++)
            {
                weightsArray[i] = rand.NextSingle();
            }

            example = new double[weights];

            Console.WriteLine("Сгенерированные примеры: ");
            Train();

            for (int i = 0; i < weights; i++)
                weightsArray[i] = (weightsArray[i] > 0) ? weightsArray[i] : 0;

            Console.WriteLine();
            Output(weightsArray);
            Console.WriteLine();

            for (int i = 0; i < weights; i++)
            {
                Console.Write($"Введите {i + 1} число в вашем примере: ");
                example[i] = double.Parse(Console.ReadLine());
            }

            Console.WriteLine();

            for (int i = 0; i < weights; i++)
            {
                Console.Write($"{example[i]} ");
            }
            Console.WriteLine($"- {Activate(RightProp(example))}");
            Console.ReadLine();
        }


        static double Activate(double value)      //однозначный ответ, 1 или 0
        {
            return (value <= 0) ? 0 : 1;
        }

        //Само вычисление результата на основании имеющихся входных данных и настроенных весов
        static double RightProp(double[] inputs)
        {
            double res = 0;

            for (int i = 0; i < weights; i++)
                res = res + (inputs[i] * weightsArray[i]);

            return res;
        }


        //Генерация чисел для обучения
        static void Generate(ref int[,] inputs, ref int[] expected)
        {
            Random rand = new Random();

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < weights; j++)
                {
                    inputs[i, j] = rand.Next(0, 2);
                }

                expected[i] = inputs[i, 0] * inputs[i, 1];
            }
        }

        static void Output(int[,] inputs, int[] expected)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < weights; ++j)
                {
                    Console.Write($"{inputs[i, j]} ");
                }

                Console.Write($"- {expected[i]}");
                Console.WriteLine();
            }
        }

        static void Output(double[] expected)
        {
            for (int i = 0; i < weights; i++)
            {
                Console.Write($"{expected[i]} ");
            }
            Console.WriteLine();
        }


        // inputs[5, weights] = столько сколько ввёл человек
        // expected[5]
        static void Train()
        {
            int[,] inputs = new int[5, weights];       //входные значения, первое число - номер примера, второе - номер числа в этом примере
            int[] expected = new int[5];               //результаты из примеров
            double[] temp = new double[weights];       //нужно для передачи массива как параметра

            double LR = 0.01;      //так надо
            double epoch = 100000;    //так надо
            double error;         //количество ошибки (разность между ожидаемым и получившимся результатом)

            for (int i = 0; i < epoch; i++)     //эпохи обучения
            {
                Generate(ref inputs, ref expected);        //Генерируем примеры для обучения и выводим их на экран
                Output(weightsArray);

                for (int j = 0; j < 5; j++)          //кол-во примеров
                {
                    for (int k = 0; k < weights; k++)
                    {
                        temp[k] = inputs[j, k];      //j номер примера, k номер числа в примере
                    }

                    error = expected[j] - Activate(RightProp(temp));

                    for (int k = 0; k < weights; k++)
                    {
                        weightsArray[k] += LR * error * inputs[j, k];     //опять же, j - номер примера, k - номер числа в примере
                    }
                }
            }
        }
    }
}