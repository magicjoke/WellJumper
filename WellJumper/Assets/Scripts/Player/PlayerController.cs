using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;

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


    // Player Poses
    public Vector2 lastPos;
    public Vector2 currPos;
    public Quaternion startPos;

    [SerializeField] private GameObject jumpGO;

    //SCORE
    private GameObject score;

    //GAME CONTROLLER
    private GameObject gameController;

    // Rays
    public LayerMask whatIsGround;
    public float wallCheckDistance = 2f;
    public GameObject playerDestroyParticle;

    //Stretch 
    [SerializeField] private bool stretchOnce = false;

    // 
    public State currentState;
    public enum State {
        inActive,
        active,
        sticked
    }

    //skins
     private Object[] textures;

    void Start()
    {
        jumpOnce = true;

        score = GameObject.FindGameObjectWithTag("Score");
        gameController = GameObject.FindGameObjectWithTag("GameController");
        startPos = transform.rotation;

        // State
        currentState = State.inActive;

        // Anim
        GetComponent<Animator>().Play("Idle");
        getSkin();
    }

    public void getSkin(){
        string currentSkinName = PlayerPrefs.GetString("CurrentSkinName");
        var textures = Resources.LoadAll("Skins", typeof(SkinsScriptableObject)).Cast<SkinsScriptableObject>().ToArray();
        foreach (var t in textures){
            if(currentSkinName == t.skinName ){
                GetComponent<SpriteRenderer>().sprite = t.skinSprite;
                GetComponent<Animator>().runtimeAnimatorController = t.skinAnim;
            }
        }
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

        if (score)
        {
            score.GetComponent<Text>().text = maxPlayerHeight.ToString("F0") + "m";
        }

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

        RaycastHit2D groundCheck = Physics2D.Raycast(this.transform.position, Vector2.down, 0.7f, whatIsGround);

        if(playerVelocity == 0 && jumpOnce == true && groundCheck)
        {
            if(currentState == State.active){
               StartCoroutine(jumpCourNew());
               gameController.GetComponent<GameController>().changeToActiveState();
            }
            lastPos = this.transform.position; 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //DOTween.KillAll();
            //PlayerPrefs.DeleteAll();
            currentState = State.active;
            freezeCharacter(false);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x + 4f, transform.position.y + 4f) * (speed * 1f));
        }
        if(currentState == State.sticked){
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            freezeCharacter(true);
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DeathZone")
        {
            //Scene scene = SceneManager.GetActiveScene();
            //SceneManager.LoadScene(scene.name);
            StartCoroutine(characterDeath());
            //gameController.GetComponent<GameController>().activateGameOver();

        }
        if(collision.gameObject.tag == "BestScoreLine"){
            Transform bestScoreTrans = collision.gameObject.transform;
            GameObject scoreParticle = Instantiate(collision.gameObject.GetComponent<BestScore>().particles, bestScoreTrans.position, Quaternion.identity);
            scoreParticle.transform.Rotate(90f,0f, 0f);

            bestScoreTrans.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (collision.gameObject.tag == "Coin")
        {
            gameController.GetComponent<GameController>().updateCoins(1);
            //collision.gameObject.GetComponent<CoinController>().instParticles(); 
            collision.gameObject.GetComponent<Animator>().SetTrigger("CoinPickUp");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject, 1f);
            //gameController.GetComponent<GameController>().coins
        }
        if(collision.gameObject.tag == "BubbleBlock"){
            Transform BubbleBlockTrans = collision.gameObject.transform;
            this.transform.parent = collision.gameObject.transform;

            Sequence stickySequence = DOTween.Sequence()
                .Join(transform.DOMove(new Vector3(BubbleBlockTrans.position.x, BubbleBlockTrans.transform.position.y, BubbleBlockTrans.transform.position.z), 1f).OnComplete(() => currentState = State.sticked ));

            collision.gameObject.GetComponent<BubbleBlockController>().stickedPlayer = this.gameObject;


        }
        if(collision.gameObject.tag == "JumperBlock"){
            collision.gameObject.GetComponent<Animator>().SetTrigger("JumperActive");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Random.Range(jumpRange.y, jumpRange.y)) * (speed * 2f));
        }
        if(collision.gameObject.tag == "JumperBlockLeft"){
            collision.gameObject.GetComponent<Animator>().SetTrigger("JumperActive");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x + 5f, transform.position.y + 5f) * (speed * 1.5f));
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x + 5f, transform.position.y + 5f) * speed);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * (speed * 1.5f));

           
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
            //Debug.Log("Jumper Left");
        }
        if(collision.gameObject.tag == "JumperBlockRight"){
            collision.gameObject.GetComponent<Animator>().SetTrigger("JumperActive");
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x - 5f, transform.position.y + 5f) * (speed * 1.5f));
            //GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x - 5f, transform.position.y + 5f) * speed);
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, 0), Random.Range(jumpRange.y, jumpRange.y)) * (speed * 1.5f));
            //Debug.Log("Jumper Right");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Block" || collision.gameObject.tag == "BrokenBlock" || collision.gameObject.tag == "RotateBlock" || collision.gameObject.tag == "MoveBlock")
        {
            lastPos = this.transform.position;
            //stretchPlayer();
            //DOTween.KillAll ();
        }
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Block" || collision.gameObject.tag == "BrokenBlock")
        {
            //stretchPlayer();
        }

        if(collision.gameObject.tag == "MoveBlock"){
            this.transform.parent = collision.gameObject.transform;
        }

    }

    private void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.tag == "MoveBlock"){
            this.transform.parent = null;
        }
        // if(collision.gameObject.tag == "BubbleBlock"){
        //     this.transform.parent = null;
        // }
    }
    // IEnumerator jumpCour()
    // {
    //     jumpOnce = false;
    //     yield return new WaitForSeconds(jumpDelay);


    //     stretchPlayer();
    //     GameObject jumpPart = Instantiate(jumpGO, new Vector3(transform.position.x, transform.position.y + 0.35f), Quaternion.identity);

    //     Destroy(jumpPart, 2f);

    //     GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
    //     if(numJumps < 1)
    //     {
    //         numJumps++;
    //     }
    //     yield return new WaitForSeconds(1f);
    //     jumpOnce = true;
    // }

    IEnumerator jumpCourNew()
    {
        jumpOnce = false;
        stretchOnce = false;
        //GetComponent<Rigidbody2D>().simulated = false;
        yield return new WaitForSeconds(jumpDelay);


        stretchPlayer();
        GameObject jumpPart = Instantiate(jumpGO, new Vector3(transform.position.x, transform.position.y + 0.35f), Quaternion.identity);
        Destroy(jumpPart, 2f);


        string sideCheck = searchWall();

        if (sideCheck == "WallLeftRight"){
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Random.Range(jumpRange.y, jumpRange.y)) * speed);
            Debug.Log("WallLeftRightJumpUP");
        } else if(sideCheck == "WallLeft"){
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(0, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
        } else if(sideCheck == "WallRight"){
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, 0), Random.Range(jumpRange.y, jumpRange.y)) * speed);
        } else {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);
        }
        Debug.Log("Jumpscasm");
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-jumpRange.x, jumpRange.x), Random.Range(jumpRange.y, jumpRange.y)) * speed);


        //float range;
        //if (Random.value < .5)
        //{
        //    range = Random.Range(-3f, -1f);
        //} else {
        //    range = Random.Range(3f, 1f);
        //}

        //float newY = transform.position.y + 3f;
        //float newX = transform.position.x + range;
        //Debug.Log(newY + " " + newX);

        //Sequence jumpIn = DOTween.Sequence();
        //    jumpIn.Append(transform.DOMoveX(newX, 1).SetEase(Ease.InQuad))
        //      .Join(transform.DOMoveY(newY, 1).SetEase(Ease.OutQuad))
        //      .OnComplete(() => {
              
        //          Debug.Log("UpFinished");
        //          float newX2 = transform.position.x + range;
        //          float newY2 = transform.position.y - 3f;
        //          Sequence jumpOut = DOTween.Sequence();
        //            jumpOut.Append(transform.DOMoveX(newX2, 1).SetEase(Ease.OutQuad) )//.SetEase(Ease.InQuad)
        //            .Join(transform.DOMoveY(newY2, 1).SetEase(Ease.InQuad))
        //            .OnComplete(() => { Debug.Log("DownFinished");  });
          
        //      });


        //.PrependInterval(1);
        //.Insert(0, transform.DOScale(new Vector3(3, 3, 3), mySequence.Duration() ));


        //Sequence sequence = DOTween.Sequence()
        //    .Join(transform.DOMoveX(newX, 1).SetEase(Ease.InQuad)//.SetEase(Ease.InQuad)
        //    .OnComplete(() =>
        //    {
        //        transform.DOMoveY(transform.position.y + 5, 1);
        //        //transform.DORotate(new Vector3(0.0f, 180.0f, 0.0f), 0.5f, RotateMode.Fast);
        //        //transform.DOScale(new Vector3(1f, 1f), 0.1f);
        //    }
        //    ));


        if (numJumps < 1)
        {
            numJumps++;
        }
        yield return new WaitForSeconds(1f);
        jumpOnce = true;
        stretchOnce = true;
        //GetComponent<Rigidbody2D>().simulated = true;
    }

    public void stretchPlayer()
    {
        if(stretchOnce == false){
            Sequence sequence = DOTween.Sequence()
                .Join(transform.DOScale(new Vector3(0.7f, 1.4f), 0.1f).OnComplete(() => transform.DOScale(new Vector3(1f, 1f), 0.1f)));
        }
        
    }

    public IEnumerator characterDeath(){
        freezeCharacter(true);
        string whereIsWall = searchWall();
        if(whereIsWall == "WallLeft"){
            deathKnokback(new Vector2(this.transform.position.x +1f, this.transform.position.y + 1f));
        } else if (whereIsWall == "WallRight"){
            deathKnokback(new Vector2(this.transform.position.x -1f, this.transform.position.y + 1f));
        } else if (whereIsWall == "WallUp"){
            deathKnokback(new Vector2(this.transform.position.x, this.transform.position.y - 2f));
        } else if (whereIsWall == "WallDown"){
            deathKnokback(new Vector2(this.transform.position.x, this.transform.position.y + 2f));
        } else {
            deathKnokback(new Vector2(this.transform.position.x, this.transform.position.y + 1f));
        }
        // max height
        float curMaxHeigth = gameController.GetComponent<GameController>().getMaxHeigth();
        if(curMaxHeigth < maxPlayerHeight){
            gameController.GetComponent<GameController>().saveMaxHeight(maxPlayerHeight);
        }
        //
        gameController.GetComponent<GameController>().saveCoins();
        yield return new WaitForSeconds(2f);
        gameController.GetComponent<GameController>().activateGameOver();
        freezeCharacter(false);

    }

    public void freezeCharacter(bool freeze){
        if(freeze){
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        } else {
            this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }

    public void deathKnokback(Vector2 moveKnokback){
            Sequence sequence = DOTween.Sequence();
                sequence.Append(transform.DOMove(moveKnokback, 1f ).SetEase(Ease.Linear))
                .Join(transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 210f), 1f).SetEase(Ease.Linear) )
                .Join(transform.DOScale(new Vector3(1.4f, 1.4f), 0.8f))
                .OnComplete(() => { 

                   this.GetComponent<SpriteRenderer>().enabled = false;
                    GameObject deathParticle = Instantiate(playerDestroyParticle, this.transform.position, Quaternion.identity);
                    deathParticle.transform.Rotate(90f,0f, 0f);

                });
    }

    public string searchWall()
    {
        RaycastHit2D wallLEFT = Physics2D.Raycast(this.transform.position, Vector2.left, wallCheckDistance, whatIsGround);
        RaycastHit2D wallRIGHT = Physics2D.Raycast(this.transform.position, Vector2.right, wallCheckDistance, whatIsGround);
        RaycastHit2D wallUP = Physics2D.Raycast(this.transform.position, Vector2.up, wallCheckDistance, whatIsGround);
        RaycastHit2D wallDOWN= Physics2D.Raycast(this.transform.position, Vector2.down, wallCheckDistance, whatIsGround);

        if (wallLEFT && wallRIGHT){ return "WallLeftRight"; }
        else if (wallLEFT) { return "WallLeft"; }
        else if (wallRIGHT) { return "WallRight"; } 
        else if (wallUP) { return "WallUp"; }
        else if (wallDOWN) { return "WallDown"; }
        else { return "NoWall"; }
    }

    public void changeToActiveState(){
        currentState = State.active;
    }
}
