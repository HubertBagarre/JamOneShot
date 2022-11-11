using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D slimeRb;
    [SerializeField] private Vector2 inputAxis;
    [SerializeField] private Transform slimeBody;
    [SerializeField] private float borneArrow;
    [SerializeField] private float stretchAmount;
    [SerializeField] private float speed;
    [SerializeField] private float accelFactor;
    [SerializeField] private float launchStrength;
    
    private PlayerInfo infos;
    
    private Vector2 _normalContact;
    private Vector2 _launchDirection;
    private bool _split;
    private int _remainingRebound;
    private int _maxRebound;

    public bool onWall;
    public bool canLook;
    public bool canJump;
    
    private void Start()
    {
        onWall = true;
        infos = GetComponent<PlayerInfo>();
    }

    private void FixedUpdate()
    {
        if (!canLook) return;
        slimeBody.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up,inputAxis));
        slimeBody.localScale = new Vector3(1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        _normalContact =(Vector2)transform.position-col.GetContact(0).point;
        _normalContact.Normalize();
        if (_remainingRebound > 0)
        {
            _remainingRebound--;
            _launchDirection = Vector2.Reflect(_launchDirection, _normalContact);
            slimeRb.velocity = _launchDirection.normalized * speed * (1+(_maxRebound+1-_remainingRebound)*accelFactor);
            Launch();
        }
        else
        {
            onWall = true;
            slimeRb.velocity = Vector2.zero;
        }
    }

    private void Launch()
    {
        if(!canJump) return;
        slimeRb.AddForce(_launchDirection*launchStrength,ForceMode2D.Impulse);
        _normalContact = Vector2.zero;
        onWall = false;
    }
    
    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
        if (_normalContact == Vector2.zero) return;
        if (Vector3.Dot(inputAxis, _normalContact) < borneArrow) inputAxis = _normalContact;
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
