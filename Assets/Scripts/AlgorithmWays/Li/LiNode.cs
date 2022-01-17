using System;
using DefaultNamespace;
using Random = UnityEngine.Random;

[Serializable]
public class LiNode : GraphNode
{
    public int number;
    public float Depth;
    public bool IsBusy = false;
    private VisualPoint _visualNode;
    public VisualPoint VisualNode => _visualNode;

    public LiNode() : base()
    {
        SetDefault();
    }

    public LiNode(Point point) : base(point)
    {
        SetDefault();
    }
    public LiNode(INode graphNode) : base(graphNode)
    {
        SetDefault();
    }

    private void SetDefault()
    {
        number = -1;
        Depth = 0;
    }
    
    public void GenerateDepths(float minDepth, float maxDepth)
    {
        Depth = Random.Range(minDepth, maxDepth);
    }

    public void SetGraphics(VisualPoint visualNode)
    {
        _visualNode = visualNode;
    }
}
