Practical_DSA
=================

A Collection of popular and interesting datastructures, algorithms and programming puzzles using C# and Python. 

NOTE:Most of these algorithms are for academic purposes, although some have been tested against medium(10^6) sized test data and can be used for smaller projects.

./basic_algorithms
	./basic_algorithms/MergeSort.cs
	./basic_algorithms/QuickSort.cs
	./basic_algorithms/ShortestPath.cs - Gives an effective templated approach for shortest path Djikstra algorithm.
./basic_datastructures
	./basic_datastructures/BinarySearchTree.cs
	./basic_datastructures/DoublyLinkedList.cs
	./basic_datastructures/Heap.cs
	./basic_datastructures/LRUCache.cs : C# implementation of Least Recently Used cache using a backing Priority Heap(a.k.a Priority Queue) http://en.wikipedia.org/wiki/Cache_algorithms
	./basic_datastructures/PriorityHeap.cs
	./basic_datastructures/Helpers
		./basic_datastructures/Helpers/DoublyLinkedListNode.cs
		./basic_datastructures/Helpers/HeapStrategy.cs
		./basic_datastructures/Helpers/IndexChangedEventArgs.cs
		./basic_datastructures/Helpers/Node.cs
./dynamic_programming
	./dynamic_programming/assembly_scheduling.py
	./dynamic_programming/knapsack.py
	./dynamic_programming/LCS.py
	./dynamic_programming/Tests
		./dynamic_programming/Helpers/knapsack_big_test.txt
		./dynamic_programming/Helpers/knapsack_test.txt
./elementary_practice_problems
	./elementary_practice_problems/character_swapping.py
	./elementary_practice_problems/gradeschool_tables.py
	./elementary_practice_problems/NumericalAlgorithms.cs
	./elementary_practice_problems/read_file.py
	./elementary_practice_problems/Helpers
		./elementary_practice_problems/Helpers/temp.txt
./interesting_puzzles
	./interesting_puzzles/JosephusPuzzle.cs
	./interesting_puzzles/tower_of_boxes.py
	./interesting_puzzles/2DBinPacking.cs
	./interesting_puzzles/JosephusPuzzle.cs
	./interesting_puzzles/fix_your_diet.py
	Tests/
		./interesting_puzzles/ftest_2d_binpacking.in
		./interesting_puzzles/ftest_fix_your_diet.in
