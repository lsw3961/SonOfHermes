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
    [SerializeField] float indicatorRadius = 1;
    [SerializeField] float dashRadius = 10;



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

        if ( isDashing)
        {
            if (lastDirection.x == 0)
            {
                lastDirection.x = 1;
            }
            Vector2 ScreenMouse = Camera.main.ScreenToWorldPoint(reader.MousePosition);
            Vector2 betterTransform = this.transform.position;

            Vector2 offset = betterTransform +(ScreenMouse - betterTransform).normalized * dashRadius;


            if (Vector2.Distance(ScreenMouse,betterTransform ) > Vector2.Distance(betterTransform, offset))
            {
                this.transform.position = Vector2.MoveTowards(transform.position, (Vector2)offset, dashSpeed);
            }
            else
                this.transform.position = Vector2.MoveTowards(transform.position, (Vector2)ScreenMouse, dashSpeed);
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
        offset = offset * indicatorRadius;
        dashTargetFinalPosition = offset;
        dashTarget.transform.localPosition = dashTargetFinalPosition;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere((Vector2)this.transform.position, dashRadius);
    }

}
