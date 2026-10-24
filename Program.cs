using System;

namespace Lab5_Variant14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int choice = 0;

            while (choice != 4)
            {
                Console.WriteLine("\n--- МЕНЮ (Вариант 14) ---");
                Console.WriteLine("1. Двумерный массив (Удалить столбцы с K1 по K2)");
                Console.WriteLine("2. Рваный массив (Добавить строку в начало)");
                Console.WriteLine("3. Обработка строки");
                Console.WriteLine("4. Выход");

                choice = ReadInt("Выберите пункт: ");

                switch (choice)
                {
                    case 1: TaskMatrix(); break;
                    case 2: TaskJagged(); break;
                    case 3: TaskString(); break;
                    case 4: Console.WriteLine("Выход..."); break;
                    default: Console.WriteLine("Ошибка: неверный пункт меню."); break;
                }
            }
        }

        // Простая функция ввода числа с обработкой ошибок (try-catch из методички)
        static int ReadInt(string message)
        {
            while (true)
            {
                try
                {
                    Console.Write(message);
                    return Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException)
                {
                    Console.WriteLine("Ошибка: нужно ввести целое число!");
                }
            }
        }

        // ==========================================
        // ЗАДАЧА 1: ДВУМЕРНЫЙ МАССИВ
        // ==========================================
        static void TaskMatrix()
        {
            Console.WriteLine("\n--- Двумерный массив ---");
            int rows = ReadInt("Введите количество строк: ");
            int cols = ReadInt("Введите количество столбцов: ");

            int[,] matrix = new int[rows, cols];
            Random rnd = new Random();

            // Заполнение случайными числами
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = rnd.Next(1, 10);
                }
            }

            PrintMatrix(matrix, "Исходная матрица:");

            int k1 = ReadInt($"Введите K1 (от 0 до {cols - 1}): ");
            int k2 = ReadInt($"Введите K2 (от {k1} до {cols - 1}): ");

            // Проверка на ошибку (из методички: нельзя удалить больше, чем есть)
            if (k1 < 0 || k2 >= cols || k1 > k2)
            {
                Console.WriteLine("Ошибка: неверные индексы K1 и K2.");
                return;
            }
            if (k2 - k1 + 1 >= cols)
            {
                Console.WriteLine("Ошибка: нельзя удалить все столбцы!");
                return;
            }

            // Создаем новую матрицу, где меньше столбцов
            int newCols = cols - (k2 - k1 + 1);
            int[,] newMatrix = new int[rows, newCols];

            for (int i = 0; i < rows; i++)
            {
                int newJ = 0;
                for (int j = 0; j < cols; j++)
                {
                    // Если столбец не входит в удаляемый диапазон - копируем его
                    if (j < k1 || j > k2)
                    {
                        newMatrix[i, newJ] = matrix[i, j];
                        newJ++;
                    }
                }
            }

            PrintMatrix(newMatrix, "Результат:");
        }

        // Использование параметра по умолчанию (separator = " ")
        static void PrintMatrix(int[,] matrix, string title, string separator = " ")
        {
            Console.WriteLine(title);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + separator);
                }
                Console.WriteLine();
            }
        }

        // ==========================================
        // ЗАДАЧА 2: РВАНЫЙ МАССИВ
        // ==========================================
        static void TaskJagged()
        {
            Console.WriteLine("\n--- Рваный массив ---");
            int rows = ReadInt("Введите количество строк: ");

            int[][] jagged = new int[rows][];
            Random rnd = new Random();

            // Заполнение рваного массива
            for (int i = 0; i < rows; i++)
            {
                int cols = ReadInt($"Введите длину строки {i}: ");
                jagged[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jagged[i][j] = rnd.Next(1, 10);
                }
            }

            PrintJagged(jagged, "Исходный рваный массив:");

            // Создаем новую строку для добавления в начало
            int newLength = ReadInt("Введите длину новой строки: ");
            int[] newRow = new int[newLength];
            for (int i = 0; i < newLength; i++)
            {
                newRow[i] = rnd.Next(1, 10);
            }

            // Создаем новый массив на 1 строку больше
            int[][] result = new int[rows + 1][];
            result[0] = newRow; // Первая строка - новая

            // Копируем старые строки со сдвигом на 1 вниз
            for (int i = 0; i < rows; i++)
            {
                result[i + 1] = jagged[i];
            }

            PrintJagged(result, "Результат:");
        }

        static void PrintJagged(int[][] jagged, string title)
        {
            Console.WriteLine(title);
            for (int i = 0; i < jagged.Length; i++)
            {
                for (int j = 0; j < jagged[i].Length; j++)
                {
                    Console.Write(jagged[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        // ==========================================
        // ЗАДАЧА 3: СТРОКА
        // ==========================================
        static void TaskString()
        {
            Console.WriteLine("\n--- Строка ---");
            Console.Write("Введите строку: ");
            string text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Ошибка: строка пустая!");
                return;
            }

            // Простое разбиение на слова по пробелам (по методичке)
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // 1. Переворачиваем каждое слово
            for (int i = 0; i < words.Length; i++)
            {
                char[] chars = words[i].ToCharArray(); // Перевод в массив символов
                Array.Reverse(chars);                  // Метод класса Array
                words[i] = new string(chars);          // Обратно в строку
            }

            // 2. Сортировка слов по убыванию длины (простой "пузырек")
            // (В методичке в 14 варианте в примере сортировка по возрастанию, но в задании написано "по убыванию")
            for (int i = 0; i < words.Length; i++)
            {
                for (int j = i + 1; j < words.Length; j++)
                {
                    if (words[i].Length < words[j].Length)
                    {
                        // Меняем местами
                        string temp = words[i];
                        words[i] = words[j];
                        words[j] = temp;
                    }
                }
            }

            // Собираем обратно в строку
            string result = string.Join(" ", words);
            Console.WriteLine("Результат: " + result);
        }
    }
}