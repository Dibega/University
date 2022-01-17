using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField] private GameObject _pointPrefab;
    [SerializeField] private VisualPoint _terrainPrefab;
    [SerializeField] private float _step;
    public float Step => _step;

    [SerializeField] private List<Material> _materials;
    private Transform[,] _gridTransform;
    public Transform[,] GridTransform => _gridTransform;
    
    private VisualPoint[,] _gridVisual;
    public VisualPoint[,] GridVisual => _gridVisual;

    public void CreateTerrain(LiNode[,] allNodes)
    {
        _gridTransform = new Transform[allNodes.GetUpperBound(0) + 1, allNodes.GetUpperBound(1) + 1];
        _gridVisual = new VisualPoint[allNodes.GetUpperBound(0) + 1, allNodes.GetUpperBound(1) + 1];
        
        for (int i = 0; i <= _gridTransform.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= _gridTransform.GetUpperBound(1); j++)
            {
                var point = Instantiate(_pointPrefab, new Vector3(i * _step, 0, j * _step), Quaternion.identity);
                point.transform.SetParent(transform);
                _gridTransform[i, j] = point.transform;

                VisualPoint visualTerrain = Instantiate<VisualPoint>(_terrainPrefab, point.transform);
                visualTerrain.Init(allNodes[i, j]); 
                
                var localPosition = visualTerrain.transform.position;
                visualTerrain.transform.position = new Vector3(localPosition.x, allNodes[i, j].Depth*0.1f,localPosition.z);
                visualTerrain.GetComponentInChildren<MeshRenderer>().material = GetMaterial(allNodes[i, j].Depth);

                _gridVisual[i,j] = visualTerrain;
            }
        }
    }

    private Material GetMaterial(float depth)
    {
        if (depth > 1)
        {
            return _materials[0];
        }

        if (depth > 0)
        {
            return _materials[1];
        }

        if (depth > -0.5)
        {
            return _materials[2];
        }

        if (depth > -1)
        {
            return _materials[3];
        }

        if (depth > -2)
        {
            return _materials[4];
        }
        
        if (depth > -3)
        {
            return _materials[5];
        }

        return _materials[6];
    }
    
}