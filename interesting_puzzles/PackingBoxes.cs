using System;
using System.Collections.Generic;

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
    FULL = true,
    EMPTY = false
};

class StateCell : Cell
{
    CellState _state;

    public CellState State
    {
        get { return _state; }
    }

    public StateCell(int length, int width, CellState state)
        : base(length, width)
    {
        _state = state;
    }
}

class StateCellNode : StateCell
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

    public StateCellNode(int length, int width, CellState initState)
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

    public bool IsStable
    {
        get { return _isStable; }
    }

    public DropBox(Tuple<int, int>[] boxes, int length, int width)
    {
        _length = length;
        _width = width;
        _isStable = true;
        _rootBox = new StateCell(_length, _width, CellState.EMPTY);
        // keep adding boxes until the dropbox(container) is stable.
        for (int i = 0; i < boxes.Length && _isStable; i++)
        {
            Add(box);
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
    private void Add(Tuple<int, int> box)
    {
        
        int length = box.Item1;
        int width = box.Item2;
        StateCellNode foundBox = FindEmptyBox(_rootBox, length, width);
        if (foundBox == null)
        {
            _isStable = false;
            return;
        }
        FillEmptyBox(foundBox, length, width);

    }

    private StateCellNode FindEmptyBox(StateCellNode root, int length, int width)
    {
        StateCellNode foundBox = null;
        if (root == null)
            return foundBox;
        if (root.State == CellState.EMPTY)
        {
            int cellLength = root.Length;
            int cellWidth = root.Width;
            if (Math.Max(cellLength, cellWidth) >= Math.Max(length, width) && Math.Min(cellLength, cellWidth) >= Math.Min(length, width))
                return root;
        }
        foundBox = FindEmptyBox(root.Top, length, width);
        if (foundBox == null)
            foundBox = FindEmptyBox(root.Right, length, width);
        return foundBox;
    }

    private bool FillEmptyBox(StateCellNode box, int length, int width)
    {
        //right cut 
        StateCellNode rightBox = null;
        int rightLeftOver = box.Width - width;
        box.Width = units;
        if (rightLeftOver > 0)
            rightBox = new StateCellNode(box.Length, rightLeftOver, CellState.Empty);
        box.Right = rightBox;

        //top cut
        StateCellNode topBox = null;
        int topLeftOver = box.Length - length;
        box.Length = length;
        if (topLeftOver > 0)
            topBox = new StateCellNode(topLeftOver,box.Width, leftOver, CellState.Empty);
        box.Top = topBox;

        box.State = CellState.Full;
    }

    public void Dump()
    {
        //Dump the state of the tree.
        //Future:change this to dump the state of box in stderr.
        throw new NotImplementedException();
    }
}

class Solution
{
    public static void Main()
    {
        //Cord[] boxes = {new Cord(8,8),new Cord(4,3), new Cord(4,3)};
        Tuple<int, int>[] boxes = { new Tuple<int, int>(8, 8), new Tuple<int, int>(4, 3), new Tuple<int, int>(4, 3) };
        int minLength = 8;
        int maxLength = 16;
        int minWidth = 8;
        int maxWidth = 14;
        int bestArea = 0;

        int dropBoxLength = minLength;
        int dropBoxWidth = maxWidth;
        int bestArea = maxWidth * maxLength;
        while (dropBoxLength <= maxLength && dropBoxWidth >= minWidth)
        {
            if (dropBoxLength * dropBoxWidth < bestArea)
            {
                DropBox dp = new DropBox(boxes, dropBoxLength, dropBoxWidth);
                if (dp.IsStable)
                {
                    dropBoxWidth--;
                    continue;
                }
                else
                {
                    dropBoxLength++;
                    continue;
                }
            }
            dropBoxWidth--;
        }
       
        Console.WriteLine("{0}", bestArea);
    }
}
