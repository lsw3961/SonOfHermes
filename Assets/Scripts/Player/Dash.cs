using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    //dash variables
    [SerializeField] private float dashStartTime = 2f;
    [SerializeField] private float dashSpeed = 5f;
    private float dashTime = 0f;
    private bool isDashing = false;
    Movement movement;
    Vector2 lastDirection = Vector2.zero;
    Rigidbody2D rb;
    GameObject dashIndicator;
    [SerializeField] GameObject dashTarget;
    [SerializeField] InputReader reader;
    [SerializeField] float radius = 1;



    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
    }


    #region Enable & Disable
    private void OnEnable()
    {
        reader.DashEvent += DashTrigger;

    }

    private void OnDisable()
    {
        reader.DashEvent -= DashTrigger;

    }

    #endregion
    // Update is called once per frame
    void FixedUpdate()
    {
        if (lastDirection != movement.lastDirection) 
            lastDirection = movement.lastDirection;
        DashAction();
        DashTimeCounter();
        DashIndicator();
    }

    private void DashTrigger()
    {
        isDashing = true;
    }
    private void DashAction()
    {

        if (dashTime <= 0 && isDashing)
        {
            if (lastDirection.x == 0)
            {
                lastDirection.x = 1;
            }
            Vector3 ScreenMouse = Camera.main.ScreenToWorldPoint(reader.MousePosition);

            dashTime = dashStartTime;
            //rb.velocity += new Vector2((ScreenMouse.x * dashSpeed), (ScreenMouse.y * dashSpeed));
            rb.velocity = Vector2.zero;
            rb.AddForce(((Vector2)ScreenMouse - (Vector2)transform.position).normalized * dashSpeed,ForceMode2D.Impulse);
            isDashing = false;
        }

    }
    private void DashTimeCounter()
    {
        dashTime -= Time.deltaTime;
    }

    private void DashCheck()
    {
        if (dashTime <= 0)
        {
            dashIndicator.SetActive(true);
        }
        else
        {
            dashIndicator.SetActive(false);
        }
    }

    private void DashIndicator() 
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(reader.MousePosition) - transform.position;

        mousePosition.Normalize();

        float rotation_z = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;

        dashTarget.transform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        DashIndicatorMove();
    }
    private void DashIndicatorMove() 
    {
        Vector3 dashTargetFinalPosition = dashTarget.transform.position;
        Vector3 ScreenMouse = Camera.main.ScreenToWorldPoint(reader.MousePosition);
        dashTargetFinalPosition.x = ScreenMouse.x;
        dashTargetFinalPosition.y = ScreenMouse.y;

        Vector3 offset = dashTargetFinalPosition - this.transform.position;
        offset.Normalize();
        offset = offset * radius;
        dashTargetFinalPosition = offset;
        dashTarget.transform.localPosition = dashTargetFinalPosition;

    }
}
