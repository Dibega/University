using System;
using System.Collections.Generic;

public class Graph<T> where T : class, INode, new()
{
    protected T[,] _allNodes;

    public T[,] AllNodes => _allNodes;

    //private Queue<GraphNode> _way;    в другом классе это лишь граф

    public Graph(int width)
    {
        CaclulateGraph(width);
    }

    public void GenerateDepths(float minDepth,float maxDepth)
    {
        for (int i = 0; i <= _allNodes.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= _allNodes.GetUpperBound(1); j++)
            {
                if (i == _allNodes.GetUpperBound(0) || i == 0 ||
                    j == _allNodes.GetUpperBound(1) || j == 0)
                {
                    ((LiNode)(INode)_allNodes[i,j]).GenerateDepths(maxDepth,maxDepth);
                }
                else
                {
                    ((LiNode)(INode)_allNodes[i,j]).GenerateDepths(minDepth,maxDepth);
                }
            }
        }
        
    }
    
    public List<T> GetFreeNodes(float depth)
    {
        List<T> nodes = new List<T>();
        foreach (INode node in _allNodes)
        {
            if (((LiNode) node).Depth < depth && !((LiNode) node).IsBusy)
            {
                nodes.Add((T)node);
            }
        }

        return nodes;
    }

    public bool IsGraphNull
    {
        get
        {
            bool isNull = false;
            foreach (var el in _allNodes)
            {
                if (el != null)
                {
                    isNull = true;
                }
            }

            return isNull;
        }
    }

    public bool IsNodeNull(Point point)
    {
        if (!IsInRange(point,_allNodes))
            return true;
        if (_allNodes[point.X, point.Y] == null)
            return true;

        return false;
    }

    public T GetGraph(Point point)
    {
        if (!IsInRange(point,_allNodes))
            return null;

        var mainNode = _allNodes[point.X, point.Y];
        if (mainNode == null)
            return null;

        return mainNode;
    }

    private void CaclulateGraph(int width)
    {
        CreateAllNodes(width);
        CreateGraph();
    }

    private void CreateAllNodes(int width)
    {
        _allNodes = new T[width, width];
        for (int x = 0; x <= _allNodes.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _allNodes.GetUpperBound(1); y++)
            {
                //_allNodes[x, y] = new INode(new Point(x, y));
                T node = new T();
                node.Point = new Point(x, y);
                _allNodes[x, y] = node;
            }
        }
    }

    private void CreateGraph()
    {
        for (int i = 0; i <= _allNodes.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= _allNodes.GetUpperBound(1); j++)
            {
                if (_allNodes[i, j] != null)
                {
                    AddChildren(_allNodes[i, j]);
                }
            }
        }
    }

    private void AddChildren(INode nowNode)
    {
        AddChild(new Point(nowNode.Point.X + 1, nowNode.Point.Y), nowNode);
        AddChild(new Point(nowNode.Point.X - 1, nowNode.Point.Y), nowNode);
        AddChild(new Point(nowNode.Point.X, nowNode.Point.Y - 1), nowNode);
        AddChild(new Point(nowNode.Point.X, nowNode.Point.Y + 1), nowNode);
        
        AddChild(new Point(nowNode.Point.X + 1, nowNode.Point.Y + 1), nowNode);
        AddChild(new Point(nowNode.Point.X - 1, nowNode.Point.Y - 1), nowNode);
        AddChild(new Point(nowNode.Point.X + 1, nowNode.Point.Y - 1), nowNode);
        AddChild(new Point(nowNode.Point.X - 1, nowNode.Point.Y + 1), nowNode);
    }

    private void AddChild(Point point, INode nowNode)
    {
        if (IsInRange(point,_allNodes))
        {
            if (_allNodes[point.X, point.Y] != null)
            {
                nowNode.Neigbours.Add(_allNodes[point.X, point.Y]);
            }
        }
    }

    public static bool IsInRange(Point point, Array pointMatrix)
    {
        if (point.X > pointMatrix.GetUpperBound(0) || point.X < 0 ||
            point.Y > pointMatrix.GetUpperBound(1) || point.Y < 0)
        {
            return false;
        }

        return true;
    }
}