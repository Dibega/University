using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Ship : MonoBehaviour
{
    private bool _isAlive = true;
    private bool _isOnSpot = false;
    [Header("speed")] private float _currentSpeed;
    public float CurrentSpeed => _currentSpeed;

    [SerializeField] private float _maxMaxSpeed = 15f;
    public float MaxMaxSpeed => _maxMaxSpeed;
    [SerializeField] private float _minMaxSpeed = 10f;
    public float MinMaxSpeed => _minMaxSpeed;

    private float _maxSpeed = 0.8f;
    public float MaxSpeed => _maxSpeed;

    [Header("rotation")] [SerializeField] private float _rotationAngle;
    public float RotationAngle => _rotationAngle;

    private static float _rotationStep = 45;
    public static float RotationStep => _rotationStep;

    [Header("draft")] [SerializeField] private float _draft;
    public float Draft => _draft;
    [Header("materials")] [SerializeField] private Material _deadShipMaterial;
    [SerializeField] private Material _onSpotShipMaterial;

    private float _pointSize;

    public float CurrentTimeInPoint => _pointSize / _currentSpeed;

    private Core _core;
    private Li _algorithmLi;
    private Graph<LiNode> _graphLi;

    private Point _currentWaypoint;

    private LiNode _currentNode;
    public LiNode CurrentNode => _currentNode;

    private LiNode _endNode;
    public LiNode EndNode => _endNode;

    private LiNode _forwardNode;

    private Transform[,] _transformsGrid;

    private Queue<Point> _waypoints;

    public void Init(Core core, Li algorithmLi, Graph<LiNode> graphLi, float pointSize, Transform[,] transformsGrid,
        float minDraft, float maxDraft)
    {
        _core = core;
        _algorithmLi = algorithmLi;
        _graphLi = graphLi;
        _pointSize = pointSize;
        _transformsGrid = transformsGrid;

        _draft = Random.Range(minDraft, maxDraft);
        _maxSpeed = Random.Range(_minMaxSpeed, _maxMaxSpeed);
        _currentSpeed = _maxSpeed;
        List<LiNode> freeNodes = _graphLi.GetFreeNodes(_draft);

        var spawnNode = freeNodes[Random.Range(0, freeNodes.Count - 1)];
        freeNodes.Remove(spawnNode);

        _currentNode = spawnNode;
        transform.position = transformsGrid[_currentNode.Point.X, _currentNode.Point.Y].position;
        _endNode = freeNodes[Random.Range(0, freeNodes.Count - 1)];

        FindWay();

        if (_waypoints.Count != 0)
        {
            _forwardNode = FindLiNode(_waypoints.Peek());
            RotateToForwardPoint();
        }
        else
        {
            _isOnSpot = true;
        }

        SetNodeIsBusy(_currentNode, true);
    }

    private void FindWay()
    {
        _waypoints = _algorithmLi.Search(_currentNode.Point, _endNode.Point, this);
        if (_waypoints.Count != 0)
        {
            _currentWaypoint = _waypoints.Dequeue();
        }

        //_forwardNode = (LiNode)_currentNode.Neigbours[Random.Range(0, _currentNode.Neigbours.Count - 1)];
        ShowWay();
    }

    private void ShowWay()
    {
        string str = $"{gameObject.name} way: ";
        foreach (var point in _waypoints)
        {
            str += $"[{point.X},{point.Y}] -> ";
        }

        str += "end";
        Debug.Log(str);
    }

    private void Update()
    {
        Move();
    }

    private static float _cellSize = 1;
    private float _pastTime = 0;

    private void Move()
    {
        if (!_isOnSpot && _isAlive)
        {
            if (_cellSize / _currentSpeed - _pastTime <= 0)
            {
                _pastTime = 0;
                MoveToForwardPoint();

                if (_forwardNode.Depth > _draft)
                {
                    Die();
                    return;
                }

                if (_forwardNode.IsBusy)
                {
                    _core.KillShipsOnPoint(_forwardNode.Point);
                    return;
                }

                FindNextForwardNode();
                DefineIsCurrentWay();
            }
            else
            {
                _pastTime += Time.deltaTime;
            }

            ChoiceRotationAndForwardNode();
        }
    }

    private void MoveToForwardPoint()
    {
        SetNodeIsBusy(_currentNode, false);
        var nextPosition = _forwardNode.VisualNode.transform.position;
        nextPosition.y = 0;
        transform.position = nextPosition;
    }

    private void FindNextForwardNode()
    {
        Point forwardPointNormalize = _forwardNode.Point - _currentNode.Point;
        _currentNode = _forwardNode;
        SetNodeIsBusy(_currentNode, true);
        _currentWaypoint = _waypoints.Dequeue();
        _forwardNode = FindLiNode(forwardPointNormalize + _currentNode.Point);
    }

    private void DefineIsCurrentWay()
    {
        if (_currentWaypoint != _currentNode.Point) //&& FindLiNode(_waypoints.Peek()).IsBusy)
        {
            FindWay();
        }
    }

    private void ChoiceRotationAndForwardNode()
    {
        if (_waypoints.Count == 0)
        {
            if (_currentNode == _endNode)
            {
                _isOnSpot = true;
                transform.GetChild(0).GetComponent<MeshRenderer>().material = _onSpotShipMaterial;
                Debug.Log($"{gameObject.name} on spot");
                return;
            }
            else
            {
                FindWay();
                if (_waypoints.Count == 0)
                {
                    _currentSpeed *= 0.5f;
                    Debug.Log($"{gameObject.name} haven't way");
                    return;
                }
                else
                {
                    _currentSpeed = _maxSpeed;
                }
            }
        }

        if (_forwardNode.Point == _waypoints.Peek() && _forwardNode.IsBusy)
        {
            Debug.Log($"{gameObject.name} see ship! Maneuver!");
            FindWay();
            if (_waypoints.Count == 0)
            {
                return;
            }
        }

        if (_forwardNode.Point == _waypoints.Peek() && !_forwardNode.IsBusy)
        {
            return;
        }


        Point forwardPointNormalize = _forwardNode.Point - _currentNode.Point;
        Point arrivalPointNormalize = _waypoints.Peek() - _currentNode.Point;

        if (GetCountToNextWaypoint(eHand.left, forwardPointNormalize, arrivalPointNormalize) <=
            GetCountToNextWaypoint(eHand.right, forwardPointNormalize, arrivalPointNormalize))
        {
            _forwardNode = FindLiNode(_currentNode.Point + GetHandPoint(eHand.left, forwardPointNormalize));
        }
        else
        {
            _forwardNode = FindLiNode(_currentNode.Point + GetHandPoint(eHand.right, forwardPointNormalize));
        }

        RotateToForwardPoint();
        ChoiceSpeed();
    }

    private void ChoiceSpeed()
    {
        if (_forwardNode.Point != _waypoints.Peek())
        {
            _currentSpeed *= 0.5f;
            Debug.Log($"Change {gameObject.name}'s speed from {_currentSpeed * 2} to {_currentSpeed}");
        }
        else
        {
            _currentSpeed = _maxSpeed;
        }
    }

    static List<Point> _orderedPoints = new List<Point>()
    {
        new Point(0, -1), new Point(-1, -1), new Point(-1, 0),
        new Point(-1, 1), new Point(0, 1), new Point(1, 1), new Point(1, 0), new Point(1, -1)
    };

    static LinkedList<Point> _linkedListOrderedPoints = new LinkedList<Point>(_orderedPoints);

    private LinkedListNode<Point> GetOffsetNode(Point point)
    {
        var currentPoint = _orderedPoints.Find(el => el == point);
        return _linkedListOrderedPoints.Find(currentPoint);
    }

    private enum eHand
    {
        left,
        right
    }

    private int GetCountToNextWaypoint(eHand hand, Point point, Point arrivalPoint)
    {
        int count = 0;
        if (point == arrivalPoint)
        {
            return count;
        }

        count++;
        var nextPointToCheck = GetHandPoint(hand, point);
        return count + GetCountToNextWaypoint(hand, nextPointToCheck, arrivalPoint);
    }

    private Point GetHandPoint(eHand hand, Point point)
    {
        var linkedForwardNode = GetOffsetNode(point);
        if (hand == eHand.left)
        {
            return (linkedForwardNode.Previous ?? linkedForwardNode.List.Last).Value;
        }
        else
        {
            return (linkedForwardNode.Next ?? linkedForwardNode.List.First).Value;
        }
    }

    private LiNode FindLiNode(Point point)
    {
        bool isInRange = Graph<LiNode>.IsInRange(point, _transformsGrid);
        LiNode node = null;
        if (isInRange)
        {
            node = _graphLi.AllNodes[point.X, point.Y];
        }

        return node;
    }

    private void RotateToForwardPoint()
    {
        var currentWaypoint = _forwardNode.VisualNode.transform;
        var dir = (currentWaypoint.position - transform.position).normalized;
        var quaternion = Quaternion.LookRotation(dir);
        var euler = quaternion.eulerAngles;
        euler.x = 0;
        transform.rotation = Quaternion.Euler(euler);
    }

    public void Die()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = _deadShipMaterial;
        Debug.Log($"{gameObject.name} is dead");
        _isAlive = false;
    }

    private void SetNodeIsBusy(LiNode node, bool busy)
    {
        node.IsBusy = busy;
        node.VisualNode.SetBusy();
    }

    //private void 
}