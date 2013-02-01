using System;

class Anagrams
{
	static void Main()
	{
		string line1 = Console.ReadLine();
		string line2 = Console.ReadLine();
		if(line1.Length != line2.Length){
			PrintStatus(false);
			return;
		}
		string alphabetizedLine1 = Alphabetize(line1);
		string alphabetizedLine2 = Alphabetize(line2);
		if(alphabetizedLine1.Equals(alphabetizedLine2))
			PrintStatus(true);
		else
			PrintStatus(false);
	}

	private static string Alphabetize(string line){
		char[] chars = line.ToCharArray();
		Array.Sort(chars,StringComparer.OrdinalIgnoreCase);
		return new String(chars);
	}

	private static void PrintStatus(bool status)
	{
		if(status)
			Console.WriteLine("Anagrams!");
		else
			Console.WriteLine("Not anagrams!");
	}
}
