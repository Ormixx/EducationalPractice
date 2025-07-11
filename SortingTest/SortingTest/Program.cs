using SortingTest;
using System;
using System.Diagnostics;
using System.Linq;
using ScottPlot;
using System.Drawing.Imaging;



class Program
{
    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Выберите эксперимент:");
            Console.WriteLine("1. Измерение времени сортировок");
            Console.WriteLine("2. Подсчёт количества сравнений в алгоритмах сортировки");
            Console.WriteLine("3. Подсчёт количества обменов (перестановок) в алгоритмах сортировки");
            Console.WriteLine("0. Выход");
            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

            if (input == "0")
                break;

            switch (input)
            {
                case "1":
                    RunSortingTimeExperiment();
                    break;

                case "2":
                    RunComparisonCountExperiment();
                    break;

                case "3":
                    RunSwapCountExperiment();
                    break;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void RunSortingTimeExperiment()
    {
        Random rnd = new Random();

        int[] sizes = new int[10];
        long[] timesBubble = new long[10];
        long[] timesSelection = new long[10];
        long[] timesMerge = new long[10];
        long[] timesQuick = new long[10];

        Stopwatch sw = new Stopwatch();

        Console.WriteLine();
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", "Размер", "Пузырьковая(мс)", "Выбор(мс)", "Слияние(мс)", "Быстрая(мс)");

        for (int i = 0, size = 2000; size <= 20000; size += 2000, i++)
        {
            sizes[i] = size;

            int[] original = new int[size];
            for (int j = 0; j < size; j++)
                original[j] = rnd.Next(0, 100000);

            int[] arrBubble = new int[size];
            int[] arrSelection = new int[size];
            int[] arrMerge = new int[size];
            int[] arrQuick = new int[size];

            Array.Copy(original, arrBubble, size);
            Array.Copy(original, arrSelection, size);
            Array.Copy(original, arrMerge, size);
            Array.Copy(original, arrQuick, size);

            sw.Restart();
            SortingAlgorithms.BubbleSort(arrBubble);
            sw.Stop();
            timesBubble[i] = sw.ElapsedMilliseconds;

            sw.Restart();
            SortingAlgorithms.SelectionSort(arrSelection);
            sw.Stop();
            timesSelection[i] = sw.ElapsedMilliseconds;

            sw.Restart();
            SortingAlgorithms.MergeSort(arrMerge);
            sw.Stop();
            timesMerge[i] = sw.ElapsedMilliseconds;

            sw.Restart();
            SortingAlgorithms.QuickSort(arrQuick, 0, arrQuick.Length - 1);
            sw.Stop();
            timesQuick[i] = sw.ElapsedMilliseconds;

            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", size, timesBubble[i], timesSelection[i], timesMerge[i], timesQuick[i]);
        }

        Console.WriteLine();
        Console.WriteLine("Хотите построить график и сохранить в файл? (д/н)");
        string answer = Console.ReadLine().ToLower();

        if (answer == "д" || answer == "y" || answer == "yes")
        {
            BuildAndSavePlot(sizes, timesBubble, timesSelection, timesMerge, timesQuick);
        }
        else
        {
            Console.WriteLine("График не будет построен.");
        }
    }

    static void RunComparisonCountExperiment()
    {
        int[] sizes = new int[10];
        long[] compsBubble = new long[10];
        long[] compsSelection = new long[10];
        long[] compsMerge = new long[10];
        long[] compsQuick = new long[10];

        Console.WriteLine();
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", "Размер", "Пузырьковая", "Выбор", "Слияние", "Быстрая");

        Random rnd = new Random();

        for (int i = 0, size = 2000; size <= 20000; size += 2000, i++)
        {
            sizes[i] = size;

            int[] original = new int[size];
            for (int j = 0; j < size; j++)
                original[j] = rnd.Next(0, 100000);

            int[] arrBubble = new int[size];
            int[] arrSelection = new int[size];
            int[] arrMerge = new int[size];
            int[] arrQuick = new int[size];

            Array.Copy(original, arrBubble, size);
            Array.Copy(original, arrSelection, size);
            Array.Copy(original, arrMerge, size);
            Array.Copy(original, arrQuick, size);

            SortingAlgorithms.ComparisonCount = 0;
            SortingAlgorithms.BubbleSort(arrBubble);
            compsBubble[i] = SortingAlgorithms.ComparisonCount;

            SortingAlgorithms.ComparisonCount = 0;
            SortingAlgorithms.SelectionSort(arrSelection);
            compsSelection[i] = SortingAlgorithms.ComparisonCount;

            SortingAlgorithms.ComparisonCount = 0;
            SortingAlgorithms.MergeSort(arrMerge);
            compsMerge[i] = SortingAlgorithms.ComparisonCount;

            SortingAlgorithms.ComparisonCount = 0;
            SortingAlgorithms.QuickSort(arrQuick, 0, arrQuick.Length - 1);
            compsQuick[i] = SortingAlgorithms.ComparisonCount;

            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}",
                size, compsBubble[i], compsSelection[i], compsMerge[i], compsQuick[i]);
        }

        Console.WriteLine();
        Console.WriteLine("Хотите построить график и сохранить в файл? (д/н)");
        string answer = Console.ReadLine().ToLower();

