using System;
using System.Collections.Generic;

public struct HeapValue:IComparable<HeapValue>
{
	int _heapKey;
	string _dictionaryKey;

	public int HeapKey {
		get{ return _heapKey;}
	}

	public string CacheKey {
		get{ return _dictionaryKey;}
	}

	public HeapValue (int heapKey, string dictionaryKey)
	{
		_heapKey = heapKey;
		_dictionaryKey = dictionaryKey;
	}

	public int CompareTo (HeapValue other)
	{
		if (other.Equals (null))
			return -1;

		if (this.HeapKey < other.HeapKey)
			return -1;
		else if (this.HeapKey > other.HeapKey)
			return 1;
		else
			return 0;
	}
}

public struct CacheValue
{
	int _heapIndex;
	string _keyValue;

	public int HeapIndex {
		get {
			return _heapIndex;
		}
		set{ _heapIndex = value;}
	}

	public string Value {
		get{ return _keyValue;}
	}

	public CacheValue (int heapPointer, string dictionaryValue)
	{
		_heapIndex = heapPointer;
		_keyValue = dictionaryValue;
	}
}

public class LRUCache 
{
	Dictionary<string,CacheValue> _cache;
	PriorityHeap<HeapValue> _minHeap;
	int _count;
	int _bound;
	int _heapKey;

	public LRUCache()
	{
		_cache = new Dictionary<string, CacheValue> ();
		_minHeap = new PriorityHeap<HeapValue> (HeapStrategy.Minheap);
		_count = default(int);
		_bound = default(int);
		_heapKey = default(int);
		_minHeap.IndexChanged += HandleIndexChanged;
		;
	}

	private void HandleIndexChanged (Heap<HeapValue> sender, IndexChangeEventArgs<HeapValue> e)
	{
		int index1 = e.Change1.Item1;
		string cacheKey1 = e.Change1.Item2.CacheKey;
		CacheValue newCacheValue1 = new CacheValue (index1, _cache [cacheKey1].Value);
		_cache [cacheKey1] = newCacheValue1;

		int index2 = e.Change2.Item1;
		string cacheKey2 = e.Change2.Item2.CacheKey;
		CacheValue newCacheValue2 = new CacheValue (index2, _cache [cacheKey2].Value);
		_cache [cacheKey2] = newCacheValue2;
	}

	public void Bound (int value)
	{
		_bound = value;
		CompactCache ();
	}

	private void CompactCache ()
	{
		while (_bound < _count) {
			string heapKey = _minHeap [0].CacheKey;
			_minHeap.Remove (0);
			_cache.Remove (heapKey);
			_count--;
		}
	}

	public void Set (string key, string value)
	{
		if (_cache.ContainsKey (key)) {
			CacheValue oldCacheValue = _cache [key];
			CacheValue newCacheValue = new CacheValue (oldCacheValue.HeapIndex, value);
			_cache [key] = newCacheValue;
			HeapValue heapValue = new HeapValue (_heapKey++, key);
			_minHeap.IncreaseKey (oldCacheValue.HeapIndex, heapValue);
			
		} else {
			CacheValue cacheValue = new CacheValue (_count++, value);
			HeapValue heapValue = new HeapValue (_heapKey++, key);
			_cache.Add (key, cacheValue);
			_minHeap.Add (heapValue);
			CompactCache ();
		}
	}

	public void Get (string key)
	{
		if (_cache.ContainsKey (key)) {
			string value = _cache [key].Value;
			int heapIndex = _cache [key].HeapIndex;
			HeapValue heapValue = new HeapValue (_heapKey++, key);
			_minHeap.IncreaseKey (heapIndex, heapValue);
			Console.WriteLine ("{0}", value);
			return;
		}
		Console.WriteLine ("{0}", "NULL");
	}

	public void Peek (string key)
	{
		if (_cache.ContainsKey (key)) {
			Console.WriteLine ("{0}", _cache [key].Value);
			return;
		}
		Console.WriteLine ("{0}", "NULL");
	}

	public void Dump (bool debug = false)
	{
		List<string> unsorted = new List<string> (_count);
		for (int i = 0; i < _count; i++)
			unsorted.Add (_minHeap [i].CacheKey);
		unsorted.Sort (StringComparer.Ordinal);
		foreach (string s in unsorted)
			Console.WriteLine ("{0} {1}", s, _cache [s].Value);
		if (debug) {
			Console.WriteLine ("---Debug----");
			Debug ();
		}
	}

	private void Debug ()
	{
		Console.WriteLine ("Count:{0}Bound:{1}HeapKey:{2}", _count, _bound, _heapKey);
		Console.WriteLine ("--Cache Dump--");
		foreach (KeyValuePair<string,CacheValue> cacheItem in _cache)
			Console.WriteLine ("{0}:({1},{2})", cacheItem.Key, cacheItem.Value.HeapIndex, cacheItem.Value.Value);
		Console.WriteLine ("--Heap Dump--");
		for (int i = 0; i < _minHeap.Count; i++)
			Console.WriteLine ("{0}:({1},{2})", i, _minHeap [i].CacheKey, _minHeap [i].HeapKey);
	}

	public static void SelfTest()
	{
		LRUCache cache = new LRUCache();
		int n = int.Parse (Console.ReadLine ().Trim ());
		if (n == default(int))
			throw new Exception ("something wrong with input");
		for (int i =0; i < n; i++) {
			string line = Console.ReadLine ().Trim ();
			string[] command = line.Split (' ');
			switch (command [0]) {
			case "BOUND":
				cache.Bound (int.Parse (command [1].Trim ()));
				break;
			case "SET":
				cache.Set (command [1].Trim (), command [2].Trim ());
				break;
			case "GET":
				cache.Get (command [1].Trim ());
				break;
			case "PEEK":
				cache.Peek (command [1].Trim ());
				break;
			case "DUMP":
				cache.Dump ();
				break;
			default:
				throw new Exception ("Command not supported");
				
			}
		}
	}
}
