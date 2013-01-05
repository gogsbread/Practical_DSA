using System;
using System.Collections.Generic;

public class IndexChangeEventArgs<T>
{
	Tuple<int,T> _change1;
	Tuple<int,T> _change2;

	public IndexChangeEventArgs (int index1, T value1, int index2, T value2)
	{
		_change1 = new Tuple<int, T> (index1, value1);
		_change2 = new Tuple<int, T> (index2, value2);
	}

	public Tuple<int,T> Change1 {
		get{ return _change1;}
	}

	public Tuple<int,T> Change2 {
		get{ return _change2;}
	}
}
