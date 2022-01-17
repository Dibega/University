public sealed class BFS : Algorithm<BFSNode>
{
    public BFS(Graph<BFSNode> graph) : base(graph)
    {
    }

    protected override void InitializeStartPoint(BFSNode startNode)
    {
        startNode.State = GraphState.Black;
    }

    protected override void CalculateWay(Ship ship)
    {
        while (_queueSearch.Count != 0)
        {
            BFSNode current = _queueSearch.Dequeue();

            if (current.Point == _endPoint)
            {
                MakeWay(current);
                return;
            }

            foreach (BFSNode child in current.Neigbours)
            {
                if (child.State == GraphState.White)
                {
                    child.State = GraphState.Black;
                    child.Root = current;
                    _queueSearch.Enqueue(child);
                }
            }
        }
    }

    private void MakeWay(INode node)
    {
        if (node.Root != null)
        {
            MakeWay(node.Root);
        }
        _way.Enqueue(node.Point);
    }
}