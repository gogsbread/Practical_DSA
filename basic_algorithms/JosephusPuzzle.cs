using System;
using System.Collections.Generic;

public class Josephus
{
	//table of N people.
	// every Kth person has to leave.
	// who is the last to leave.
	int _k = default(int);
	List<int> _persons = null;

	public Josephus (int k, int n)
	{
		_persons = new List<int> (n);
		for (int i = 0; i < n; i++) {
			_persons.Add (i + 1);
		}
		_k = k;
	}

	public int GetSurvivor ()
	{
		int count = -1;
		while (_persons.Count > 1) {
			for (int i = 0; i < _k; i++) {
				count = ++count % _persons.Count;
			}
			_persons.RemoveAt (count); 
			count--;
		}
		return _persons [0];
	}

	void RemoveK ()
	{

		int count = -1;
		for (int i = 0; i < _k; i++) {
			count = ++count % _persons.Count;
		}
		_persons.RemoveAt (count); 
	}
}

public class Client
{
	static void Main ()
	{
		Josephus j = new Josephus (3, 8);
		Console.WriteLine (j.GetSurvivor ());
		j = new Josephus (2323, 344);
		Console.WriteLine (j.GetSurvivor ());
	}
}


