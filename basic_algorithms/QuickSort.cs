using System;

namespace Sorting 
{
	public class QuickSorted
	{
		static void Main ()
		{
			int[] unsorted = {92,23,44,221,334,4212,-32,1,-34435687,34};
			QuickSort(unsorted);
			foreach (int i in unsorted)
				Console.WriteLine (i);
		}

		public static void QuickSort(int[] unsorted){
			QuickSortInternal(unsorted, 0, unsorted.Length - 1);
		}

		static void QuickSortInternal(int[] unsorted, int head, int tail)
		{
			if (head < tail) {
				int middle = Partition (unsorted, head, tail);
				QuickSortInternal (unsorted, head, middle - 1);
				QuickSortInternal (unsorted, middle + 1, tail);
			}
		}

		static int Partition (int[] unsorted, int head, int tail)
		{
			MedianTail(unsorted,head,tail);
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

		//Pick the head,middle and tail elements and determine the median. Then shift the median to the right.
		static void MedianTail(int[] unsorted,int head,int tail)
		{
			int middle = (head+tail)/2;

			if(unsorted[head] > unsorted[middle])
				Swap(unsorted,head,middle);
			if(unsorted[head] > unsorted[tail])
				Swap(unsorted,head,tail);
			if(unsorted[middle] > unsorted[tail])
				Swap(unsorted,middle,tail);

			Swap(unsorted,tail,middle);
		}

		static void Swap (int[] array, int i, int j)
		{
			int temp = array [j];
			array [j] = array [i];
			array [i] = temp;
		}
	}
}


