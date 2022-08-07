using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private Block _blockPrefab;
        [SerializeField] private List<BlockType> _blockTypes;

        [Header("Amount Blocks")] 
        [SerializeField] private int _amountFirst, 
            _amountSecond,
            _amoundThird,
            _amountFourth;
        
        private List<Block> _blocks = new List<Block>();
        private TileBoard _tileBoard;

        public List<Block> Blocks => _blocks;
        private BlockType GetBlockTypeValue(int value) => 
            _blockTypes.First(v => v.Value == value);
        public void Initialize(TileBoard tileBoard)
        {
            _tileBoard = tileBoard;
        }

        public void SpawnBlocks()
        {
            SpawnTypeBlocks(1, _amountFirst);
            SpawnTypeBlocks(2, _amountSecond);
            SpawnTypeBlocks(3, _amoundThird);
            SpawnTypeBlocks(4, _amountFourth);
        }

        private void SpawnTypeBlocks(int value,int amountFirst)
        {
            var freeTiles = _tileBoard.Tiles
                .Where(n => !n.OccupiedBlock)
                .OrderBy(b => Random.value).ToList();
            
            foreach (Tile tile in freeTiles.Take(amountFirst))
            {
                SpawnBlock(tile, value);
            }
        }

        public void SpawnBlock(Tile tile, int value)
        {
            Block block = Instantiate(_blockPrefab, 
                new Vector3(tile.Pos.x, tile.Pos.y + 0.5f, tile.Pos.z),
                Quaternion.identity,transform);
                
            block.Initialize(GetBlockTypeValue(value));
            block.SetBlock(tile);
            _blocks.Add(block);
        }

        public void RemoveBlock(Block block)
        {
            _blocks.Remove(block);
            Destroy(block.gameObject);
        }
    }
    [Serializable]
    public struct BlockType
    {
        public int Value;
        public Color Color;
    }
}