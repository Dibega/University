using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class ParkingBlock:MonoBehaviour, IParkingBlock
    {
        [SerializeField] private List<ParkingWaypoint> _points;
        private bool _isActive;

        public int Count => _points.Count;
        public bool IsActive => _isActive;

        public void Active(bool isActive)
        {
            _isActive = isActive;
            foreach (var point in _points)
            {
                point.IsActive = isActive;
            }
        }

        public List<ParkingWaypoint> GetParkingPoints()
        {
            return _points;
        }
    }
}