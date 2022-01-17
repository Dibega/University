using System.Collections.Generic;

namespace Pathfinding
{
    public interface IParkingBlock
    {
        int Count { get; }
        bool IsActive { get; }
        void Active(bool isActive);
        List<ParkingWaypoint> GetParkingPoints();
    }
}