using System;
using System.Collections.Generic;
using System.IO;

class Cell
{
	int _length;
	int _width;

	public int Length { get { return _length; } set { _length = value; } }

	public int Width { get { return _width; } set { _width = value; } }

	public Cell (int length, int width)
	{
		_length = length;
		_width = width;
	}
}

enum CellState:int
{
	FULL = 1,
	EMPTY =0 
}
;

class StateCell : Cell
{
	CellState _state;

	public CellState State {
		get { return _state; }
		set{ _state = value;}
	}

	public StateCell (int length, int width, CellState state)
        : base(length, width)
	{
		_state = state;
	}
}

class StateCellNode : StateCell
{
	StateCellNode _top;
	StateCellNode _right;

	public StateCellNode Top {
		get { return _top; }
		set { _top = value; }
	}

	public StateCellNode Right {
		get { return _right; }
		set { _right = value; }
	}

	public StateCellNode (int length, int width, CellState initState)
        : base(length, width, initState)
	{
	}
}

class DropBox
{
	StateCellNode _rootBox;
	int _length;
	int _width;
	bool _isStable;

	public bool IsStable {
		get { return _isStable; }
	}

	public DropBox (Tuple<int, int>[] boxes, int length, int width)
	{
		_length = length;
		_width = width;
		_isStable = true;
		_rootBox = new StateCellNode (_length, _width, CellState.EMPTY);
		// keep adding boxes until the dropbox(container) is stable.
		for (int i = 0; i < boxes.Length && _isStable; i++) {
			Add (boxes [i]);
		}
	}

	//check if the box can be fit?
	//check if root is unfilled.
	//yes
	//split the node
	//fill it
	//no
	//check top(recurse)
	//check bottom(recurse)
	private void Add (Tuple<int, int> box)
	{
        
		int length = box.Item1;
		int width = box.Item2;
		StateCellNode foundBox = FindEmptyBox (_rootBox, length, width);
		if (foundBox == null) {
			_isStable = false;
			return;
		}
		FillEmptyBox (foundBox, length, width);

	}

	private StateCellNode FindEmptyBox (StateCellNode root, int length, int width)
	{
		StateCellNode foundBox = null;
		if (root == null)
			return foundBox;
		if (root.State == CellState.EMPTY) {
			int cellLength = root.Length;
			int cellWidth = root.Width;
			if (Math.Max (cellLength, cellWidth) >= Math.Max (length, width) && Math.Min (cellLength, cellWidth) >= Math.Min (length, width))
				return root;
		}
		foundBox = FindEmptyBox (root.Top, length, width);
		if (foundBox == null)
			foundBox = FindEmptyBox (root.Right, length, width);
		return foundBox;
	}

	private void FillEmptyBox (StateCellNode box, int length, int width)
	{
		//right cut 
		StateCellNode rightBox = null;
		if (box.Length < length || box.Width < width) {
			int temp = length;
			length = width;
			width = temp;
		}
		int rightLeftOver = box.Width - width;
		box.Width = width;
		if (rightLeftOver > 0)
			rightBox = new StateCellNode (box.Length, rightLeftOver, CellState.EMPTY);
		box.Right = rightBox;

		//top cut
		StateCellNode topBox = null;
		int topLeftOver = box.Length - length;
		box.Length = length;
		if (topLeftOver > 0)
			topBox = new StateCellNode (topLeftOver, box.Width, CellState.EMPTY);
		box.Top = topBox;

		box.State = CellState.FULL;
	}

	public void Dump ()
	{
		//Dump the state of the tree.
		//Future:change this to dump the state of box in stderr.
		InOrderDump(_rootBox,0);
	}

	private void InOrderDump (StateCellNode root,int level)
	{
		if (root == null)
			return;
		Console.WriteLine ("{2}:({0},{1})", root.Length, root.Width,level);
		InOrderDump(root.Top,level+1);
		InOrderDump(root.Right,level+1);
	}
}

class Solution
{
	public static void Main ()
	{
		//Cord[] boxes = {new Cord(8,8),new Cord(4,3), new Cord(4,3)};
		//test case 1
		/*Tuple<int, int>[] boxes = {
			new Tuple<int, int> (8, 8),
			new Tuple<int, int> (4, 3),
			new Tuple<int, int> (4, 3)
		}*/
		//test case 2. best : 100 ; algo: 105
		/*Tuple<int, int>[] boxes = {
			new Tuple<int, int> (10, 5),
			new Tuple<int, int> (5, 2),
			new Tuple<int, int> (5, 2),
			new Tuple<int, int> (5, 2),
			new Tuple<int, int> (5, 2),
			new Tuple<int, int> (5, 2)
		};*/


		int minLength = int.MaxValue;
		int maxLength = 0;
		int minWidth = int.MaxValue;
		int maxWidth = 0;
		bool DEBUG = false;
		FileStream inputStream = new FileStream("packing_boxes_sample_input.txt",FileMode.Open,FileAccess.Read);
		StreamReader sr = new StreamReader(inputStream);
		//int n = int.Parse(sr.ReadLine());
		int n = int.Parse(Console.ReadLine());
		Tuple<int, int>[] boxes = new Tuple<int, int>[n]; 
		for(int i=0; i< n ; i++)
		{
			//string[] inputs = sr.ReadLine().Split(' ');
			string[] inputs = Console.ReadLine().Split(' ');
			int templength = int.Parse(inputs[0]);
			int tempwidth = int.Parse(inputs[1]);
			int length = Math.Max(templength,tempwidth);
			int width = Math.Min(templength,tempwidth);
			boxes[i] = new Tuple<int, int>(length,width);
			maxLength += length;
			maxWidth += width;
			minLength = Math.Min(minLength,length);
			minWidth = Math.Min(minWidth,width);
		}

		int dropBoxLength = minLength;
		int dropBoxWidth = maxWidth;
		int bestArea = maxWidth * maxLength;
		int bestWidth = maxWidth;
		int bestLength = maxLength;
		while (dropBoxLength <= maxLength && dropBoxWidth >= minWidth) {
			if (dropBoxLength * dropBoxWidth < bestArea) {
				DropBox dp = new DropBox (boxes, dropBoxLength, dropBoxWidth);
				if (dp.IsStable) {
					bestArea = dropBoxLength * dropBoxWidth;
					bestWidth = dropBoxWidth;
					bestLength = dropBoxLength;
					if(DEBUG){
						Console.WriteLine("--DEBUG--");
						Console.WriteLine("({0},{1})",dropBoxLength,dropBoxWidth);
						dp.Dump();
					}
					dropBoxWidth--;
					continue;
				} else {
					dropBoxLength++;
					continue;
				}
			}
			dropBoxWidth--;
		}
       
		//Console.WriteLine ("({1},{2}),{0}", bestArea, bestLength, bestWidth);
		Console.WriteLine ("{0}", bestArea);
	}
}
