using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    public GameObject levelLayout;


    void Start()
    {
        StartCoroutine(spawnLevel());
    }

    IEnumerator spawnLevel()
    {
        yield return new WaitForSeconds(4f);
        Instantiate(levelLayout, transform.position,transform.rotation);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 14);
        StartCoroutine(spawnLevel());
    }
}
