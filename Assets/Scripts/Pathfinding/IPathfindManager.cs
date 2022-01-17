using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public enum PathfindingZone
    {
        Vehicle,
        Characters
    }

    public interface IPathfindingManager
    {
        void Register(PathfindingZone zone, IPathfinding pathfinding);
        IPathfinding Get(PathfindingZone zone);
    }

    public interface IPathfinding
    {
        IWaypoint GetNearestPoint(Vector3 point);
        Stack<IWaypoint> GetRoute(IWaypoint start, IWaypoint end);
        IList<IWaypoint> GetAllWaypoints();
    }

    public interface IWaypoint
    {
        Vector3 Position { get; }
        IList<IWaypoint> Neighbours { get; }
    }

    public interface IParkingWaypoint : IWaypoint
    {
        bool IsActive { set; get; }
    }

    public interface IWaypointGrid
    {
        IList<IWaypoint> Waypoints { get; }
    }
}