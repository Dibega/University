using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Pathfinding
{
    public class SimplePathfinding : IPathfinding
    {
        private readonly IWaypointGrid _grid;

        public SimplePathfinding(IWaypointGrid grid)
        {
            _grid = grid;
        }

        public IWaypoint GetNearestPoint(Vector3 point)
        {
            Profiler.BeginSample("get nearest point");

            var cnt = _grid.Waypoints.Count;
            var resultMagn = float.MaxValue;
            IWaypoint resultWaypoint = null;
            for (int i = 0; i < cnt; i++)
            {
                var sqrMag = (_grid.Waypoints[i].Position - point).sqrMagnitude;
                if (sqrMag < resultMagn)
                {
                    resultMagn = sqrMag;
                    resultWaypoint = _grid.Waypoints[i];
                }
            }

            Profiler.EndSample();

            return resultWaypoint;
        }

        public IList<IWaypoint> GetAllWaypoints()
        {
            return _grid.Waypoints;
        }

        private readonly NodePool _pool = new NodePool(50);
        private readonly Dictionary<IWaypoint, Node> _cache = new Dictionary<IWaypoint, Node>(100);
        private readonly Queue<Node> _searchNodes = new Queue<Node>(100);
        private readonly HashSet<IWaypoint> _pendingPoints = new HashSet<IWaypoint>();
        
        public Stack<IWaypoint> GetRoute(IWaypoint start, IWaypoint end)
        {
            Profiler.BeginSample("GetRoute");

            Profiler.BeginSample("wave forward");
            var startNode = _pool.GetNode(0, start);

            var i = 0;
            _pendingPoints.Clear();
            _searchNodes.Enqueue(startNode);
            while (_searchNodes.Count != 0)
            {
                i++;
                var node = _searchNodes.Dequeue();
                node.Value = i;
                _cache.Add(node.Waypoint, node);
                _pendingPoints.Remove(node.Waypoint);

                if (node.Waypoint == end)
                    break;

                for (int j = 0; j < node.Waypoint.Neighbours.Count; j++)
                {
                    var child = node.Waypoint.Neighbours[j];
                    //for gates
                    if(!((Waypoint)child).transform.gameObject.activeInHierarchy)
                        continue;
                    //
                    if (!_cache.ContainsKey(child) && !_pendingPoints.Contains(child))
                    {
                        var childNode = _pool.GetNode(i, child);
                        AddNeighborDirection(childNode,node);
                        _searchNodes.Enqueue(childNode);
                        _pendingPoints.Add(child);
                    }
                }
            }

            if (!_cache.ContainsKey(end))
                return null;

            Profiler.EndSample();

            Profiler.BeginSample("backtrace");

            var path = new Stack<IWaypoint>(i);
            var current = end;
            while (current != start)
            {
                _cache.TryGetValue(current, out var node);
                path.Push(current);

                var min = node.Value;
                IWaypoint minNode = null;

                _directionCache.TryGetValue(node, out var listDirectionNeighbors);
                for (int j = 0; j < listDirectionNeighbors.Count; j++)
                {
                    var neighbour = listDirectionNeighbors[j].Waypoint;
                    if (!_cache.TryGetValue(neighbour, out var neighbourNode)) continue;
                    if (neighbourNode.Value >= min) continue;
                    min = neighbourNode.Value;
                    minNode = neighbour;
                }

                current = minNode;
                if (current == null)
                    break;
            }

            path.Push(start);
            Profiler.EndSample();

            Profiler.BeginSample("cleanup cache and search queue");

            var cacheEnumerator = _cache.GetEnumerator();
            while (cacheEnumerator.MoveNext())
                _pool.Release(cacheEnumerator.Current.Value);

            _cache.Clear();
            _directionCache.Clear();

            var queueEnumerator = _searchNodes.GetEnumerator();
            while (queueEnumerator.MoveNext())
                _pool.Release(queueEnumerator.Current);

            _searchNodes.Clear();

            Profiler.EndSample();

            Profiler.EndSample();
            return path;
        }

        private class Node
        {
            public int Value;
            public IWaypoint Waypoint;

            public Node(int value, IWaypoint waypoint)
            {
                Value = value;
                Waypoint = waypoint;
            }
        }
        
        private Dictionary<Node, List<Node>> _directionCache = new Dictionary<Node, List<Node>>(100);

        private void AddNeighborDirection(Node toNode, Node fromNode)
        {
            if (!_directionCache.ContainsKey(toNode))
            {
                _directionCache.Add(toNode, new List<Node>(4) {fromNode});
                return;
            }

            if (!_directionCache[toNode].Contains(fromNode))
                _directionCache[toNode].Add(fromNode);
        }

        private class NodePool
        {
            private readonly Stack<Node> _freeNodes;
            private readonly List<Node> _usedNodes;

            public NodePool(int count)
            {
                _freeNodes = new Stack<Node>(count);
                _usedNodes = new List<Node>(count);
                for (int i = 0; i < count; i++)
                {
                    _freeNodes.Push(GetNewNode());
                }
            }

            private Node GetNewNode()
            {
                return new Node(0, null);
            }

            public Node GetNode(int value, IWaypoint waypoint)
            {
                var node = _freeNodes.Count > 0 ? _freeNodes.Pop() : GetNewNode();

                _usedNodes.Add(node);
                node.Value = value;
                node.Waypoint = waypoint;

                return node;
            }

            public void Release(Node node)
            {
                node.Value = 0;
                node.Waypoint = null;

                var idx = 0;
                var cnt = _usedNodes.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (node == _usedNodes[i])
                    {
                        idx = i;
                        break;
                    }
                }

                _usedNodes.RemoveAt(idx);
                _freeNodes.Push(node);
            }
        }
    }
}