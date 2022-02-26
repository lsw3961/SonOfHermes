using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //basic and/or global varaibles
    private Rigidbody2D rb;
    public InputReader reader;
    private SpriteRenderer spriteRenderer;

    //Varibales needed for running
    public Vector2 lastDirection = Vector2.zero;
    private Vector2 dir = Vector2.zero;
    private float acceleration = 5;
    private float maxSpeed = 5;

    //Variables needed for Jumping
    private float jumpForce = 5;
    private float fallMultiplier = 5;
    private float lowJumpMultiplier = 5;
    private float hangTime = .2f;
    private float hangCounter = 0;
    private bool isJumping = false;
    private bool isJumpingReleased = true;
    private bool onGround = true;

    //collision check varaibles
    [SerializeField] private Vector2 bottomOffset = Vector2.zero;
    [SerializeField] private float overlapRadius = 2;
    [SerializeField] private LayerMask groundedLayer = 0;

    //player information from scriptable object
    [SerializeField]Player player;

    //animator needed for animations
    private Animator anim;
    void Start()
    {
        SetDefaultValues();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        anim.SetBool("isGrounded", true);
        anim.SetBool("isRunning", false);

    }

    private void SetDefaultValues()
    {
        acceleration = player.acceleration;
        maxSpeed = player.maxSpeed;

        jumpForce = player.jumpForce;
        fallMultiplier = player.fallMultiplier;
        lowJumpMultiplier = player.lowJumpMultiplier;
        hangTime = player.hangTime;
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
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
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

    /// <summary>
    /// Jump Release will trigger once the player takes their finger off the button. it triggers the low/high jump code
    /// </summary>
    private void JumpReleased()
    {
        isJumpingReleased = true;
        reader.JumpReleaseEvent -= JumpReleased;
    }

    /// <summary>
    /// this is the code that is the low/high jump code
    /// </summary>
    private void FixJump()
    {
        if (isJumping && hangCounter > 0 && !(rb.velocity.y>0))
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

    /// <summary>
    /// Check if the player is grounded.
    /// </summary>
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
        player.isGrounded = onGround;
    }

    /// <summary>
    /// cayote time 
    /// </summary>
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
