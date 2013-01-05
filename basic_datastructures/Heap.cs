using System;
using System.Collections.Generic;

public class Heap<T> : IEnumerable<T> where T : IComparable<T>
{
	private T[] _heap;
	private HeapStrategy _strategy = HeapStrategy.Minheap;
	private delegate bool Compare (T first, T second);

	private int _count = default(int);
	private int _capacity = 10;
	public delegate void IndexChangeEventHandler (Heap<T> sender, IndexChangeEventArgs<T> e);

	public event IndexChangeEventHandler IndexChanged;

	public int Count {
		get{ return _count;}
	}

	public HeapStrategy Strategy {
		get{ return _strategy;}
	}

	protected T[] HeapCollection {
		get{ return _heap;}
	}

	public T this [int index] {
		get {
			if (index < 0 || index > Count)
				throw new IndexOutOfRangeException ();
			return _heap [index];
		}
	}

	public Heap ():this(new List<T>(),HeapStrategy.Minheap)
	{
	}

	public Heap (HeapStrategy strategy):this(new List<T>(),strategy)
	{
	}

	public Heap (IList<T> collection):this(collection,HeapStrategy.Minheap)
	{
	}

	public Heap (IList<T> collection, HeapStrategy strategy)
	{
		_heap = new T[Math.Max (_capacity, collection.Count)];

		for (int i = 0; i < collection.Count; i++) {
			_heap [i] = collection [i];
			_count++;
		}
		_strategy = strategy;
		BuildHeap ();
	}

	public void Add (T item)
	{
		if (Count == _capacity) {
			_capacity = 2 * _capacity;
			Array.Resize (ref _heap, _capacity);
		}
		_heap [_count++] = item;
		CompactHeap ();
	}

	private void BuildHeap ()
	{
		int n = Count;
		for (int i = n/2; i >=0; i--) {
			if (_strategy == HeapStrategy.Maxheap)
				MaxHeapify (i);
			else
				MinHeapify (i);
		}
	}

	protected void MaxHeapify (int currentNodeIndex)
	{
		int leftChildNodeIndex = GetLeftChildIndex (currentNodeIndex);
		int rightChildNodeIndex = GetRightChildIndex (currentNodeIndex);
		int n = Count;
		int largestNodeIndex = currentNodeIndex;
		if (leftChildNodeIndex < n && _heap [currentNodeIndex].CompareTo (_heap [leftChildNodeIndex]) < 0)
			largestNodeIndex = leftChildNodeIndex;
		if (rightChildNodeIndex < n && _heap [largestNodeIndex].CompareTo (_heap [rightChildNodeIndex]) < 0)
			largestNodeIndex = rightChildNodeIndex;
		if (largestNodeIndex != currentNodeIndex) {
			Swap (currentNodeIndex, largestNodeIndex);
			MaxHeapify (largestNodeIndex);
		}
	}

	protected void MinHeapify (int currentNodeIndex)
	{
		int leftChildNodeIndex = GetLeftChildIndex (currentNodeIndex);
		int rightChildNodeIndex = GetRightChildIndex (currentNodeIndex);
		int n = Count;
		int smallesNodeIndex = currentNodeIndex;
		if (leftChildNodeIndex < n && _heap [currentNodeIndex].CompareTo (_heap [leftChildNodeIndex]) > 0)
			smallesNodeIndex = leftChildNodeIndex;
		if (rightChildNodeIndex < n && _heap [smallesNodeIndex].CompareTo (_heap [rightChildNodeIndex]) > 0)
			smallesNodeIndex = rightChildNodeIndex;
		if (smallesNodeIndex != currentNodeIndex) {
			Swap (currentNodeIndex, smallesNodeIndex);
			MinHeapify (smallesNodeIndex);
		}
	}

	private void CompactHeap ()
	{
		if (_strategy == HeapStrategy.Maxheap)
			CompactMaxHeap (Count - 1);
		else
			CompactMinHeap (Count - 1);

	}
	//Viewing heap in bottom-up order.
	protected void CompactMaxHeap (int index)
	{
		int currentNodeIndex = index;
		int parentNodeIndex = GetParentIndex (currentNodeIndex);
			
		while (currentNodeIndex > 0 && _heap[currentNodeIndex].CompareTo(_heap[parentNodeIndex]) > 0) {
			Swap (currentNodeIndex, parentNodeIndex);
			currentNodeIndex = parentNodeIndex;
			parentNodeIndex = GetParentIndex (currentNodeIndex);
		}
	}

	protected void CompactMinHeap (int index)
	{
		int currentNodeIndex = index;
		int parentNodeIndex = GetParentIndex (currentNodeIndex);
			
		while (currentNodeIndex > 0 && _heap[currentNodeIndex].CompareTo(_heap[parentNodeIndex]) < 0) {
			Swap (currentNodeIndex, parentNodeIndex);
			currentNodeIndex = parentNodeIndex;
			parentNodeIndex = GetParentIndex (currentNodeIndex);
		}
	}

	public bool Remove (int index)
	{
		if (index < 0 || index > Count - 1)
			return false;

		int n = Count;
		int lastIndex = n - 1;
		Swap (index, lastIndex);
		_count = _count - 1;
		if (_strategy == HeapStrategy.Maxheap)
			MaxHeapify (index);
		else
			MinHeapify (index);
		return true;
	}

	public bool Remove (T value)
	{
		int itemIndex = FindItem (value);
		if (itemIndex == -1)
			return false;
		return Remove (itemIndex);
	}

	public bool Contains (T item)
	{
		return FindItem (item) != -1 ? true : false;
	}

	private int FindItemSlow (T item)
	{
		for (int i = 0; i < Count; i++) {
			if (item.CompareTo (_heap [i]) == 0)
				return i;
		}
		return -1;
	}

	private int FindItem (T item)
	{
		int height = (int)Math.Log (Count, 2);
		int currentLevel = 0;
		int startNode = 0;

		while (currentLevel <= height) {
			int maxNodesAtLevel = (int)Math.Pow (2, currentLevel);
			int endNode = startNode + maxNodesAtLevel;
			int count = 0;
			while (startNode < Count && startNode < endNode) {
				if (item.CompareTo (_heap [startNode]) == 0)
					return startNode;
				else if (item.CompareTo (_heap [GetParentIndex (startNode)]) > 0 && item.CompareTo (_heap [startNode]) < 0)
					count++;
				startNode++;
			}
			if (count == maxNodesAtLevel) {
				return -1;
			}
			currentLevel++;
		}
		return -1;
	}

	private void Swap (int nodeIndex1, int nodeIndex2)
	{
		T temp = _heap [nodeIndex1];
		_heap [nodeIndex1] = _heap [nodeIndex2];
		_heap [nodeIndex2] = temp;
		OnIndexChanged (new IndexChangeEventArgs<T> (nodeIndex1, _heap [nodeIndex1], nodeIndex2, _heap [nodeIndex2]));
	}

	private void OnIndexChanged (IndexChangeEventArgs<T> e)
	{
		if (IndexChanged != null)
			IndexChanged (this, e);
	}

	private int GetParentIndex (int nodeIndex)
	{
		return (nodeIndex - 1) / 2;
	}

	private int GetLeftChildIndex (int nodeIndex)
	{
		return 2 * nodeIndex + 1;
	}

	private int GetRightChildIndex (int nodeIndex)
	{
		return 2 * (nodeIndex + 1);
	}

	public IEnumerator<T> GetEnumerator ()
	{
		for (int i = 0; i < Count; i++)
			yield return _heap [i];
	}

	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
	{
		return GetEnumerator ();
	}
}
