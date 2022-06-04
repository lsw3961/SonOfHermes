using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    public LayerMask layer;
    public bool isGrounded;
    private void Start()
    {
        isGrounded = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //use bitwise operation to determine the collision
         if ((layer.value & 1<<collision.gameObject.layer) == 1<<collision.gameObject.layer) 
        {
            isGrounded = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((layer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //use bitwise operation to determine the collision
        if ((layer.value & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            isGrounded = false;
        }
    }
}
