using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class BlockSpawnController : MonoBehaviour
{
    public GameObject prefab;

    private GameObject player;
    private GameObject gameController;

    // TILEMAP
    public Tilemap tilemap;
    public TileBase tileBase;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Player = Player.GetComponent<PlayerController>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    public void Update()
    {

        ////////////////////////////////
        Vector3Int tilemapPos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        Debug.Log(tilemapPos);
        Tile tile = tilemap.GetTile<Tile>(tilemapPos);

        //tilemap.SetTile(tilemapPos, tileBase);
        Debug.Log(tile);
        ////////////////////////////////

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Debug.Log(hit.collider);

                if(player.GetComponent<PlayerController>().numJumps == 1)
                {
                    if (hit.collider.gameObject.tag == "Block")
                    {
                        Debug.Log("BLOCK!");

                    }
                    else if(hit.collider.gameObject.tag == "BrokenBlock"){}
                    else if(hit.collider.gameObject.tag == "RotateBlock"){}
                    else if(hit.collider.gameObject.tag == "Player"){
                        //hit.collider.gameObject.GetComponent<StickyBlockController>().checkStickedPlayer();
                        stickyPlayerClick();
                        Debug.Log("PlayerClicked");
                    }
                    else if (hit.collider.gameObject.tag == "Coin")
                    {
                        gameController.GetComponent<GameController>().coins++;
                        Destroy(hit.collider.gameObject);
                        Debug.Log("Coin!");

                    }
                    else if(hit.collider.gameObject.tag == "MoveBlock"){}
                    else if(hit.collider.gameObject.tag == "StickyBlock"){
                        hit.collider.gameObject.GetComponent<StickyBlockController>().checkStickedPlayer();

                    }
                    //else if(hit.collider.gameObject.tag == "PlacementPlace")
                    //{
                    //    Debug.Log("PlacementPlace!");
                    //}
                    else
                    {
                        Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);

                        GameObject spawnBlock = Instantiate(prefab, hit.collider.gameObject.transform.position, Quaternion.identity);
                        gameController.GetComponent<GameController>().lastBlockPos = hit.collider.gameObject.transform.position;
                        Sequence sequence = DOTween.Sequence()
                           .Join(spawnBlock.transform.DOScale(new Vector3(0.7f, 1.4f), 0.1f).OnComplete(() => spawnBlock.transform.DOScale(new Vector3(1f, 1f), 0.1f)));

                        player.GetComponent<PlayerController>().numJumps--;

                        Destroy(spawnBlock, 5f);
                    }
                }

                if(hit.collider.gameObject.tag == "BrokenBlock")
                {
                    Debug.Log("BrokenBlock!");
                    hit.collider.gameObject.GetComponent<Animator>().Play("BrokeAnim");
                    Sequence sequence = DOTween.Sequence();
                    //    sequence.Join(hit.collider.gameObject.transform.DOScale(new Vector3(0.7f, 1.4f), 0.1f)
                    //    .OnComplete(() => hit.collider.gameObject.transform.DOScale(new Vector3(1f, 1f), 0.1f)));
                    float time = 0.12f;

                    if(hit.collider.gameObject.GetComponent<BrokenBlock>().HP > 1){
                        sequence.Append(hit.collider.gameObject.transform.DORotate(new Vector3(0, 0, 8), time))
                        .Append(hit.collider.gameObject.transform.DORotate(new Vector3(0, 0, -8), time))
                        .OnComplete( () => hit.collider.gameObject.transform.DORotate(new Vector3(0, 0, 0), time) );
                    }
                    

                }
                if(hit.collider.gameObject.tag == "StickyBlock"){
                    //stickyPlayerClick();
                    hit.collider.gameObject.GetComponent<StickyBlockController>().checkStickedPlayer();
                }
                if(hit.collider.gameObject.tag == "RotateBlock"){

                    Transform hitTrans = hit.collider.gameObject.transform;
                    float currRot =  hit.collider.gameObject.transform.rotation.eulerAngles.z;
                    float transDeg = currRot + -90f;

                    hitTrans.DORotate(new Vector3(0, 0, transDeg), 1f);

                    Debug.Log("Rotate Block!");
                }


                if(hit.collider.gameObject.tag == "BrokenBlock")
                {
                    hit.collider.gameObject.GetComponent<BrokenBlock>().HP--;
                    Debug.Log("PlaceBrokenBlock!");
                }

            } else {
                            
                tilemap.SetTile(tilemapPos, tileBase);
            }
        }
    }

    public void stickyPlayerClick(){
        if(player.GetComponent<PlayerController>().currentState == PlayerController.State.sticked){
            player.GetComponent<PlayerController>().freezeCharacter(false);
            player.GetComponent<PlayerController>().currentState = PlayerController.State.active;

            //player.GetComponent<PlayerController>().search();
            string wallSide = player.GetComponent<PlayerController>().search();
            float rnd = Random.Range(3f, 5f);
            if(wallSide == "WallRight"){
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x - rnd, player.transform.position.y + 5f) * (100 / 2));
            } else if (wallSide == "WallLeft"){
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(player.transform.position.x + rnd, player.transform.position.y + 5f) * (100 / 2));
            }else {
                player.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x, transform.position.y + 4f) * (100 / 2));
            }
            Debug.Log(wallSide);
        }
    }

}
