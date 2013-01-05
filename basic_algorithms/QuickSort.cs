using System;

namespace RandomMusings
{
	public class QuickSorted
	{
		static void Main ()
		{
			int[] unsorted = {92,23,44,221,334,4212,-32,1,-34435687,34};
			QuickSort (unsorted, 0, unsorted.Length - 1);
			foreach (int i in unsorted)
				Console.WriteLine (i);
		}

		static void QuickSort (int[] unsorted, int head, int tail)
		{
			if (head < tail) {
				int middle = Partition (unsorted, head, tail);
				QuickSort (unsorted, head, middle - 1);
				QuickSort (unsorted, middle + 1, tail);
			}
		}

		static int Partition (int[] unsorted, int head, int tail)
		{
			int pivot = unsorted [tail];
			int division = head;
			for (int i = head; i < tail; i ++) {
				if (unsorted [i] <= pivot) {
					Swap (unsorted, division, i);
					division++;
				}
			}
			Swap (unsorted, tail, division);
			return division;
		}

		static void Swap (int[] array, int i, int j)
		{
			int temp = array [j];
			array [j] = array [i];
			array [i] = temp;
		}
	}
}


