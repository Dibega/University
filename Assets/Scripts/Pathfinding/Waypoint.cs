using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Pathfinding
{
    public class Waypoint : MonoBehaviour, IWaypoint
    {
        public Vector3 Position => transform.position;
        [SerializeField] private List<Waypoint> _neighbours;
        private LiNode _node;
        public LiNode Node => _node;

#if UNITY_EDITOR
        public List<Waypoint> EDITOR_NEIGHBOURS => _neighbours;
#endif
        private IList<IWaypoint> _cachedNeighbours;
        public IList<IWaypoint> Neighbours => _cachedNeighbours;

        private void Awake()
        {
            _cachedNeighbours = new List<IWaypoint>(_neighbours.Count);
            for (int i = 0; i < _neighbours.Count; i++)
            {
                _cachedNeighbours.Add(_neighbours[i]);
            }
        }

#if UNITY_EDITOR
        public void AddNeighbour(Waypoint neighbour)
        {
            if (_neighbours == null)
            {
                Debug.Log(gameObject.name + " adding null neighbour");
            }

            if ( _neighbours.Contains(neighbour))
                return;

            _neighbours.Add(neighbour);
            EditorUtility.SetDirty(gameObject);
        }
#endif

        public void Init(LiNode node)
        {
            _node = node;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position,0.1f);
            if (_neighbours == null)
                return;

            foreach (var neighbour in _neighbours)
            {
                if (neighbour == null)
                    continue;

                Gizmos.DrawLine(transform.position, neighbour.Position);
            }
        }
    }
}