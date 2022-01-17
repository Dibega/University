using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

namespace Pathfinding
{
    [Serializable]
    public class SimpleGrid : MonoBehaviour, IWaypointGrid
    {
        private List<Waypoint> _waypoints;
        private List<IWaypoint> _cached;
        public IList<IWaypoint> Waypoints => _cached;

        public void Init(List<Waypoint> waypoints)
        {
            _waypoints = waypoints;
            _cached = new List<IWaypoint>();
            foreach (var waypoint in _waypoints)
            {
                _cached.Add(waypoint);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("fix neighbours")]
        private void FixNeighbours()
        {
            foreach (var waypoint in _waypoints)
            {
                foreach (var neighbour in waypoint.EDITOR_NEIGHBOURS)
                {
                    neighbour.AddNeighbour(waypoint);
                }
            }
        }

        [ContextMenu("collect waypoints")]
        private void CollectWaypoints()
        {
            var collectedWaypoints = GetComponentsInChildren<Waypoint>().ToList();
            //remove empty
            foreach (var waypoint in _waypoints)
            {
                if (waypoint == null)
                {
                    _waypoints.Remove(waypoint);
                }
            }
            
            //add children if !Contains
            foreach (var waypoint in collectedWaypoints)
            {
                if (!_waypoints.Contains(waypoint))
                {
                    _waypoints.Add(waypoint);
                }
            }
            
            EditorUtility.SetDirty(gameObject);
        }
#endif
    }
}