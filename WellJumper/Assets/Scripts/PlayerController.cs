using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float playerVelocity;
    private bool jumpOnce;

    public float speed;
    public float jumpDelay;

    public Vector2 jumpRange;

    public int numJumps;

    public float maxPlayerHeight;
    public float currentPlayerHeight;


    public Vector2 lastPos;
    public Vector2 currPos;

    //SCORE
    private GameObject score;

    void Start()
    {
        jumpOnce = true;

        score = GameObject.FindGameObjectWithTag("Score");
    }

    void ChangeDirection() //change sprite xy
    {
        if (currPos.x > lastPos.x && this.GetComponent<SpriteRenderer>().flipX)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
            //GetComponent<Animator>().Play("Rotate_1");
        }
        else if (currPos.x < lastPos.x && !this.GetComponent<SpriteRenderer>().flipX)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
            //GetComponent<Animator>().Play("Rotate_1");
        }
    }

    void Update()
    {
        currPos = this.transform.position;
        ChangeDirection();

        score.GetComponent<Text>().text = maxPlayerHeight.ToString("F0");
        //verticalSpeed = this.transform.position.y;

        if (this.GetComponent<Rigidbody2D>().velocity.y > 2.3)
        {
            GetComponent<Animator>().Play("MovingUp");

        }
        else if (this.GetComponent<Rigidbody2D>().velocity.y < -2.3)
        {
            GetComponent<Animator>().Play("MovingDown");
        }
        else if (this.GetComponent<Rigidbody2D>().velocity.y > 1.8 || this.GetComponent<Rigidbody2D>().velocity.y < -1.8)
        {
            GetComponent<Animator>().Play("Idle");
        }

        currentPlayerHeight = this.transform.position.y;

        if (currentPlayerHeight > maxPlayerHeight)
        {
            maxPlayerHeight = currentPlayerHeight;
            //lastPos = this.transform.position;
        }

        playerVelocity = this.GetComponent<Rigidbody2D>().velocity.magnitude;

        if(playerVelocity == 0 && jumpOnce == true)
        {
            //jumpOnce = false;
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-realSquareSize.x, realSquareSize.x), Random.Range(realSquareSize.y, realSquareSize.y)) * speed);
            //numJumps++;
            StartCoroutine(jumpCour());
            lastPos = this.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DeathZone")
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            lastPos = this.transform.position;
        }
    }

    IEnumerator jumpCour()
    {
        jumpOnce = false;
        yield return new WaitForSeconds(jumpDelay);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
        if(numJumps < 1)
        {
            numJumps++;
        }
        yield return new WaitForSeconds(1f);
        jumpOnce = true;
    }
}
