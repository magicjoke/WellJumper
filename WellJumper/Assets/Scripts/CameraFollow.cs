using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //define player game object
    private GameObject player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    //wait for lateupdate
    //void LateUpdate()
    //{
    //    //define start and end of level. 
    //    //This will detach the camera from the player until it enters the range again

    //    if (player.transform.position.y > 0f && player.transform.position.y < 10f)
    //    {
    //        transform.position = new Vector3(player.transform.position.y, 0f, -10f);
    //    }
    //}

    void LateUpdate()
    {

        if (player.transform.position.y > 4f && player.transform.position.y > player.GetComponent<PlayerController>().maxPlayerHeight-1)
        {
            //transform.position = Vector3.SmoothDamp(0f, player.transform.position.y, -10f);
            transform.position = Vector3.Lerp(new Vector3(this.transform.position.x, this.transform.position.y, -10f), new Vector3(0f, player.transform.position.y, -10f), 1f);
        } else if (player.transform.position.y < player.GetComponent<PlayerController>().maxPlayerHeight)
        {
            //Debug.Log("CAMERANOFOLOW");
        }
    }
}

