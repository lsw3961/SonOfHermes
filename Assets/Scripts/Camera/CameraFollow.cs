using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float damping;
    private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        Vector3 movePosition = target.position + offset;
        if (target.localScale.x <0) 
        {
            movePosition = new Vector3(target.position.x-offset.x,movePosition.y,movePosition.z);        }
        
        transform.position = Vector3.SmoothDamp(transform.position,movePosition,ref velocity,damping);
    }
}
