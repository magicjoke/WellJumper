using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBlock : MonoBehaviour
{

    public Sprite broken_one;
    public GameObject particle;
    public float HP = 2;

    // Update is called once per frame
    void Update()
    {
        if (HP == 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = broken_one;
        }
        else if(HP == 0)
        {
            GameObject particleClone = Instantiate(particle, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            particleClone.transform.Rotate(90f,0f, 0f);

            Destroy(particleClone, 2f);
            
            Destroy(this.gameObject);
        }
    }
}
