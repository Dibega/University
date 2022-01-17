using System;

public class AStarNode : GraphNode, IComparable<AStarNode>
{
    public float G { set; get; }
    public float H { set; get; }
    public float F
    {
        get { return G + H; }
    }

    public AStarNode() : base()
    {
        SetDefault();
    }

    public AStarNode(Point point) : base(point)
    {
        SetDefault();
    }
    public AStarNode(INode graphNode) : base(graphNode)
    {
        SetDefault();
    }

    private void SetDefault()
    {
        G = 0;
    }

    public int CompareTo(AStarNode other)
    {
        return other.F.CompareTo(F);
    }
}
