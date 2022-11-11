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

    private void FixedUpdate()
    {
        arrow.localScale = new Vector3(inputAxis.magnitude+1,1,0);
        arrow.localPosition = new Vector3(inputAxis.x, inputAxis.y, 0);
        arrow.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.right,inputAxis));
    }

    public void Launch(InputAction.CallbackContext ctx)
    {
        slimeRb.AddForce(inputAxis.normalized*launchStrength,ForceMode2D.Impulse);
    }

    public void OnMoveInput(InputAction.CallbackContext ctx)
    {
        inputAxis = ctx.ReadValue<Vector2>();
    }
}
