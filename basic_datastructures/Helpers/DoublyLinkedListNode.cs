[Serializable()]
public class DoublyLinkedListNode<T>
{
	T _value;
	DoublyLinkedListNode<T> _prev;
	DoublyLinkedListNode<T> _next;

	public DoublyLinkedListNode ()
	{
	}

	public T Value {
		get{ return _value;}
		set{ _value = value;}
	}

	public DoublyLinkedListNode (T val, DoublyLinkedListNode<T> prev, DoublyLinkedListNode<T> next)
	{
		_prev = prev;
		_next = next;
		_value = val;
	}

	public DoublyLinkedListNode<T> Previous {
		get{ return _prev;}
		set{ _prev = value;}
	}

	public DoublyLinkedListNode<T> Next {
		get{ return _next;}
		set{ _next = value;}
	}
	}
