using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace RandomMusings
{
	public struct Vertex{
		public int X; public int Y;
		public Vertex(int x, int y){
			X = x;
			Y = y;
		}
	}
	
	public class ShortestPath
	{
		static char[,] _maze = new char[4,4];
		static Vertex start =  new Vertex(2,1);
		static Vertex end = new Vertex(1,2);
		
		public List<Vertex> SearchShortestPath (Vertex start,Func<Vertex,bool> IsGoal, Func<Vertex,List<Vertex>> Successors)
		{
			Queue<List<Vertex>> frontier = new Queue<List<Vertex>>();
			HashSet<Vertex> explored = new HashSet<Vertex>();
			frontier.Enqueue(new List<Vertex>(){start});
			while(frontier.Count > 0){
				List<Vertex> path = frontier.Dequeue();
				Vertex v = path[path.Count - 1];
				if(!explored.Contains(v)){
					explored.Add(v);
					if(IsGoal(v))
						return path;
					List<Vertex> successors = Successors(v);
					foreach(Vertex vertex in successors){
						path.Add(vertex);
						frontier.Enqueue(path);}
				}
			}
			return null;
		}
		
		public List<Vertex> Successors(Vertex v){
			int colLeft = v.X - 1;
			int colRight = v.X + 1;
			int rowTop = v.Y - 1;
			int rowBottom = v.Y + 1;
			List<Vertex> successors = new List<Vertex>();
			if(rowTop > 0 && colLeft > 0)
				for(int i = 0 ; i < 3; i++)
					successors.Add(new Vertex(rowTop,i+colLeft));
			return successors;
		}
		
		public bool IsGoal(Vertex v){
			return (v.X == end.X && v.Y == end.Y);
		}
		
		static void Main(){
			Init();
			Show();
		}
		
		static private void Show(){
			for(int i = 0; i < 4;i++){
				for(int j=0;j<4;j++)
					Console.Write("{0}\t",_maze[i,j]);
				Console.WriteLine();
			}
		}
		
		private static  void Init(){	
			for(int i = 0 ; i < 4; i++){
				_maze[0,i] = '#';
				_maze[3,i] = '#';
				if(i != 3)
					_maze[1,i] = '#';
				if(i !=0)
					_maze[2,i] = '.';
			}
			_maze[1,3] = '.';
			_maze[2,0] = '#';
		}
	}
}

