using System;
using System.Collections.Generic;

namespace RandomMusings
{
	public class NumericalAlgorithms
	{
		static void Main(){
			/*Console.WriteLine(IsPrime(13));
			Console.WriteLine(IsPrime(7));
			Console.WriteLine(IsPrime(4));*/
			Console.WriteLine(ToBinary(742));
		}

		static bool IsPrime(int n){
			int srt = (int)(Math.Sqrt((double)n));
			bool isPrime = true;
			for(int i=2; i <= srt; i++){
				if(n % i == 0){
					isPrime = false;
					break;
				}
			}
			return isPrime;
		}

		static string ToBinary(int n){
			int r = 0;
			List<int> binary = new List<int>();

			while(n / 2 != 0){
				r = n%2;
				binary.Add(r);
				n = n / 2;
			}
			binary.Add(1);
			string sb = string.Empty;
			foreach(int b in binary){
				sb = b + sb;
			}
			return sb;

		}
	}
}

