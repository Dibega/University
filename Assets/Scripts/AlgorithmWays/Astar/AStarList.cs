using System;
using System.Collections.Generic;

public sealed class AStarList : Algorithm<AStarNode>
{
    private HashSet<AStarNode> _closed= new HashSet<AStarNode>();
    private List<AStarNode> _open = new List<AStarNode>();
    private float distanseBetweenPoint = 1f;
    public AStarList(Graph<AStarNode> graph) : base(graph)
    {

    }

    protected override void InitializeStartPoint(AStarNode startNode)
    {
        _closed.Clear();
        _open.Clear();
        startNode.H = CalculateDistanseToEnd(startNode);
    }

    protected override void CalculateWay(Ship ship)
    {
        _open.Add(_queueSearch.Peek());

        while (_open.Count != 0)
        {
            var current = SearchMinF();
            _open.Remove(current);
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

                if (!_open.Contains(node))
                {
                    _open.Add(node);
                }
            }
        }
    }

    //public void SortAdd(AStarNode node)
    //{
    //    List<AStarNode> localStack = new List<AStarNode>();
    //    if (_open.Count == 0)
    //    {
    //        _open.Add(node);
    //    }

    //    for (int i = 0; i < _open.Count; i++)
    //    {
    //        if (node.F < _open().F)
    //        {
    //            _open.Push(node);
    //            break;
    //        }
    //        else
    //        {
    //            localStack.Push(_open.Pop());
    //        }
    //    }

    //    for (int i = 0; i < localStack.Count; i++)
    //    {
    //        _open.Push(localStack.Pop());
    //    }
    //}

    public AStarNode SearchMinF()
    {
        AStarNode minNode = _open[0];
        foreach (var node in _open)
        {
            if (node.F < minNode.F)
            {
                minNode = node;
            }
        }

        return minNode;
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
