using System;
using System.Collections.Generic;

namespace Graph
{
	public class GraphNode<T> where T : IComparable
	{
		private T _value;
		private GraphNodeList<GraphNode<T>,T> _neighbors;

		public GraphNode()
			: this(default(T))
		{
		}

		public GraphNode(T value)
			: this(value, new List<GraphNode<T>>())
		{
		}

		public GraphNode(T value, IEnumerable<GraphNode<T>> neighbors)
		{
			_value = value;
			_neighbors = new GraphNodeList<GraphNode<T>,T>(neighbors);
		}

		public GraphNodeList<GraphNode<T>,T> Neighbors
		{
			get { return _neighbors; }
		}

		public T Value
		{
			get { return _value; }
		}

		public override bool Equals(object obj)
		{
			GraphNode<T> otherNode = obj as GraphNode<T>;
			if (otherNode == null)
				return false;
			if (object.ReferenceEquals(otherNode, this))
				return true;
			if (otherNode.Value.Equals(this.Value) && (otherNode.Neighbors.Count == this.Neighbors.Count))
			{
				HashSet<T> valueSet = new HashSet<T>();
				foreach(GraphNode<T> node in this.Neighbors)
					valueSet.Add(node.Value);
				foreach(GraphNode<T> node in otherNode.Neighbors)
				{
					if(!valueSet.Contains(node.Value))
						return false;
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hashcode = 0;
			foreach (char c in this.Value.ToString())
				hashcode += (int)c;
			foreach(GraphNode<T> node in this.Neighbors)
				foreach(char ch in node.Value.ToString())
					hashcode += (int)ch;
			return hashcode;
		}
	}
}