        if (answer == "д" || answer == "y" || answer == "yes")
        {
            var plt = new ScottPlot.Plot(800, 600);
            plt.Title("Количество сравнений в алгоритмах сортировки");
            plt.XLabel("Размер массива");
            plt.YLabel("Количество сравнений");

            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), compsBubble.Select(x => (double)x).ToArray(), label: "Пузырьковая");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), compsSelection.Select(x => (double)x).ToArray(), label: "Выбор");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), compsMerge.Select(x => (double)x).ToArray(), label: "Слияние");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), compsQuick.Select(x => (double)x).ToArray(), label: "Быстрая");

            plt.Legend();

            SavePlot(plt, "comparison_count_experiment");
        }
        else
        {
            Console.WriteLine("График не будет построен.");
        }
    }

    static void RunSwapCountExperiment()
    {
        int[] sizes = new int[10];
        long[] swapsBubble = new long[10];
        long[] swapsSelection = new long[10];
        long[] swapsMerge = new long[10];
        long[] swapsQuick = new long[10];

        Console.WriteLine();
        Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}", "Размер", "Пузырьковая", "Выбор", "Слияние", "Быстрая");

        for (int i = 0, size = 2000; size <= 20000; size += 2000, i++)
        {
            sizes[i] = size;

            int[] original = new int[size];
            for (int j = 0; j < size; j++)
                original[j] = size - j;

            int[] arrBubble = new int[size];
            int[] arrSelection = new int[size];
            int[] arrMerge = new int[size];
            int[] arrQuick = new int[size];

            Array.Copy(original, arrBubble, size);
            Array.Copy(original, arrSelection, size);
            Array.Copy(original, arrMerge, size);
            Array.Copy(original, arrQuick, size);

            SortingAlgorithms.SwapCount = 0;
            SortingAlgorithms.BubbleSort(arrBubble);
            swapsBubble[i] = SortingAlgorithms.SwapCount;

            SortingAlgorithms.SwapCount = 0;
            SortingAlgorithms.SelectionSort(arrSelection);
            swapsSelection[i] = SortingAlgorithms.SwapCount;

            swapsMerge[i] = 0;

            SortingAlgorithms.SwapCount = 0;
            SortingAlgorithms.QuickSort(arrQuick, 0, arrQuick.Length - 1);
            swapsQuick[i] = SortingAlgorithms.SwapCount;

            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15} {4,-15}",
                size, swapsBubble[i], swapsSelection[i], swapsMerge[i], swapsQuick[i]);
        }

        Console.WriteLine();
        Console.WriteLine("Хотите построить график и сохранить в файл? (д/н)");
        string answer = Console.ReadLine().ToLower();

        if (answer == "д" || answer == "y" || answer == "yes")
        {
            var plt = new ScottPlot.Plot(800, 600);
            plt.Title("Количество обменов в алгоритмах сортировки");
            plt.XLabel("Размер массива");
            plt.YLabel("Количество обменов");

            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), swapsBubble.Select(x => (double)x).ToArray(), label: "Пузырьковая");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), swapsSelection.Select(x => (double)x).ToArray(), label: "Выбор");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), swapsMerge.Select(x => (double)x).ToArray(), label: "Слияние");
            plt.AddScatter(sizes.Select(x => (double)x).ToArray(), swapsQuick.Select(x => (double)x).ToArray(), label: "Быстрая");

            plt.Legend();

            SavePlot(plt, "swap_count_experiment");
        }
        else
        {
            Console.WriteLine("График не будет построен.");
        }
    }

    static void SavePlot(ScottPlot.Plot plt, string baseFileName)
    {
        string folderPath = @"C:\Users\Пользователь\Desktop\Practica\Учебная практика. Рязанов А.В. ПИНб-21\Grafs";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string extension = ".png";
        string filePath = Path.Combine(folderPath, baseFileName + extension);

        int count = 1;
        while (File.Exists(filePath))
        {
            filePath = Path.Combine(folderPath, $"{baseFileName}_{count}{extension}");
            count++;
        }

        using (var bmp = plt.Render())
        {
            bmp.Save(filePath, ImageFormat.Png);
        }

        Console.WriteLine($"График сохранён в файл {filePath}");
    }

    static void BuildAndSavePlot(int[] sizes, long[] timesBubble, long[] timesSelection, long[] timesMerge, long[] timesQuick)
    {
        var plt = new ScottPlot.Plot(800, 600);

        plt.Title("Сравнение времени сортировок");
        plt.XLabel("Размер массива");
        plt.YLabel("Время (мс)");
        plt.AddScatter(sizes.Select(x => (double)x).ToArray(), timesBubble.Select(x => (double)x).ToArray(), label: "Пузырьковая");
        plt.AddScatter(sizes.Select(x => (double)x).ToArray(), timesSelection.Select(x => (double)x).ToArray(), label: "Выбор");
        plt.AddScatter(sizes.Select(x => (double)x).ToArray(), timesMerge.Select(x => (double)x).ToArray(), label: "Слияние");
        plt.AddScatter(sizes.Select(x => (double)x).ToArray(), timesQuick.Select(x => (double)x).ToArray(), label: "Быстрая");

        plt.Legend();

        SavePlot(plt, "sorting_times");
    }
}
