using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] Animator anim;
    public void ShakeCam() 
    {
        anim.SetTrigger("shake");
    }
}
