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

    public int Length { get { return _length; } }

    public int Width { get { return _width; } }

	public Cell (int length, int width, Cord start)
	{
		_length = length;
		_width = width;
		_refPoint = start;
	}
}

/*class Cell
{
    int _length;
    int _width;
    Cord _refPoint;

    public Cord TopLeft { get { return new Cord(_refPoint.X, _refPoint.X + _length); } }
    /*public Cord BottomLeft{get{return _refPoint.X}}
    public Cord TopRight{get;}
    public Cord BottomRight{get;}

    public int LeftBound { get { return _refPoint.X; } }

    public int TopBound { get { return _refPoint.X + _length; } }

    public int Length { get { return _length; } }

    public int Width { get { return _width; } }

    public Cell(int length, int width, Cord start)
    {
        _length = length;
        _width = width;
        _refPoint = start;
    }
}*/

class Cell
{
    int _length;
    int _width;

    public int Length { get { return _length; } set { return _length; } }
    public int Width { get { return _width; } set { return _width; } }
    public Cell(int length, int width)
    {
        _length = length;
        _width = width;
    }
}

enum CellState
{
	OFF = true,
	ON = false
};

class StateCell:Cell
{
	CellState _state;

    public CellState State
    {
        get { return _state; }
    }

	public StateCell (int length, int width, CellState state):base(length,width)
	{
        _state = state;
	}
}

class StateCellNode: StateCell
{
   StateCell _top;
   StateCell _right;

    public StateCell Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public StateCell Right
    {
        get { return _right; }
        set { _right = value; }
    }

    public StateCellNode(int length, int width,CellState initState)
        : base(length, width, initState)
    {
    }
}

/*class GrowingBlob
{
    StateCellNode _root;

    public StateCellNode Root
    {
        get {return _root;}
    }

    public GrowingBlob(int length, int width)
    {
        _root = new StateCellNode(length, width, new Cord(0, 0), CellState.OFF);
    }

    public bool Add(int length, int width)
    {
        //if root is unfilled . fill it.
        // if root is filled. go right until you find a cell that is OFF.
        // check if the cell can hold
        if(_root.State == CellState.OFF)

    }

    private void SplitNode(StateCellNode node,int top, int right)
    {
        
    }
}*/

class DropBox
{
	StateCell _rootBox;
	int _length;
	int _width;
	bool _isStable;

	public bool IsStable
	{
		get{return _isStable;}
	}

	public DropBox(Tuple<int,int>[] boxes,int length,int width){
		boxes = new List<StateCell>();
		_length = length;
		_width = width;
        _isStable = true;
        _rootBox = new StateCell(_length, _width, CellState.ON);
        // keep adding boxes until the dropbox(container) is stable.
        for(int i = 0; i < boxes.Length && _isStable; i++)
        {
            Add(box);
        }
	}

    private void Add(Tuple<int, int> box)
    {
        //check if the box can be fit?
        // if it can be fit 
        if(_rootBox == null)
            _rootBox = new 
    }

    private StateCellNode FindEmptyBox(int length,int width)
    {
    }

    private void SplitBox()
    {
    }

	private bool ComposeBox(Tuple<int,int>[] boxes)
	{
		foreach(Tuple<int,int> box in boxes){
		}
	}
}

abstract class  GrowthStrategy
{

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
