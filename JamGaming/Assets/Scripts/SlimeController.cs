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
    [SerializeField] private float launchStrength;
    [SerializeField] private Transform arrow;

    private bool _onWall;
    private Vector2 _normalContact;
    private bool _split;

    private void Start()
    {
        _onWall = true;
    }

    private void FixedUpdate()
    {
        arrow.localScale = new Vector3(inputAxis.magnitude+1,1,0);
        arrow.localPosition = new Vector3(inputAxis.x, inputAxis.y, 0);
        arrow.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.right,inputAxis));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        slimeRb.gravityScale = 0;
        _normalContact =(Vector2)transform.position-col.GetContact(0).point;
        _normalContact.Normalize();
        _onWall = true;
        slimeRb.velocity = Vector2.zero;
    }

    public void Launch(InputAction.CallbackContext ctx)
    {
        if (!_onWall) return;
        slimeRb.AddForce(inputAxis.normalized*launchStrength,ForceMode2D.Impulse);
        _normalContact = Vector2.zero;
        _onWall = false;
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
        if (Vector3.Dot(inputAxis, _normalContact) < 0) inputAxis = new Vector2(0, 0);
    }
}
