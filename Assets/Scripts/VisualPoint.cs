using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class VisualPoint : MonoBehaviour
    {
        [SerializeField] private TextMesh _textMesh;
        [SerializeField] private TextMesh _busyText;
        private LiNode _liNode;
        public LiNode LiNode => _liNode;
        
        public void Init(LiNode liNode)
        {
            _liNode = liNode;
            _liNode.SetGraphics(this);
            _textMesh.text = $" Depth:\n    {_liNode.Depth:F2}\n\n    [{liNode.Point.X},{liNode.Point.Y}]";
        }

        public void SetBusy()
        {
            _busyText.text = _liNode.IsBusy ? "true" : "";
        }
    }
}