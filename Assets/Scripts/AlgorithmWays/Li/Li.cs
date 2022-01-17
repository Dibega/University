using Pathfinding;

public sealed class Li : Algorithm<LiNode>
{
    public Li(Graph<LiNode> graph) : base(graph)
    {
    }

    protected override void InitializeStartPoint(LiNode startNode)
    {
        startNode.number = 0;
    }

    protected override void CalculateWay(Ship ship)
    {
        ClearNeighboursInGraph();
        _graph.AllNodes[ship.CurrentNode.Point.X, ship.CurrentNode.Point.Y].number = 0;
        
        while (_queueSearch.Count != 0)
        {
            LiNode current = _queueSearch.Dequeue();

            if (current.Point == _endPoint)
            {
                MakeWay(current);
                return;
            }

            foreach (LiNode neighbour in current.Neigbours)
            {
                if (neighbour.number == -1 && ship.Draft > neighbour.Depth && !neighbour.IsBusy)
                {
                    neighbour.number = current.number + 1;
                    _queueSearch.Enqueue(neighbour);
                }
            }
        }
    }

    private void ClearNeighboursInGraph()
    {
        foreach (var node in _graph.AllNodes)
        {
            node.number = -1;
        }
    }

    private void MakeWay(LiNode node)
    {
        if (node.number == 0)
        {
            _way.Enqueue(node.Point);
            return;
        }

        for (int i = 0; i < node.Neigbours.Count; i++)
        {
            LiNode nowNeigbour = (LiNode)node.Neigbours[i];

            if (nowNeigbour.number == node.number - 1)
            {
                MakeWay((LiNode)node.Neigbours[i]);

                _way.Enqueue(node.Point);
                break;
            }
        }

        ClearLiNodes();
    }

    private void ClearLiNodes()
    {
        foreach (var liNode in _graph.AllNodes)
        {
            liNode.number = -1;
        }
    }
}
