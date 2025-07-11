using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingTest
{
    class SortingAlgorithms
    {
        public static long ComparisonCount = 0;
        public static long SwapCount = 0;
        public static void BubbleSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    ComparisonCount++;
                    if (arr[j] > arr[j + 1])
                    {
                        SwapCount++;
                        int temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        public static void SelectionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    ComparisonCount++;
                    if (arr[j] < arr[minIndex])
                        minIndex = j;
                }
                if (minIndex != i)
                {
                    SwapCount++;
                    int temp = arr[minIndex];
                    arr[minIndex] = arr[i];
                    arr[i] = temp;
                }
            }
        }

        public static void MergeSort(int[] arr)
        {
            if (arr.Length <= 1)
                return;

            int mid = arr.Length / 2;
            int[] left = new int[mid];
            int[] right = new int[arr.Length - mid];

            Array.Copy(arr, 0, left, 0, mid);
            Array.Copy(arr, mid, right, 0, arr.Length - mid);

            MergeSort(left);
            MergeSort(right);
            Merge(arr, left, right);
        }

        private static void Merge(int[] arr, int[] left, int[] right)
        {
            int i = 0, j = 0, k = 0;
            while (i < left.Length && j < right.Length)
            {
                ComparisonCount++;
                if (left[i] <= right[j])
                    arr[k++] = left[i++];
                else
                    arr[k++] = right[j++];
            }
            while (i < left.Length)
                arr[k++] = left[i++];
            while (j < right.Length)
                arr[k++] = right[j++];
        }

        public static void QuickSort(int[] arr, int left, int right)
        {
            if (left >= right)
                return;

            int pivot = arr[(left + right) / 2];
            int i = left;
            int j = right;

            while (i <= j)
            {
                while (true)
                {
                    ComparisonCount++;
                    if (!(arr[i] < pivot)) break;
                    i++;
                }
                while (true)
                {
                    ComparisonCount++;
                    if (!(arr[j] > pivot)) break;
                    j--;
                }

                if (i <= j)
                {
                    SwapCount++;
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    i++;
                    j--;
                }
            }

            if (left < j) QuickSort(arr, left, j);
            if (i < right) QuickSort(arr, i, right);
        }
    }
}
