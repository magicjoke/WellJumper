using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBlockController : MonoBehaviour
{
    public GameObject stickedPlayer;

    public void checkStickedPlayer(){
        if(stickedPlayer){
            if(stickedPlayer.GetComponent<PlayerController>().currentState == PlayerController.State.sticked){
                stickedPlayer.GetComponent<PlayerController>().freezeCharacter(false);
                stickedPlayer.GetComponent<PlayerController>().currentState = PlayerController.State.active;

                //player.GetComponent<PlayerController>().search();
                string wallSide = stickedPlayer.GetComponent<PlayerController>().search();
                float rnd = Random.Range(3f, 5f);
                if(wallSide == "WallRight"){
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x - rnd, stickedPlayer.transform.position.y + 5f) * (100 / 2));
                } else if (wallSide == "WallLeft"){
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x + rnd, stickedPlayer.transform.position.y + 5f) * (100 / 2));
                }else {
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x, stickedPlayer.transform.position.y + 4f) * (100 / 2));
                }
                Debug.Log(wallSide);
            }
        } else {
            Debug.Log("No Player");
        }
    }
}
