﻿using System.Collections;
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

    public GameObject stageBroke;

    // HitBlockParticle
    public ParticleSystem hitBlockParticle;
    public ParticleSystem destroyBlockParticle;
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
        //Debug.Log(tilemapPos);
        Tile tile = tilemap.GetTile<Tile>(tilemapPos);

        //tilemap.SetTile(tilemapPos, tileBase);
        //Debug.Log(tile);
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
                    else if (hit.collider.gameObject.tag == "Coin"){}
                    else if(hit.collider.gameObject.tag == "MoveBlock"){}
                    else if(hit.collider.gameObject.tag == "StickyBlock"){}
                    else {
                        // Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);
                    }
                }

                if (hit.collider.gameObject.tag == "Coin")
                {
                    gameController.GetComponent<GameController>().updateCoins(1);
                    hit.collider.gameObject.GetComponent<Animator>().SetTrigger("CoinPickUp");
                    Destroy(hit.collider.gameObject, 2f);
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
                if(hit.collider.gameObject.tag == "BubbleBlock"){
                    //stickyPlayerClick();
                    //hit.collider.gameObject.GetComponent<StickyBlockController>().
                    hit.collider.gameObject.GetComponent<BubbleBlockController>().checkStickedPlayer();
                    //Destroy(hit.collider.gameObject);
                }
                if(hit.collider.gameObject.tag == "RotateBlock"){

                    GameObject rotateBlock = hit.collider.gameObject;
                    RotateBlockController rotateContr = rotateBlock.GetComponent<RotateBlockController>();

                    if(rotateContr.isRotating == true){
                        Debug.Log("Still rotating");
                    } else {
                        Transform hitTrans = rotateBlock.transform;
                        float currRot =  rotateBlock.transform.rotation.eulerAngles.z;
                        float transDeg = currRot + -90f;

                        StartCoroutine(rotateContr.rotateFreeze(1f));
                        //rotateContr.rotateFreeze(1f);
                        hitTrans.DORotate(new Vector3(0, 0, transDeg), 1f);
                        
                        Debug.Log("Rotate Block!");
                    }
                }


                if(hit.collider.gameObject.tag == "BrokenBlock")
                {
                    GameObject brkBlock = hit.collider.gameObject;
                    Transform brkBlockTrans = hit.collider.gameObject.transform;
                    brkBlock.GetComponent<BrokenBlock>().HP--;
                    
                    // GameObject particleClone = Instantiate(brkBlock.GetComponent<BrokenBlock>().hitParticle, new Vector3(brkBlockTrans.position.x, brkBlockTrans.position.y, brkBlockTrans.position.z), Quaternion.identity);
                    // particleClone.transform.Rotate(90f,0f, 0f);
                    // Destroy(particleClone, 2f);
                    spawnHitParticle(brkBlockTrans);

                    Debug.Log("PlaceBrokenBlock!");
                }

            } else {
                if(gameController.GetComponent<GameController>().currentState != GameController.State.idle){
                    //StartCoroutine(destroyTileAfterTime(stageBroke, tilemap, tilemapPos, tileBase));
                    tilemap.SetTile(tilemapPos, tileBase);
                }
            }
        }
    }

    public IEnumerator destroyTileAfterTime(GameObject brknStage, Tilemap tilemap, Vector3Int tilePos, TileBase tileBase){

        Vector3 wrldPos = tilemap.GetCellCenterWorld(tilePos);
        //Debug.Log(wrldPos);

        tilemap.SetTile(tilePos, tileBase);
        Instantiate(stageBroke, wrldPos, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        spawnHitParticleCor(wrldPos, hitBlockParticle);
        yield return new WaitForSeconds(1f);
        spawnHitParticleCor(wrldPos, hitBlockParticle);
        yield return new WaitForSeconds(1f);
        spawnHitParticleCor(wrldPos, destroyBlockParticle);
        tilemap.SetTile(tilePos, null);
    }

    public void spawnHitParticleCor(Vector3 brkBlockTrans, ParticleSystem particle){
        ParticleSystem hitPart = Instantiate(particle, brkBlockTrans, Quaternion.identity);
        hitPart.transform.Rotate(90f,0f, 0f);
        Destroy(hitPart, 2f);
    }
    public void spawnHitParticle(Transform brkBlockTrans){
        ParticleSystem hitPart = Instantiate(hitBlockParticle, new Vector3(brkBlockTrans.position.x, brkBlockTrans.position.y, brkBlockTrans.position.z), Quaternion.identity);
        hitPart.transform.Rotate(90f,0f, 0f);
        Destroy(hitPart, 2f);
    }

    public void stickyPlayerClick(){
        if(player.GetComponent<PlayerController>().currentState == PlayerController.State.sticked){
            player.GetComponent<PlayerController>().freezeCharacter(false);
            player.GetComponent<PlayerController>().currentState = PlayerController.State.active;

            //player.GetComponent<PlayerController>().search();
            string wallSide = player.GetComponent<PlayerController>().searchWall();
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
