using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CharacterMovement : MonoBehaviour
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
        //gridPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private IEnumerator Start()
    {
        yield return null;
        yield return null;
        yield return null;

        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        MoveCharacter();
        AnimateCharacter();
    }

    void HandleInput()
    {
        if (_isMoving) return;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (!CanMove((Vector2.up))) return;
            gridPosition.y += 1;
            _isMoving = true;
            _isMovingUp = true;
            _isMovingDown = false;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y + cellSize);

        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            if (!CanMove((Vector2.down))) return;
            gridPosition.y -= 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = true;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y - cellSize);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (!CanMove((Vector2.left))) return;
            transform.localScale = Vector3.one + Vector3.left * 2;
            gridPosition.x -= 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = false;
            targetPosition = new Vector3(targetPosition.x - cellSize, targetPosition.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (!CanMove((Vector2.right))) return;
            transform.localScale = Vector3.one;
            gridPosition.x += 1;
            _isMoving = true;
            _isMovingUp = false;
            _isMovingDown = false;
            targetPosition = new Vector3(targetPosition.x + cellSize, targetPosition.y);
        }
    }

    void MoveCharacter()
    {
        if (!_isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            GameController.CallOnPlayerPositonUpdate(gridPosition);
            _isMoving = false;
        }
    }

    public bool CanMove(Vector3 direction)
    {
        var hits = Physics2D.RaycastAll(transform.position, direction, 1, _blockingMask);
        _gizmos = transform.position + direction;

        return hits.Length == 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _gizmos);
    }

    #region Animations

    void AnimateCharacter()
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
