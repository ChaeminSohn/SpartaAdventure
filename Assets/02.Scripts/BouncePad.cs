using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [Header("������ ����")]
    [SerializeField] private float bouncePower;
    [SerializeField] private LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if(collision.gameObject.TryGetComponent<PlayerMovementCtrl>(out PlayerMovementCtrl player))
            {
                player.Jump(bouncePower);
            }
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddForce(Vector2.up * bouncePower, ForceMode.Impulse);
            }
        }
    }
}
