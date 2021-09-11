using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Grid
{
public class Node
{
    public Vector3Int position;
    public bool isBeingChecked = false;
    public bool isAlreadyChecked = false;
}

public class AStarNode : Node
{
    public int hCost;
    public int fCost;
    public int gCost;
    public AStarNode parent;
}

public class BFSNode : Node
{
    public BFSNode parent;
    public int distance = 0;
}
}