using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTree
{
	public class Node<T>
	{

		T _value;
		Node<T> _left;
		Node<T> _right;

		public T Value {
			get{ return _value;}
			set{ _value = value;}
		}

		public Node<T> Left {
			get{ return _left;}
			set{ _left = value;}
		}

		public Node<T> Right {
			get{ return _right;}
			set{ _right = value;}
		}

		public Node (T item)
		{
			_value = item;
		}

		public Node ()
		{
			_value = default(T);
			_left = null;
			_right = null;
		}
	}
}
