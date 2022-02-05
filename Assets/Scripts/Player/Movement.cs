using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //basic and/or global varaibles
    private Rigidbody2D rb;
    public InputReader reader;

    //move direcions
    private Vector2 dir = Vector2.zero;
    private Vector2 lastDirection = Vector2.zero;

    [SerializeField] private float acceleration = 5;
    [SerializeField] private float maxSpeed = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    #region Enable & Disable
    private void OnEnable()
    {
        reader.MoveEvent += Move;
    }

    private void OnDisable()
    {
        reader.MoveEvent -= Move;

    }

    #endregion

    private void FixedUpdate()
    {
        Walk(dir);
    }

    /// <summary>
    /// these methods are the movement methods and deal with how the player moves
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        dir = direction;
        if (dir != Vector2.zero)
        {
            lastDirection = dir;
        }
        if (lastDirection == Vector2.left)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }
    private void Walk(Vector2 dir)
    {
        float finalSpeed = rb.velocity.x;
        finalSpeed += (dir.x * acceleration);
        finalSpeed = Mathf.Clamp(finalSpeed, -maxSpeed, maxSpeed);
        rb.velocity = (new Vector2(finalSpeed, rb.velocity.y));
        
    }
}
