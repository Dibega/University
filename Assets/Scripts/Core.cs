using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Core : MonoBehaviour
{
    [SerializeField] private int _gridWidth = 30;

    [Header("Visual")] 
    [SerializeField] private VisualController _visualController;
    
    [Header("Ship")]
    [SerializeField] private int _shipCount = 2;
    [SerializeField] private Ship _shipPrefab;
    
    [SerializeField] private float _minDraft = -0.5f;
    public float MinDraft => _minDraft;

    [SerializeField] private float _maxDraft = -3;
    public float MaxDraft => _maxDraft;
    
    [Header("Terrain depth")]
    [SerializeField] private float minDepth = -5;
    [SerializeField] private float maxDepth = 2;
    
    private Li _algorithmLi;
    private Graph<LiNode> _graphLi;

    private List<Ship> _ships = new List<Ship>();
    // Start is called before the first frame update
    void Awake()
    {
        _graphLi = new Graph<LiNode>(_gridWidth);
        _graphLi.GenerateDepths(minDepth, maxDepth);
        
        _algorithmLi = new Li(_graphLi);
        _visualController.CreateTerrain(_graphLi.AllNodes);
        
        for (int i = 0; i < _shipCount; i++)
        {
            var ship = Instantiate(_shipPrefab,Vector3.zero,Quaternion.identity);
            ship.gameObject.name += $"_{i}";
            ship.Init(this,_algorithmLi,_graphLi,_visualController.Step,_visualController.GridTransform,_minDraft,_maxDraft);
            _ships.Add(ship);
        }
    }

    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillShipsOnPoint(Point point)
    {
        var pointTransform = _visualController.GridTransform[point.X, point.Y];
        foreach (var ship in _ships)
        {
            var shipPos = ship.transform.position;
            if (shipPos.x + 0.1 >= pointTransform.position.x && shipPos.x - 0.1 <= pointTransform.position.x &&
                shipPos.z + 0.1 >= pointTransform.position.z && shipPos.z - 0.1 <= pointTransform.position.z)
            {
                ship.Die();
                ship.enabled = false;
            }
        }
    }
}
