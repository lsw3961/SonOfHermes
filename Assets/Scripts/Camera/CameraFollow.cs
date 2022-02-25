using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Camera cam;
    [SerializeField] Vector2 camOffset = new Vector2(20,20);
    [SerializeField] float yOffset = 15;

    private void FixedUpdate()
    {
        StartCoroutine(CheckCam());
    }
    IEnumerator CheckCam()
    {
        //check the camera bounds
        yield return new WaitForSeconds(0.1f);
        Vector3 screenPos = cam.WorldToScreenPoint(player.transform.position);
        float ratioX = screenPos.x / cam.pixelWidth;
        float ratioY = screenPos.y / cam.pixelHeight;

        if (ratioX < 0f) // if we're below our safe frame
            MoveCam(-1, 0);
        else if (ratioX > 1f)
        {
            MoveCam(1, 0);
        }
        if (ratioY < 0f) // if we're above our safe frame, return false        
            MoveCam(0, -1);
        else if (ratioY > 1f)
        {
            MoveCam(0, 1);
        }

    }
    private void MoveCam(int leftRight, int upDown) 
    {

        if (leftRight < 0)
        {
            this.transform.position = new Vector3(player.transform.position.x - camOffset.x ,cam.transform.position.y,this.transform.position.z);
        }
        else if (leftRight > 0) 
        {
            this.transform.position = new Vector3(player.transform.position.x + camOffset.x, cam.transform.position.y , this.transform.position.z);
        }

        if (upDown < 0)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - camOffset.y, this.transform.position.z);
        }
        else if (upDown > 0) 
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + camOffset.y, this.transform.position.z);
        }


    }
}
