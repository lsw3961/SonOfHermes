using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Experimental.Rendering.LWRP;   
using UnityEngine.Experimental.Rendering.Universal; 
public class Dash : MonoBehaviour
{

    //dash variables
    private float dashSpeed = 5f;
    private float dashTime = 0f;
    private bool isDashing = false;
    float indicatorRadius = 1;
    float dashRadius = 10;
    LayerMask nonDashableLayers;

    Movement movement;
    Rigidbody2D rb;
    [SerializeField] GameObject dashTarget;
    [SerializeField] InputReader reader;
    [SerializeField] GameManger gameManger;
    [SerializeField] Light2D playerLight;
    [SerializeField] Player player;
    private TrailRenderer trailRenderer;
    private ParticleSystem dashParticleSystem;
    private DashPowers powers;


    private int dashAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        SetDefaultValues();
        trailRenderer = GetComponent<TrailRenderer>();
        dashParticleSystem = GetComponent<ParticleSystem>();
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
        powers = GetComponent<DashPowers>();
    }

    private void SetDefaultValues()
    {
        dashAmount = player.airDashLimit;
        dashSpeed = player.dashSpeed;
        indicatorRadius = player.indicatorRadius;
        dashRadius = player.dashRadius;
        nonDashableLayers = player.NonDashAbleLayers;
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
        if (player.hasDash) 
        {
            DashAction();
            DashTimeCounter();
            DashIndicator();
            if (player.isGrounded) 
            {
                dashAmount = player.airDashLimit;
            }
        }

    }

    private void DashTrigger()
    {
        if (dashTime <= 0)
        {
            isDashing = true;
            player.isDashing = true;
        }
        else
            isDashing = false;

    }
    private void DashAction()
    {

        if ( isDashing)
        {
            if (!player.isGrounded && !player.hasJumpDash) 
            {
                isDashing = false;
                return;
            }
            if (dashAmount <= 0) 
            {
                isDashing = false;
                return;
            }
            Vector2 ScreenMouse = Camera.main.ScreenToWorldPoint(reader.MousePosition);
            Vector2 betterTransform = this.transform.position;

            Vector2 offset = betterTransform +(ScreenMouse - betterTransform).normalized * CheckDashRay((ScreenMouse - betterTransform));


            if (Vector2.Distance(ScreenMouse,betterTransform ) > Vector2.Distance(betterTransform, offset))
            {
                this.transform.position = Vector2.MoveTowards(transform.position,offset, dashSpeed);
                if (CheckDashRay((ScreenMouse - betterTransform)) != dashRadius)
                {
                    gameManger.CameraShake();
                    if (player.hasGroundPound)
                    {
                        powers.DashGroundPound(betterTransform, this.transform.position);
                    }
                }

            }
            else
                this.transform.position = Vector2.MoveTowards(transform.position,ScreenMouse, dashSpeed);

            isDashing = false;
            dashTime = player.dashTimeLimit;
            dashAmount--;
            dashParticleSystem.Play(false);
            ChangeEffects();
            StartCoroutine(ColorDash());
        }
    }

    private void DashPowersCheck(Vector2 oldPosition, Vector2 newPosition)
    {
        ////called with new and old position
        //if (player.hasDashAttack) 
        //{
        //    powers.DashAttack(oldPosition,newPosition);
        //}
        //called with new and old position

        //goes in start
        //if (player.hasPhaseDash) 
        //{
        //    powers.PhaseDash();
        //}
        //if (player.hasDashBounce) 
        //{
        //    powers.DashBounce(oldPosition, newPosition);
        //}
        //if (player.hasDashBlast) 
        //{
        //    powers.DashBlast(oldPosition,newPosition);
        //}
    }

    private void ChangeEffects()
    {
        trailRenderer.startColor = new Color(0, 220, 235, 150);
        trailRenderer.endColor = new Color(0, 220, 235, 150);

        playerLight.pointLightOuterRadius *= 3;
        playerLight.pointLightInnerRadius *= 3;
    }

    private void JumpingDash() 
    {
    
    }
    private float CheckDashRay(Vector2 screenMouse)
    {
        float distance = dashRadius;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, screenMouse,Vector2.Distance(transform.position,screenMouse),nonDashableLayers,0);
        if (hit) 
        {
            Debug.Log("bing bong");
            distance = hit.distance;
        }
        return distance;
    }

    private void DashTimeCounter()
    {
        dashTime -= Time.deltaTime;
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

    IEnumerator ColorDash()
    {
        yield return new WaitForSeconds(.2f);
        trailRenderer.startColor = new Color(255, 220, 0, 150);
        trailRenderer.endColor = new Color(255, 220, 0, 150);
        playerLight.pointLightOuterRadius /= 3;
        playerLight.pointLightInnerRadius /= 3;
        player.isDashing = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere((Vector2)this.transform.position, player.dashRadius);
    }



}
