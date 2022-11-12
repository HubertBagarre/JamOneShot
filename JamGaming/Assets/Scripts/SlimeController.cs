using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Vector2 inputAxis;
    [SerializeField] private Transform slimeBody;
    [SerializeField] private Transform slimeBase;
    [SerializeField] private EffectTriggerer fez;
    [SerializeField] private float borneArrow;
    [SerializeField] private float speed;
    [SerializeField] private float accelFactor;
    [SerializeField] private float launchStrength;
    [SerializeField] private float jumpTimerAtLanding;
    [SerializeField] private Animator animator;

    public PlayerInfo infos;

    public Vector2 normalContact;
    private Vector2 _launchDirection;
    private Vector2 _lastAllowedDirection;
    private bool _split;
    public int _remainingRebound;
    public int _maxRebound;
    [SerializeField] private float _timer;

    public Rigidbody2D slimeRb;
    public bool onWall;
    public bool canLook;
    public bool canJump;
    public bool travelling;

    private WaitForSeconds jumpWait;
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Bounce = Animator.StringToHash("Bounce");
    private static readonly int Land = Animator.StringToHash("Land");

    private void Start()
    {
        onWall = true;
        infos = GetComponent<PlayerInfo>();
        animator = GetComponent<Animator>();
        jumpWait = new WaitForSeconds(0.1f);
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        UpdateAxis();
        UpdateBodyRotation();
    }

    private void UpdateBodyRotation()
    {
        if (!canLook) return;
        if (travelling) return;
        slimeBody.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, inputAxis));
    }

    private void UpdateAxis()
    {
        if (normalContact == Vector2.zero) return;
        if (inputAxis.magnitude < 0.3) inputAxis = normalContact;
        if (Vector3.Dot(inputAxis, normalContact) < borneArrow) inputAxis = _lastAllowedDirection;
        else _lastAllowedDirection = inputAxis;
    }

    public void Collision(Collision2D col)
    {
        fez.canKill = true;
        normalContact = col.GetContact(0).normal;
        if (_remainingRebound > 0)
        {
            _remainingRebound--;
            _launchDirection = Vector2.Reflect(_launchDirection, normalContact);
            slimeRb.velocity = _launchDirection.normalized * speed *
                               (1 + (_maxRebound + 1 - _remainingRebound) * accelFactor);
            animator.SetTrigger(Bounce);
            Launch();
        }
        else
        {
            travelling = false;
            animator.SetTrigger(Land);
            fez.enabled = false;
            _lastAllowedDirection = normalContact;
            transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, normalContact));
            slimeRb.velocity = Vector2.zero;
            slimeRb.bodyType = RigidbodyType2D.Kinematic;
            _timer = 0;
            onWall = true;
            ShowBase(true);
        }
    }

    private IEnumerator LateJump()
    {
        yield return jumpWait;
        ShowBase(false);
        Launch();
    }

    private void Launch()
    {
        if (!canJump) return;
        if (_timer < jumpTimerAtLanding) return;
        onWall = false;
        travelling = true;
        fez.enabled = true;
        slimeRb.bodyType = RigidbodyType2D.Dynamic;
        transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, _launchDirection));
        slimeBody.localRotation = Quaternion.Euler(0, 0, 0);
        slimeRb.AddForce(_launchDirection * launchStrength, ForceMode2D.Impulse);
        normalContact = Vector2.zero;
    }

    public void Deflect()
    {
        canJump = true;
        slimeRb.velocity = Vector2.zero;
        _launchDirection = -_launchDirection;
        Launch();
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
    }

    public void NoRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        animator.SetTrigger(Jump);
        StartCoroutine(LateJump());
    }

    public void OneRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 1;
        _maxRebound = _remainingRebound;
        StartCoroutine(LateJump());
    }

    public void TwoRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 2;
        _maxRebound = _remainingRebound;
        StartCoroutine(LateJump());
    }

    public void ThreeRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 3;
        _maxRebound = _remainingRebound;
        StartCoroutine(LateJump());
    }

    public void ShowBase(bool value)
    {
        slimeBase.gameObject.SetActive(value);
    }
}