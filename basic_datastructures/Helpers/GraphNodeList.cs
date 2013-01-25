using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Graph
{
	public class GraphNodeList<TGraph,TGraphNode> : Collection<TGraph> where TGraph:GraphNode<TGraphNode> where TGraphNode:IComparable
	{
		public GraphNodeList()
			: this(new List<TGraph>())
		{
		}

		public GraphNodeList(IEnumerable<TGraph> nodeList)
		{
			IEnumerator<TGraph> iterator = nodeList.GetEnumerator();
			while (iterator.MoveNext())
			{
				Add(iterator.Current);
			}
		}
		//TODO:The item added should be checked if it is present as part of another collection. This is helpful because we are directly adding a element whose reference is also present elsewhere.
		public new void Add(TGraph item)
		{
			base.Add(item);
		}

		public GraphNode<TGraphNode> FindByValue(TGraphNode value)
		{
			IEnumerator<TGraph> nodeEnumerator = base.GetEnumerator();
			while (nodeEnumerator.MoveNext())
			{
				GraphNode<TGraphNode> current = nodeEnumerator.Current;
				if (current.Value.Equals(value))
					return current;
			}
			return null;
		}
	}
}
