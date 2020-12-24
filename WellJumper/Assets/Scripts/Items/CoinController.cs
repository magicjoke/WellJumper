using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinController : MonoBehaviour
{
    
    public GameObject coinDestrParticle;

    public void instParticles(){

        Sequence sequence = DOTween.Sequence()
            .Join(transform.DOScale(new Vector3(0f, 1.4f), 0.1f)
            .OnComplete( 
                () => 
                Destroy(gameObject)
            ) );

        GameObject coinParticles = Instantiate(coinDestrParticle, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        coinParticles.transform.Rotate(90f,0f, 0f);

        Destroy(coinParticles, 2f);
    }
}
