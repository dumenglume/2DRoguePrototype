using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GraphComponent : MonoBehaviour
{
    NodeGraph<Vector2, float> graph;

    void Start()
    {
        graph = new NodeGraph<Vector2, float>();
        var node1 = new Node<Vector2>() { Value = Vector2.zero, NodeColor = Color.red };
        var node2 = new Node<Vector2>() { Value = Vector2.one, NodeColor = Color.cyan };
        var edge1 = new Edge<float, Vector2>() { Value = 1.0f, From = node1, To = node2, EdgeColor = Color.yellow };

        graph.Nodes.Add(node1);
        graph.Nodes.Add(node2);
        graph.Edges.Add(edge1);
    }

    void OnDrawGizmos() 
    {
        if (graph == null)
        {
            Start();
        }

        foreach (var node in graph.Nodes)
        {
            Gizmos.color = node.NodeColor;
            Gizmos.DrawSphere(node.Value, 0.125f);
        }

        foreach (var edge in graph.Edges)
        {
            Gizmos.color = edge.EdgeColor;
            Gizmos.DrawLine(edge.From.Value, edge.To.Value);
        }
    }
}
