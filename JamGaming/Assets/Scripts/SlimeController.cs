using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D slimeRb;
    [SerializeField] private Vector2 inputAxis;
    [SerializeField] private float launchStrength;
    [SerializeField] private Transform arrow;

    private bool onWall;
    private Vector2 normalContact;

    private void Start()
    {
        onWall = true;
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
        normalContact = col.GetContact(0).normal;
        onWall = true;
        slimeRb.velocity = Vector2.zero;
    }

    public void Launch(InputAction.CallbackContext ctx)
    {
        if (!onWall) return;
        slimeRb.AddForce(inputAxis.normalized*launchStrength,ForceMode2D.Impulse);
        normalContact = Vector2.zero;
        onWall = false;
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
    }
}
