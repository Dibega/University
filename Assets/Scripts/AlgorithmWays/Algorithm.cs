using System.Collections.Generic;

public abstract class Algorithm<T> where T:class,INode,new()
{
    protected Graph<T> _graph;
    protected Queue<T> _queueSearch = new Queue<T>();
    protected Point _endPoint;

    protected Queue<Point> _way = new Queue<Point>();

    public Algorithm(Graph<T> graph)
    {
        _graph = graph;
    }

    public Queue<Point> Search(Point startPoint, Point endPoint, Ship ship)
    {
        _queueSearch.Clear();
        _endPoint = endPoint;
        _way.Clear();

        if (_graph.IsNodeNull(_endPoint))
            return CopyWay();

        T mainNode = _graph.GetGraph(startPoint);
        if (mainNode == null)
            return CopyWay();

        InitializeStartPoint(mainNode);
        _queueSearch.Enqueue(mainNode);

        CalculateWay(ship);
        return CopyWay();
    }

    protected abstract void InitializeStartPoint(T startNode);

    protected abstract void CalculateWay(Ship ship);


    protected Queue<Point> CopyWay()
    {
        Queue<Point> newWay = new Queue<Point>();
        foreach (var point in _way)
        {
            newWay.Enqueue(point);
        }
        _way.Clear();
        return newWay;
    }
}
