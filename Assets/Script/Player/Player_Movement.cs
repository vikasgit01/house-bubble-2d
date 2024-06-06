using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Utils;

//This script is on bubble GameObject
public class Player_Movement : MonoBehaviour
{


    private Rigidbody2D _rb2d;
    private Animator _playerAnimator;
    private Vector3 _mousePoint;
    private Vector2 _dir;
    private Vector2 _movement;
    private bool _isMoveing;
    
    private ParticleSystem _footStepsParticalSystem;


    [SerializeField] private float movSpeedMouse = 5f;
    [SerializeField] private float moveSpeedKeyboard = 100f;


    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Player.Instance.canMove = true;
        _footStepsParticalSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        #region MouseClickMovement
        //MouseClickMovement();
        #endregion

        if (Player.Instance.canMove)
        {
          #region Keybord Inputs
            KeyboardInputs();
            #endregion


        }
        else
        {
            PlayerCantMove();
        }

            #region Ideal animation
            IdealAnimation();
        #endregion

      
    }

    private void FixedUpdate()
    {
        movingPlayer();
    }

    private void MouseClickMovement()
    {
        if (Input.GetMouseButton(0))
        {
            _mousePoint = Utils.GetMouseWorldPosition();
            _isMoveing = true;
        }

        if (_isMoveing && transform.position != _mousePoint)
        {
            Vector2 aimDir = Utils.GetDirToMouse(transform.position);
            _playerAnimator.SetFloat("Speed", aimDir.sqrMagnitude);

            Vector2 targetDirection = aimDir.normalized;
            _playerAnimator.SetFloat("Horizontal", targetDirection.x);
            _playerAnimator.SetFloat("Vertical", targetDirection.y);

            transform.position = Vector2.MoveTowards(transform.position, _mousePoint, movSpeedMouse * Time.deltaTime);

        }
        else
        {
            _isMoveing = false;
        }

    }

    private void IdealAnimation()
    {

        _mousePoint = Utils.GetMouseWorldPosition();
        Vector2 targetDirection = Utils.GetNormalizedDirToMouse(transform.position);
        _playerAnimator.SetFloat("IdealHorizontal", targetDirection.x);
        _playerAnimator.SetFloat("IdealVertical", targetDirection.y);

    }

    private void KeyboardInputs()
    {
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        _dir = new Vector2(_movement.x, _movement.y).normalized;

        #region PlayerAnimations
        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
        _playerAnimator.SetFloat("Speed", _movement.sqrMagnitude);
        #endregion 
    }

    private void movingPlayer()
    {
        _rb2d.velocity = _dir * moveSpeedKeyboard * Time.fixedDeltaTime;
    }

    private void PlayerCantMove()
    {
        _rb2d.velocity = Vector3.zero;
        _dir = Vector3.zero;
        _playerAnimator.SetFloat("Horizontal", _movement.x);
        _playerAnimator.SetFloat("Vertical", _movement.y);
        _playerAnimator.SetFloat("Speed", 0);
    }

    public  ParticleSystem footStepsEffect()
    {
        return _footStepsParticalSystem;
    }
}
