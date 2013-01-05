using System;

namespace RandomMusings
{
	public class MergingSort
	{
		static void Main ()
		{
			int[] unsorted = {92,23,44,221,334,4212,-32,1,-34435687,34};
			MergeSort (unsorted, 0, unsorted.Length - 1);
			foreach (int i in unsorted)
				Console.WriteLine (i);

		}

		static void MergeSort (int[] unsorted, int head, int tail)
		{
			if (head < tail) {
				int middle = (head + tail) / 2;
				MergeSort (unsorted, head, middle);
				MergeSort (unsorted, middle + 1, tail);
				Merge (unsorted, head, middle, tail);
			}
		}

		static void Merge (int[] unsorted, int head, int middle, int tail)
		{
			int[] left = new int[middle - head + 2];
			int[] right = new int[tail - middle + 1];
			for (int i = 0; i < left.Length -1; i++)
				left [i] = unsorted [head + i];
			left [left.Length - 1] = int.MaxValue;
			for (int i = 0; i < right.Length - 1; i++)
				right [i] = unsorted [middle + i + 1];
			right [right.Length - 1] = int.MaxValue;
			for (int k = head, i =0,j=0; k <= tail; k++) {
				unsorted [k] = (left [i] <= right [j]) ? left [i++] : right [j++];
			}
		}
	}
}


