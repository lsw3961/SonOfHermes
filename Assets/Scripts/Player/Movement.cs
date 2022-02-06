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

    //jump variables
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private float fallMultiplier = 5;
    [SerializeField] private float lowJumpMultiplier = 5;
    [SerializeField] private float hangTime = .2f;
    private float hangCounter = 0;
    private bool isJumping = false;
    private bool isJumpingReleased = true;
    private bool onGround = true;
    [SerializeField] private Vector2 bottomOffset = Vector2.zero;
    [SerializeField] private float overlapRadius = 2;
    [SerializeField] private LayerMask groundedLayer = 0;

    private Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetBool("isGrounded", true);
        anim.SetBool("isMoving", false);

    }

    #region Enable & Disable
    private void OnEnable()
    {
        reader.MoveEvent += Move;
        reader.JumpEvent += Jump;

    }

    private void OnDisable()
    {
        reader.MoveEvent -= Move;
        reader.JumpEvent -= Jump;

    }

    #endregion

    private void FixedUpdate()
    {
        Walk(dir);
        HangTime();
        IsGrounded();
        FixJump();
    }

    /// <summary>
    /// these methods are the movement methods and deal with how the player moves
    /// </summary>
    /// <param name="direction"></param>
    private void Move(Vector2 direction)
    {
        dir = direction;
        if (dir == Vector2.zero) 
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool("isRunning", false);
            return;
        }
        anim.SetBool("isRunning", true);
        if (dir != Vector2.zero)
        {
            lastDirection = dir;
            rb.velocity = new Vector2(0, rb.velocity.y);
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

    /// <summary>
    /// these methods for checking jumping, 1 to read from the input reader, 1 to release from the input reader, and 1 to do the actualy calcuations 
    /// </summary>
    private void Jump()
    {
        isJumping = true;
        isJumpingReleased = false;
        reader.JumpReleaseEvent += JumpReleased;

    }
    private void JumpReleased()
    {
        isJumpingReleased = true;
        reader.JumpReleaseEvent -= JumpReleased;
    }
    private void FixJump()
    {
        if (isJumping && hangCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity += Vector2.up * jumpForce;
            anim.SetBool("isGrounded", false);
            anim.SetTrigger("Jumping");

        }
        if (rb.velocity.y < 0)
        {

            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && isJumpingReleased)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
        isJumping = false;


    }
    private void IsGrounded()
    {
        if (Physics2D.OverlapCircle((Vector2)this.transform.position + bottomOffset, overlapRadius, groundedLayer))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            onGround = true;
            anim.SetBool("isGrounded",true);
        }
        else
        {
            onGround = false;
        }
    }
    private void HangTime()
    {
        if (onGround)
        {
            hangCounter = hangTime;
        }
        else
        {
            hangCounter -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere((Vector2)this.transform.position + bottomOffset, overlapRadius);
    }
}
