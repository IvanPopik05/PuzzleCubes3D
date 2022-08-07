using DefaultNamespace;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Block OccupiedBlock { get; set; }
    public ObstacleBlock OccupiedObstacleBlock { get; set; }
    public Vector3 Pos => transform.position;
    public Vector3 BlockPos => transform.position + (Vector3.up * 0.5f);
}