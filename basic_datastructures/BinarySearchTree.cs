using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

using BinaryTree;

namespace BinarySearchTree
{
	public class BST<T> //: ICollection<T>
	{
		Node<T> _root;

		public BST ()
		{
		}

		public Comparer<T> Comparer {
			get{ return Comparer<T>.Default;}
		}

		public Node<T> Root {
			get{ return _root;}
		}

		public void Add (T item)
		{
			Node<T> newNode = new Node<T> ();
			newNode.Value = item;

			if (_root == null)
				_root = newNode;
			else {
				InsertNode (newNode);
			}
		}

		private void InsertNode (Node<T> newNode)
		{
			Node<T> iterNode = _root;

			while (true) {
				if (Comparer.Compare (newNode.Value, iterNode.Value) <= 0) { //less than or equal; go left
					if (iterNode.Left == null) {
						iterNode.Left = newNode;
						return;
					} else {
						iterNode = iterNode.Left;
						continue;
					}
				} else { //greater
					if (iterNode.Right == null) {
						iterNode.Right = newNode;
						return;
					} else {
						iterNode = iterNode.Right;
						continue;
					}
				}
			}
		}

		public void Traverse (Node<T> startNode, int level)
		{
			if (startNode == null)
				return;
			else {
				Console.WriteLine ("{0},{1}", level, startNode.Value);
				Traverse (startNode.Left, level + 1);
				Traverse (startNode.Right, level + 1);
			}
		}

		public bool Remove (T item)
		{
			Tuple<Node<T>,Node<T>> foundNodes = Find (_root, item); 
			Node<T> parent = foundNodes.Item1; 
			Node<T> node = foundNodes.Item2;

			if (node == null && parent == null)
				return false;
			else {
				if (node.Left == null && node.Right == null) {
					node = null;
					return true;
				} else if (node.Left != null && node.Right == null) {
					if (Comparer.Compare (node.Value, parent.Value) == 1)
						parent.Right = node.Left;
					else
						parent.Left = node.Left;
					return true;
				} else if (node.Right != null && node.Left == null) {
					if (Comparer.Compare (node.Value, parent.Value) == 1)
						parent.Right = node.Right;
					else
						parent.Left = node.Right;
					return true;
				} else {
					if (Comparer.Compare (node.Value, parent.Value) == 1) {
						Node<T> right = FindRightMost (node.Left); 
						right.Left = node.Left;
						right.Right = node.Right;
						parent.Right = right; 
					} else
						parent.Left = FindRightMost (node.Left);
					return true;
				}
			}
		}

		private Node<T> FindRightMost (Node<T> root)
		{
			if (root.Right == null)
				return root;
			else
				return FindRightMost (root.Right);
		}

		private Node<T> FindLeftMost (Node<T> root)
		{
			throw new NotImplementedException ();
		}

		private Tuple<Node<T>,Node<T>> Find (Node<T> tree, T item)
		{
			if (tree == null)
				return new Tuple<Node<T>, Node<T>> (null, null);
			else if (tree.Value.Equals (item))
				return new Tuple<Node<T>, Node<T>> (null, tree);
			else if (tree.Left.Value.Equals (item))
				return new Tuple<Node<T>, Node<T>> (tree, tree.Left);
			else if (tree.Right.Value.Equals (item))
				return new Tuple<Node<T>, Node<T>> (tree, tree.Right);
			else if (Comparer.Compare (item, tree.Value) < 0)
				return Find (tree.Left, item);
			else
				return Find (tree.Right, item);
		}

		public IEnumerable<T> PreTraverse (Node<T> node, ICollection<T> retList)
		{
			if (node != null) {
				retList.Add (node.Value);
				PreTraverse (node.Left, retList);
				PreTraverse (node.Right, retList);
			}
			return retList;
		}

		public IEnumerable<T> PostTraverse (Node<T> node, ICollection<T> retList)
		{
			if (node != null) {
				PostTraverse (node.Left, retList);
				PostTraverse (node.Right, retList);
				retList.Add (node.Value);
			}
			return retList;
		}

		public IEnumerable<T> InTraverse (Node<T> node, ICollection<T> retList)
		{
			if (node != null) {
				InTraverse (node.Left, retList);
				retList.Add (node.Value);
				InTraverse (node.Right, retList);
			}
			return retList;
		}

		public IEnumerable<T> BreadthFirst (Node<T> node, ICollection<T> retList)
		{
			Queue<Node<T>> successors = new Queue<Node<T>> ();
			while (node != null) {
				retList.Add (node.Value);
				if (node.Left != null)
					successors.Enqueue (node.Left);
				if (node.Right != null)
					successors.Enqueue (node.Right);
				if (successors.Count > 0)
					node = successors.Dequeue ();
				else
					node = null;
			}
			return retList;
		}

		public IEnumerable<T> DepthFirst (Node<T> node, ICollection<T> retList)
		{
			Stack<Node<T>> successors = new Stack<Node<T>> ();
			while (node != null) {
				retList.Add (node.Value);
				if (node.Left != null)
					successors.Push (node.Left);
				if (node.Right != null)
					successors.Push (node.Right);
				if (successors.Count > 0)
					node = successors.Pop ();
				else
					node = null;
			}
			return retList;
		}
	}
}
