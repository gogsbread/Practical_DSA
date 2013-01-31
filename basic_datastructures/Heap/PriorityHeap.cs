using System;
using System.Collections.Generic;

public class PriorityHeap<T>:Heap<T> where T:IComparable<T>
{
	public PriorityHeap ():base()
	{
	}
		
	public PriorityHeap (HeapStrategy strategy):base(strategy)
	{
	}
	
	public PriorityHeap (IList<T> collection):this(collection)
	{
	}

	public PriorityHeap (IList<T> collection, HeapStrategy strategy):base(collection,strategy)
	{
	}

	public void IncreaseKey (int index, T newKey)
	{
		if (newKey.CompareTo (base.HeapCollection [index]) < 0)
			throw new InvalidOperationException ("New key cannot be lesser than the older key");
		base.HeapCollection [index] = newKey;

		if (base.Strategy == HeapStrategy.Minheap)
			base.MinHeapify (index);
		else
			base.CompactMaxHeap (index);
	}

	public void DecreaseKey (int index, T newKey)
	{
		if (newKey.CompareTo (base.HeapCollection [index]) > 0)
			throw new InvalidOperationException ("New key cannot be greater than the older key");
		base.HeapCollection [index] = newKey;

		if (base.Strategy == HeapStrategy.Minheap)
			base.CompactMinHeap (index);
		else
			base.MaxHeapify (index);
	}

	public T ExtractTop ()
	{
		T top = HeapCollection [0];
		base.Remove (0);
		return top;
	}
}
