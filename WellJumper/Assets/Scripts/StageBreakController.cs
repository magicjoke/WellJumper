using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBreakController : MonoBehaviour
{
    public Sprite stageOne;
    public Sprite stageTwo;
    public Sprite stageThree;
    //public Sprite stageOne;

    public void Start(){
        StartCoroutine(stageProc());
    }

    public IEnumerator stageProc(){
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = stageOne;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = stageTwo;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().sprite = stageThree;
        Destroy(gameObject);
    }
}
