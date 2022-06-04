using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakwayObject : MonoBehaviour
{
    [SerializeField] private string breakerObjectTag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == breakerObjectTag) 
        {
            //Debug.Log("HIt");
            this.gameObject.SetActive(false);
            //play breakaway animation
        }
    }
}
