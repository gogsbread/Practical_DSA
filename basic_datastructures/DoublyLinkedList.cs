[Serializable()]
public class DoublyLinkedList<T> : ICollection<T>
{
	DoublyLinkedListNode<T> _head = null;
	DoublyLinkedListNode<T> _tail = null;
	int _count = 0;

	public DoublyLinkedListNode<T> Head {
		get{ return _head;}
	}

	public DoublyLinkedListNode<T> Tail {
		get{ return _tail;}
	}
		#region ICollection implementation

	public void Clear ()
	{
		throw new NotImplementedException ();
	}

	public bool Contains (T item)
	{
		IEnumerator<T> listIter = GetEnumerator ();
		while (listIter.MoveNext()) {
			if (listIter.Current.Equals (item))
				return true;
		}
		return false;
	}

	public void CopyTo (T[] array, int arrayIndex)
	{
		throw new NotImplementedException ();
	}

	public bool Remove (T item)
	{
		DoublyLinkedListNode<T> iteratorNode = _head;
		bool found = false;
		while (iteratorNode != null) {
			if (iteratorNode.Value.Equals (item)) {
				iteratorNode.Previous.Next = iteratorNode.Next;
				if (iteratorNode.Next != null)
					iteratorNode.Next.Previous = iteratorNode.Previous;
				found = true;
				break;
			}
			iteratorNode = iteratorNode.Next;
		}
		return found;
	}

	public int Count {
		get{ return _count;}
	}

	public bool IsReadOnly {
		get{ return false;}
	}

		#endregion

		#region IEnumerable implementation

	public IEnumerator<T> GetEnumerator ()
	{
		DoublyLinkedListNode<T> iteratorNode = _head;
		while (iteratorNode != null) {
			yield return iteratorNode.Value;
			iteratorNode = iteratorNode.Next;
		}
	}

		#endregion

		#region IEnumerable implementation

	IEnumerator IEnumerable.GetEnumerator ()
	{
		return GetEnumerator ();
	}

		#endregion

	public void Add (T item)
	{
		AddLast (item);
	}

	public void AddLast (T item)
	{
		DoublyLinkedListNode<T> node = new DoublyLinkedListNode<T> ();
		node.Value = item;
		if (_head == null) {
			_head = node;
			_tail = node;
		} else {
			_tail.Next = node;
			node.Previous = _tail;
			_tail = node;
		}
	}

	public void AddAfter (DoublyLinkedListNode<T> node, T value)
	{
		//if node == head then insert
		//node.next = head.next
		//head.next = node
		//node.prev = head
		//scan for node values
		//if node == tail insert
		//change node.addLast()
		//call node.addLast()
		// if node inbetween insert and change
		//node

		if (node == _head) {
			AddLast (value);
			return;
		}
		DoublyLinkedListNode<T> newNode = new DoublyLinkedListNode<T> ();
		newNode.Value = value;
		newNode.Next = node.Next;
		newNode.Previous = node;
		node.Next.Previous = newNode;
		node.Next = newNode;
	}
	}
