using System;
using System.Collections.Generic;

class Anagrams
{
	static void Main()
	{
		string line1 = Console.ReadLine();
		string line2 = Console.ReadLine();
		Dictionary<int,int> line1Count = new Dictionary<int,int>();
		Dictionary<int,int> line2Count = new Dictionary<int,int>();
		if(line1.Length != line2.Length){
			PrintStatus(false);
			return;
		}
		for(int i=0; i<line1.Length;i++)
		{
			int char1 = (int)line1[i];
			int char2 = (int)line2[i];
			if(line1Count.ContainsKey(char1))
				line1Count[char1] += 1;
			else
				line1Count.Add(char1,1);

			if(line2Count.ContainsKey(char2))
				line2Count[char2] += 1;
			else
				line2Count.Add(char2,1);
		}
		foreach(KeyValuePair<int,int> line1char in line1Count){
			if(!line2Count.ContainsKey(line1char.Key)){
				PrintStatus(false);
				return;
			}
			if(line2Count[line1char.Key] != line1char.Value){
				PrintStatus(false);
				return;
			}
		}
		PrintStatus(true);
	}



	private static void PrintStatus(bool status)
	{
		if(status)
			Console.WriteLine("Anagrams!");
		else
			Console.WriteLine("Not anagrams!");
	}
}
