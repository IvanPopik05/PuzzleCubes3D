using DefaultNamespace;
using UnityEngine;

public class ObstaclesBoard : MonoBehaviour
{
    private const int OBSTACLE_COUNT_X = 10;
    private const int OBSTACLE_COUNT_Y = 15;
    
    [SerializeField] private ObstacleBlock _obstaclePrefab;
    [SerializeField] private float _yOffset = 0.5f;

    private TileBoard _tileBoard;
    private Camera _camera;

    public void Initialize(TileBoard tileBoard)
    {
        _camera = Camera.main;
        _tileBoard = tileBoard;
        
        _camera.transform.position = new Vector3(
            (float) _tileBoard.TileCountX / 2 - 0.5f, 
            10, 
            (float) _tileBoard.TileCountY / 2 - 0.5f);

        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        for (int x = -OBSTACLE_COUNT_X; x < OBSTACLE_COUNT_X; x++)
        {
            for (int y = -OBSTACLE_COUNT_Y; y < OBSTACLE_COUNT_Y; y++)
            {
                if (_tileBoard.TilesPos.Contains(new Vector2Int(x, y)))
                    continue;

                ObstacleBlock newObstacle = SpawnObstacleBlock(x, y);
                newObstacle.name = $"Obstacle: {x}_{y}";
            }
        }
    }

    private ObstacleBlock SpawnObstacleBlock(int x, int y)
    {
        ObstacleBlock newObstacle = Instantiate(_obstaclePrefab,
            new Vector3(x, _yOffset, y),
            Quaternion.identity, transform);

        return newObstacle;
    }
}