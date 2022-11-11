using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadColliderScript : MonoBehaviour
{
    [SerializeField] private SlimeController slimeController;
    
    private GameObject _lastCol;
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        col.GetComponent<IEffect>()?.OnTrigger(slimeController);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == _lastCol) return;
        _lastCol = collision.gameObject;
        slimeController.Collision(collision);
    }
}
