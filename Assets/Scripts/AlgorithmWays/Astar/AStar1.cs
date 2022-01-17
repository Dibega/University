using System;
using System.Collections.Generic;

public sealed class AStar1 : Algorithm<AStarNode>
{
    private HashSet<AStarNode> _closed= new HashSet<AStarNode>();
    private float distanseBetweenPoint = 1f;
    public AStar1(Graph<AStarNode> graph) : base(graph)
    {

    }

    protected override void InitializeStartPoint(AStarNode startNode)
    {
        _closed.Clear();
        startNode.H = CalculateDistanseToEnd(startNode);
    }

    protected override void CalculateWay(Ship ship)
    {
        while (_queueSearch.Count != 0)
        {
            var current = _queueSearch.Dequeue();
            //_queueSearch.Remove(current);
            _closed.Add(current);

            if (current.Point == _endPoint)
            {
                MakeWay(current);
                break;
            }

            foreach (AStarNode node in current.Neigbours)
            {
                if (node.Root == current)
                {
                    continue;
                }

                float G = current.G + distanseBetweenPoint;

                if (_closed.Contains(node) && G >= node.G)
                {
                    continue;
                }
                else
                {
                    node.Root = current;
                    node.G = G;
                    node.H = CalculateDistanseToEnd(node);
                }

                if (!_queueSearch.Contains(node))
                {
                    _queueSearch.Enqueue(node);
                }
            }
        }
    }

    private float CalculateDistanseToEnd(AStarNode node)
    {
        float x = _endPoint.X - node.Point.X;
        float y = _endPoint.Y - node.Point.Y;
        return (float)Math.Sqrt(x * x + y * y);

        //return Math.Abs(_endPoint.XField - node.Point.XField) + Math.Abs(_endPoint.YField - node.Point.YField);
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
