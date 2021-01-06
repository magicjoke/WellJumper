using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BubbleBlockController : MonoBehaviour
{
    public GameObject stickedPlayer;
    public GameObject bubbleDestroyParticle;


    public void Start(){
        //this.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 2.0f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            // Sequence sequence = DOTween.Sequence();
            //     sequence.Append(transform.DOScale(new Vector3(0.5f,0.5f), 1f ).SetEase(Ease.Linear))
            //     .Join(transform.DOScale(new Vector3(1f, 1f), 0.8f))
            //     .OnComplete(() => { 
            //         //transform.DOScale(new Vector3(1f,1f), 1f ).SetEase(Ease.Linear);
            //     });//.SetLoops(-1, LoopType.Incremental);

        StartCoroutine(bubbleWobble());
        StartCoroutine(bubbleFloat());

    }

    public void checkStickedPlayer(){
        if(stickedPlayer){
            if(stickedPlayer.GetComponent<PlayerController>().currentState == PlayerController.State.sticked){
                stickedPlayer.GetComponent<PlayerController>().freezeCharacter(false);
                stickedPlayer.GetComponent<PlayerController>().currentState = PlayerController.State.active;

                //Instantiate(bubbleDestroyParticle, transform.position, Quaternion.identity);

                GameObject bubbleDestrPart = Instantiate(bubbleDestroyParticle, transform.position, Quaternion.identity);
                bubbleDestrPart.transform.Rotate(90f,0f, 0f);
                Destroy(bubbleDestrPart, 2f);
                //player.GetComponent<PlayerController>().search();
                string wallSide = stickedPlayer.GetComponent<PlayerController>().searchWall();
                float rnd = Random.Range(3f, 5f);
                if(wallSide == "WallRight"){
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x - rnd, stickedPlayer.transform.position.y + 5f) * (100 / 2));
                    stickedPlayer.transform.parent = null;
                    Destroy(this.gameObject);
                } else if (wallSide == "WallLeft"){
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x + rnd, stickedPlayer.transform.position.y + 5f) * (100 / 2));
                    stickedPlayer.transform.parent = null;
                    Destroy(this.gameObject);
                }else {
                    stickedPlayer.GetComponent<Rigidbody2D>().AddForce(new Vector2(stickedPlayer.transform.position.x, stickedPlayer.transform.position.y + 4f) * (100 / 2));
                    stickedPlayer.transform.parent = null;
                    Destroy(this.gameObject);
                }
                //Destroy(gameObject);
                Debug.Log(wallSide);
            }
        } else {
            Debug.Log("No Player");
        }
    }

    IEnumerator bubbleWobble(){
        
        while(true){
            
            float rndTime = Random.Range(2f, 4f);
            yield return new WaitForSeconds(rndTime);
            Sequence sequence = DOTween.Sequence()
                .Join(transform.DOScale(new Vector3(0.7f, 1.2f), 0.05f).OnComplete(() => transform.DOScale(new Vector3(1f, 1f), 0.05f)));
        }

        //bblStretch();
    }

    IEnumerator bubbleFloat(){
        float rndTime = Random.Range(0.5f, 1.5f);
        transform.DOMove(new Vector3(transform.position.x, transform.position.y + 0.10f), 0.4f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(rndTime);
        while(true){
            yield return new WaitForSeconds(0.85f);
            Sequence sequence = DOTween.Sequence()
                .Join(transform.DOMove(new Vector3(transform.position.x, transform.position.y - 0.20f), 0.4f).SetEase(Ease.Linear)
                .OnComplete( () => 
                    transform.DOMove(new Vector3(transform.position.x, transform.position.y + 0.20f), 0.4f)).SetEase(Ease.Linear)
                );
        }

    }

}
