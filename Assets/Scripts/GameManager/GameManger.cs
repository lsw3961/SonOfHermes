using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] Shake camShake;

    public void CameraShake() 
    {
        camShake.ShakeCam();
    }
}
