using System;
using System.Collections.Generic;
using System.IO;

/// <Summary>
/// Objective:
///     To find the best way to pack all files into a dropbox such that the dropbox size is minimum.
/// ThoughProcess:
///     This problem is structurally similar to 2D bin packing problem which is NP hard.
///     The given algorithm is along the lines of best fit. It is optimized for most of the test cases.
/// Elements:
///     File : Is the basic structure that holds length and width.
///     Dropbox : is the container box whose best dimensions should be determined.
/// Design process:
///         1) The algorithm standardizes the input so that length is the max of the given dimension and width is the min of the given dimensions.
///         2) This partially ordered input set is then sorted in decreasing order of length to impose total order.
///         3) We always try to fill from bottom to top and from left to right in that order.
///         4) The algorithm takes rotation into consideration.
///         5) The heuristics :
///                 a) start with the additve width Sum(all widths) and maximum height. We know that this will hold all constituent files.
///                 b) Now reduce the width by 1 and test if the dropbox is still stable.
///                 c) keep decreasing the width until the dropbox become unstable at which state you increase the height by 1
///                 d) We keep performing this heuristics until we reach a length which is Sum(all lengths) and width = max of all width.
///                 e) we also track the best dimension(l*w) so as to not iterate with dimensions that are larger in area.
///         6) I used a binary tree to hold the filled and empty spaces.The empty cells wil be at the leaf nodes and I am yet to optimize the tree search for this known condition. I am also in dearth of any better datastructures as are some limitations with binary tree that I can foresee.
///         7) The binary tree uses cell splitting to store filled and unfilled files, the operation of which demands a blog post. 
/// </Summary>

enum FileState : int
{
    FULL = 1,
    EMPTY = 0
}

/// <Summary>
/// Datastructure to hold the file type.
///     Getters and Setters are public because the file size should be altered when the holding box resizes.
/// </Summary>
class File
{
    int _length;
    int _width;

    public int Length { get { return _length; } set { _length = value; } }
    public int Width { get { return _width; } set { _width = value; } }

    public File(int length, int width)
    {
        _length = length;
        _width = width;
    }
}

/// <Summary>
/// Extends File ADT to add State(FULL,EMPTY) property.
/// </Summary>
class StatedFile : File
{
    FileState _state;

    public FileState State
    {
        get { return _state; }
        set { _state = value; }
    }

    public StatedFile(int length, int width, FileState state)
        : base(length, width)
    {
        _state = state;
    }
}

/// <Summary>
/// Acts as a binary node that holds 2 children(Top & Left)
/// </Summary>
class FileNode : StatedFile
{
    FileNode _top;
    FileNode _right;

    public FileNode Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public FileNode Right
    {
        get { return _right; }
        set { _right = value; }
    }

    public FileNode(int length, int width, FileState initState)
        : base(length, width, initState)
    {
    }
}

/// <Summary>
/// The container boxes that tries to accomodate all given files. It becomes unstable when it could not fit all files.
/// The box starts with a given dimension and gets split across top and bottom when new files are added. Smaller boxes are spawned as files are added and the big box is split.
/// </Summary>
class DropBox
{
    FileNode _rootBox;
    int _length;
    int _width;
    bool _isStable;

    public bool IsStable
    {
        get { return _isStable; }
    }

    public DropBox(Tuple<int, int>[] files, int length, int width)
    {
        _length = length;
        _width = width;
        _isStable = true;
        _rootBox = new FileNode(_length, _width, FileState.EMPTY);
        // keep adding files until the dropbox(container) is stable.
        for (int i = 0; i < files.Length && _isStable; i++)
        {
            Add(files[i]);
        }
    }

