using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 25f; // Speed of movement between cells
    public Vector2 gridPosition = new Vector2(0, 0); // Initial grid position
    public float cellSize = 1f; // Size of each cell

    [SerializeField] LayerMask _blockingMask;

    private bool _isMoving = false;
    private bool _isMovingUp = false;
    private bool _isMovingDown = false;
    private Vector2 _gizmos;
    private Vector3 targetPosition;
    private Animator _anim;

    #region Cached Properties

    private int _currentState;
    private float _lockedTill;
    private float _walkAnimTime = 0.12f;
    private float _walkUpAnimTime = 0.15f;
    private float _walkDownAnimTime = 0.15f;

    private static readonly int Idle = Animator.StringToHash("Player_idle");
    private static readonly int WalkHorizontal = Animator.StringToHash("Player_walk");
    private static readonly int WalkUp = Animator.StringToHash("Player_walk_up");
    private static readonly int WalkDown = Animator.StringToHash("Player_walk_down");
    private static readonly int Attack = Animator.StringToHash("Attack");

    #endregion

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        gridPosition = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        MoveBoss();
        AnimateBoss();
    }

    void UpdatePath()
    {
        if (_isMoving) return;

        {
            if (!CanMove((Vector2.up))) return;
            gridPosition.y += 1;
            _isMoving = true;
            _isMovingUp = true;
            _isMovingDown = false;
        }
        {
            if (!CanMove((Vector2.down))) return;
            gridPosition.y -= 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = true;
        }
        {
            if (!CanMove((Vector2.left))) return;
            transform.localScale = Vector3.one + Vector3.left * 2;
            gridPosition.x -= 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = false;
        }
        {
            if (!CanMove((Vector2.right))) return;
            transform.localScale = Vector3.one;
            gridPosition.x += 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = false;
        }

        if (_isMoving)
        {
            SetTargetPosition();
        }
    }

    void MoveBoss()
    {
        if (!_isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            _isMoving = false;
        }
    }

    void SetTargetPosition()
    {
        targetPosition = new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0);
    }

    public bool CanMove(Vector3 direction)
    {
        var hits = Physics2D.RaycastAll(transform.position, direction, 1, _blockingMask);
        _gizmos = transform.position + direction;

        return hits.Length == 0;
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

        if (_isMoving)
        {
            if (_isMovingUp) return LockState(WalkUp, _walkUpAnimTime);
            if (_isMovingDown) return LockState(WalkDown, _walkDownAnimTime);

            return LockState(WalkHorizontal, _walkAnimTime);
        }
        return Idle;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }

    #endregion
}
