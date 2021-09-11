using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://youtu.be/lCMJFIleAzg

public class DijkstraTest : MonoBehaviour
{

    public class DijkstraGraph
    {
        public string[] nodes;
        public uint[] edges; // Corridoors / connections?
    }

    public class DijkstraAlgorithmData
    {
        public DijkstraGraph graph;
        public uint openListSize;
        public uint[] openList;
        public uint[] nodeCost;
        public uint[] prevNode;

        public uint startingNode;
        public uint endingNode;
    }

    public const uint INFINITY = 9999;
    public const uint INVALID_NODE = 6666;

    public static DijkstraGraph SetupGraph(string[] nodes, uint[] edges)
    {
        var dg = new DijkstraGraph();
        dg.nodes = nodes;
        dg.edges = edges;

        return dg;
    }

    public static DijkstraAlgorithmData StartDijkstra(DijkstraGraph graph, uint startingNode, uint endingNode)
    {
        var data = new DijkstraAlgorithmData();
        data.graph        = graph;
        data.startingNode = startingNode;
        data.endingNode   = endingNode;

        // Initialize node costs
        data.nodeCost = new uint[graph.nodes.Length];
        data.prevNode = new uint[graph.nodes.Length];

        for (int i = 0; i < graph.nodes.Length; i++)
        {
            data.nodeCost[i] = INFINITY;
            data.prevNode[i] = INVALID_NODE;
        }

        data.nodeCost[startingNode] = 0;

        // Initialize open list
        data.openList = new uint[graph.nodes.Length];
        data.openList[0] = startingNode;
        data.openListSize = 1;

        return data;
    }

    public static bool Process(DijkstraAlgorithmData data)
    {
        // Pop node out of list and store it in local variable
        var node = data.openList[data.openListSize - 1];
        data.openListSize -= 1;

        for (int i = 0; i < data.graph.edges.Length; i += 3)
        {
            var edgeStart = data.graph.edges[i];
            var edgeEnd   = data.graph.edges[i + 1];
            var cost      = data.graph.edges[i + 2];

            if (edgeStart == node)
            {
                if (data.nodeCost[edgeEnd] > data.nodeCost[node] + cost)
                {
                    data.nodeCost[edgeEnd] = data.nodeCost[node] + cost;
                    data.prevNode[edgeEnd] = node;
                    data.openList[data.openListSize] = edgeEnd;
                    data.openListSize += 1;
                }
            }
        }

        if (data.openListSize == 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public static uint[] GetPath(DijkstraAlgorithmData data)
    {
        var countWalker = data.prevNode[data.endingNode];
        int count = 1;

        while (countWalker != INVALID_NODE)
        {
            count += 1;
            countWalker = data.prevNode[countWalker];
        }

        if (count == 0)
        {
            return null;
        }

        else
        {
            var ret = new uint[count];
            int i = 1;
            var pathWalker = data.endingNode;

            while (pathWalker != INVALID_NODE)
            {
                ret[count - i] = pathWalker;
                i += 1;
                pathWalker = data.prevNode[pathWalker];
            }

            return ret;
        }
    }

    protected void Start()
    {
        var grid = SetupGraph(new string[]{ "A", "B"}, new uint[]{0, 1, 15,
                                                                 1, 2, 30});
        var algo = StartDijkstra(grid, 0, 2);

        while (!Process(algo)) {}

        var path = GetPath(algo);
        var s = "";

        for (int i = 0; i < path.Length; i++)
        {
            if (i != 0)
            {
                s += ", ";
            }

            s += "" + path[i];
        }

        Debug.Log(s);
    }
}