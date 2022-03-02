using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowers : MonoBehaviour
{
    [SerializeField] Player player;
    Collider2D[] results;
    [SerializeField] string enemyTag = "Enemy";
    //if the user strikes the ground they launch a force outward that damages and knocks back foes
    public void DashGroundPound(Vector2 oldPosition, Vector2 newPosition) 
    {
        //check if the oldposition y is greater then the new position y
        //if it is return
        if (oldPosition.y < newPosition.y) 
            return;

        //otherwise

        //create two rayspheres on the left and right of the player.
        //if we hit anything
        //check that the thing has a rigidbody.
        //if it does apply a force in the opposite direction to the player
        //left side ray
        results = Physics2D.OverlapCircleAll((Vector2)this.transform.position + new Vector2(player.groundPoundOffset.x * -1, player.groundPoundOffset.y), player.groundPoundRadius, player.groundPoundLayer);
        for (int i = 0; i < results.Length; i++)
        {
                Debug.Log("Enemy Hit right");
        }
        results = Physics2D.OverlapCircleAll((Vector2)this.transform.position + new Vector2(player.groundPoundOffset.x, player.groundPoundOffset.y), player.groundPoundRadius, player.groundPoundLayer);
        for (int i = 0; i < results.Length; i++)
        {
                Debug.Log("Enemy Hit left");

        }



    }

    //a basic attack that piggybacks off of the dash
    public bool DashAttack(Vector2 oldPosition, Vector2 newPosition) 
    {
        //cast a line between the old position and the new position.
        //if theres something there find its tag
            //if the matches an attackable foe, call their damage script, rn just trigger a debug

        return false;

    }

    //Allows you to phase through certain objects to get to secret areas
    public bool PhaseDash() 
    {
        //set the nondashable layer to no longer include the dashable layer
        return false;

    }

    //when you dash you send a wave of energy in front of you that can break structures and damage foes
    public bool DashBlast(Vector2 oldPosition, Vector2 newPosition) 
    {
        //get the direction of the bast
            //play a particle system with collision on that sends out particles in that direction
            //call on particle collision
        return false;

    }

    //When dashing against a wall you bounce off of it
    public bool DashBounce(Vector2 oldPosition, Vector2 newPosition) 
    {
        return false;

    }
    private void OnParticleCollision(GameObject other)
    {
        //check the tag of the object against a lit of hittable tags
        //if something hits
            //call that objects hit function
            //will trigger either damage or destruction
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere((Vector2)this.transform.position + new Vector2(player.groundPoundOffset.x*-1,player.groundPoundOffset.y), player.groundPoundRadius);
        Gizmos.DrawWireSphere((Vector2)this.transform.position + player.groundPoundOffset, player.groundPoundRadius);
    }
}
