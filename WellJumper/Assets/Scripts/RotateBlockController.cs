using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlockController : MonoBehaviour
{
    public bool isRotating;


    public void Start(){
        isRotating = false;
    }

    public IEnumerator rotateFreeze(float waitTime){

        isRotating = true;
        Debug.Log("true");
        yield return new WaitForSeconds(waitTime);
        isRotating = false;
        Debug.Log("false");
    }
}
