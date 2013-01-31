/// <summary>
/// Problem Statement:
/// 	Assume you have farm and your farmers have cells of land in it. Each year there is rainfall and your objective is to help farmers determine the flow of water 
/// 	into their cell of land. You will be given elevation data for each cell of land.
///
/// 	We will follow the basic rules of gravity for rainwater flow. A cell with higher elevation will flow into the lowest of its four neighboring cells. if the 
/// 	cell is the lowest of all its neighbors it is the sink. Water can flow only in horizontal or vertical direction.
/// 
/// 	Your objective is to divide the piece of farm into basins. A basin is the count of cells that have a common sink into which water flows either directly or indirectly. The basin
/// 	should be displayed in descending order
///		
/// 	Input(elevation data):
/// 	3
/// 	1 5 2
/// 	2 3 7
/// 	6 4 5
///		
/// 	Output:
/// 	7 2
/// 
/// 	Illustration of basins Formed:
/// 	A A B
/// 	A A B
/// 	A A A

///		Input(elevation data):
/// 	1
/// 	10
/// 
/// 	Output:
/// 	1
/// 
/// 	There will be only one basin formed as there is only one cell inside the farm.
/// 	
/// </summary>

using System;
using System.Collections.Generic;
using System.IO;

class Cell
{
	int _elevation;
	int _positionX;
	int _positionY;
	Cell _flowsInto;

	public int Elevation {
		get{ return _elevation;}
	}

	public int PositionX {
		get{ return _positionX;}
	}

	public int PositionY {
		get{ return _positionY;}
	}

	public Cell FlowsInto {
		get{ return _flowsInto;}
		set{ _flowsInto = value;}
	}

	public Cell (int elevation, int x, int y)
	{
		_elevation = elevation;
		_positionX = x;
		_positionY = y;
	}
}

class FarmLand
{
	Cell[,] _cells;
	int _size = default(int);

	public FarmLand (int size, Cell[,] cells)
	{
		_size = size;
		_cells = cells;
	}

	//scan thru all nodes and assign the flows
	public void NavigateAndAssignFlows ()
	{
		for (int i=0; i<_size; i++) {
			for (int j=0; j<_size; j++) {
				Cell workingCell = _cells [i, j];
				List<Cell> neighbors = FindNeighbors (workingCell);
				//lowest possible elevation
				Cell sink = new Cell (int.MaxValue, -1, -1);
				foreach (Cell cell in neighbors) {
					if (cell.Elevation < workingCell.Elevation && cell.Elevation < sink.Elevation)
						sink = cell;
				}
				workingCell.FlowsInto = (sink.Elevation == int.MaxValue) ? null : sink;
			}
		}
	}

	//Cells that point to the same sink form  one basin.
	public string FindBasins ()
	{
		Dictionary<Tuple<int,int>,int> basins = new Dictionary<Tuple<int,int>, int> ();
		for (int i=0; i < _size; i++) {
			for (int j=0; j<_size; j++) {
				Cell workingCell = _cells [i, j];
				Cell sink = FindSink (workingCell);
				if (sink == null) {
					Tuple<int,int> key = new Tuple<int, int> (workingCell.PositionX, workingCell.PositionY);
					if (!basins.ContainsKey (key))
						basins.Add (key, 1);
				} else {
					Tuple<int,int> key = new Tuple<int, int> (sink.PositionX, sink.PositionY);
					if (basins.ContainsKey (key))
						basins [key]++;
					else
						basins.Add (key, 1);
				}
			}
		}
		List<int> basinCounts = new List<int> ();
		foreach (KeyValuePair<Tuple<int,int>,int> basin in basins) {
			basinCounts.Add (basin.Value);
		}
		basinCounts.Sort ();
		basinCounts.Reverse ();
		string s = string.Empty;
		foreach (int count in basinCounts)
			s += count + " ";
		return s.TrimEnd ();
	}

	private void Dump ()
	{
		for (int i=0; i<_size; i++) {
			for (int j=0; j<_size; j++) {
				if (_cells [i, j].FlowsInto != null)
					Console.WriteLine ("{0},{1}:{2}", i, j, _cells [i, j].FlowsInto.Elevation);
			}
		}
	}

	private Cell FindSink (Cell cell)
	{
		if (cell.FlowsInto == null)
			return cell;
		else
			return FindSink (cell.FlowsInto);
	}

	private List<Cell> FindNeighbors (Cell cell)
	{
		List<Cell> neighbors = new List<Cell> (8);
		int i = cell.PositionX;
		int j = cell.PositionY;
		if (i - 1 >= 0)
			neighbors.Add (_cells [i - 1, j]);
		if (i + 1 < _size)
			neighbors.Add (_cells [i + 1, j]);
		if (j - 1 >= 0)
			neighbors.Add (_cells [i, j - 1]);
		if (j + 1 < _size)
			neighbors.Add (_cells [i, j + 1]);
		return neighbors;		
	}
}

class Solution
{

	static void Main ()
	{
		//int n = int.Parse(Console.ReadLine());
		using (FileStream fs = new FileStream("Tests/HelptheFarmer.test",FileMode.Open,FileAccess.Read)) {
			StreamReader sr = new StreamReader (fs);
			int n = int.Parse (sr.ReadLine ());
			Cell[,] cells = new Cell[n, n];
			for (int i=0; i<n; i++) {
				//string[] row = Console.ReadLine().Split(' ');
				string[] row = sr.ReadLine ().Split (' ');
				for (int j=0; j<n; j++) {
					string cell = row [j];
					cells [i, j] = new Cell (int.Parse (cell), i, j);
				}
			}

			FarmLand land = new FarmLand (n, cells);
			land.NavigateAndAssignFlows ();
			Console.WriteLine (land.FindBasins ());
		}
	}
}
