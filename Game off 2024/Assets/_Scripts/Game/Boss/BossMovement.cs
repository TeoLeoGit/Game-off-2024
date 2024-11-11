using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 25f; // Speed of movement between cells
    public Vector2 gridPosition = new Vector2(0, 0); // Initial grid position
    public float cellSize = 1f; // Size of each cell

    private bool _isMovingRight = false;
    private bool _isMovingLeft = false;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;

    private Vector2 _playerGridPos;
    private Vector2 _targetPosition;
    private Animator _anim;
    private List<PathNode> _pathToPlayer;

    #region Cached Properties

    private int _currentState;
    private float _lockedTill;
    private float _walkRightAnimTime = 0.25f;
    private float _walkLeftAnimTime = 0.25f;
    private float _walkUpAnimTime = 0.25f;
    private float _walkDownAnimTime = 0.25f;

    private static readonly int Idle = Animator.StringToHash("Boss_idle");
    private static readonly int WalkLeft = Animator.StringToHash("Boss_walk_left");
    private static readonly int WalkRight = Animator.StringToHash("Boss_walk_right");
    private static readonly int WalkUp = Animator.StringToHash("Boss_walk_up");
    private static readonly int WalkDown = Animator.StringToHash("Boss_walk_down");

    #endregion

    private void Awake()
    {
        GameController.OnPlayerPositionUpdate += UpdatePath;

        _targetPosition = transform.position;
        _anim = GetComponentInChildren<Animator>();
        //gridPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnDestroy()
    {
        GameController.OnPlayerPositionUpdate -= UpdatePath;
    }

    void Update()
    {
        MoveBoss();
        AnimateBoss();
    }

    void UpdatePath(Vector2 playerNewPos)
    {
        if (playerNewPos == _playerGridPos) return;
        _playerGridPos = playerNewPos;
        _pathToPlayer = GameController.RequestPathToPosition(gridPosition, playerNewPos);
    }

    void MoveBoss()
    {
        if (_pathToPlayer == null) return;
        MainController.PlaySound(SoundType.BossChase);

        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _targetPosition) < 0.01f)
        {
            transform.position = _targetPosition;
            GameController.CallOnReachPathNode(gridPosition);
            if (_pathToPlayer.Count == 0)
            {
                _pathToPlayer = null;
                _isMovingRight = false;
                _isMovingLeft = false;
                _isMovingDown = false;
                _isMovingUp = false;
                return;
            }

            //Move next
            var nextnode = _pathToPlayer.First();
            var horizonDir = nextnode.x - gridPosition.x;
            var verticalDir = nextnode.y - gridPosition.y;
            if (horizonDir < 0) //Left
            {
                gridPosition.x -= 1;
                _isMovingRight = false;
                _isMovingLeft = true;
                _isMovingUp = false;
                _isMovingDown = false;
                _targetPosition = new Vector2(_targetPosition.x - cellSize, _targetPosition.y);
            }
            else if (horizonDir > 0) //Right
            {
                gridPosition.x += 1;
                _isMovingRight = true;
                _isMovingLeft = false;
                _isMovingUp = false;
                _isMovingDown = false;
                _targetPosition = new Vector2(_targetPosition.x + cellSize, _targetPosition.y);
            }
            else if (verticalDir > 0) //Up
            {
                gridPosition.y += 1;
                _isMovingRight = false;
                _isMovingLeft = false;
                _isMovingUp = true;
                _isMovingDown = false;
                _targetPosition = new Vector2(_targetPosition.x, _targetPosition.y + cellSize);
            }
            else if (verticalDir < 0) //Down
            {
                gridPosition.y -= 1;
                _isMovingRight = false;
                _isMovingLeft = false;
                _isMovingUp = false;
                _isMovingDown = true;
                _targetPosition = new Vector2(_targetPosition.x, _targetPosition.y - cellSize);
            }
            _pathToPlayer.RemoveAt(0);
        }
    }

   

    #region Animations

    void AnimateBoss()
    {
        var state = GetState();
        if (state == _currentState) return;
        _anim.CrossFade(state, 0, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;

        // Priorities
        if (_isMovingLeft) return LockState(WalkLeft, _walkLeftAnimTime);
        if (_isMovingRight) return LockState(WalkRight, _walkRightAnimTime);
        if (_isMovingUp) return LockState(WalkUp, _walkUpAnimTime);
        if (_isMovingDown) return LockState(WalkDown, _walkDownAnimTime);

        return Idle;
        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    #endregion
}
