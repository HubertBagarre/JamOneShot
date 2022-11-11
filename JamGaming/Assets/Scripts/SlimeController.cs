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
    [SerializeField] private float borneArrow;
    [SerializeField] private float speed;
    [SerializeField] private float accelFactor;
    [SerializeField] private float launchStrength;
    [SerializeField] private float jumpTimerAtLanding;
    
    private PlayerInfo infos;
    
    private Vector2 _normalContact;
    private Vector2 _launchDirection;
    private Vector2 _lastAllowedDirection;
    private bool _split;
    private int _remainingRebound;
    private int _maxRebound;
    [SerializeField] private float _timer;

    public Rigidbody2D slimeRb;
    public bool onWall;
    public bool canLook;
    public bool canJump;
    public bool travelling;
    
    private void Start()
    {
        onWall = true;
        infos = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (!canLook) return;
        if (travelling) return;
        _timer += Time.deltaTime;
        slimeBody.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,inputAxis));
    }

    public void Collision(Collision2D col)
    {
        _normalContact =col.GetContact(0).normal;
        if (_remainingRebound > 0)
        {
            _remainingRebound--;
            _launchDirection = Vector2.Reflect(_launchDirection, _normalContact);
            slimeRb.velocity = _launchDirection.normalized * speed * (1+(_maxRebound+1-_remainingRebound)*accelFactor);
            Launch();
        }
        else
        {
            travelling = false;
            _lastAllowedDirection = _normalContact;
            transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,_normalContact));
            slimeRb.velocity = Vector2.zero;
            _timer = 0;
            onWall = true;
        }
    }

    private void Launch()
    {
        if(!canJump) return;
        if (_timer < jumpTimerAtLanding) return;
        onWall = false;
        travelling = true;
        transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,_launchDirection));
        slimeBody.localRotation = Quaternion.Euler(0,0,0);
        slimeRb.AddForce(_launchDirection*launchStrength,ForceMode2D.Impulse);
        _normalContact = Vector2.zero;
    }
    
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
        if (_normalContact == Vector2.zero) return;
        
        if (Vector3.Dot(inputAxis, _normalContact) < borneArrow) inputAxis = _lastAllowedDirection;
        else _lastAllowedDirection = inputAxis;
    }
    public void NoRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        Launch();
    }
    public void OneRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 1;
        _maxRebound = _remainingRebound;
        Launch();
    }
    
    public void TwoRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 2;
        _maxRebound = _remainingRebound;
        Launch();
    }
    
    public void ThreeRebound(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        if (inputAxis.sqrMagnitude == 0) return;
        _launchDirection = inputAxis.normalized;
        _remainingRebound = 3;
        _maxRebound = _remainingRebound;
        Launch();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IEffect>()?.OnTrigger(this);
    }
}
