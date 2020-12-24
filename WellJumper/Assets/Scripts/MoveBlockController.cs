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

    public void Update(){

    }

    public void OnMouseEnter(){
        //Debug.Log("Entered");
    }

    void OnMouseDrag()
    {   
        if(!isVertical){
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(worldPosition.x >= rightLimit.transform.position.x){
                transform.position = new Vector3(rightLimit.transform.position.x, transform.position.y, transform.position.z);
            } else if (worldPosition.x <= leftLimit.transform.position.x){
                transform.position = new Vector3(leftLimit.transform.position.x, transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(worldPosition.x, transform.position.y, transform.position.z);
            }
        } else {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(worldPosition.y >= topLimit.transform.position.y){
                transform.position = new Vector3(transform.position.x, topLimit.transform.position.y, transform.position.z);
            } else if (worldPosition.y <= botLimit.transform.position.y){
                transform.position = new Vector3(transform.position.x, botLimit.transform.position.y, transform.position.z);
            } else {
                transform.position = new Vector3(transform.position.x, worldPosition.y, transform.position.z);
            }
        }

    }
}
