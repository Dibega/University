using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class TestCharacter : MonoBehaviour
    {
        [SerializeField] private bool _teleport;
        [SerializeField] private bool _testPath;
        [SerializeField] private PathfindingTest _testScript;

        [SerializeField] private Waypoint _wp1, _wp2;

        [SerializeField] private float _speed = 2;
        private Stack<IWaypoint> _route;

        [ContextMenu("teleport  to nearest point")]
        private void TeleportToNearestPoint()
        {
            var nearestPoint = _testScript.Manager.GetNearestPoint(transform.position).Position;
            transform.position = nearestPoint;
        }

        private void Update()
        {
            if (_teleport)
            {
                TeleportToNearestPoint();
                _teleport = false;
            }

            if (_testPath)
            {
                _testPath = false;

                _route = new Stack<IWaypoint>(_testScript.Manager.GetRoute(_wp1, _wp2));
            }

            if (_route != null && _route.Count > 0)
            {
                var currentPoint = _route.Peek();
                transform.position =
                    Vector3.MoveTowards(transform.position, currentPoint.Position, Time.deltaTime * _speed);

                if ((transform.position - currentPoint.Position).sqrMagnitude <= 0.1)
                    _route.Pop();
            }
        }
    }
}