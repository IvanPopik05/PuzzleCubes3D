using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace
{
   public class TileBoard : MonoBehaviour
   {
       private const int TILE_COUNT_X = 5;
       private const int TILE_COUNT_Y = 5;
       
       [SerializeField] private Tile _tilePrefab;
       [SerializeField] private float _tileSize;
       [SerializeField] private List<Vector2Int> _skipTiles;
       [SerializeField] private List<Vector2Int> _addTiles;
       
        private List<Tile> _tiles;
        private List<Vector2Int> _tilesPos;
        public List<Tile> Tiles => _tiles;
        public List<Vector2Int> TilesPos => _tilesPos;
        public int TileCountX => TILE_COUNT_X;
        public int TileCountY => TILE_COUNT_Y;

        public void Initialize() => 
            GenerateAll(TILE_COUNT_X,TILE_COUNT_Y,transform);

        private void GenerateAll(int tileCountX, int tileCountY, Transform parent)
        {
            _tiles = new List<Tile>();
            _tilesPos = new List<Vector2Int>();
            for (int x = 0; x < tileCountX; x++)
            {
                for (int y = 0; y < tileCountY; y++)
                {
                    if(_skipTiles.Contains(new Vector2Int(x,y)))
                        continue;
                    
                    _tiles.Add(GenerateSingle(_tileSize, x, y,parent));
                    _tilesPos.Add(new Vector2Int(x,y));
                }
            }

            foreach (Vector2Int addTilePos in _addTiles)
            {
                if (!_tilesPos.Contains(addTilePos))
                {
                    _tiles.Add(GenerateSingle(_tileSize, addTilePos.x, addTilePos.y,parent));
                    _tilesPos.Add(addTilePos);
                }
            }
        }

        private Tile GenerateSingle(float tileSize, int x, int y,Transform parent)
        {
            Tile newTile = Instantiate(_tilePrefab, new Vector3(x, 0, y), Quaternion.identity, parent);
            newTile.name = $"Tile: {x}_{y}";
            newTile.transform.localScale *= tileSize;
            return newTile;
        }

#if UNITY_EDITOR
       private void OnDrawGizmosSelected()
       {
           for (int x = 0; x < TILE_COUNT_X; x++)
           {
               for (int y = 0; y < TILE_COUNT_Y; y++)
               {
                   if(_skipTiles.Contains(new Vector2Int(x,y)))
                       continue;
                    
                   Gizmos.color = Color.white;
                   Handles.DrawWireCube(new Vector3(x,0,y), Vector3.up);
               }
           }
            
           foreach (Vector2Int addTilePos in _addTiles)
           {  
               Gizmos.color = Color.white;
               Handles.DrawWireCube(new Vector3(addTilePos.x,0,addTilePos.y), Vector3.up);
           }
       } 
#endif
   }
}