using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using InputSystem;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        private readonly Vector3 leftOrRightTurnDirectionEffect = new Vector3(0,90,0);
        private readonly Vector3 backOrForwardTurnDirectionEffect = new Vector3(0,180,0);
        
        [SerializeField] private ObstaclesBoard _obstaclesBoard;
        [SerializeField] private TileBoard _tileBoard;
        [SerializeField] private BlockSpawner _blocksSpawner;
        [SerializeField] private float _travelTimeBlock = 0.8f;
        [SerializeField] private float _rotateDuration = 0.05f;
        [SerializeField] private int angleRotation = 20;
        [SerializeField] private Ease _moveEase = Ease.InExpo;
        [SerializeField] private Ease _rotateEase;
        
        private IInput _inputService;
        private GameState _currentState;
        
        private Vector3 _directionTrailEffect;

        private int _winCondition = 5;

        private void Start()
        {
            _inputService = GetInputDevice();
            _tileBoard.Initialize();
            _obstaclesBoard.Initialize(_tileBoard);
            _blocksSpawner.Initialize(_tileBoard);
            
            _blocksSpawner.SpawnBlocks();

            ChangeState(GameState.WaitingInput);
            _inputService.Pressed += MovementFromDirection;
        }

        private void Update()
        {
            if (_currentState != GameState.WaitingInput)
                return;

            _inputService.UpdateInput();
        }

        private void ChangeState(GameState newState)
        {
            _currentState = newState;

            switch (_currentState)
            {
                case GameState.WaitingInput:
                    break;
                case GameState.Moving:
                    break;
                case GameState.Win:
                    break;
                case GameState.Lose:
                    break;
            }
        }

        private void MovementFromDirection(Vector3 dir)
        {
            ChangeState(GameState.Moving);

            List<Block> orderBlocks = _blocksSpawner.Blocks
                .OrderBy(b => b.Pos.x)
                .ThenBy(b => b.Pos.y).ToList();

            if (dir == Vector3.right || dir == Vector3.forward)
            {
                orderBlocks.Reverse();
            }

            _directionTrailEffect = dir == Vector3.right || dir == Vector3.left ? leftOrRightTurnDirectionEffect : backOrForwardTurnDirectionEffect;
            
            foreach (Block block in orderBlocks)
            {
                block.TurnTrailEffect(_directionTrailEffect);
                Tile next = block.Tile;
                do
                {
                    block.SetBlock(next);
                    Tile possibleTile = GetTileAtPosition(next.Pos + dir);

                    if (possibleTile)
                    {
                        if (possibleTile.OccupiedBlock && possibleTile.OccupiedBlock.CanMerge(block.Value))
                            block.MergeBlock(possibleTile.OccupiedBlock);
                        else if (possibleTile.OccupiedBlock == null) 
                            next = possibleTile;
                    }
                } while (next != block.Tile);
            }

            var sequence = DOTween.Sequence();

            foreach (Block block in orderBlocks)
            {
                Vector3 movePoint = block.MergingBlock != null ? block.MergingBlock.Tile.Pos : block.Tile.BlockPos;
                sequence.Insert(0, block.transform.DOMove(movePoint, _travelTimeBlock).SetEase(_moveEase));

                sequence.Insert(0, block.transform.DOLocalRotate(ChooseDirectionOfRotationCube(dir) * angleRotation,  _rotateDuration)).SetEase(_rotateEase);
                sequence.Insert(1, block.transform.DOLocalRotate(Vector3.zero,  _rotateDuration)).SetEase(_rotateEase);
            }

            sequence.OnComplete(() =>
            {
                foreach (Block block in orderBlocks.Where(b => b.MergingBlock)) 
                    MergeBlocks(block.MergingBlock, block);
            
                ChangeState(GameState.WaitingInput);
            });
        }

        private void MergeBlocks(Block baseBlock, Block mergingBlock)
        {
            _blocksSpawner.SpawnBlock(baseBlock.Tile,baseBlock.Value + 1);
            
            _blocksSpawner.RemoveBlock(baseBlock);
            _blocksSpawner.RemoveBlock(mergingBlock);
        }

        private Tile GetTileAtPosition(Vector3 pos) =>
            _tileBoard.Tiles.FirstOrDefault(t => t.Pos == pos);

        private Vector3 ChooseDirectionOfRotationCube(Vector3 dir)
        {
            if (dir == Vector3.right) 
                return Vector3.back;
            if (dir == Vector3.left) 
                return Vector3.forward;
            if (dir == Vector3.forward) 
                return Vector3.left;
            
            return dir == Vector3.back ? Vector3.right : Vector3.zero;
        }

        private IInput GetInputDevice()
        {
            if (Application.isEditor) 
                return new DesktopInput();
            
            return new MobileInput();
        }
    }

    public enum GameState
    {
        WaitingInput,
        Moving,
        Win,
        Lose
    }
}