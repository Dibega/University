public class BFSNode : GraphNode
{
    public GraphState State = GraphState.White;

    public BFSNode() : base()
    {
    }

    public BFSNode(Point point) : base(point)
    {
    }
    public BFSNode(INode graphNode) : base(graphNode)
    {
    }
}
