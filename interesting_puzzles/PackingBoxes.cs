using System;
using System.Collections.Generic;

struct Cord
{
	int _x;
	int _y;

	public int X{ get { return _x; } }

	public int Y{ get { return _y; } }

	public Cord (int x, int y)
	{
		_x = x;
		_y = y;
	}
}

class Cell
{
	int _length;
	int _width;
	Cord _refPoint;

	public Cord TopLeft{ get { return new Cord (_refPoint.X, _refPoint.X + _length); } }
	/*public Cord BottomLeft{get{return _refPoint.X}}
	public Cord TopRight{get;}
	public Cord BottomRight{get;}*/

	public int LeftBound{ get { return _refPoint.X; } }

	public int TopBound{ get { return _refPoint.X + _length; } }

	public Cell (int length, int width, Cord start)
	{
		_length = length;
		_width = width;
		_refPoint = start;
	}
}

enum CellState:bool
{
	OFF = true,
	ON = false
};

class StateCell:Cell
{
	CellState _state;

	public StateCell (int length, int width, Cord start, CellState initState):base(length,width,start)
	{
		_state = initState;
	}
}

class DropBox
{
	List<StateCell> boxes;
	int _length;
	int _width;
	bool _state;

	public bool IsStable
	{
		get{return _state;}
	}

	public DropBox(Tuple<int,int>[] boxes,int length,int width){
		boxes = new List<StateCell>();
		_length = length;
		_width = width;
		boxes.Add(new StateCell(length,width));
		_state = ComposeBox(boxes);
	}

	private bool ComposeBox(Tuple<int,int>[] boxes)
	{
		foreach(Tuple<int,int> box in boxes){
		}
	}
}

class Solution
{
	public static void Main(){
		//Cord[] boxes = {new Cord(8,8),new Cord(4,3), new Cord(4,3)};
		Tuple<int,int>[] boxes = {new Tuple<int,int>(8,8),new Tuple<int, int>(4,3),new Tuple<int,int>(4,3)};
		int minLength = 8;
		int maxLength = 16;
		int minWidth = 8;
		int maxWidth = 14;
		int bestArea = 0;
		for(int l=minLength; l <= maxLength; l++)
		{
			for(int w=minWidth;w <= maxWidth; w++)
			{
				DropBox dp = new DropBox(boxes,l,w);
				if(dp.IsStable){
					bestArea = Math.Min(bestArea,l*w);
					if(l == minLength)
						maxWidth = w;
					else
						maxLength = l;
				}
			}
		}
		Console.WriteLine("{0}",bestArea);
	}
}