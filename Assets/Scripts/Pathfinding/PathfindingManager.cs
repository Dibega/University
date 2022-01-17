using System;
using System.Collections.Generic;

namespace Pathfinding
{
    public class PathfindingManager : IPathfindingManager
    {
        private IDictionary<int, IPathfinding> _pathfindings = new Dictionary<int, IPathfinding>();

        public void Register(PathfindingZone zone, IPathfinding pathfinding)
        {
            if (_pathfindings.ContainsKey((int) zone))
                throw new Exception($"zone {zone} already in pathfinding");

            _pathfindings.Add((int) zone, pathfinding);
        }

        public IPathfinding Get(PathfindingZone zone)
        {
            return _pathfindings.TryGetValue((int) zone, out var pathfinding) ? pathfinding : null;
        }

        public void Dispose()
        {
            _pathfindings = null;
        }
    }
}