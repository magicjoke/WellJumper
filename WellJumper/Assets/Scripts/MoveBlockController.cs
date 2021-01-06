using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockController : MonoBehaviour
{

    public bool isVertical = false;
    public Transform topLimit;
    public Transform botLimit;
    public Transform leftLimit;
    public Transform rightLimit;
    public LayerMask whatIsGround;

    public void Update(){

    }

    public void OnMouseEnter(){
        //Debug.Log("Entered");
    }

    void OnMouseDrag()
    {   

        if(!isVertical){
            Vector2 blockLEFTlimit = new Vector2(transform.position.x - 0.55f, transform.position.y);
            Vector2 blockRIGHTlimit = new Vector2(transform.position.x + 0.55f, transform.position.y);
            RaycastHit2D blockLEFT = Physics2D.Raycast(blockLEFTlimit, Vector2.left, 0.05f, whatIsGround);
            RaycastHit2D blockRIGHT = Physics2D.Raycast(blockRIGHTlimit, Vector2.right, 0.05f, whatIsGround);

            if(blockLEFT){
                Debug.Log("Block left");
            } 
            if(blockRIGHT){
                Debug.Log("Block right");
            } 

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosition.x >= rightLimit.transform.position.x && !blockRIGHT){
                transform.position = new Vector3(rightLimit.transform.position.x, transform.position.y, transform.position.z);
            } else if (mousePosition.x <= leftLimit.transform.position.x && !blockLEFT){
                transform.position = new Vector3(leftLimit.transform.position.x, transform.position.y, transform.position.z);
            } else {
                if(blockLEFT) {
                    Debug.Log("LeftBlocked");
                    if(mousePosition.x <= blockLEFTlimit.x + 0.55f){
                        transform.position = new Vector3(blockLEFTlimit.x + 0.55f, transform.position.y, transform.position.z);
                    } else {
                        transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
                    }
                } else if (blockRIGHT){
                    Debug.Log("RightBlocked");
                   if(mousePosition.x >= blockRIGHTlimit.x - 0.55f){
                        transform.position = new Vector3(blockRIGHTlimit.x - 0.55f, transform.position.y, transform.position.z);
                    } else {
                        transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
                    }
                } else {
                    transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
                }
                //transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
            }
        } else {
            Vector2 blockUPlimit = new Vector2(transform.position.x, transform.position.y + 0.55f);
            Vector2 blockDOWNlimit = new Vector2(transform.position.x, transform.position.y - 0.55f);
            RaycastHit2D blockUP = Physics2D.Raycast(blockUPlimit, Vector2.up, 0.05f, whatIsGround);
            RaycastHit2D blockDOWN= Physics2D.Raycast(blockDOWNlimit, Vector2.down, 0.05f, whatIsGround);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(mousePosition.y >= topLimit.transform.position.y && !blockUP){
                transform.position = new Vector3(transform.position.x, topLimit.transform.position.y, transform.position.z);
            } else if (mousePosition.y <= botLimit.transform.position.y && !blockDOWN){
                transform.position = new Vector3(transform.position.x, botLimit.transform.position.y, transform.position.z);
            } else {
                if(blockUP){
                    if(mousePosition.y >= blockUPlimit.y - 0.55f){
                        transform.position = new Vector3(transform.position.x, blockUPlimit.y - 0.55f, transform.position.z);
                    } else {
                        transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
                    }
                } else if(blockDOWN){
                    if(mousePosition.y <= blockDOWNlimit.y + 0.55f){
                        transform.position = new Vector3(transform.position.x, blockDOWNlimit.y + 0.55f, transform.position.z);
                    } else {
                        transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
                }
                //transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
            }
        }

    }
}
