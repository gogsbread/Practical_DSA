using System;

namespace Sorting
{
	public class InsertionSorted
	{
		static void Main(){
			int[] unsorted = {92,23,44,221,334,4212,-32,1,-34435687,34};
			InsertionSort(unsorted);
			foreach (int i in unsorted)
				Console.WriteLine (i);
		}

		public static void InsertionSort(int[] unsorted){
			for(int j=1;j<unsorted.Length;j++){
				int hold = unsorted[j];
				int i=j-1;
				while(i>=0 && unsorted[i]>hold){
					unsorted[i+1] = unsorted[i];
					i = i-1;
				}
				unsorted[i+1] = hold;
			}
		}
	}
}
