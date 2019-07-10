using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnController : MonoBehaviour
{
    public GameObject prefab;

    private GameObject Player;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        //Player = Player.GetComponent<PlayerController>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {

                if(Player.GetComponent<PlayerController>().numJumps == 1)
                {
                    if (hit.collider.gameObject.tag == "Block")
                    {
                        Debug.Log("BLOCK!");

                    }
                    else
                    {
                        Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);

                        Instantiate(prefab, hit.collider.gameObject.transform.position, Quaternion.identity);
                        Player.GetComponent<PlayerController>().numJumps--;
                    }
                }

            }
        }
    }

}
