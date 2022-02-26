using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Player/Player")]
public class Player : ScriptableObject
{

//Set Variables-------------------------
    //Movement
        //movement variables
        [SerializeField] public float acceleration = 5;
        [SerializeField] public float maxSpeed = 5;
        //jump variables
        [SerializeField] public float jumpForce = 5;
        [SerializeField] public float fallMultiplier = 5;
        [SerializeField] public float lowJumpMultiplier = 5;
        [SerializeField] public float hangTime = .2f;
    //Dashing
        [SerializeField] public int airDashLimit = 1;
        [SerializeField] public float dashTimeLimit = .3f;
    //-----------------------------------------

    //Retrieved Variables--------------------------
    [SerializeField] public bool isGrounded;
    //---------------------------------------------

    //Get Variables-------------------------------
    public bool hasDash = false;
    public bool hasJumpDash = false;
    public bool hasDashAttack = false;
    public bool hasGroundPound = false;
    public bool hasPhaseDash = false;
    public bool hasDashBlast = false;
    public bool hasDashBounce = false;
    public bool HasLightBlast = false;
    //--------------------------------------------
}
