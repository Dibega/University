using UnityEngine;

namespace Pathfinding
{
    public class PathfindingTest : MonoBehaviour
    {
        [SerializeReference] private SimpleGrid _grid;
        public IPathfinding Manager { get; private set; }
        private void Awake()
        {
            Manager = new SimplePathfinding(_grid);
        }
    }
}