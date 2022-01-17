using System;
using System.Collections.Generic;

[Serializable]
public class GraphNode : INode
{
    public Point Point { set; get; }
    public INode Root { set; get; }
    public List<INode> Neigbours { set; get; }
    //public Dictionary<INode,>

    public GraphNode()
    {
        Neigbours = new List<INode>();
    }

    public GraphNode(Point point)
    {
        Point = point;
        Neigbours = new List<INode>();
    }

    public GraphNode(INode graphNode)
    {
        Point = graphNode.Point;
        Root = graphNode.Root;
        Neigbours = graphNode.Neigbours;
    }
}
