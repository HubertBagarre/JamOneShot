using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D slimeRb;
    [SerializeField] private Vector2 inputAxis;
    [SerializeField] private float launchStrength;
    
    private void Launch()
    {
        slimeRb.AddForce(inputAxis.normalized*launchStrength,ForceMode2D.Impulse);
    }
}
