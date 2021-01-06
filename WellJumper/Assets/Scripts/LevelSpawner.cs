using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{

    public List<GameObject> levelLayouts;


    void Start()
    {
        StartCoroutine(spawnLevel());
    }

    IEnumerator spawnLevel()
    {
        int rndmLayout = Random.Range(0, (levelLayouts.Count));

        yield return new WaitForSeconds(4f);
        Instantiate(levelLayouts[rndmLayout], transform.position,transform.rotation);
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 15);
        StartCoroutine(spawnLevel());
    }
}
