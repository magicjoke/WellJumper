using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenBlock : MonoBehaviour
{

    public Sprite broken_one;
    public Sprite broken_two;
    public Sprite broken_three;
    public GameObject particle;
    public GameObject hitParticle;
    public float HP = 3;

    // Update is called once per frame
    void Update()
    {
        if(HP == 3){
            this.GetComponent<SpriteRenderer>().sprite = broken_one;
        }
        else if (HP == 2)
        {
            this.GetComponent<SpriteRenderer>().sprite = broken_two;
        }
        else if(HP == 1){
            this.GetComponent<SpriteRenderer>().sprite = broken_three;
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
