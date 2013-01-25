using System;
using System.Collections.Generic;

namespace Graph
{
	public class WeightedGraphNode<T> : GraphNode<T> where T:IComparable
	{
		private List<int> _weights;

		public WeightedGraphNode() : this(default(T)) { }
		public WeightedGraphNode(T value) : this(value, new List<WeightedGraphNode<T>>()) { }
		public WeightedGraphNode(T value, IEnumerable<WeightedGraphNode<T>> neighbors) : base(value, neighbors) { _weights = new List<int>(); }

		public IList<int> Costs { get { return _weights; } }

		public override bool Equals(object obj)
		{
			WeightedGraphNode<T> otherNode = obj as WeightedGraphNode<T>;
			if (otherNode == null || !otherNode.Costs.Count.Equals(this.Costs.Count))
				return false;
			foreach(int cost in otherNode.Costs)
				if(!this.Costs.Contain(cost))
					return false;
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			int hash = base.GetHashCode();
			foreach (int cost in this.Costs)
				hash += cost;
			return hash;
		}
	}
}
