using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private int _value;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _trailEffect;
        [SerializeField] private TextMeshPro _valueTextPro;
        
        private bool _isMerging;
        public Vector3 Pos => transform.position;
        public int Value => _value;
        public Block MergingBlock { get; set; }
        public Tile Tile { get; set; }

        public void Initialize(BlockType type)
        {
            _value = type.Value;
            _meshRenderer.material.SetColor("_EmissionColor", type.Color);
            _trailEffect.startColor = type.Color;
            _valueTextPro.text = _value.ToString();
        }

        public void SetBlock(Tile tile)
        {
            if (Tile)
                Tile.OccupiedBlock = null;

            Tile = tile;
            Tile.OccupiedBlock = this;
        }
        public void TurnTrailEffect(Vector3 dir) => 
            _trailEffect.transform.rotation = Quaternion.Euler(dir);

        public void MergeBlock(Block blockToMergeWith)
        {
            MergingBlock = blockToMergeWith;

            Tile.OccupiedBlock = null;
            blockToMergeWith._isMerging = true;
        }

        public bool CanMerge(int value) =>
            value == Value && !_isMerging && MergingBlock == null;
    }
}