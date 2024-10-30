using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce = 10f;

    //public void PlatformInteraction()
    //{
    //    AddForce(Vector3.up * jumpForce, ForceMode.Impulse();
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

}