    private void Add(Tuple<int, int> box)
    {

        int length = box.Item1;
        int width = box.Item2;
        FileNode foundBox = FindEmptyBox(_rootBox, length, width);
        if (foundBox == null)
        {
            _isStable = false;
            return;
        }
        FillEmptyBox(foundBox, length, width);

    }
    /// <Summary>
    /// performs a depth first search with runtime of O(|E|)
    /// </Summary>
    private FileNode FindEmptyBox(FileNode root, int length, int width)
    {
        FileNode foundBox = null;
        if (root == null)
            return foundBox;
        if (root.State == FileState.EMPTY)
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

    private void FillEmptyBox(FileNode box, int length, int width)
    {
        //right cut
        FileNode rightBox = null;
        if (box.Length < length || box.Width < width)
        {
            int temp = length;
            length = width;
            width = temp;
        }
        int rightLeftOver = box.Width - width;
        box.Width = width;
        if (rightLeftOver > 0)
            rightBox = new FileNode(box.Length, rightLeftOver, FileState.EMPTY);
        box.Right = rightBox;

        //top cut
        FileNode topBox = null;
        int topLeftOver = box.Length - length;
        box.Length = length;
        if (topLeftOver > 0)
            topBox = new FileNode(topLeftOver, box.Width, FileState.EMPTY);
        box.Top = topBox;

        box.State = FileState.FULL;
    }

    public void Dump(bool debug=false)
    {
        //Dump the state of the tree.
        //Future:change this to dump the state of box in stderr.
        if(!debug)
            InOrderDump(_rootBox, 0);
    }

    private void InOrderDump(FileNode root, int level)
    {
        if (root == null)
            return;
        Console.WriteLine("{2}:({0},{1})", root.Length, root.Width, level);
        InOrderDump(root.Top, level + 1);
        InOrderDump(root.Right, level + 1);
    }
}

class Solution
{
    public static void Main()
    {

        int minLength = int.MaxValue;
        int maxLength = 0;
        int minWidth = int.MaxValue;
        int maxWidth = 0;
        bool DEBUG = false;

        FileStream inputStream = new FileStream("packing_boxes_sample_input.txt", FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(inputStream);
        //int n = int.Parse(sr.ReadLine());
        int n = int.Parse(Console.ReadLine());
        Tuple<int, int>[] files = new Tuple<int, int>[n];
        //create a tuple of length&width representing all files.
        for (int i = 0; i < n; i++)
        {
            //string[] inputs = sr.ReadLine().Split(' ');
            string[] inputs = Console.ReadLine().Split(' ');
            int templength = int.Parse(inputs[0]);
            int tempwidth = int.Parse(inputs[1]);
            int length = Math.Max(templength, tempwidth);
            int width = Math.Min(templength, tempwidth);
            files[i] = new Tuple<int, int>(length, width);
            maxLength += length;
            maxWidth += width;
            minLength = Math.Min(minLength, length);
            minWidth = Math.Min(minWidth, width);
        }

        int dropBoxLength = minLength;
        int dropBoxWidth = maxWidth;
        int bestArea = maxWidth * maxLength;
        int bestWidth = maxWidth;
        int bestLength = maxLength;
        //heuristics as described in summary at top
        while (dropBoxLength <= maxLength && dropBoxWidth >= minWidth)
        {
            if (dropBoxLength * dropBoxWidth < bestArea)
            {
                DropBox dp = new DropBox(files, dropBoxLength, dropBoxWidth);
                if (dp.IsStable)
                {
                    bestArea = dropBoxLength * dropBoxWidth;
                    bestWidth = dropBoxWidth;
                    bestLength = dropBoxLength;
                    if (DEBUG)
                    {
                        Console.WriteLine("--DEBUG--");
                        Console.WriteLine("({0},{1})", dropBoxLength, dropBoxWidth);
                        dp.Dump();
                    }
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
        //Console.WriteLine ("({1},{2}),{0}", bestArea, bestLength, bestWidth);
        Console.WriteLine("{0}", bestArea);
    }
}


//Cord[] files = {new Cord(8,8),new Cord(4,3), new Cord(4,3)};
//test case 1
/*Tuple<int, int>[] files = {
new Tuple<int, int> (8, 8),
new Tuple<int, int> (4, 3),
new Tuple<int, int> (4, 3)
}*/
//test case 2. best : 100 ; algo: 105
/*Tuple<int, int>[] files = {
new Tuple<int, int> (10, 5),
new Tuple<int, int> (5, 2),
new Tuple<int, int> (5, 2),
new Tuple<int, int> (5, 2),
new Tuple<int, int> (5, 2),
new Tuple<int, int> (5, 2)
};*/
