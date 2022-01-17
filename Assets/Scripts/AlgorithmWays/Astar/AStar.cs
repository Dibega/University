using System;
using System.Collections.Generic;

public sealed class AStar : Algorithm<AStarNode>
{
    private HashSet<AStarNode> _closed = new HashSet<AStarNode>();
    private Stack<AStarNode> _open = new Stack<AStarNode>();
    private float distanseBetweenPoint = 1f;

    private int _localListCount;
    Stack<AStarNode> localStack = new Stack<AStarNode>();
    public AStar(Graph<AStarNode> graph) : base(graph)
    {

    }

    protected override void InitializeStartPoint(AStarNode startNode)
    {
        _closed.Clear();
        _open.Clear();
    }

    protected override void CalculateWay(Ship ship)
    {
        SortedPush(_queueSearch.Peek());

        while (_open.Count != 0)
        {
            var current = _open.Pop();
            //_open.(current);
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
                    SortedPush(node);
                    //_open.Push(node);
                }
            }
        }
    }

    private void SortedPush(AStarNode node)
    {
        localStack.Clear();
        //if (_open.Count == 0)
        //{
        //    _open.Push(node);
        //    return;
        //}

        _localListCount = _open.Count;
        for (int i = 0; i < _localListCount; i++)
        {
            if (node.F <= _open.Peek().F)
            {
                break;
            }
            else
            {
                localStack.Push(_open.Pop());
            }
        }

        _open.Push(node);

        _localListCount = localStack.Count;
        for (int i = 0; i < _localListCount; i++)
        {
            _open.Push(localStack.Pop());
        }
    }

    private float CalculateDistanseToEnd(AStarNode node)
    {
        float x = _endPoint.X - node.Point.X;
        float y = _endPoint.Y - node.Point.Y;
        return (float)Math.Sqrt(x * x + y * y);

        //return Math.Abs(_endPoint.X - node.Point.X) + Math.Abs(_endPoint.Y - node.Point.Y);
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
