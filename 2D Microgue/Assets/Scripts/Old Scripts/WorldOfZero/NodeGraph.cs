using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGraph<Vector2, TEdgeType>
{
    public NodeGraph()
    {
        Nodes = new List<Node<Vector2>>();
        Edges = new List<Edge<TEdgeType, Vector2>>();
    }

    public List<Node<Vector2>> Nodes { get; private set; }
    public List<Edge<TEdgeType, Vector2>> Edges { get; private set; }
}

public class Node<Vector2>
{
    public Color NodeColor { get; set; }
    public Vector2 Value { get; set; }
}

public class Edge<TEdgeType, Vector2>
{
    public Color EdgeColor { get; set; }
    public TEdgeType Value { get; set; }

    public Node<Vector2> From { get; set; }
    public Node<Vector2> To { get; set; }
}
